using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace ServerPatches.Patches
{
    public class PatchMissionCustomGameServerComponent
    {
        static bool hitOnce = false;
        public static bool Prefix(MissionCustomGameServerComponent __instance, MissionPeer killerPeer, Agent killedAgent, MissionPeer assistorPeer)
        {
            if (!hitOnce)
            {
                Logging.Instance.Info("PatchMissionCustomGameServerComponent.Prefix has been hit once");
                hitOnce = true;
            }

            // Call parent
            //base.OnPlayerKills(killerPeer, killedAgent, assistorPeer);
            MissionLobbyComponent mlcInstance = (MissionLobbyComponent)__instance;
            HarmonyHelper.CallMethod(mlcInstance, "OnPlayerKills", new object[] { killerPeer, killedAgent, assistorPeer });

            
            PlayerId id = killerPeer.Peer.Id;
            //BattleResult currentBattleResult = this._battleResult.GetCurrentBattleResult();
            MultipleBattleResult _battleResult = Traverse.Create(__instance).Field("_battleResult").GetValue() as MultipleBattleResult;
            BattleResult currentBattleResult = _battleResult.GetCurrentBattleResult();

            //if (__instance._warmupEnded)            
            bool _warmupEnded = (bool)Traverse.Create(__instance).Field("_warmupEnded").GetValue();
            if (_warmupEnded)
            {
                currentBattleResult.PlayerEntries[id].PlayerStats.Kills = killerPeer.KillCount;
            }
            if (killerPeer != null && killedAgent != null && killedAgent.IsHuman)
            {
                GameLog gameLog = new GameLog(GameLogType.Kill, killerPeer.Peer.Id, MBCommon.GetTotalMissionTime());
                Dictionary<string, string> data = gameLog.Data;
                string key = "IsFriendly";
                Team team = killerPeer.Team;
                BattleSideEnum? battleSideEnum = (team != null) ? new BattleSideEnum?(team.Side) : null;
                BattleSideEnum? battleSideEnum2;
                if (killedAgent == null)
                {
                    battleSideEnum2 = null;
                }
                else
                {
                    Team team2 = killedAgent.Team;
                    battleSideEnum2 = ((team2 != null) ? new BattleSideEnum?(team2.Side) : null);
                }
                BattleSideEnum? battleSideEnum3 = battleSideEnum2;
                data.Add(key, (battleSideEnum.GetValueOrDefault() == battleSideEnum3.GetValueOrDefault() & battleSideEnum != null == (battleSideEnum3 != null)).ToString());
                if (killedAgent.MissionPeer != null)
                {
                    gameLog.Data.Add("Victim", killedAgent.MissionPeer.Peer.Id.ToString());
                }
                if (assistorPeer != null)
                {
                    gameLog.Data.Add("Assist", assistorPeer.Peer.Id.ToString());
                }
                //if (this._warmupEnded && this._gameMode.GetMissionType() == MissionLobbyComponent.MultiplayerGameType.Siege)
                _warmupEnded = (bool)Traverse.Create(__instance).Field("_warmupEnded").GetValue();
                MissionMultiplayerGameModeBase _gameMode = Traverse.Create(__instance).Field("_gameMode").GetValue() as MissionMultiplayerGameModeBase;
                if (_warmupEnded && _gameMode.GetMissionType() == MissionLobbyComponent.MultiplayerGameType.Siege)
                {
                    Agent controlledAgent = killerPeer.ControlledAgent;
                    //if (((controlledAgent != null) ? controlledAgent.CurrentlyUsedGameObject : null) != null)
                    if (controlledAgent != null && controlledAgent.CurrentlyUsedGameObject != null)
                    {
                        BattlePlayerStatsSiege battlePlayerStatsSiege = currentBattleResult.PlayerEntries[killerPeer.Peer.Id].PlayerStats as BattlePlayerStatsSiege;
                        int siegeEngineKills = battlePlayerStatsSiege.SiegeEngineKills;
                        battlePlayerStatsSiege.SiegeEngineKills = siegeEngineKills + 1;
                    }
                }
                //this._gameLogger.GameLogs.Add(gameLog);
                MultiplayerGameLogger _gameLogger = Traverse.Create(__instance).Field("_gameLogger").GetValue() as MultiplayerGameLogger;
                _gameLogger.GameLogs.Add(gameLog);
            }

            //this._customBattleServer.UpdateBattleStats(this._battleResult.GetCurrentBattleResult(), this._teamScores, this._playerScores);
            CustomBattleServer _customBattleServer = Traverse.Create(__instance).Field("_customBattleServer").GetValue() as CustomBattleServer;
            Dictionary<int,int> _teamScores = Traverse.Create(__instance).Field("_teamScores").GetValue() as Dictionary<int, int>;
            Dictionary<PlayerId,int> _playerScores = Traverse.Create(__instance).Field("_playerScores").GetValue() as Dictionary<PlayerId, int>;
            currentBattleResult = _battleResult.GetCurrentBattleResult();

            _customBattleServer.UpdateBattleStats(currentBattleResult, _teamScores, _playerScores);





            return false;
        }
    }
}
