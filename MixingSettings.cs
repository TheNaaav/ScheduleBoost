namespace ScheduleBoost.Config
{
    public static class MixingSettings
    {
        public static bool EnableInstantMixing
        {
            get => ModState.InstantMixingEnabled;
            set => ModState.InstantMixingEnabled = value;
        }
    }
}
