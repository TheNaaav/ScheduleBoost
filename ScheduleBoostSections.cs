using UnityEngine;
using MelonLoader;
using static Il2CppScheduleOne.Console;
using ScheduleBoost.Data;
using ScheduleBoost.Config;
using static Il2CppSystem.Guid;
using static Il2CppScheduleOne.Map.ScheduledMaterialChange;

namespace ScheduleBoost.GUI
{
    public class ScheduleBoostSections
    {
        private bool offerChanceEnabled = true;
        private int selectedCategoryIndex = 0;
        private int selectedItemIndex = 0;
        private string selectedCategory = "";
        private string selectedItem = "";

        public void DrawAddXPSection()
        {
            GUILayout.Label("Add XP:");

            if (GUILayout.Button("Add 500 XP", GUILayout.Width(140)))
                ExecuteXPCommand(500);

            if (GUILayout.Button("Add 1000 XP", GUILayout.Width(140)))
                ExecuteXPCommand(1000);

            if (GUILayout.Button("Add 5000 XP", GUILayout.Width(140)))
                ExecuteXPCommand(5000);

            GUILayout.Space(10);
        }

        public void DrawOfferToggleSection()
        {

            ModState.Offer100Enabled = GUILayout.Toggle(ModState.Offer100Enabled, "Offer 100% Chance", GUILayout.Width(140));

            ModState.SMSCounterOfferAlwaysAccepted = GUILayout.Toggle(ModState.SMSCounterOfferAlwaysAccepted, "SMS Counter Always Accept", GUILayout.Width(140));

            GUILayout.Space(10);
        }

        public void DrawInfoLabels()
        {
            GUILayout.Space(10);
            GUILayout.Label("Stack Limit Settings");

            bool newEnableStack = GUILayout.Toggle(StackSettings.EnableCustomStacking, "Enable Custom Stacking");

            GUILayout.Label($"Current Limit: {StackSettings.CustomStackLimit}");
            int newLimit = (int)GUILayout.HorizontalSlider(StackSettings.CustomStackLimit, 20, 1000);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-100", GUILayout.Width(50))) newLimit = Mathf.Max(newLimit - 100, 20);
            if (GUILayout.Button("-50", GUILayout.Width(40))) newLimit = Mathf.Max(newLimit - 50, 20);
            if (GUILayout.Button("-25", GUILayout.Width(40))) newLimit = Mathf.Max(newLimit - 25, 20);
            if (GUILayout.Button("-10", GUILayout.Width(40))) newLimit = Mathf.Max(newLimit - 10, 20);
            if (GUILayout.Button("-1", GUILayout.Width(40))) newLimit = Mathf.Max(newLimit - 1, 20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+1", GUILayout.Width(40))) newLimit = Mathf.Min(newLimit + 1, 1000);
            if (GUILayout.Button("+10", GUILayout.Width(40))) newLimit = Mathf.Min(newLimit + 10, 1000);
            if (GUILayout.Button("+25", GUILayout.Width(40))) newLimit = Mathf.Min(newLimit + 25, 1000);
            if (GUILayout.Button("+50", GUILayout.Width(40))) newLimit = Mathf.Min(newLimit + 50, 1000);
            if (GUILayout.Button("+100", GUILayout.Width(50))) newLimit = Mathf.Min(newLimit + 100, 1000);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("20", GUILayout.Width(50))) newLimit = 20;
            if (GUILayout.Button("50", GUILayout.Width(50))) newLimit = 50;
            if (GUILayout.Button("100", GUILayout.Width(50))) newLimit = 100;
            if (GUILayout.Button("250", GUILayout.Width(50))) newLimit = 250;
            if (GUILayout.Button("500", GUILayout.Width(50))) newLimit = 500;
            if (GUILayout.Button("750", GUILayout.Width(50))) newLimit = 750;
            if (GUILayout.Button("1000", GUILayout.Width(50))) newLimit = 1000;
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            if (GUILayout.Button("Reset to Default (20)"))
            {
                newLimit = 20;
            }

            // Save if something changed
            if (newEnableStack != StackSettings.EnableCustomStacking || newLimit != StackSettings.CustomStackLimit)
            {
                StackSettings.EnableCustomStacking = newEnableStack;
                StackSettings.CustomStackLimit = newLimit;
                StackSettings.Save();
            }
        }

        public void DrawSaveSection()
        {
            GUILayout.Label("Save Game:");
            if (GUILayout.Button("Save Game Forces", GUILayout.Width(140)))
                ExecuteSaveCommand("SaveGame");
        }

        private void ExecuteXPCommand(int xp)
        {
            try
            {
                var command = new GiveXP();
                var args = new Il2CppSystem.Collections.Generic.List<string>();
                args.Add(xp.ToString());
                command.Execute(args);
                MelonLogger.Msg($"[ScheduleBoost] addxp {xp} executed!");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ScheduleBoost] Failed to add XP: {ex.Message}");
            }
        }

