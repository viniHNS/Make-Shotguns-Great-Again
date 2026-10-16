using System;
using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.Communications;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace makeshotgunsgreatagain.Patches
{
    /// <summary>
    /// Debug tool: a hotkey arms a malfunction type and the local player's next
    /// shot gets it. Every fire operation that can malfunction asks
    /// FirearmController.GetMalfunctionState once per shot, so the postfix swaps
    /// the rolled result, keeping the same weapon/magazine requirements the game
    /// uses when it picks a type.
    /// </summary>
    internal class ForceMalfunctionPatch : ModulePatch
    {
        private static Weapon.EMalfunctionState _armed = Weapon.EMalfunctionState.None;

        // Context of the armed type: it only applies to the same weapon in the
        // same world (raid or hideout), so it can't leak to another gun or raid.
        private static string _armedWeaponId;
        private static GameWorld _armedWorld;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(Player.FirearmController),
                nameof(Player.FirearmController.GetMalfunctionState));
        }

        /// <summary>
        /// Arms the given type for the next shot. Pressing the key of the armed
        /// type again cancels it.
        /// </summary>
        public static void Toggle(Weapon.EMalfunctionState state)
        {
            try
            {
                if (_armed == state)
                {
                    Clear();
                    Notify($"Forced malfunction cancelled: {state}");
                    return;
                }

                Weapon weapon = GetMainPlayerFirearm()?.Item;
                if (weapon == null)
                {
                    Warn("Forced malfunction: hold a firearm first.");
                    return;
                }

                string reason = GetUnsupportedReason(weapon, state);
                if (reason != null)
                {
                    Warn($"Can't force {state} on {weapon.ShortName.Localized()}: {reason}");
                    return;
                }

                _armed = state;
                _armedWeaponId = weapon.Id;
                _armedWorld = Singleton<GameWorld>.Instance;
                Notify($"Next shot: {state}");
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[ForceMalfunction] Toggle error: {ex}");
            }
        }

        /// <summary>
        /// Called every frame by the plugin. Drops the armed type when the keys
        /// are switched off, or when the player no longer holds the weapon it was
        /// armed on (weapon swap, death, end of raid, hideout to raid). Weapons
        /// with no malfunctions never reach the postfix, so this is what keeps
        /// the type from waiting forever.
        /// </summary>
        public static void Tick(bool keysEnabled)
        {
            if (_armed == Weapon.EMalfunctionState.None) return;

            try
            {
                if (!keysEnabled)
                {
                    Plugin.LogSource.LogInfo($"[ForceMalfunction] Forced {_armed} discarded: keys disabled");
                    Clear();
                    return;
                }

                if (!IsArmedContext(GetMainPlayerFirearm()))
                {
                    Discard("weapon changed or raid ended");
                }
            }
            catch (Exception ex)
            {
                Clear();
                Plugin.LogSource.LogError($"[ForceMalfunction] Tick error: {ex}");
            }
        }

        [PatchPostfix]
        private static void Postfix(
            Player.FirearmController __instance,
            bool hasAmmoInMag,
            bool doesWeaponHaveBoltCatch,
            bool isMagazineInserted,
            ref Weapon.EMalfunctionState __result,
            ref Weapon.EMalfunctionSource malfunctionSource)
        {
            try
            {
                if (_armed == Weapon.EMalfunctionState.None) return;
                if (__instance == null || !ReferenceEquals(__instance, GetMainPlayerFirearm())) return;

                if (!IsArmedContext(__instance))
                {
                    Discard("weapon changed or raid ended");
                    return;
                }

                Weapon.EMalfunctionState state = _armed;
                Clear();

                string reason = GetUnsupportedReason(__instance.Item, state)
                    ?? GetShotStateReason(state, hasAmmoInMag, doesWeaponHaveBoltCatch, isMagazineInserted);
                if (reason != null)
                {
                    Warn($"Forced {state} skipped: {reason}");
                    return;
                }

                __result = state;
                malfunctionSource = Weapon.EMalfunctionSource.ConsoleCommand;
                Plugin.LogSource.LogInfo($"[ForceMalfunction] Forced {state} on {__instance.Item.ShortName.Localized()}");
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"[ForceMalfunction] Patch error: {ex}");
            }
        }

        private static bool IsArmedContext(Player.FirearmController firearm)
        {
            Weapon weapon = firearm != null ? firearm.Item : null;
            GameWorld world = Singleton<GameWorld>.Instance;
            return weapon != null && weapon.Id == _armedWeaponId
                && world != null && ReferenceEquals(world, _armedWorld);
        }

        private static void Discard(string reason)
        {
            Weapon.EMalfunctionState state = _armed;
            Clear();
            Warn($"Forced {state} discarded: {reason}");
        }

        private static void Clear()
        {
            _armed = Weapon.EMalfunctionState.None;
            _armedWeaponId = null;
            _armedWorld = null;
        }

        private static Player.FirearmController GetMainPlayerFirearm()
        {
            GameWorld gameWorld = Singleton<GameWorld>.Instance;
            Player player = gameWorld != null ? gameWorld.MainPlayer : null;
            return player != null ? player.HandsController as Player.FirearmController : null;
        }

        // Weapon-level checks: fire operations that never roll a malfunction,
        // and the per-type Allow* flags of the weapon template.
        private static string GetUnsupportedReason(Weapon weapon, Weapon.EMalfunctionState state)
        {
            if (weapon is Revolver || weapon.ReloadMode == Weapon.EReloadMode.OnlyBarrel
                || weapon.IsOneOff || weapon.IsFlareGun || weapon is RocketLauncher)
            {
                return "this weapon type has no malfunctions in the game";
            }

            switch (state)
            {
                case Weapon.EMalfunctionState.Misfire:
                    return weapon.AllowMisfire ? null : "the weapon does not allow misfires";
                case Weapon.EMalfunctionState.Jam:
                    return weapon.AllowJam ? null : "the weapon does not allow jams";
                case Weapon.EMalfunctionState.Feed:
                    return weapon.AllowFeed ? null : "the weapon does not allow feed failures";
                case Weapon.EMalfunctionState.SoftSlide:
                case Weapon.EMalfunctionState.HardSlide:
                    return weapon.AllowSlide ? null : "the weapon does not allow slide malfunctions";
                default:
                    return "unknown malfunction type";
            }
        }

        // Shot-level checks, mirroring FirearmController.GetSpecificMalfunctionVariants.
        // HardSlide normally also needs durability <= 5%; that part is skipped on purpose.
        private static string GetShotStateReason(Weapon.EMalfunctionState state, bool hasAmmoInMag, bool hasBoltCatch, bool isMagazineInserted)
        {
            switch (state)
            {
                case Weapon.EMalfunctionState.Jam:
                    return hasAmmoInMag || !hasBoltCatch || !isMagazineInserted ? null : "last round with bolt catch (the bolt locks back instead)";
                case Weapon.EMalfunctionState.Feed:
                    return hasAmmoInMag && isMagazineInserted ? null : "needs a round left in the magazine";
                case Weapon.EMalfunctionState.SoftSlide:
                case Weapon.EMalfunctionState.HardSlide:
                    return hasAmmoInMag ? null : "needs a round left in the magazine";
                default:
                    return null;
            }
        }

        private static void Notify(string message)
        {
            Plugin.LogSource.LogInfo($"[ForceMalfunction] {message}");
            NotificationManager.DisplayMessageNotification(message);
        }

        private static void Warn(string message)
        {
            Plugin.LogSource.LogWarning($"[ForceMalfunction] {message}");
            NotificationManager.DisplayWarningNotification(message);
        }
    }
}
