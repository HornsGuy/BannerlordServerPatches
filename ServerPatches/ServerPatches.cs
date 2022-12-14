using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using ServerPatches.Patches;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.Diamond.Rest;
using ServerPatches.LoggingPatches;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.Diamond;

namespace ServerPatches
{
    public class ServerPatches : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Setup();
        }

        private void LoadSettings()
        {
            Settings.LoadSettings("ServerPatches\\settings.json");
        }

        private void Setup()
        {
            LoadSettings();

            Logging.Instance.StartLogging("ServerPatches\\Logs", LoggingInstance.LogLevel.Info, Settings.GetNumberOfLogsToKeep());
            Logging.Rest.StartLogging("ServerPatches\\RestLogs", LoggingInstance.LogLevel.Info,Settings.GetNumberOfRestLogsToKeep());

            Harmony harmony = new Harmony("ServerPatches");

            harmony.PatchAll();       

            HarmonyHelper.AddPrefix(harmony, typeof(MissionLobbyComponent), "SendPeerInformationsToPeer", BindingFlags.NonPublic | BindingFlags.Instance, typeof(PatchMissionLobbyComponent_SendPeerInformationsToPeer), "Prefix");
            HarmonyHelper.AddPrefix(harmony, typeof(MissionNetworkComponent), "SendSpawnedMissionObjectsToPeer", BindingFlags.NonPublic | BindingFlags.Instance, typeof(PatchMissionNetworkComponent), "Prefix");
        }

    }
}
