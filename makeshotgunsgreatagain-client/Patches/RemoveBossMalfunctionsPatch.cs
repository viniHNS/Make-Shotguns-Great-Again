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
}
