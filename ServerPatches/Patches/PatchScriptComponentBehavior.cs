using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;

namespace ServerPatches.Patches
{
    class PatchScriptComponentBehavior
    {
        public static bool Prefix(ScriptComponentBehavior __instance, ScriptComponentBehavior.TickRequirement value)
        {
            // From the memory dumps, it seems there is a race condition where the ManagedScriptHolder instance in SpawnedItemEntity is null.
            // Unclear if it is "safe" to skip this if it is null, but will crash otherwise
            ManagedScriptHolder managedScriptHolder = Traverse.Create(__instance).Field("ManagedScriptHolder").GetValue() as ManagedScriptHolder;
            if (managedScriptHolder == null)
            {
                Logging.Instance.Error("PatchScriptComponentBehavior: ScriptComponentBehavior.ManagedScriptHolder was null");
                return false;
            }

            return true;
        }
    }
}
