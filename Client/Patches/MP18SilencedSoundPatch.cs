using System;
using System.Reflection;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// The vanilla MP-18 prefab has its "mp18_main_silenced" sound bank in the
    /// Doublet field (only used by multi-barrel shots) and BodySilenced empty,
    /// so a suppressed MP-18 plays the unsuppressed sound. Moves the bank to
    /// BodySilenced before the setter falls back to Body.
    /// </summary>
    internal class MP18SilencedSoundPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.PropertySetter(typeof(WeaponSoundPlayer), nameof(WeaponSoundPlayer.IsSilenced));
        }

        [PatchPrefix]
        private static void Prefix(WeaponSoundPlayer __instance)
        {
            try
            {
                if (__instance == null) return;
                if (__instance.BodySilenced != null || __instance.Doublet == null) return;
                if (!__instance.Doublet.name.EndsWith("_silenced", StringComparison.Ordinal)) return;

                __instance.BodySilenced = __instance.Doublet;
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[MP18SilencedSound] Patch error: {ex}");
            }
        }
    }
}
