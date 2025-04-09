using MelonLoader;

namespace ScheduleBoost
{
    public static class ModState
    {
        public static bool InstantDeliveryEnabled = false;
        public static bool NoDebtEnabled = false;
        public static bool NoOrderLimit = false;
        public static bool Offer100Enabled = false;            // 🔹 NY
        public static bool InstantMixingEnabled = true;
        public static bool SMSCounterOfferAlwaysAccepted = false;

        private static MelonPreferences_Category category;
        private static MelonPreferences_Entry<bool> instantDeliveryEntry;
        private static MelonPreferences_Entry<bool> noDebtEntry;
        private static MelonPreferences_Entry<bool> noLimitEntry;
        private static MelonPreferences_Entry<bool> offer100Entry;       // 🔹 NY
        private static MelonPreferences_Entry<bool> mixingEntry;
        private static MelonPreferences_Entry<bool> smsCounterEntry;
        public static void Load()
        {
            category = MelonPreferences.CreateCategory("ScheduleBoostSettings");
            instantDeliveryEntry = category.CreateEntry("InstantDelivery", false);
            noDebtEntry = category.CreateEntry("NoDebt", false);
            noLimitEntry = category.CreateEntry("NoOrderLimit", false);
            offer100Entry = category.CreateEntry("Offer100", false);             // 🔹 NY
            mixingEntry = category.CreateEntry("InstantMixing", true);
            smsCounterEntry = category.CreateEntry("SMSCounterOfferAlwaysAccepted", false);

            // Load values from saved settings
            InstantDeliveryEnabled = instantDeliveryEntry.Value;
            NoDebtEnabled = noDebtEntry.Value;
            NoOrderLimit = noLimitEntry.Value;
            Offer100Enabled = offer100Entry.Value;
            InstantMixingEnabled = mixingEntry.Value;
            SMSCounterOfferAlwaysAccepted = smsCounterEntry.Value; // 👈 Ny
        }

        public static void Save()
        {
            instantDeliveryEntry.Value = InstantDeliveryEnabled;
            noDebtEntry.Value = NoDebtEnabled;
            noLimitEntry.Value = NoOrderLimit;
            offer100Entry.Value = Offer100Enabled;             // 🔹 NY
            mixingEntry.Value = InstantMixingEnabled;
            smsCounterEntry.Value = SMSCounterOfferAlwaysAccepted; // 👈 Ny
            category.SaveToFile();
        }
    }
}
