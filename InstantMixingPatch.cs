using HarmonyLib;
using Il2CppScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleBoost
{
    public static class InstantMixSystem
    {
        public static List<MixingStation> mixingStations = new();
        public static List<MixingStationMk2> mixingStationsMk2 = new();
        public static Dictionary<MixingStation, int> lastNonZeroMS = new();
        public static Dictionary<MixingStationMk2, int> lastNonZeroMSMk2 = new();

        public static void Update()
        {
            foreach (var ms in mixingStations)
            {
                if (ms != null && ms.GetMixQuantity() > 0)
                    lastNonZeroMS[ms] = ms.GetMixQuantity();
            }

            foreach (var ms2 in mixingStationsMk2)
            {
                if (ms2 != null && ms2.GetMixQuantity() > 0)
                    lastNonZeroMSMk2[ms2] = ms2.GetMixQuantity();
            }
        }
    }

    // Harmony patches (kan ligga i samma fil eller egen fil)
    [HarmonyPatch(typeof(MixingStation), "Awake")]
    public class MixingStationAwakePatch
    {
        static void Postfix(MixingStation __instance)
        {
            __instance.MixTimePerItem = 1;
            __instance.MaxMixQuantity = 250;
            InstantMixSystem.mixingStations.Add(__instance);
        }
    }

    [HarmonyPatch(typeof(MixingStationMk2), "Awake")]
    public class MixingStationMk2AwakePatch
    {
        static void Postfix(MixingStationMk2 __instance)
        {
            __instance.MixTimePerItem = 1;
            __instance.MaxMixQuantity = 250;
            InstantMixSystem.mixingStationsMk2.Add(__instance);
        }
    }

    [HarmonyPatch(typeof(MixingStation), "MixingStart")]
    public class MixingStationMixingStartPatch
    {
        static bool Prefix(MixingStation __instance)
        {
            if (InstantMixSystem.lastNonZeroMS.TryGetValue(__instance, out int amount))
                __instance.CurrentMixTime = amount - 1;
            else
                __instance.CurrentMixTime = Mathf.Max(0, __instance.GetMixQuantity() - 1);
            return true;
        }
    }

    [HarmonyPatch(typeof(MixingStationMk2), "MixingStart")]
    public class MixingStationMk2MixingStartPatch
    {
        static bool Prefix(MixingStationMk2 __instance)
        {
            if (InstantMixSystem.lastNonZeroMSMk2.TryGetValue(__instance, out int amount))
                __instance.CurrentMixTime = amount - 1;
            else
                __instance.CurrentMixTime = Mathf.Max(0, __instance.GetMixQuantity() - 1);
            return true;
        }
    }
}
