using System;
using System.Reflection;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// The game builds the fireport list of a multi-barrel weapon from two fixed
    /// nodes (fireport_l, fireport_r) and FirearmController.CurrentFireport indexes
    /// it by the chamber that fired, so a weapon with a third chamber throws on
    /// the third shot. When the barrel also has a fireport_t node (Chiappa Triple
    /// Crown) this postfix appends it as the third entry. Two-barrel weapons have
    /// no such node and are left untouched.
    /// </summary>
    internal class MultiBarrelFireportsPatch : ModulePatch
    {
        private const string TopFireportName = "fireport_t";

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(PlayerBones), nameof(PlayerBones.FindMultiBarrelsFireports));
        }

        [PatchPostfix]
        private static void Postfix(PlayerBones __instance, ref BifacialTransform[] __result)
        {
            try
            {
                // null: not a multi-barrel weapon. Anything but the two stock
                // entries means someone else already changed the list.
                if (__result == null || __result.Length != 2) return;

                Transform root = __instance.WeaponRoot != null ? __instance.WeaponRoot.Original : null;
                if (root == null) return;

                Transform top = TransformTools.FindTransformRecursive(root, TopFireportName, false);
                if (top == null) return;

                BifacialTransform topFireport = new BifacialTransform { Original = top };
                __result = new[] { __result[0], __result[1], topFireport };

                // The game reads the field it stored, not only the return value.
                __instance.MultiBarrelsFireports = __result;
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[MultiBarrelFireports] {ex}");
            }
        }
    }
}
