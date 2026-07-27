using System;
using System.Reflection;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// Applies a calibrated pitch/yaw correction to the KS-23's shot vector
    /// while aiming, fixing its off-reticle optics zeroing.
    /// </summary>
    internal class KS23MountAlignmentPatch : ModulePatch
    {
        private const string KS23_TPL = "5e848cc2988a8701445df1e8";

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), "InitiateShot");
        }

        [PatchPrefix]
        private static void Prefix(Player.FirearmController __instance, ref Vector3 shotDirection)
        {
            try
            {
                if (!Plugin.KS23_MountFixEnabled.Value) return;
                if (__instance == null) return;
                if (__instance.Item == null) return;
                if (__instance.Item.TemplateId != KS23_TPL) return;
                if (!__instance.IsAiming) return;

                float pitch = Plugin.KS23_MountPitch.Value;
                float yaw = Plugin.KS23_MountYaw.Value;
                if (pitch == 0f && yaw == 0f) return;

                Transform fireport = null;
                if (__instance.CurrentFireport != null)
                {
                    fireport = __instance.CurrentFireport.Original;
                }
                Vector3 right = fireport != null ? fireport.right : Vector3.right;

                Vector3 yawAxis = Vector3.Cross(shotDirection, right);
                if (yawAxis.sqrMagnitude < 0.0001f) yawAxis = Vector3.up;
                yawAxis.Normalize();

                Vector3 corrected = Quaternion.AngleAxis(yaw, yawAxis)
                                  * Quaternion.AngleAxis(pitch, right)
                                  * shotDirection;

                if (Plugin.KS23_MountDebug.Value)
                {
                    Plugin.LogSource.LogInfo(
                        $"[KS23] Shot correction applied: pitch={pitch}° yaw={yaw}° | {shotDirection} -> {corrected}");
                }

                shotDirection = corrected;
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[KS23] Patch error: {ex}");
            }
        }
    }
}
