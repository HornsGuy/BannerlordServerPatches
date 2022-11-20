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

namespace ServerPatches
{
    public class ServerPatches : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Setup();
        }

        private void Setup()
        {
            Logging.Instance.StartLogging("ServerPatches", Logging.LogLevel.Info);

            Harmony harmony = new Harmony("ServerPatches");

            HarmonyHelper.AddPrefix(harmony, typeof(MissionLobbyComponent), "SendPeerInformationsToPeer", BindingFlags.NonPublic | BindingFlags.Instance, typeof(PatchMissionLobbyComponent_SendPeerInformationsToPeer), "Prefix");
            HarmonyHelper.AddPrefix(harmony, typeof(MissionNetworkComponent), "SendSpawnedMissionObjectsToPeer", BindingFlags.NonPublic | BindingFlags.Instance, typeof(PatchMissionNetworkComponent), "Prefix");
            HarmonyHelper.AddPrefix(harmony, typeof(MissionCustomGameServerComponent), "OnPlayerKills", BindingFlags.NonPublic | BindingFlags.Instance, typeof(PatchMissionCustomGameServerComponent), "Prefix");
        }

    }
}
