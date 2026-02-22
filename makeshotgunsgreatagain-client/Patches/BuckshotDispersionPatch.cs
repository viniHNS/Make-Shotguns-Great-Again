using System.Reflection;
using SPT.Reflection.Patching;
using HarmonyLib;
using EFT;

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
        private static void Prefix(AmmoItemClass ammo)
        {
            if (ammo == null) return;
            if (ammo.ProjectileCount <= 1) return;

            if (ammo.buckshotDispersion < 0.01f)
            {
                ammo.buckshotDispersion = FALLBACK_DISPERSION;
            }
        }
    }
}
