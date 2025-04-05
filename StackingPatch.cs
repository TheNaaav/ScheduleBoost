using HarmonyLib;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using ScheduleBoost.Config;

namespace ScheduleBoost.Patches
{
    [HarmonyPatch]
    public static class StackingPatch
    {
        [HarmonyPatch(typeof(ItemInstance), "get_StackLimit")]
        [HarmonyPostfix]
        private static void StackLimitPatch(ref int __result)
        {
            if (StackSettings.EnableCustomStacking)
                __result = StackSettings.CustomStackLimit;
        }

        [HarmonyPatch(typeof(MixingStation), "Start")]
        [HarmonyPrefix]
        private static bool MixQuantityPatch(MixingStation __instance)
        {
            if (StackSettings.EnableCustomStacking)
            {
                __instance.MixTimePerItem = 1;
                __instance.MaxMixQuantity = StackSettings.CustomStackLimit;
            }
            return true;
        }

        [HarmonyPatch(typeof(DryingRack), "Awake")]
        [HarmonyPrefix]
        private static bool DryingRackPatch(DryingRack __instance)
        {
            if (StackSettings.EnableCustomStacking)
                __instance.ItemCapacity = StackSettings.CustomStackLimit;
            return true;
        }
    }
}
