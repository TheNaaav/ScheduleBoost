using HarmonyLib;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Packaging;
using UnityEngine;
using Il2CppScheduleOne.UI.Handover;
using Il2CppSystem.Collections.Generic;
using Il2CppScheduleOne.ItemFramework;
using static Il2CppScheduleOne.UI.Handover.HandoverScreen;
using static MelonLoader.MelonLogger;

namespace ScheduleBoost
{
    [HarmonyPatch]
    public static class OfferChancePatch
    {
        public static bool Enabled = true;

        // NPC trade (walk-up interaction)
        [HarmonyPatch(typeof(Customer), nameof(Customer.GetOfferSuccessChance))]
        [HarmonyPrefix]
        public static bool Prefix_GetOfferSuccessChance(ref float __result)
        {
            if (!Enabled) return true;
            __result = 1f;
            return false;
        }

        [HarmonyPatch(typeof(Il2CppScheduleOne.Economy.Customer), "EvaluateCounteroffer")]
        public static class EvaluateCounterofferPatch
        {
            public static void Postfix(Customer __instance,ref bool __result)
            {
                if (!ModState.SMSCounterOfferAlwaysAccepted) return;

                __result = true;
            }
        }
    }
}
