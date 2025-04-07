using HarmonyLib;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI.Phone;
using Il2CppScheduleOne.ItemFramework;
using MelonLoader;

namespace ScheduleBoost.Patches
{
    [HarmonyPatch(typeof(Supplier), "DeaddropConfirmed")]
    public class Patch_DeaddropConfirmed
    {
        static void Prefix(Supplier __instance, Il2CppSystem.Collections.Generic.List<PhoneShopInterface.CartEntry> cart)
        {
            if (!ModState.InstantDeliveryEnabled)
                return;

            foreach (var entry in cart)
            {
                var listing = entry.Listing;
                var def = listing?.Item;
                int quantity = entry.Quantity;

                if (def != null)
                {
                    ItemInstance instance = def.GetDefaultInstance(quantity);
                    PlayerInventory.Instance.AddItemToInventory(instance);

                    // ✅ Logg varje gång ett item ges
                    MelonLogger.Msg($"[ScheduleBoost] Instant delivery: +{quantity}x {def.Name}");
                }
                else
                {
                    MelonLogger.Warning("[ScheduleBoost] Listing.Item was null.");
                }
            }

            cart.Clear(); // Undvik duplicering i dead drop
        }
    }
}
