using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Voice;
using UnityEngine;
using YAPYAP;

namespace YapYapLocalMultiplayer
{
    [BepInAutoPlugin]
    public partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; } = null!;
        internal static ConfigEntry<string> VoiceAppID = null!;
        internal static ConfigEntry<KeyCode> ToggleNetworkUI = null!;
        private static string _voiceAppID = string.Empty;
        internal static string GetAppID
        {
            get
            {
                if (VoiceAppID == null || string.IsNullOrWhiteSpace(VoiceAppID.Value))
                {
                    Log.LogInfo($"Voice Server App ID replacement will be loaded from {VoiceAppPathID}");
                    if (TryGetAppID(out _voiceAppID))
                        Log.LogMessage($"Got Voice Server App ID of {_voiceAppID}");

                    return _voiceAppID;
                }
                else
                {
                    Log.LogInfo($"Voice Server App ID replacement will be loaded from configuration item - {VoiceAppID.Definition.Key}");
                    _voiceAppID = VoiceAppID.Value;
                    Log.LogMessage($"Got Voice Server App ID of {_voiceAppID}");
                    return _voiceAppID;
                }
            }
        }

        private static string _appIdFilePath = string.Empty;
        internal static string VoiceAppPathID
        {
            get
            {
                if (_appIdFilePath == string.Empty)
                {
                    _appIdFilePath = Path.Combine(@"%userprofile%\appdata\LocalLow\maisonbap\YAPYAP", "LocalMultiplayer");
                    _appIdFilePath = Environment.ExpandEnvironmentVariables(_appIdFilePath);
                }

                return _appIdFilePath;
            }
        }

        internal static UINetwork? UINetworkInstance { get; set; } = null;

        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Log.LogMessage($"Plugin {Name} is loaded!");
            VoiceAppID = Config.Bind("Settings", "Voice App ID", "", new ConfigDescription("The App ID of your custom Photon Voice Application\nChanging this mid-game will require a return to main menu for changes to take effect"));
            ToggleNetworkUI = Config.Bind("Settings", "Toggle NetworkDebug UI", KeyCode.Equals, new ConfigDescription("Set this to the key that you wish to use to toggle the Network Debug UI"));
        }

        private static bool TryGetAppID(out string appID)
        {
            appID = string.Empty;

            if (!Directory.Exists(VoiceAppPathID))
                Directory.CreateDirectory(VoiceAppPathID);

            if (!File.Exists(VoiceAppPathID + "\\AppIdVoice.txt"))
            {
                File.WriteAllText(VoiceAppPathID + "\\AppIdVoice.txt", string.Empty);
                return false;
            }

            appID = File.ReadAllText(VoiceAppPathID + "\\AppIdVoice.txt");
            return true;
        }

        [HarmonyPatch(typeof(YapFsm), nameof(YapFsm.Initialise))]
        public class FsmYapHook
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Stfld)
                    {
                        var field = codes[i].operand as FieldInfo;
                        if (field != null && (field.Name == "_initSteam" || field.Name == "_startHost"))
                        {
                            if (i > 0 && codes[i - 1].opcode == OpCodes.Ldc_I4_1)
                            {
                                codes[i - 1].opcode = OpCodes.Ldc_I4_0;
                            }
                        }
                    }
                }
                return codes;
            }
        }

        [HarmonyPatch(typeof(UINetwork), nameof(UINetwork.RefreshUI))]
        public class LetMeClickDebugPanel
        {
            public static void Postfix(UINetwork __instance)
            {
                UINetworkInstance = __instance;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        [HarmonyPatch(typeof(UIPlayer), nameof(UIPlayer.Update))]
        public class KeyListenerHook
        {
            public static void Postfix()
            {
                if (UINetworkInstance == null)
                    return;

                if (Input.GetKeyDown(ToggleNetworkUI.Value))
                {
                    UINetworkInstance.mainPanel.gameObject.SetActive(!UINetworkInstance.mainPanel.gameObject.activeSelf);
                }
            }
        }

        [HarmonyPatch(typeof(PhotonAppSettings), nameof(PhotonAppSettings.LoadOrCreateSettings))]
        public class BypassAudioAuthentication
        {
            public static bool Prefix()
            {
                string appID = GetAppID;
                if (string.IsNullOrWhiteSpace(appID))
                {
                    Log.LogWarning($"PHOTON VOICE APP ID NOT SET!\nSet your photon voice app ID via the Voice App ID config setting or at the global file {VoiceAppPathID}\\AppIdVoice.txt");
                    return true;
                }

                PhotonAppSettings.instance = ScriptableObject.CreateInstance<PhotonAppSettings>();
                PhotonAppSettings.instance.AppSettings = new Photon.Realtime.AppSettings()
                {
                    AppIdVoice = appID,
                    AuthMode = Photon.Realtime.AuthModeOption.Auth,
                    EnableProtocolFallback = true,
                    Protocol = ExitGames.Client.Photon.ConnectionProtocol.Udp
                };

                Log.LogMessage($"Photon Voice Server settings overwritten to use custom app id - {appID}");
                return false;
            }
        }
    }
}
