using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.Diamond;

namespace ServerPatches.LoggingPatches
{
    [HarmonyPatch(typeof(CustomBattleServer), "BeforeStartingNextBattle")]
    public class PatchBeforeStartingNextBattle
    {
        //TODO: We still need to clean out the gameLogs array somehow. That array is going to continue growing forever which is a problem
        static int count = 1;
        public static bool Prefix(CustomBattleServer __instance, GameLog[] gameLogs)
        {
            if(Settings.AreStoringAddGameLogsMessagesToLogFile())
            {
                Logging.Rest.Info($"Begin AddGameLogsMessage Override {count}: ");
                foreach (var log in gameLogs)
                {
                    Logging.Rest.Info($"{log.Type} {log.Player} {log.GameTime} {log.GetDataAsString()}");
                }

                Logging.Rest.Info($"End AddGameLogsMessage Override {count}");

                count += 1;
            }

            return false;
        }
    }
}
