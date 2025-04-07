using MelonLoader;

namespace ScheduleBoost.Config
{
    public static class StackSettings
    {
        public static bool EnableCustomStacking = true;
        public static int CustomStackLimit = 250;

        private static MelonPreferences_Category category;
        private static MelonPreferences_Entry<bool> stackingEntry;
        private static MelonPreferences_Entry<int> limitEntry;

        public static void Load()
        {
            category = MelonPreferences.CreateCategory("StackSettings");
            stackingEntry = category.CreateEntry("EnableCustomStacking", true);
            limitEntry = category.CreateEntry("CustomStackLimit", 250);

            EnableCustomStacking = stackingEntry.Value;
            CustomStackLimit = limitEntry.Value;
        }

        public static void Save()
        {
            stackingEntry.Value = EnableCustomStacking;
            limitEntry.Value = CustomStackLimit;

            category.SaveToFile();
        }
    }
}
