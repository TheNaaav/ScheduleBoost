using HarmonyLib;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;

namespace ScheduleBoost.Patches
{
    [HarmonyPatch]
    public static class StackingPatch
    {
        [HarmonyPatch(typeof(ItemInstance), "get_StackLimit")]
        [HarmonyPostfix]
        private static void StackLimitPatch(ref int __result)
        {
            __result = 250;
        }

        [HarmonyPatch(typeof(MixingStation), "Start")]
        [HarmonyPrefix]
        private static bool MixQuantityPatch(MixingStation __instance)
        {
            __instance.MixTimePerItem = 1;
            __instance.MaxMixQuantity = 250;
            return true;
        }

        [HarmonyPatch(typeof(DryingRack), "Awake")]
        [HarmonyPrefix]
        private static bool DryingRackPatch(DryingRack __instance)
        {
            __instance.ItemCapacity = 250;
            return true;
        }
    }
}
