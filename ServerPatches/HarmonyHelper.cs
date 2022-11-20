using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace ServerPatches
{
    public static class HarmonyHelper
    {
        public static void AddPrefix(Harmony harmony, Type classToPatch, string functionToPatchName, BindingFlags flags, Type patchClass, string functionPatchName)
        {
            var functionToPatch = classToPatch.GetMethod(functionToPatchName, flags);
            var newHarmonyPatch = patchClass.GetMethod(functionPatchName);
            harmony.Patch(functionToPatch, prefix: new HarmonyMethod(newHarmonyPatch));
        }

        public static void CallMethod(object obj, string method, object[] arguments)
        {
            Traverse.Create(obj).Method(method, arguments);
        }
    }
}
