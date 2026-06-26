using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace VanguardMod
{
    [BepInPlugin("com.vanguardmod.forgepatch", "VG Forge Streamliner", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ConfigEntry<KeyCode> ToggleKey;

        private void Awake()
        {
            ToggleKey = Config.Bind("General", "ToggleKey", KeyCode.F10,
                "Key to show/hide the Forge Streamliner debug panel.");

            new Harmony("com.vanguardmod.forgepatch").PatchAll();
            gameObject.AddComponent<DebugMenu>();
            Logger.LogInfo("VG Forge Streamliner loaded.");
        }
    }
}
