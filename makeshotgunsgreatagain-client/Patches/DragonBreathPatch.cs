using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace makeshotgunsgreatagain.Patches
{

    internal class DragonBreathPatch : ModulePatch
    {
        private const string DragonBreathAmmoId = "698924bf6dcd41ac313f5921";
        internal static bool DebugMode = false;
        internal static bool IsDragonBreathShot = false;

        private static GameObject _cachedPrefab;
        private static Material _particleMaterial;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(Player.FirearmController),
                nameof(Player.FirearmController.InitiateShot)
            );
        }

        // Prefix: flag must be set before InitiateShot calls MuzzleManager.Shot
        [PatchPrefix]
        private static void Prefix(Player.FirearmController __instance, AmmoItemClass ammo)
        {
            try
            {
                if (__instance == null || ammo == null) return;

                if (DebugMode) Plugin.LogSource.LogInfo($"[DB] Shot fired. Ammo: {ammo.TemplateId}");

                IsDragonBreathShot = (ammo.TemplateId == DragonBreathAmmoId);
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[DB] Patch Error: {ex}");
            }
        }

        internal static void SpawnEffect(Transform muzzle)
        {
            try
            {
                if (_cachedPrefab == null) BuildPrefab();
                if (_cachedPrefab == null) return;

                if (DebugMode)
                    Plugin.LogSource.LogInfo($"[DB] Muzzle axes — forward={muzzle.forward} | -up={-muzzle.up}");

                // Barrel direction is -up on Tarkov fireport transforms
                Vector3 dir = -muzzle.up;
                if (dir.sqrMagnitude < 0.01f) dir = muzzle.forward;
                Quaternion rot = Quaternion.LookRotation(dir);

                GameObject fx = GameObject.Instantiate(_cachedPrefab, muzzle.position, rot);
                if (fx != null)
                {
                    fx.SetActive(true);
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

        private static void BuildPrefab()
        {
            try
            {
                if (_cachedPrefab != null) return;

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
                        Plugin.LogSource.LogError("[DB] CRITICAL: No shader found!");
                        return;
                    }

                    _particleMaterial = new Material(shader);
                    _particleMaterial.mainTexture = GenerateSparkTexture();

                    if (_particleMaterial.HasProperty("_TintColor"))
                        _particleMaterial.SetColor("_TintColor", new Color(1f, 0.7f, 0.3f, 1f));
                    if (_particleMaterial.HasProperty("_Color"))
                        _particleMaterial.SetColor("_Color", new Color(1f, 0.7f, 0.3f, 1f));
                }

                GameObject go = new GameObject("DragonBreath_FX");
                GameObject.DontDestroyOnLoad(go);
                go.SetActive(false);

                ParticleSystem ps = go.AddComponent<ParticleSystem>();
                ParticleSystemRenderer psr = go.GetComponent<ParticleSystemRenderer>();

                // Main
                var main = ps.main;
                main.duration = 0.5f;
                main.loop = false;
                main.playOnAwake = true;
                main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1.2f);
                main.startSpeed = new ParticleSystem.MinMaxCurve(150f, 250f);
                main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
                main.gravityModifier = 0.6f;
                main.simulationSpace = ParticleSystemSimulationSpace.World;
                main.maxParticles = 400;

                // Color: white-hot -> orange -> ember
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
                        new GradientAlphaKey(0f, 0f),
                        new GradientAlphaKey(1f, 0.03f),
                        new GradientAlphaKey(0.6f, 0.8f),
                        new GradientAlphaKey(0f, 1f)
                    }
                );

                var col = ps.colorOverLifetime;
                col.enabled = true;
                col.color = new ParticleSystem.MinMaxGradient(colorGrad);

                // Size over lifetime (shrink)
                var sol = ps.sizeOverLifetime;
                sol.enabled = true;
                sol.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0.1f));

                // Emission
                var emission = ps.emission;
                emission.enabled = true;
                emission.rateOverTime = 0;
                emission.SetBursts(new ParticleSystem.Burst[]
                {
                    new ParticleSystem.Burst(0.0f, 120, 180),
                    new ParticleSystem.Burst(0.05f, 30, 50)
                });

                // Shape (cone)
                var shape = ps.shape;
                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.Cone;
                shape.angle = 3f;
                shape.radius = 0.01f;
                shape.position = new Vector3(0f, 0f, 0.25f);

                // Noise
                var noise = ps.noise;
                noise.enabled = true;
                noise.strength = 1.2f;
                noise.frequency = 1.5f;
                noise.scrollSpeed = 3f;
                noise.quality = ParticleSystemNoiseQuality.Medium;

                // Velocity limit (air drag)
                var limitVel = ps.limitVelocityOverLifetime;
                limitVel.enabled = true;
                limitVel.dampen = 0.09f;
                limitVel.limit = 45f;

                // Collision
                var collision = ps.collision;
                collision.enabled = true;
                collision.type = ParticleSystemCollisionType.World;
                collision.bounce = 0.15f;
                collision.dampen = 0.85f;
                collision.lifetimeLoss = 0.3f;
                collision.quality = ParticleSystemCollisionQuality.Medium;

                // Trails
                var trails = ps.trails;
                trails.enabled = true;
                trails.ratio = 1.0f;
                trails.lifetime = new ParticleSystem.MinMaxCurve(0.04f, 0.08f);
                trails.minVertexDistance = 0.1f;
                trails.widthOverTrail = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));
                trails.inheritParticleColor = true;
                trails.dieWithParticles = true;

                // Lights
                var lights = ps.lights;
                lights.enabled = true;
                lights.ratio = 0.2f;
                lights.range = new ParticleSystem.MinMaxCurve(2f, 5f);
                lights.intensity = new ParticleSystem.MinMaxCurve(1f, 2f);
                lights.maxLights = 4;
                lights.useRandomDistribution = true;
                lights.sizeAffectsRange = false;
                lights.alphaAffectsIntensity = true;

                GameObject lightGO = new GameObject("SparkLightTemplate");
                lightGO.transform.SetParent(go.transform);
                Light lt = lightGO.AddComponent<Light>();
                lt.type = LightType.Point;
                lt.color = new Color(1.0f, 0.85f, 0.4f);
                lt.range = 3f;
                lt.intensity = 1f;
                lt.shadows = LightShadows.None;
                lt.enabled = false;
                lights.light = lt;

                // Renderer
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