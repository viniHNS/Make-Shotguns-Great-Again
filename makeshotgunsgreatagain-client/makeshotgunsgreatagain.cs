using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using makeshotgunsgreatagain.Patches;

namespace makeshotgunsgreatagain
{
    // first string below is your plugin's GUID, it MUST be unique to any other mod. Read more about it in BepInEx docs. Be sure to update it if you copy this project.
    [BepInPlugin("com.vinihns.makeshotgunsgreatagain", "makeshotgunsgreatagain", "1.10.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        public static ConfigEntry<bool> CanResolveMalfunctionsWithoutInspection;
        public static ConfigEntry<bool> RemoveBossMalfunctions;

        // BaseUnityPlugin inherits MonoBehaviour, so you can use base unity functions like Awake() and Update()
        private void Awake()
        {
            // save the Logger to public static field so we can use it elsewhere in the project
            LogSource = Logger;

            // Config
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

            // Patches
            new DragonBreathPatch().Enable();
            new DragonBreathMuzzlePatch().Enable();
            new CanResolveMalfunctionsWithoutInspectionPatch().Enable();
            new RemoveBossMalfunctionsPatch().Enable();

            LogSource.LogInfo("plugin loaded!");
        }
    }
}

