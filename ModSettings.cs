using MelonLoader;

namespace ScheduleBoost
{
    public static class ModSettings
    {
        private static MelonPreferences_Category category;

        public static void Setup()
        {
            category = MelonPreferences.CreateCategory("ScheduleBoost", "Schedule Boost Settings");

            category.CreateEntry("InstantDeliveryEnabled", false, "Instant Delivery");
            category.CreateEntry("NoDebtEnabled", false, "No Debt");
            category.CreateEntry("NoOrderLimit", false, "No Order Limit");

            category.SaveToFile(false); // spara direkt
        }

        public static bool InstantDeliveryEnabled
        {
            get => category.GetEntry<bool>("InstantDeliveryEnabled").Value;
            set => category.GetEntry<bool>("InstantDeliveryEnabled").Value = value;
        }

        public static bool NoDebtEnabled
        {
            get => category.GetEntry<bool>("NoDebtEnabled").Value;
            set => category.GetEntry<bool>("NoDebtEnabled").Value = value;
        }

        public static bool NoOrderLimit
        {
            get => category.GetEntry<bool>("NoOrderLimit").Value;
            set => category.GetEntry<bool>("NoOrderLimit").Value = value;
        }

        public static void Save() => category.SaveToFile(false);
    }
}