        private void ExecuteSaveCommand(string saveName)
        {
            try
            {
                var command = new Save();
                var args = new Il2CppSystem.Collections.Generic.List<string>();
                args.Add(saveName);
                command.Execute(args);
                MelonLogger.Msg($"[ScheduleBoost] savegame {saveName} executed!");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ScheduleBoost] Failed to save game: {ex.Message}");
            }
        }

        public void DrawGiveItemSection()
        {
            GUILayout.Label("Give Item:");

            // CATEGORY BUTTONS
            foreach (var category in ScheduleBoostItemData.ItemCategories.Keys)
            {
                GUIStyle categoryStyle = new GUIStyle(UnityEngine.GUI.skin.button);
                if (category == selectedCategory)
                {
                    categoryStyle.normal.textColor = Color.white;
                    categoryStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.6f, 0.2f)); // green
                }

                if (GUILayout.Button(category, categoryStyle))
                {
                    // Toggle category
                    if (selectedCategory == category)
                    {
                        selectedCategory = "";
                        selectedItem = "";
                    }
                    else
                    {
                        selectedCategory = category;
                        selectedItem = "";
                    }
                }
            }

            // ITEM BUTTONS (if category selected)
            if (!string.IsNullOrEmpty(selectedCategory) &&
                ScheduleBoostItemData.ItemCategories.TryGetValue(selectedCategory, out var items))
            {
                GUILayout.Space(10);
                GUILayout.Label($"Items in {selectedCategory}:");

                foreach (var item in items)
                {
                    GUIStyle itemStyle = new GUIStyle(UnityEngine.GUI.skin.button);
                    if (item == selectedItem)
                    {
                        itemStyle.normal.textColor = Color.white;
                        itemStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.4f, 0.8f)); // blue
                    }

                    if (GUILayout.Button(item, itemStyle))
                        selectedItem = item;
                }

                GUILayout.Space(10);
                GUILayout.Label("Select Amount:");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("x1")) GiveItem(selectedItem, 1);
                if (GUILayout.Button("x10")) GiveItem(selectedItem, 10);
                if (GUILayout.Button("x20")) GiveItem(selectedItem, 20);
                if (GUILayout.Button("x250")) GiveItem(selectedItem, 250);
                GUILayout.EndHorizontal();
            }
        }

        private void GiveItem(string itemID, int amount)
        {
            try
            {
                var command = new Il2CppScheduleOne.Console.AddItemToInventoryCommand();
                var args = new Il2CppSystem.Collections.Generic.List<string>();
                args.Add(itemID);
                args.Add(amount.ToString()); // Convert amount to string
                command.Execute(args);
                MelonLogger.Msg($"[ScheduleBoost] Gave {amount}x {itemID}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ScheduleBoost] Failed to give item: {ex.Message}");
            }
        }

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pixels);
            result.Apply();
            return result;
        }

        public void DrawTeleportSection()
        {
            GUILayout.Label("Teleport To:");

            string[] teleportLocations = new string[]
            {
                "Motel", "Townhall", "Bungalow", "Sweatshop",
                "Docks", "Northtown", "Rv", "Carwash",
                "Postoffice", "Laundromat", "Tacoticklers", "Barn"
            };

            int columns = 2;
            int total = teleportLocations.Length;
            int rows = Mathf.CeilToInt((float)total / columns);

            foreach (var location in teleportLocations)
            {
                if (GUILayout.Button(location))
                {
                    ExecuteTeleport(location);
                }
            }
        }

        private void ExecuteTeleport(string location)
        {
            try
            {
                var command = new Il2CppScheduleOne.Console.Teleport();
                var args = new Il2CppSystem.Collections.Generic.List<string>();
                args.Add(location);
                command.Execute(args);
                MelonLogger.Msg($"[ScheduleBoost] Teleported to {location}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ScheduleBoost] Failed to teleport: {ex.Message}");
            }
        }

        private void TryTeleportTo(string location)
        {
            try
            {
                var command = new Il2CppScheduleOne.Console.Teleport();
                var args = new Il2CppSystem.Collections.Generic.List<string>();
                args.Add(location);
                command.Execute(args);
                MelonLogger.Msg($"[ScheduleBoost] Teleported to {location}");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ScheduleBoost] Failed to teleport: {ex.Message}");
            }
        }

        public void DrawMixingSection()
        {
            GUILayout.Label("Mixing Settings", GUILayout.Width(180));

            bool newToggle = GUILayout.Toggle(MixingSettings.EnableInstantMixing, "Enable Instant Mixing");
            if (newToggle != MixingSettings.EnableInstantMixing)
            {
                MixingSettings.EnableInstantMixing = newToggle;
                ModState.Save();
            }

            if (!MixingSettings.EnableInstantMixing)
                GUILayout.Label("Warning: Mixing will be slow again!", GUILayout.Width(180));
        }

        public void DrawTrashSection()
        {
            GUILayout.Space(10);
            GUILayout.Label("Cleanup Tools", GUILayout.Width(140));

            if (GUILayout.Button("Clear Trash", GUILayout.Width(140)))
            {
                try
                {
                    var command = new Il2CppScheduleOne.Console.ClearTrash();
                    var args = new Il2CppSystem.Collections.Generic.List<string>();
                    args.Add("cleartrash");
                    command.Execute(args);

                    MelonLogger.Msg("[ScheduleBoost] Cleared trash using 'cleartrash' command.");
                }
                catch (System.Exception ex)
                {
                    MelonLogger.Error($"[ScheduleBoost] Failed to clear trash: {ex.Message}");
                }
            }
        }

        public void DrawDeliveryOptions()
        {
            GUILayout.Label("Delivery Options", GUILayout.Width(140));

            bool newInstant = GUILayout.Toggle(ModState.InstantDeliveryEnabled, "Instant Delivery");
            bool newDebt = GUILayout.Toggle(ModState.NoDebtEnabled, "No Debt");
            bool newLimit = GUILayout.Toggle(ModState.NoOrderLimit, "No Order Limit");

            if (newInstant != ModState.InstantDeliveryEnabled || newDebt != ModState.NoDebtEnabled || newLimit != ModState.NoOrderLimit)
            {
                ModState.InstantDeliveryEnabled = newInstant;
                ModState.NoDebtEnabled = newDebt;
                ModState.NoOrderLimit = newLimit;
                ModState.Save(); // Spara direkt vid ändring
            }

        }

    }
}
