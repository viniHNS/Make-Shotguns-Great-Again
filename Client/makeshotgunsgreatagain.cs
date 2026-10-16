using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using makeshotgunsgreatagain.Patches;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace makeshotgunsgreatagain
{
    [BepInPlugin("com.vinihns.makeshotgunsgreatagain", "makeshotgunsgreatagain", "1.18.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        public static ConfigEntry<bool> CanResolveMalfunctionsWithoutInspection;
        public static ConfigEntry<bool> RemoveBossMalfunctions;

        // Dragon Breath configs
        public static ConfigEntry<int> DB_MaxParticles;
        public static ConfigEntry<int> DB_ParticleCount;
        public static ConfigEntry<float> DB_DestroyTimer;
        public static ConfigEntry<bool> DB_TrailsEnabled;
        public static ConfigEntry<bool> DB_CollisionEnabled;
        public static ConfigEntry<bool> DB_LightsEnabled;

        // KS-23 mount calibration configs
        public static ConfigEntry<bool> KS23_MountFixEnabled;
        public static ConfigEntry<float> KS23_MountPitch;
        public static ConfigEntry<float> KS23_MountYaw;
        public static ConfigEntry<bool> KS23_MountDebug;

        public static ConfigEntry<float> DB_SpreadAngle;
        public static ConfigEntry<float> DB_NoiseStrength;
        public static ConfigEntry<float> DB_NoiseFrequency;
        public static ConfigEntry<float> DB_Dampen;
        public static ConfigEntry<float> DB_StartOffset;

        // Debug: forced malfunction hotkeys
        public static ConfigEntry<bool> Debug_ForceMalfunctionKeys;
        private static (ConfigEntry<KeyboardShortcut> Key, Weapon.EMalfunctionState State)[] _forceMalfunctionKeys;
        public static ConfigEntry<bool> Debug_LogPelletSpread;

        private void Awake()
        {
            LogSource = Logger;

            CanResolveMalfunctionsWithoutInspection = Config.Bind(
                "Malfunctions",
                "Skip Inspection Before Clearing",
                true,
                "Allow clearing weapon malfunctions without inspecting the weapon first.");

            RemoveBossMalfunctions = Config.Bind(
                "Malfunctions",
                "Remove Boss Forced Malfunctions",
                true,
                "Prevent bosses from forcing a weapon malfunction on the player.");

           
            // 1. Toggles (Buttons/Checkboxes)
            DB_TrailsEnabled = Config.Bind(
                "Dragon Breath",
                "Trails Enabled",
                true,
                new ConfigDescription("Enable particle trails (burning streaks). Disable for better performance.",
                null,
                new ConfigurationManagerAttributes { Order = 100 }));

            DB_CollisionEnabled = Config.Bind(
                "Dragon Breath",
                "Collision Enabled",
                true,
                new ConfigDescription("Enable particle collision with walls (ricochet). Disable for better performance.",
                null, 
                new ConfigurationManagerAttributes { Order = 99 }));

            DB_LightsEnabled = Config.Bind(
                "Dragon Breath",
                "Lights Enabled",
                true,
                new ConfigDescription("Enable dynamic point lights on particles. Disable for better performance.",
                null, 
                new ConfigurationManagerAttributes { Order = 98 }));

            
            DB_MaxParticles = Config.Bind(
                "Dragon Breath",
                "Max Particles",
                400,
                new ConfigDescription(
                    "Maximum number of particles alive at the same time. Lower = better performance in full auto.",
                    new AcceptableValueRange<int>(50, 800),
                    new ConfigurationManagerAttributes { Order = 90 }));

            DB_ParticleCount = Config.Bind(
                "Dragon Breath",
                "Particles Per Shot",
                100,
                new ConfigDescription(
                    "Number of spark particles spawned per shot (main burst). Lower = better performance.",
                    new AcceptableValueRange<int>(20, 300),
                    new ConfigurationManagerAttributes { Order = 89 }));

            DB_DestroyTimer = Config.Bind(
                "Dragon Breath",
                "Effect Duration",
                4.0f,
                new ConfigDescription(
                    "Seconds before the effect GameObject is destroyed. Lower = less particle stacking.",
                    new AcceptableValueRange<float>(1.0f, 8.0f),
                    new ConfigurationManagerAttributes { Order = 88 }));

            DB_SpreadAngle = Config.Bind(
                "Dragon Breath",
                "Spread Angle",
                4.0f,
                new ConfigDescription("Angle of the particle cone.", 
                new AcceptableValueRange<float>(1f, 45f),
                new ConfigurationManagerAttributes { Order = 87 }));

            DB_NoiseStrength = Config.Bind(
               "Dragon Breath",
               "Noise Strength",
               6.0f,
               new ConfigDescription("Intensity of the turbulence/random movement.", 
               new AcceptableValueRange<float>(0f, 10f),
               new ConfigurationManagerAttributes { Order = 86 }));

            DB_NoiseFrequency = Config.Bind(
               "Dragon Breath",
               "Noise Frequency",
               5.0f,
               new ConfigDescription("Frequency of the turbulence.", 
               new AcceptableValueRange<float>(0f, 10f),
               new ConfigurationManagerAttributes { Order = 85 }));

            DB_Dampen = Config.Bind(
               "Dragon Breath",
               "Air Resistance",
               0.30f,
               new ConfigDescription("How much the particles slow down over time (0-1). Higher = more cloud-like.", 
               new AcceptableValueRange<float>(0f, 1f),
               new ConfigurationManagerAttributes { Order = 84 }));

            DB_StartOffset = Config.Bind(
               "Dragon Breath",
               "Start Offset",
               0.20f,
               new ConfigDescription("How far from the muzzle the effect starts (meters).", 
               new AcceptableValueRange<float>(0f, 1f),
               new ConfigurationManagerAttributes { Order = 83 }));

            KS23_MountFixEnabled = Config.Bind(
                "KS-23 Mount Calibration",
                "Enable Mount Alignment Fix",
                true,
                new ConfigDescription("Correct the KS-23 shot direction while aiming so optics on the rail mount hit where the reticle points.",
                null,
                new ConfigurationManagerAttributes { Order = 100 }));

            KS23_MountPitch = Config.Bind(
                "KS-23 Mount Calibration",
                "Shot Pitch Correction",
                0.311f,
                new ConfigDescription("Vertical shot correction in degrees while aiming the KS-23. Adjust at the shooting range until impacts match the reticle height. Applied per shot, changes take effect immediately.",
                new AcceptableValueRange<float>(-5f, 5f),
                new ConfigurationManagerAttributes { Order = 99 }));

            KS23_MountYaw = Config.Bind(
                "KS-23 Mount Calibration",
                "Shot Yaw Correction",
                0.31f,
                new ConfigDescription("Horizontal shot correction in degrees while aiming the KS-23. Adjust at the shooting range until impacts match the reticle horizontally. Applied per shot, changes take effect immediately.",
                new AcceptableValueRange<float>(-5f, 5f),
                new ConfigurationManagerAttributes { Order = 98 }));

            KS23_MountDebug = Config.Bind(
                "KS-23 Mount Calibration",
                "Debug Logging",
                false,
                new ConfigDescription("Log each applied shot correction to the BepInEx console.",
                null,
                new ConfigurationManagerAttributes { Order = 97 }));

            Debug_ForceMalfunctionKeys = Config.Bind(
                "Debug",
                "Enable Forced Malfunction Keys",
                false,
                new ConfigDescription("Enable the hotkeys below. Each one forces that malfunction on your next shot; press it again to cancel. Revolvers, break-action and one-shot weapons have no malfunctions in the game.",
                null,
                new ConfigurationManagerAttributes { Order = 100 }));

            _forceMalfunctionKeys = new[]
            {
                (BindForceMalfunctionKey("Force Misfire", KeyCode.Keypad1, 99), Weapon.EMalfunctionState.Misfire),
                (BindForceMalfunctionKey("Force Jam", KeyCode.Keypad2, 98), Weapon.EMalfunctionState.Jam),
                (BindForceMalfunctionKey("Force Hard Slide", KeyCode.Keypad3, 97), Weapon.EMalfunctionState.HardSlide),
                (BindForceMalfunctionKey("Force Soft Slide", KeyCode.Keypad4, 96), Weapon.EMalfunctionState.SoftSlide),
                (BindForceMalfunctionKey("Force Feed Failure", KeyCode.Keypad5, 95), Weapon.EMalfunctionState.Feed),
            };

            Debug_LogPelletSpread = Config.Bind(
                "Debug",
                "Log Pellet Spread",
                false,
                new ConfigDescription("Log the pellet spread of each of your multi-pellet shots to the BepInEx log: the value the game used and the cone measured on the pellets.",
                null,
                new ConfigurationManagerAttributes { Order = 90 }));

            // Rebuild prefab when any config changes
            DB_MaxParticles.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_ParticleCount.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_TrailsEnabled.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_CollisionEnabled.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_LightsEnabled.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_SpreadAngle.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_NoiseStrength.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_NoiseFrequency.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_Dampen.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();
            DB_StartOffset.SettingChanged += (_, __) => DragonBreathPatch.InvalidatePrefab();

            new DragonBreathPatch().Enable();
            new DragonBreathMuzzlePatch().Enable();
            new CanResolveMalfunctionsWithoutInspectionPatch().Enable();
            new RemoveBossMalfunctionsPatch().Enable();
            new BuckshotDispersionPatch().Enable();
            new PelletSpreadLogPatch().Enable();
            new KS23MountAlignmentPatch().Enable();
            new MP18SilencedSoundPatch().Enable();
            new ForceMalfunctionPatch().Enable();

            LogSource.LogInfo("plugin loaded!");
        }

        private ConfigEntry<KeyboardShortcut> BindForceMalfunctionKey(string name, KeyCode key, int order)
        {
            return Config.Bind(
                "Debug",
                name,
                new KeyboardShortcut(key),
                new ConfigDescription("Hotkey: the next shot gets this malfunction (needs \"Enable Forced Malfunction Keys\").",
                null,
                new ConfigurationManagerAttributes { Order = order }));
        }

        private void Update()
        {
            bool keysEnabled = Debug_ForceMalfunctionKeys.Value;
            ForceMalfunctionPatch.Tick(keysEnabled);
            if (!keysEnabled) return;

            // Only in raid or hideout, and never while typing (flea, trader,
            // split stack, chat), so the numpad keeps working as numbers.
            GameWorld gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null || gameWorld.MainPlayer == null || IsTypingInTextField()) return;

            foreach (var (key, state) in _forceMalfunctionKeys)
            {
                if (key.Value.IsDown())
                {
                    ForceMalfunctionPatch.Toggle(state);
                }
            }
        }

        private static bool IsTypingInTextField()
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null) return false;

            GameObject selected = eventSystem.currentSelectedGameObject;
            if (selected == null) return false;

            TMP_InputField tmpField = selected.GetComponent<TMP_InputField>();
            if (tmpField != null && tmpField.isFocused) return true;

            InputField field = selected.GetComponent<InputField>();
            return field != null && field.isFocused;
        }
    }

#pragma warning disable CS0649
    internal sealed class ConfigurationManagerAttributes
    {
        public bool? ShowRangeAsPercent;
        public System.Action<BepInEx.Configuration.ConfigEntryBase> CustomDrawer;
        public bool? ReadOnly;
        public bool? HideDefaultButton;
        public bool? HideSettingName;
        public string Description;
        public string DispName;
        public int? Order;
    }
#pragma warning restore CS0649
}
