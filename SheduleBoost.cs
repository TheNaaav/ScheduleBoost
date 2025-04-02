using MelonLoader;
using ScheduleBoost;
using ScheduleBoost.GUI;

[assembly: MelonInfo(typeof(ScheduleBoostMod.ScheduleBoostMod), "ScheduleBoost", "0.5", "Naruebet")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ScheduleBoostMod
{
    public class ScheduleBoostMod : MelonMod
    {
        private static readonly ScheduleBoostGUI GUIInstance = new();

        public override void OnInitializeMelon()
        {
            GUIInstance.OnInitialize();
            HarmonyLib.Harmony harmony = new("com.scheduleboost");
            harmony.PatchAll();
            MelonLogger.Msg("ScheduleBoost initialized");
        }

        public override void OnUpdate()
        {
            GUIInstance.OnUpdate();
            InstantMixSystem.Update();
            ConsoleUnlocker.ApplyPatch();
        }

        public override void OnGUI() => GUIInstance.OnGUI();
    }
}