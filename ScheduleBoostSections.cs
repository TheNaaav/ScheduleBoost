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
            if (GUILayout.Button(offerChanceEnabled ? "Disable Offer 100%" : "Enable Offer 100%", GUILayout.Width(140)))
            {
                offerChanceEnabled = !offerChanceEnabled;
                OfferChancePatch.Enabled = offerChanceEnabled;
                MelonLogger.Msg("[ScheduleBoost] Offer Chance Patch " + (offerChanceEnabled ? "enabled" : "disabled"));
            }

            GUILayout.Space(10);
        }

        public void DrawInfoLabels()
        {
            GUILayout.Space(10);
            GUILayout.Label("Stack Limit Settings", GUILayout.Width(140));

            bool newEnableStack = GUILayout.Toggle(StackSettings.EnableCustomStacking, "Enable Custom Stacking");

            GUILayout.Label($"Current Limit: {StackSettings.CustomStackLimit}", GUILayout.Width(140));
            int newLimit = (int)GUILayout.HorizontalSlider(StackSettings.CustomStackLimit, 20, 1000, GUILayout.Width(140));

            if (newEnableStack != StackSettings.EnableCustomStacking || newLimit != StackSettings.CustomStackLimit)
            {
                StackSettings.EnableCustomStacking = newEnableStack;
                StackSettings.CustomStackLimit = newLimit;
                StackSettings.Save();
            }

            if (GUILayout.Button("Reset to Default (20)", GUILayout.Width(140)))
            {
                StackSettings.CustomStackLimit = 20;
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
                if (GUILayout.Button(location, GUILayout.Width(140)))
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
            MixingSettings.EnableInstantMixing = GUILayout.Toggle(MixingSettings.EnableInstantMixing, "Enable Instant Mixing");

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
