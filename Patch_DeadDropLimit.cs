using HarmonyLib;
using Il2CppScheduleOne.Economy;

namespace ScheduleBoost.Patches
{
    [HarmonyPatch(typeof(Supplier), nameof(Supplier.GetDeadDropLimit))]
    public class Patch_DeadDropLimit
    {
        static bool Prefix(ref float __result)
        {
            if (ModState.NoOrderLimit)
            {
                __result = 99999f;
                return false; // Skippa originalmetoden
            }
            return true; // Kör originalet om inte aktiv
        }
    }

}
