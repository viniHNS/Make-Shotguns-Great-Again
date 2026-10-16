using System;
using System.Collections.Generic;
using System.Reflection;
using Comfort.Common;
using SPT.Reflection.Patching;
using HarmonyLib;
using EFT;
using EFT.Ballistics;
using UnityEngine;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// Forces buckshotDispersion on ammo with ProjectileCount > 1
    /// when fired from non-shotgun weapons (e.g. assault rifles with buckshot ammo).
    /// Without this, assault rifle barrels have 0 ShotgunDispersion, causing
    /// all pellets to hit the exact same point.
    /// </summary>
    internal class BuckshotDispersionPatch : ModulePatch
    {
        private const float FALLBACK_DISPERSION = 3.0f;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), "InitiateShot");
        }

        [PatchPrefix]
        private static void Prefix(EFT.InventoryLogic.Ammo ammo)
        {
            if (ammo == null) return;
            if (ammo.ProjectileCount <= 1) return;

            if (ammo.buckshotDispersion < 0.01f)
            {
                ammo.buckshotDispersion = FALLBACK_DISPERSION;
            }
        }
    }

    /// <summary>
    /// Debug: logs the pellet spread of each multi-pellet shot of the local
    /// player, both the value the game used and the cone measured on the
    /// pellets it created.
    /// </summary>
    internal class PelletSpreadLogPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BallisticsCalculator), nameof(BallisticsCalculator.CreateMultiProjectileShot));
        }

        [PatchPostfix]
        private static void Postfix(EFT.InventoryLogic.Ammo ammo, Vector3 direction, List<Shot> preallocatedShots, string playerProfileID, EFT.InventoryLogic.Item weapon)
        {
            try
            {
                if (!Plugin.Debug_LogPelletSpread.Value) return;
                if (ammo == null || preallocatedShots == null || preallocatedShots.Count == 0) return;

                GameWorld gameWorld = Singleton<GameWorld>.Instance;
                Player player = gameWorld != null ? gameWorld.MainPlayer : null;
                if (player == null || player.ProfileId != playerProfileID) return;

                float max = 0f, sum = 0f;
                foreach (Shot shot in preallocatedShots)
                {
                    float angle = Vector3.Angle(direction, shot.Direction);
                    max = Mathf.Max(max, angle);
                    sum += angle;
                }
                float cone = Mathf.Atan(ammo.buckshotDispersion / 100f) * Mathf.Rad2Deg;

                string weaponInfo = "";
                if (weapon is EFT.InventoryLogic.Weapon gun)
                {
                    weaponInfo = $" | weapon template {gun.Template.ShotgunDispersion}, base {gun.ShotgunDispersionBase:0.###}, " +
                                 $"accuracy delta {gun.CenterOfImpactDelta:0.###}, total {gun.TotalShotgunDispersion:0.###}";
                }

                Plugin.LogSource.LogInfo(
                    $"[Pellets] {weapon?.ShortName.Localized()} ({weapon?.StringTemplateId}) + {ammo.ShortName.Localized()}: " +
                    $"{preallocatedShots.Count} pellets, dispersion {ammo.buckshotDispersion:0.###} (cone {cone:0.###} deg), " +
                    $"measured max {max:0.###} deg, mean {sum / preallocatedShots.Count:0.###} deg{weaponInfo}");
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[Pellets] Log error: {ex}");
            }
        }
    }
}
