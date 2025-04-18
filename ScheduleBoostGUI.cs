﻿using UnityEngine;
using MelonLoader;

namespace ScheduleBoost.GUI
{
    public class ScheduleBoostGUI
    {
        private Rect windowRect = new(60f, 60f, 420f, 450f);
        private bool showGUI = false;
        private bool showWelcome = true;
        private float welcomeTimer = 5f;
        private Vector2 scrollPosition = Vector2.zero;
        private enum GUIPage { Main, GiveItem, Stack, Teleport}
        private GUIPage currentPage = GUIPage.Main;

        private readonly ScheduleBoostSections sections = new();

        public void OnInitialize()
        {
            MelonLogger.Msg("ScheduleBoostGUI loaded");
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                showGUI = !showGUI;

            if (showWelcome)
            {
                welcomeTimer -= Time.deltaTime;
                if (welcomeTimer <= 0f)
                    showWelcome = false;
            }
        }

        public void OnGUI()
        {
            if (showWelcome)
            {
                GUIStyle welcomeStyle = new(UnityEngine.GUI.skin.label)
                {
                    fontSize = 32,
                    normal = { textColor = Color.red },
                    alignment = TextAnchor.MiddleCenter
                };
                UnityEngine.GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 125, 400, 50), "Made by Nav (F1 for open menu!)", welcomeStyle);
            }

            if (!showGUI) return;

            windowRect = UnityEngine.GUI.Window(0, windowRect, (UnityEngine.GUI.WindowFunction)DrawWindow, "ScheduleBoost by Nav");
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.BeginVertical();

            // Tab Buttons
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Main")) currentPage = GUIPage.Main;
            if (GUILayout.Button("Item")) currentPage = GUIPage.GiveItem;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stack")) currentPage = GUIPage.Stack;
            if (GUILayout.Button("Teleport")) currentPage = GUIPage.Teleport;
            GUILayout.EndHorizontal();


            GUILayout.Space(10);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(390), GUILayout.Height(360));
            if (currentPage == GUIPage.Main)
            {
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical(GUILayout.Width(180));
            
                sections.DrawOfferToggleSection();
                sections.DrawMixingSection();
                sections.DrawDeliveryOptions();

                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(180));

                sections.DrawSaveSection();
                sections.DrawTrashSection();
                sections.DrawAddXPSection();

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

            }
            else if (currentPage == GUIPage.GiveItem)
            {
                sections.DrawGiveItemSection();
            }
            else if (currentPage == GUIPage.Stack)
            {
                sections.DrawInfoLabels();
            }
            else if (currentPage == GUIPage.Teleport)
            {
                sections.DrawTeleportSection();
            }
                GUILayout.EndScrollView();

            GUILayout.EndVertical();

            UnityEngine.GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }
}
