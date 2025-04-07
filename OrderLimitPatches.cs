using HarmonyLib;
using Il2CppScheduleOne.UI.Phone;

namespace ScheduleBoost.Patches
{
    [HarmonyPatch]
    public class OrderLimitPatches
    {
        [HarmonyPatch(typeof(PhoneShopInterface), nameof(PhoneShopInterface.CanConfirmOrder))]
        [HarmonyPrefix]
        static bool CanConfirmOrderPatch(ref bool __result)
        {
            if (ModState.NoOrderLimit)
            {
                __result = true;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(PhoneShopInterface), nameof(PhoneShopInterface.GetOrderTotal))]
        [HarmonyPostfix]
        static void GetOrderTotalPatch(ref int itemCount)
        {
            if (ModState.NoOrderLimit)
            {
                itemCount = 0;
            }
        }
    }
}
