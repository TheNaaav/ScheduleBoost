using HarmonyLib;

namespace ScheduleBoost
{
    [HarmonyPatch(typeof(Il2CppScheduleOne.Economy.Customer))]
    [HarmonyPatch("GetOfferSuccessChance")]
    public static class OfferChancePatch
    {
        public static bool Enabled = true;

        public static bool Prefix(ref float __result)
        {
            if (!Enabled) return true;
            __result = 1f;
            return false;
        }
    }
}
