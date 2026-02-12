using System;
using System.Reflection;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace makeshotgunsgreatagain.Patches
{
    internal class DragonBreathMuzzlePatch : ModulePatch
    {
        private static Transform _cachedFireport;
        private static int _cachedInstanceId = -1;
        private static FieldInfo _hierarchyField;

        protected override MethodBase GetTargetMethod()
        {
            _hierarchyField = typeof(MuzzleManager).GetField("Hierarchy",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return AccessTools.Method(typeof(MuzzleManager), nameof(MuzzleManager.Shot));
        }

        [PatchPostfix]
        private static void Postfix(MuzzleManager __instance)
        {
            try
            {
                if (!DragonBreathPatch.IsDragonBreathShot) return;
                DragonBreathPatch.IsDragonBreathShot = false;

                Transform fireport = GetFireport(__instance);

                if (fireport == null)
                {
                    if (DragonBreathPatch.DebugMode)
                        Plugin.LogSource.LogWarning("[DB] Could not find 'fireport' in Hierarchy!");
                    return;
                }

                if (DragonBreathPatch.DebugMode)
                    Plugin.LogSource.LogInfo($"[DB] Spawning at fireport: '{fireport.name}' pos={fireport.position}");

                DragonBreathPatch.SpawnEffect(fireport);
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[DB] MuzzlePatch Error: {ex}");
            }
        }

        private static Transform GetFireport(MuzzleManager manager)
        {
            int id = manager.GetInstanceID();

            if (id == _cachedInstanceId && _cachedFireport != null)
                return _cachedFireport;

            if (_hierarchyField == null) return null;

            Transform hierarchy = _hierarchyField.GetValue(manager) as Transform;
            if (hierarchy == null)
            {
                if (DragonBreathPatch.DebugMode)
                    Plugin.LogSource.LogWarning("[DB] MuzzleManager.Hierarchy is null!");
                return null;
            }

            if (DragonBreathPatch.DebugMode)
                Plugin.LogSource.LogInfo($"[DB] Searching in Hierarchy: '{hierarchy.name}'");

            // Find the fireport transform in the weapon hierarchy
            Transform[] children = hierarchy.GetComponentsInChildren<Transform>();
            Transform fireport = null;

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name == "fireport")
                    fireport = children[i];
            }

            if (fireport != null)
            {
                _cachedFireport = fireport;
                _cachedInstanceId = id;
                if (DragonBreathPatch.DebugMode)
                    Plugin.LogSource.LogInfo($"[DB] Found 'fireport' at pos={fireport.position}");
                return fireport;
            }

            if (DragonBreathPatch.DebugMode)
            {
                Plugin.LogSource.LogWarning($"[DB] 'fireport' not found! Dumping children of '{hierarchy.name}':");
                for (int i = 0; i < children.Length; i++)
                    Plugin.LogSource.LogInfo($"[DB]   -> {children[i].name}");
            }

            return null;
        }
    }
}
