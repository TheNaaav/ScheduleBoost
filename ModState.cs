using MelonLoader;

namespace ScheduleBoost
{
    public static class ModState
    {
        public static bool InstantDeliveryEnabled = false;
        public static bool NoDebtEnabled = false;
        public static bool NoOrderLimit = false;


        private static MelonPreferences_Category category;
        private static MelonPreferences_Entry<bool> instantDeliveryEntry;
        private static MelonPreferences_Entry<bool> noDebtEntry;
        private static MelonPreferences_Entry<bool> noLimitEntry;

        public static void Load()
        {
            category = MelonPreferences.CreateCategory("ScheduleBoostSettings");
            instantDeliveryEntry = category.CreateEntry("InstantDelivery", false);
            noDebtEntry = category.CreateEntry("NoDebt", false);
            noLimitEntry = category.CreateEntry("NoOrderLimit", false);

            // Load values from saved settings
            InstantDeliveryEnabled = instantDeliveryEntry.Value;
            NoDebtEnabled = noDebtEntry.Value;
            NoOrderLimit = noLimitEntry.Value;
        }

        public static void Save()
        {
            instantDeliveryEntry.Value = InstantDeliveryEnabled;
            noDebtEntry.Value = NoDebtEnabled;
            noLimitEntry.Value = NoOrderLimit;

            category.SaveToFile();
        }
    }
}
