using System.Reflection;
using EFT.HealthSystem;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// Prevents bosses from forcing a weapon malfunction on the player
    /// by skipping ActiveHealthController.AddMisfireEffect entirely.
    /// </summary>
    internal class RemoveBossMalfunctionsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(ActiveHealthController),
                nameof(ActiveHealthController.AddMisfireEffect));
        }

        [PatchPrefix]
        private static bool Prefix()
        {
            // Return false to skip the original method when config is enabled
            return !Plugin.RemoveBossMalfunctions.Value;
        }
    }

    /// <summary>
    /// Covers the forced malfunction that skips AddMisfireEffect:
    /// ActiveHealthController.DoEventEffect (Halloween Zryachiy event and panic map triggers)
    /// adds the misfire effect with no time limit. Removes only the misfire and keeps the panic effect.
    /// </summary>
    internal class RemoveEventMalfunctionsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(ActiveHealthController),
                nameof(ActiveHealthController.DoEventEffect));
        }

        [PatchPostfix]
        private static void Postfix(ActiveHealthController __instance)
        {
            if (Plugin.RemoveBossMalfunctions.Value)
            {
                __instance.RemoveMisfireEffect();
            }
        }
    }
}
