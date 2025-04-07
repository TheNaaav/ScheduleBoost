using Il2CppScheduleOne.Economy;
using UnityEngine;

namespace ScheduleBoost.Systems
{
    public static class DeliveryTweaks
    {
        private static Supplier[] cachedSuppliers;
        private static float refreshTimer = 0f;
        private static float refreshInterval = 3f; // uppdatera var tredje sekund

        public static void Update()
        {
            if (!ModState.InstantDeliveryEnabled && !ModState.NoDebtEnabled)
                return;

            refreshTimer -= Time.deltaTime;

            if (cachedSuppliers == null || refreshTimer <= 0f)
            {
                cachedSuppliers = UnityEngine.Object.FindObjectsOfType<Supplier>();
                refreshTimer = refreshInterval;
            }

            foreach (var supplier in cachedSuppliers)
            {
                if (ModState.InstantDeliveryEnabled && supplier.minsUntilDeaddropReady > 0)
                    supplier.minsUntilDeaddropReady = 0;

                if (ModState.NoDebtEnabled && supplier.Debt > 0f)
                    supplier.ChangeDebt(-supplier.Debt);
            }
        }
    }
}
