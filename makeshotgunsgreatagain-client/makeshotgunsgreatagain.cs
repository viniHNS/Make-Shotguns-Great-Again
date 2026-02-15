using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using makeshotgunsgreatagain.Patches;

namespace makeshotgunsgreatagain
{
    [BepInPlugin("com.vinihns.makeshotgunsgreatagain", "makeshotgunsgreatagain", "1.11.1")]
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

        public static ConfigEntry<float> DB_SpreadAngle;
        public static ConfigEntry<float> DB_NoiseStrength;
        public static ConfigEntry<float> DB_NoiseFrequency;
        public static ConfigEntry<float> DB_Dampen;
        public static ConfigEntry<float> DB_StartOffset;

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

            LogSource.LogInfo("plugin loaded!");
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
