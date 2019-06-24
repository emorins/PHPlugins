using UnityEngine;
using IllusionPlugin;

namespace PlayHomeMirrorHelper
{
    static class UIUtils
    {
        //Initialize GUI style
        internal static void InitStyle()
        {
            boxstyle = new GUIStyle(GUI.skin.box)
            {
                stretchHeight = false,
                stretchWidth = true,
                wordWrap = true
            }; 
            windowstyle = new GUIStyle(GUI.skin.window);
            selectstyle = new GUIStyle(GUI.skin.button);
            selectstyle.onNormal.textColor = selected;
            selectstyle.onHover.textColor = selectedOnHover;
            buttonstyle = new GUIStyle(GUI.skin.button);
            sliderstyle = new GUIStyle(GUI.skin.horizontalSlider);
            thumbstyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
            labelstyle2 = new GUIStyle(GUI.skin.label);
            titlestyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold
            };
            titlestyle.onHover.textColor = Color.cyan;
            titlestyle2 = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };
            labelstyle = new GUIStyle(GUI.skin.label);
            buttonstyle2 = new GUIStyle(GUI.skin.button)
            {
                stretchHeight = true,
                stretchWidth = true,
                wordWrap = true
            };
            buttonstyle3 = new GUIStyle(GUI.skin.button);
            togglestyle = new GUIStyle(GUI.skin.toggle);
            labelstyle3 = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true
            };

            if (Screen.width > 2560)
            {
                //windowstyle.fontSize = 20;
                space = 12f;
                boxstyle.fontSize = 22;
                selectstyle.fontSize = 22;
                selectstyle.padding = new RectOffset(6, 6, 8, 8);
                buttonstyle.fontSize = 24;
                buttonstyle3.fontSize = 26;
                buttonstyle.padding = new RectOffset(10, 10, 5, 5);
                buttonstyle2.fontSize = 22;
                buttonstyle2.margin = new RectOffset(10, 10, 4, 4);
                labelstyle.fontSize = 24;
                labelstyle3.fontSize = 22;
                labelstyle2.fontSize = 20;
                titlestyle.fontSize = 30;
                titlestyle2.fontSize = 26;
                togglestyle.fontSize = 18;
                thumbstyle.fixedHeight = 20f;
                thumbstyle.padding = new RectOffset(10, 10, 10, 10);
                sliderstyle.padding = new RectOffset(-10, -10, -5, -5);
                sliderstyle.margin = new RectOffset(10, 10, 2, 2);
            }
            else if (Screen.width > 1920)
            {

                titlestyle.fontSize = 20;
                titlestyle2.fontSize = 18;
                space = 8f;
                labelstyle3.fontSize = 14;
                labelstyle2.fontSize = 12;
                buttonstyle3.fontSize = 16;
                buttonstyle2.margin = new RectOffset(6, 6, 2, 2);
                windowRect = new Rect(Screen.width * 0.5f, Screen.height * 0.64f, Screen.width * 0.33f, Screen.height * 0.5f);
            }
            else
            {
                titlestyle.fontSize = 16;
                titlestyle2.fontSize = 13;
                space = 6f;
                labelstyle2.fontSize = 9;
                buttonstyle2.fontSize = 11;
                buttonstyle2.margin = new RectOffset(4, 4, 2, 2);
                windowRect = new Rect(Screen.width * 0.5f, Screen.height * 0.64f, Screen.width * 0.33f, Screen.height * 0.6f);
            }
            titlestyle.alignment = TextAnchor.MiddleCenter;
            titlestyle.fontStyle = FontStyle.Bold;
            labelstyle.alignment = TextAnchor.MiddleLeft;
            styleInitialized = true;
        }

        internal static void LimitWindowRect()
        {
            if (windowRect.x <= 0)
            {
                windowRect.x = 5f;
            }
            if (windowRect.y <= 0)
            {
                windowRect.y = 5f;
            }
            if (windowRect.xMax >= Screen.width)
            {
                windowRect.x -= 5f + windowRect.xMax - Screen.width;
            }
            if (windowRect.yMax >= Screen.height)
            {
                windowRect.y -= 5f + windowRect.yMax - Screen.height;
            }
        }

        internal static bool ToggleGUI(bool toggle, GUIContent title, string[] switches)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            int temp = 0;
            if (toggle)
            {
                temp = 1;
            }
            temp = GUILayout.SelectionGrid(temp, new GUIContent[]
            {
                new GUIContent(switches[0], title.tooltip),
                new GUIContent(switches[1], title.tooltip)
            }, 2, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            if (temp == 0)
            {
                return false;
            }
            return true;
        }

        internal static float SliderGUI(float value, float min, float max, float reset, string labeltext, string valuedecimals)
        {
            return SliderGUI(value, min, max, reset, labeltext, "", valuedecimals);
        }

        internal static float SliderGUI(float value, float min, float max, float reset, string labeltext, string tooltip, string valuedecimals)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(labeltext, tooltip), labelstyle);
            GUILayout.Space(space);
            if (GUILayout.Button(" Reset ", buttonstyle, GUILayout.ExpandWidth(false)))
            {
                value = reset;
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(value.ToString(valuedecimals), labelstyle2);
            value = GUILayout.HorizontalSlider(value, min, max, sliderstyle, thumbstyle);
            GUILayout.Space(space);
            return value;
        }

        static Color selected = new Color(0.1f, 0.75f, 1f);
        static Color selectedOnHover = new Color(0.1f, 0.6f, 0.8f);
        internal static Rect windowRect = new Rect(Screen.width * 0.42f, Screen.height * 0.44f, Screen.width * 0.16f, Screen.height * 0.12f);
        internal static GUIStyle selectstyle;
        internal static GUIStyle buttonstyle;
        internal static GUIStyle sliderstyle;
        internal static GUIStyle thumbstyle;
        internal static GUIStyle labelstyle2;
        internal static GUIStyle titlestyle;
        internal static GUIStyle titlestyle2;
        internal static GUIStyle labelstyle;
        internal static GUIStyle buttonstyle2;
        internal static GUIStyle buttonstyle3;
        internal static GUIStyle togglestyle;
        internal static GUIStyle windowstyle;
        internal static Vector2[] scrollPosition = new Vector2[5];
        internal static bool styleInitialized = false;
        internal static float space;
        internal static GUIStyle labelstyle3;
        internal static GUIStyle boxstyle;
    }
}
