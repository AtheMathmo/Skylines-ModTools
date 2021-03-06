﻿using System;
using UnityEngine;

namespace ModTools
{
    public class SceneExplorerColorConfig : GUIWindow
    {

        private static Configuration config
        {
            get { return ModTools.Instance.config; }
        }

        private string[] availableFonts;
        private int selectedFont;

        public SceneExplorerColorConfig() : base("Font/ color configuration", new Rect(16.0f, 16.0f, 600.0f, 440.0f), skin)
        {
            onDraw = DrawWindow;
            onException = HandleException;
            visible = false;
            resizable = false;

            availableFonts = Font.GetOSInstalledFontNames();
            int c = 0;
            var configFont = config.fontName;

            foreach (var font in availableFonts)
            {
                if (font == configFont)
                {
                    selectedFont = c;
                    break;
                }

                c++;
            }
        }

        void DrawColorControl(string name, ref Color value, ColorPicker.OnColorChanged onColorChanged)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name);
            GUILayout.FlexibleSpace();
            GUIControls.ColorField(name, "", ref value, 0.0f, null, true, true, onColorChanged);
            GUILayout.EndHorizontal();
        }

        void DrawWindow()
        {
            var config = ModTools.Instance.config;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Font");

            var newSelectedFont = GUIComboBox.Box(selectedFont, availableFonts, "SceneExplorerColorConfigFontsComboBox");
            if (newSelectedFont != selectedFont)
            {
                config.fontName = availableFonts[newSelectedFont];
                selectedFont = newSelectedFont;
                UpdateFont();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            GUILayout.Label("Font size");

            var newFontSize = (int)GUILayout.HorizontalSlider((float) config.fontSize, 13.0f, 17.0f, GUILayout.Width(256));

            if (newFontSize != config.fontSize)
            {
                config.fontSize = newFontSize;
                UpdateFont();
            }

            GUILayout.EndHorizontal();
            
            DrawColorControl("Background", ref config.backgroundColor, color =>
            {
                config.backgroundColor = color;
                bgTexture.SetPixel(0, 0, config.backgroundColor);
                bgTexture.Apply();
            });

            DrawColorControl("GameObject", ref config.gameObjectColor, color => config.gameObjectColor = color);
            DrawColorControl("Component (enabled)", ref config.enabledComponentColor, color => config.enabledComponentColor = color);
            DrawColorControl("Component (disabled)", ref config.disabledComponentColor, color => config.disabledComponentColor = color);
            DrawColorControl("Selected component", ref config.selectedComponentColor, color => config.selectedComponentColor = color);
            DrawColorControl("Keyword", ref config.keywordColor, color => config.keywordColor = color);
            DrawColorControl("Member name", ref config.nameColor, color => config.nameColor = color);
            DrawColorControl("Member type", ref config.typeColor, color => config.typeColor = color);
            DrawColorControl("Member modifier", ref config.modifierColor, color => config.modifierColor = color);
            DrawColorControl("Field type", ref config.memberTypeColor, color => config.memberTypeColor = color);
            DrawColorControl("Member value", ref config.valueColor, color => config.valueColor = color);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save", GUILayout.Width(128)))
            {
                ModTools.Instance.SaveConfig();
            }

            if (GUILayout.Button("Reset", GUILayout.Width(128)))
            {
                var template = new Configuration();

                config.backgroundColor = template.backgroundColor;
                bgTexture.SetPixel(0, 0, config.backgroundColor);
                bgTexture.Apply();

                config.gameObjectColor = template.gameObjectColor;
                config.enabledComponentColor = template.enabledComponentColor;
                config.disabledComponentColor = template.disabledComponentColor;
                config.selectedComponentColor = template.selectedComponentColor;
                config.nameColor = template.nameColor;
                config.typeColor = template.typeColor;
                config.modifierColor = template.modifierColor;
                config.memberTypeColor = template.memberTypeColor;
                config.valueColor = template.valueColor;
                config.fontName = template.fontName;
                config.fontSize = template.fontSize;

                UpdateFont();
                ModTools.Instance.SaveConfig();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        void HandleException(Exception ex)
        {
            Log.Error("Exception in SceneExplorerColorConfig - " + ex.Message);
            visible = false;
        }

    }
}
