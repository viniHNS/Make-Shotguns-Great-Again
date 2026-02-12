using System.Reflection;
using HarmonyLib;
using SPT.Reflection.Patching;
using static EFT.InventoryLogic.Weapon;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// Forces IsKnownMalfType to return true so the player can clear
    /// a malfunction without inspecting the weapon first.
    /// </summary>
    internal class CanResolveMalfunctionsWithoutInspectionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(WeaponMalfunctionStateClass),
                nameof(WeaponMalfunctionStateClass.IsKnownMalfType));
        }

        [PatchPostfix]
        private static void Postfix(ref bool __result)
        {
            if (Plugin.CanResolveMalfunctionsWithoutInspection.Value)
            {
                __result = true;
            }
        }
    }
}
