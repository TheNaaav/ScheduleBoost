using MelonLoader;
using ScheduleBoost;
using ScheduleBoost.Config;
using ScheduleBoost.GUI;
using ScheduleBoost.Patches;
using ScheduleBoost.Systems;

[assembly: MelonInfo(typeof(ScheduleBoostMod.ScheduleBoostMod), "ScheduleBoost", "0.5.4", "Naruebet")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ScheduleBoostMod
{
    public class ScheduleBoostMod : MelonMod
    {
        private static readonly ScheduleBoostGUI GUIInstance = new();

        public override void OnInitializeMelon()
        {
            ModState.Load();
            StackSettings.Load();

            GUIInstance.OnInitialize();
            new HarmonyLib.Harmony("com.scheduleboost").PatchAll();
            MelonLogger.Msg("ScheduleBoost initialized");
        }


        public override void OnUpdate()
        {
            GUIInstance.OnUpdate();
            InstantMixSystem.Update();
            ConsoleUnlocker.ApplyPatch();
            DeliveryTweaks.Update();
        }

        public override void OnGUI() => GUIInstance.OnGUI();
    }
}