using HarmonyLib;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.UI;
using MelonLoader;
using System.Reflection;
using UnityEngine;

namespace ScheduleBoost
{
    public static class ConsoleUnlocker
    {
        private static bool isPatched = false;

        public static void ApplyPatch()
        {
            if (isPatched) return;
            isPatched = true;

            MelonLogger.Msg("[ConsoleUnlocker] Konsol patchad!");

            var harmony = new HarmonyLib.Harmony("com.scheduleboost.consoleunlock");

            var getter = AccessTools.PropertyGetter(typeof(Il2CppScheduleOne.UI.ConsoleUI), "IS_CONSOLE_ENABLED");
            var postfix = typeof(ConsoleUnlocker).GetMethod("ForceConsoleEnabled", BindingFlags.Static | BindingFlags.NonPublic);

            if (getter != null && postfix != null)
            {
                harmony.Patch(getter, postfix: new HarmonyMethod(postfix));
            }
        }

        private static void ForceConsoleEnabled(ref bool __result)
        {
            __result = true;
        }
    }

}
