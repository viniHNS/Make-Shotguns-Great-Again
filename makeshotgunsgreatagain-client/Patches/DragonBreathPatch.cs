using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// Patch 1: Hooks InitiateShot to detect Dragon Breath ammo.
    /// Sets a flag that DragonBreathMuzzlePatch reads to spawn the effect.
    /// Also contains all shared effect logic (SpawnEffect, BuildPrefab, texture generation).
    /// </summary>
    internal class DragonBreathPatch : ModulePatch
    {
        private const string DragonBreathAmmoId = "698924bf6dcd41ac313f5921";
        internal static bool DebugMode = true;

        // Flag set when Dragon Breath ammo is fired — read by DragonBreathMuzzlePatch
        internal static bool IsDragonBreathShot = false;

        // Cached Prefab & Material
        private static GameObject _cachedPrefab;
        private static Material _particleMaterial;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(Player.FirearmController),
                nameof(Player.FirearmController.InitiateShot)
            );
        }

        [PatchPrefix]
        private static void Prefix(Player.FirearmController __instance, AmmoItemClass ammo)
        {
            try
            {
                if (__instance == null || ammo == null) return;

                if (DebugMode) Plugin.LogSource.LogInfo($"[DB] Shot fired. Ammo: {ammo.TemplateId}");

                // Set flag BEFORE InitiateShot body runs — it calls MuzzleManager.Shot internally
                IsDragonBreathShot = (ammo.TemplateId == DragonBreathAmmoId);
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[DB] Patch Error: {ex}");
            }
        }

        // ==================== SHARED EFFECT LOGIC ====================
        // Called by DragonBreathMuzzlePatch with the correct muzzle transform

        internal static void SpawnEffect(Transform muzzle)
        {
            try
            {
                if (_cachedPrefab == null) BuildPrefab();
                if (_cachedPrefab == null) return;

                // Debug: log barrel direction
                if (DebugMode)
                    Plugin.LogSource.LogInfo($"[DB] Muzzle axes — forward={muzzle.forward} | -up={-muzzle.up}");

                // Direction: -up is barrel forward (Tarkov convention, confirmed by HollywoodFX)
                Vector3 dir = -muzzle.up;
                if (dir.sqrMagnitude < 0.01f) dir = muzzle.forward;
                Quaternion rot = Quaternion.LookRotation(dir);

                // fireport IS the exact muzzle position — no offset needed
                GameObject fx = GameObject.Instantiate(_cachedPrefab, muzzle.position, rot);
                if (fx != null)
                {
                    fx.SetActive(true);
                    // Force play — playOnAwake can fail on first shot when prefab is built same frame
                    var ps = fx.GetComponent<ParticleSystem>();
                    if (ps != null) ps.Play(true);
                    GameObject.Destroy(fx, 4.0f);
                }
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[DB] Spawn Error: {ex}");
            }
        }

        // ==================== BUILD PREFAB ====================

        private static void BuildPrefab()
        {
            try
            {
                if (_cachedPrefab != null) return;

                // --- Material (Try every known particle shader) ---
                if (_particleMaterial == null)
                {
                    string[] shaderNames = new string[]
                    {
                        "Legacy Shaders/Particles/Additive",
                        "Particles/Standard Unlit",
                        "Particles/Additive",
                        "Mobile/Particles/Additive",
                        "Sprites/Default",
                        "UI/Default"
                    };

                    Shader shader = null;
                    foreach (string name in shaderNames)
                    {
                        shader = Shader.Find(name);
                        if (shader != null)
                        {
                            if (DebugMode) Plugin.LogSource.LogInfo($"[DB] Using shader: {name}");
                            break;
                        }
                    }

                    if (shader == null)
                    {
                        Plugin.LogSource.LogError("[DB] CRITICAL: No shader found at all!");
                        return;
                    }

                    _particleMaterial = new Material(shader);
                    _particleMaterial.mainTexture = GenerateSparkTexture();

                    if (_particleMaterial.HasProperty("_TintColor"))
                        _particleMaterial.SetColor("_TintColor", new Color(1f, 0.7f, 0.3f, 1f));
                    if (_particleMaterial.HasProperty("_Color"))
                        _particleMaterial.SetColor("_Color", new Color(1f, 0.7f, 0.3f, 1f));
                }

                // --- GameObject ---
                GameObject go = new GameObject("DragonBreath_FX");
                GameObject.DontDestroyOnLoad(go);
                go.SetActive(false);

                ParticleSystem ps = go.AddComponent<ParticleSystem>();
                ParticleSystemRenderer psr = go.GetComponent<ParticleSystemRenderer>();

                // ===== MAIN =====
                var main = ps.main;
                main.duration = 0.5f;
                main.loop = false;
                main.playOnAwake = true;
                main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1.2f);
                main.startSpeed = new ParticleSystem.MinMaxCurve(150f, 250f);
                main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);  // Maior para Billboard ser visível
                main.gravityModifier = 0.6f;
                main.simulationSpace = ParticleSystemSimulationSpace.World;
                main.maxParticles = 400;

                // Color: white-hot → orange → ember
                var colorGrad = new Gradient();
                colorGrad.SetKeys(
                    new GradientColorKey[]
                    {
                        new GradientColorKey(new Color(1f, 1f, 0.9f), 0f),
                        new GradientColorKey(new Color(1f, 0.6f, 0.1f), 0.3f),
                        new GradientColorKey(new Color(1f, 0.3f, 0.0f), 0.7f),
                        new GradientColorKey(new Color(0.6f, 0.1f, 0.0f), 1f)
                    },
                    new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(0f, 0f),      // INVISÍVEL no spawn
                        new GradientAlphaKey(1f, 0.03f),   // Aparece rápido — 3% da vida (já saiu do cano)
                        new GradientAlphaKey(0.6f, 0.8f),
                        new GradientAlphaKey(0f, 1f)
                    }
                );

                // ===== COLOR OVER LIFETIME =====
                var col = ps.colorOverLifetime;
                col.enabled = true;
                col.color = new ParticleSystem.MinMaxGradient(colorGrad);

                // ===== SIZE OVER LIFETIME (Shrink) =====
                var sol = ps.sizeOverLifetime;
                sol.enabled = true;
                sol.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0.1f));

                // ===== EMISSION =====
                var emission = ps.emission;
                emission.enabled = true;
                emission.rateOverTime = 0;
                emission.SetBursts(new ParticleSystem.Burst[]
                {
                    new ParticleSystem.Burst(0.0f, 120, 180),
                    new ParticleSystem.Burst(0.05f, 30, 50)
                });

                // ===== SHAPE (CONE DE DISPARO) =====
                // >>> AJUSTE O CONE AQUI <<<
                var shape = ps.shape;
                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.Cone;
                shape.angle = 3f;       // <<< CONE ANGLE: 0=reta, 3=estreito, 6=médio, 15=bem aberto
                shape.radius = 0.01f;   // <<< CONE RADIUS: base do cone (menor = mais junto)
                shape.position = new Vector3(0f, 0f, 0.25f); // Empurra emissão 25cm para frente

                // ===== NOISE (Chaotic spark wobble) =====
                var noise = ps.noise;
                noise.enabled = true;
                noise.strength = 1.2f;
                noise.frequency = 1.5f;
                noise.scrollSpeed = 3f;
                noise.quality = ParticleSystemNoiseQuality.Medium;

                // ===== VELOCITY LIMIT (Air Drag) =====
                var limitVel = ps.limitVelocityOverLifetime;
                limitVel.enabled = true;
                limitVel.dampen = 0.09f;
                limitVel.limit = 45f;

                // ===== COLLISION (Weak Ricochet) =====
                var collision = ps.collision;
                collision.enabled = true;
                collision.type = ParticleSystemCollisionType.World;
                collision.bounce = 0.15f;
                collision.dampen = 0.85f;
                collision.lifetimeLoss = 0.3f;
                collision.quality = ParticleSystemCollisionQuality.Medium;

                // ===== TRAILS (Burning Streaks) =====
                // Curtos + inheritParticleColor: como alpha=0 no spawn, trail perto do cano é invisível
                var trails = ps.trails;
                trails.enabled = true;
                trails.ratio = 1.0f;
                trails.lifetime = new ParticleSystem.MinMaxCurve(0.04f, 0.08f);  // Bem curtos (4-8cm de rastro por frame)
                trails.minVertexDistance = 0.1f;
                trails.widthOverTrail = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));
                trails.inheritParticleColor = true;   // Herda alpha=0 perto do cano!
                trails.dieWithParticles = true;

                // ===== LIGHTS (Follow Sparks) =====
                var lights = ps.lights;
                lights.enabled = true;
                lights.ratio = 0.2f;
                lights.range = new ParticleSystem.MinMaxCurve(2f, 5f);
                lights.intensity = new ParticleSystem.MinMaxCurve(1f, 2f);
                lights.maxLights = 4;
                lights.useRandomDistribution = true;
                lights.sizeAffectsRange = false;
                lights.alphaAffectsIntensity = true;

                // Light Template — range e intensity REAIS (o módulo usa como base!)
                GameObject lightGO = new GameObject("SparkLightTemplate");
                lightGO.transform.SetParent(go.transform);
                Light lt = lightGO.AddComponent<Light>();
                lt.type = LightType.Point;
                lt.color = new Color(1.0f, 0.85f, 0.4f);
                lt.range = 3f;
                lt.intensity = 1f;
                lt.shadows = LightShadows.None;
                lt.enabled = false;    // Template fica desligado — só o módulo cria as luzes
                lights.light = lt;

                // ===== RENDERER =====
                // Billboard: cada partícula é um ponto na posição real — NADA vai para trás
                // (Stretch esticava centrado na posição, metade ia para trás)
                psr.renderMode = ParticleSystemRenderMode.Billboard;

                psr.material = _particleMaterial;
                psr.trailMaterial = _particleMaterial;

                _cachedPrefab = go;

                if (DebugMode) Plugin.LogSource.LogInfo("[DB] Prefab built OK.");
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[DB] BuildPrefab Error: {ex}");
            }
        }

        // ==================== SPARK TEXTURE ====================

        private static Texture2D GenerateSparkTexture()
        {
            int size = 32;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            float center = size / 2f;
            float maxDist = center;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    float t = Mathf.Clamp01(dist / maxDist);

                    float alpha = Mathf.Pow(1f - t, 3f);
                    float r = 1f;
                    float g = Mathf.Lerp(1f, 0.5f, t);
                    float b = Mathf.Lerp(0.9f, 0f, t);

                    tex.SetPixel(x, y, new Color(r, g, b, alpha));
                }
            }

            tex.Apply();
            return tex;
        }
    }
}