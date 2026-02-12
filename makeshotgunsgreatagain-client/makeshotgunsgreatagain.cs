using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using makeshotgunsgreatagain.Patches;

namespace makeshotgunsgreatagain
{
    [BepInPlugin("com.vinihns.makeshotgunsgreatagain", "makeshotgunsgreatagain", "1.10.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        public static ConfigEntry<bool> CanResolveMalfunctionsWithoutInspection;
        public static ConfigEntry<bool> RemoveBossMalfunctions;

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

            new DragonBreathPatch().Enable();
            new DragonBreathMuzzlePatch().Enable();
            new CanResolveMalfunctionsWithoutInspectionPatch().Enable();
            new RemoveBossMalfunctionsPatch().Enable();

            LogSource.LogInfo("plugin loaded!");
        }
    }
}
