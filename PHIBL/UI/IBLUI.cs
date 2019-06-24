using UnityEngine;
using IllusionPlugin;
using System;

namespace PHIBL
{
    static class UIUtils
    {
        //Initialize GUI style
        internal static void InitStyle()
        {
            scale.y = UnityEngine.Screen.height / Screen.height;
            scale.x = scale.y;
            scale.z = 1f;

            customscale = ModPrefs.GetFloat("PHIBL", "Window.customScale", 1f, true);
            customscale = Mathf.Clamp(customscale, 0.5f / scale.y, 2f);
            ModPrefs.SetFloat("PHIBL", "Window.customScale", customscale);
            screenBound = new Vector2(UnityEngine.Screen.width / scale.x / customscale, Screen.height / customscale);

            myfont = Font.CreateDynamicFontFromOSFont(new string[] { "Segeo UI", "Microsoft YaHei UI", "Microsoft YaHei" }, 20);
            windowtitle = new GUIStyle(GUI.skin.label)
            {
                //border = new RectOffset(1,1,1,1),
                fontSize = 24,
                padding = new RectOffset(10, 10, 6, 12),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                font = myfont
            };
            toggleButtonOn = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                stretchHeight = false,
                stretchWidth = false,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = false,
                font = myfont,
                margin = new RectOffset(4, 4, 4, 4),
                padding = new RectOffset(6, 6, 6, 12),
                fontSize = 22                
            };
            toggleButtonOff = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                stretchHeight = false,
                stretchWidth = false,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = false,
                font = myfont,
                margin = new RectOffset(4, 4, 4, 4),
                padding = new RectOffset(6, 6, 6, 12),
                fontSize = 22
            };
            toggleButtonOn.onNormal.textColor = selected;
            toggleButtonOn.onHover.textColor = selectedOnHover;
            toggleButtonOn.normal = toggleButtonOn.onNormal;
            toggleButtonOn.hover = toggleButtonOn.onHover;
            boxstyle = new GUIStyle(GUI.skin.box)
            {
                stretchHeight = false,
                stretchWidth = true,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
                font = myfont,
                fontSize = 22,
                padding = new RectOffset(6, 6, 6, 12)
			};
            windowstyle = new GUIStyle(GUI.skin.window);
            selectstyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 22,
                padding = new RectOffset(6, 6, 6, 12),
                margin = new RectOffset(4, 4, 4, 4),
                font = myfont
            };
            selectstyle.onNormal.textColor = selected;
            selectstyle.onHover.textColor = selectedOnHover;
            buttonstyleNoStretch = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                stretchHeight = false,
                stretchWidth = false,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = false,
                font = myfont,
                padding = new RectOffset(6, 6, 6, 12),
                margin = new RectOffset(4, 4, 4, 4),
                fontSize = 22
            };

            sliderstyle = new GUIStyle(GUI.skin.horizontalSlider)
            {
                padding = new RectOffset(-10, -10, -4, -4),
                fixedHeight = 16f,
                margin = new RectOffset(22, 22, 22, 22)
            };
            thumbstyle = new GUIStyle(GUI.skin.horizontalSliderThumb)
            {
                fixedHeight = 24f,
                padding = new RectOffset(14, 14, 12, 12)

            };
            labelstyle2 = new GUIStyle(GUI.skin.label)
            {
                font = myfont,
                fontSize = 20,
                margin = new RectOffset(16, 16, 8, 8)
            };
            titlestyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                font = myfont,
                fontSize = 30,
                padding = new RectOffset(6, 6, 6, 12),
                margin = new RectOffset(4, 4, 4, 4),
                alignment = TextAnchor.MiddleCenter
            };
            titlestyle.onNormal.textColor = selected;
            titlestyle.onHover.textColor = selectedOnHover;
            titlestyle2 = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                font = myfont,
                fontSize = 30,
                padding = new RectOffset(6, 6, 6, 12),
            };
            labelstyle = new GUIStyle(GUI.skin.label)
            {
                font = myfont,
                fontSize = 24,
                padding = new RectOffset(6, 6, 6, 12),
                alignment = TextAnchor.MiddleLeft
            };
            buttonstyleStrechWidth = new GUIStyle(GUI.skin.button)
            {
                stretchHeight = false,
                stretchWidth = true,
                wordWrap = true,
                fontStyle = FontStyle.Bold,
                font = myfont,
                fontSize = 22,
                margin = new RectOffset(10, 10, 5, 5),
                padding = new RectOffset(6, 6, 6, 12)
            };
            buttonstyleStrechWidth.onNormal.textColor = selected;
            buttonstyleStrechWidth.onHover.textColor = selectedOnHover;
            labelstyle3 = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                fontSize = 22
            };

            space = 12f;                                
            minwidth = Mathf.Round(0.28f * Screen.width);
            styleInitialized = true;

            windowRect.x = ModPrefs.GetFloat("PHIBL", "Window.x", windowRect.x, true);
            windowRect.y = ModPrefs.GetFloat("PHIBL", "Window.y", windowRect.y, true);
            windowRect.width = Mathf.Min(Screen.width - 10f, ModPrefs.GetFloat("PHIBL", "Window.width", windowRect.width, true));
            windowRect.height = Mathf.Min(Screen.height - 10f, ModPrefs.GetFloat("PHIBL", "Window.height", windowRect.height, true));
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
            if (windowRect.xMax >= screenBound.x)
            {
                windowRect.x -= 5f + windowRect.xMax - screenBound.x;
            }
            if (windowRect.yMax >= screenBound.y)
            {
                windowRect.y -= 5f + windowRect.yMax - screenBound.y;
            }
        }

        internal static void MoveTooltipRect()
        {
            TooltipRect.height = windowRect.height;
            TooltipRect.width = (Screen.width/customscale) * 0.16f;
            TooltipRect.y = windowRect.y;
            if (windowRect.x > (Screen.width/customscale) * 0.16f + 5f)
            {
                TooltipRect.x = windowRect.x - 0.16f * (Screen.width/customscale);
            }
            else
            {
                TooltipRect.x = windowRect.xMax + 5f;
            }
        }
        //internal static void SliderGUI(ref float value, float min, float max, string valuedecimals = "N3")
        //{
        //    string title = " " + nameof(value) + ": ";
        //    value = SliderGUI(value, min, max, title, "", valuedecimals);
        //}
        internal static void SliderGUI(ref float value, float min, float max, float reset, Action OnValueChanged, GUIContent guiContent, string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, reset, guiContent, valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
            {
                value = temp;
                OnValueChanged();
            }
        }
        internal static void SliderGUI(ref float value, float min, float max, float reset, Action OnValueChanged, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            SliderGUI(ref value, min, max, reset, OnValueChanged, new GUIContent(labeltext, tooltip), valuedecimals);
        }
        internal static void SliderGUI(ref float value, float min, float max, Action OnValueChanged, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            SliderGUI(ref value, min, max, OnValueChanged, new GUIContent(labeltext, tooltip), valuedecimals);
        }
        internal static void SliderGUI(ref float value, float min, float max, Action OnValueChanged, GUIContent guiContent, string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, guiContent, valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
            {
                value = temp;
                OnValueChanged();
            }
        }

        internal static void SliderGUI(float value, float min, float max, Action<float> SetOutput, GUIContent guiContent, string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, guiContent, valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static float SliderGUI(float value, float min, float max, GUIContent guiContent, string valuedecimals = "N3")
        {
            GUILayout.Label(new GUIContent(guiContent.text + value.ToString(valuedecimals), guiContent.tooltip), labelstyle);
            value = GUILayout.HorizontalSlider(value, min, max, sliderstyle, thumbstyle);
            return value;
        }
        internal static void SliderGUI(ref float value, float min, float max, GUIContent guiContent, string valuedecimals = "N3")
        {
            value = SliderGUI(value, min, max, guiContent, valuedecimals);
        }
        internal static float SliderGUI(float value, float min, float max, float reset, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            return SliderGUI(value, min, max, reset, new GUIContent(labeltext, tooltip), valuedecimals);
        }
        internal static void SliderGUI(float value, float min, float max, float reset, Action<float> SetOutput,string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, reset, new GUIContent(labeltext, tooltip), valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static void SliderExGUI(float value, float reset, Action<float> SetOutput, string labeltext, float power = 0.5f, string tooltip = "", string valuedecimals = "N5")
        {
            SliderExGUI(value, reset, SetOutput, new GUIContent(labeltext, tooltip), power, valuedecimals);
        }
        internal static void SliderExGUI(float value, float reset, Action<float> SetOutput, GUIContent guiContent, float power = 0.5f, string valuedecimals = "N5")
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(guiContent, labelstyle);
            GUILayout.Space(space);
            if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
            {
                value = reset;
                SetOutput(value);
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(value.ToString(valuedecimals), labelstyle2);
            var temp = GUILayout.HorizontalSlider(Mathf.Pow(value, power), 0f, 1f, sliderstyle, thumbstyle);
            temp = Mathf.Pow(temp, 1 / power);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static void SliderExGUI(float value, Func<float> reset, Action<float> SetOutput, GUIContent guiContent, float power = 0.5f, string valuedecimals = "N5")
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(guiContent, labelstyle);
            GUILayout.Space(space);
            if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
            {
                value = reset();
                SetOutput(value);
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(value.ToString(valuedecimals), labelstyle2);
            var temp = GUILayout.HorizontalSlider(Mathf.Pow(value, power), 0f, 1f, sliderstyle, thumbstyle);
            temp = Mathf.Pow(temp, 1 / power);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static void SliderGUI(ref float value, float min, float max, float reset, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            value = SliderGUI(value, min, max, reset, new GUIContent(labeltext, tooltip), valuedecimals);
        }
        internal static float SliderGUI(float value, float min, float max, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            GUILayout.Label(new GUIContent(labeltext + value.ToString(valuedecimals), tooltip) , labelstyle); 
            value = GUILayout.HorizontalSlider(value, min, max, sliderstyle, thumbstyle);
            return value;
        }
        internal static void SliderGUI(float value, float min, float max, Action<float> SetOutput ,string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, labeltext, tooltip, valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static void SliderGUI(ref float value, float min, float max, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            GUILayout.Label(new GUIContent(labeltext + value.ToString(valuedecimals), tooltip), labelstyle);
            value = GUILayout.HorizontalSlider(value, min, max, sliderstyle, thumbstyle);
        }
        internal static void SliderGUI(float value, float min, float max, Func<float> reset, Action<float> SetOutput, GUIContent label, string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, reset, label.text, label.tooltip, valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static float SliderGUI(float value, float min, float max, Func<float> reset, GUIContent label, string valuedecimals = "N3")
        {
            return SliderGUI(value, min, max, reset, label.text, label.tooltip, valuedecimals);
        }
        internal static float SliderGUI(float value, float min, float max, Func<float> reset, string labeltext, string tooltip = "", string valuedecimals = "N3")
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent(labeltext, tooltip), labelstyle);
            GUILayout.Space(space);
            if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
            {
                value = reset();
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(value.ToString(valuedecimals), labelstyle2);
            value = GUILayout.HorizontalSlider(value, min, max, sliderstyle, thumbstyle);
            return value;
        }
        internal static void SliderGUI(ref float value, float min, float max, float reset, GUIContent guiContent, string valuedecimals = "N3")
        {
            value = SliderGUI(value, min, max, reset, guiContent, valuedecimals);
        }
        internal static void SliderGUI(float value, float min, float max, float reset, Action<float> SetOutput, GUIContent guiContent, string valuedecimals = "N3")
        {
            var temp = SliderGUI(value, min, max, guiContent, valuedecimals);
            if (Math.Abs(temp - value) > Single.Epsilon)
                SetOutput(temp);
        }
        internal static float SliderGUI(float value, float min, float max, float reset, GUIContent guiContent, string valuedecimals = "N3")
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(guiContent, labelstyle);
            GUILayout.Space(space);
            if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
            {
                value = reset;
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(value.ToString(valuedecimals), labelstyle2);
            value = GUILayout.HorizontalSlider(value, min, max, sliderstyle, thumbstyle);
            return value;
        }

        internal static Color SliderGUI(Color value, Color reset, GUIContent guiContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(guiContent, labelstyle);
            GUI.color = new Color(value.r, value.g, value.b, 1f);
            GUILayout.Label(new Texture2D(64, 32, TextureFormat.RGB24, false, true), labelstyle);
            GUI.color = Color.white;
            GUILayout.FlexibleSpace();
            hsvcolorpicker = GUILayout.SelectionGrid(hsvcolorpicker, new string[]
            {
                "RGB",
                "HSV"
            }, 2, selectstyle, GUILayout.ExpandWidth(false));
            if (GUILayout.Button(GUIStrings.Reset, buttonstyleNoStretch))
            {
                value = reset;
            }
            GUILayout.EndHorizontal();
            if (hsvcolorpicker == 0)
            {
                GUILayout.Label(GUIStrings.Colorpicker_red + value.r.ToString("N3"), labelstyle);
                value.r = GUILayout.HorizontalSlider(value.r, 0f, 1f, sliderstyle, thumbstyle);
                GUILayout.Label(GUIStrings.Colorpicker_green + value.g.ToString("N3"), labelstyle);
                value.g = GUILayout.HorizontalSlider(value.g, 0f, 1f, sliderstyle, thumbstyle);
                GUILayout.Label(GUIStrings.Colorpicker_blue + value.b.ToString("N3"), labelstyle);
                value.b = GUILayout.HorizontalSlider(value.b, 0f, 1f, sliderstyle, thumbstyle);
            }
            else
            {
                Color.RGBToHSV(value, out float H, out float S, out float V);
                GUILayout.Label(GUIStrings.Colorpicker_hue + H.ToString("N3"), labelstyle);
                H = GUILayout.HorizontalSlider(H, 0f, 1f, sliderstyle, thumbstyle);
                GUILayout.Label(GUIStrings.Colorpicker_saturation + S.ToString("N3"), labelstyle);
                S = GUILayout.HorizontalSlider(S, 0f, 1f, sliderstyle, thumbstyle);
                GUILayout.Label(GUIStrings.Colorpicker_value + V.ToString("N3"), labelstyle);
                V = GUILayout.HorizontalSlider(V, 0f, 1f, sliderstyle, thumbstyle);
                value = Color.HSVToRGB(H, S, V, false);
            }
            return value;
        }
        internal static Color SliderGUI(Color value, Color reset, string labeltext, string tooltip="")
        {
            return SliderGUI(value, reset, new GUIContent(labeltext, tooltip));
        }
        internal static void SliderGUI(Color value, Color reset, Action<Color> SetOutput, string labeltext, string tooltip = "")
        {
            var temp = SliderGUI(value, reset, labeltext, tooltip);
            if (temp != value)
                SetOutput(temp);
        }
        internal static void SliderGUI(Color value, Color reset, Action<Color> SetOutput, GUIContent guiContent)
        {
            var temp = SliderGUI(value, reset, guiContent);
            if (temp != value)
                SetOutput(temp);
        }
        internal static void SliderGUI(ref Color value, Color reset, GUIContent guiContent)
        {
            value = SliderGUI(value, reset, guiContent);
        }
        internal static void SliderGUI(ref Color value, Color reset, string labeltext, string tooltip = "")
        {
            value = SliderGUI(value, reset, new GUIContent(labeltext, tooltip));
        }
        internal static void SelectGUI<TEnum>(ref TEnum selected, GUIContent title, Action<TEnum> Action)
        {
            selected = SelectGUI(selected, title, Action);
        }
        internal static void SelectGUI<TEnum>(ref TEnum selected, Action<TEnum> Action)
        {
            selected = SelectGUI(selected, Action);
        }
        internal static TEnum SelectGUI<TEnum>(TEnum selected, Action<TEnum> Action)
        {
            string[] selectionGUIContent = Enum.GetNames(typeof(TEnum));
            int temp = Array.IndexOf(selectionGUIContent, selected.ToString());
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            int index = GUILayout.SelectionGrid(temp, selectionGUIContent, selectionGUIContent.Length, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (index == temp)
                return selected;
            string selectedname = selectionGUIContent.GetValue(index).ToString();
            selected = (TEnum)Enum.Parse(typeof(TEnum), selectedname);
            Action(selected);
            return selected;
        }
        internal static TEnum SelectGUI<TEnum>(TEnum selected, GUIContent title, Action<TEnum> Action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            string[] selectionGUIContent = Enum.GetNames(typeof(TEnum));
            int temp = Array.IndexOf(selectionGUIContent, selected.ToString());
            int index = GUILayout.SelectionGrid(temp, selectionGUIContent, selectionGUIContent.Length, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            if (index == temp)
                return selected;
            string selectedname = selectionGUIContent.GetValue(index).ToString();
            selected = (TEnum)Enum.Parse(typeof(TEnum), selectedname);
            Action(selected);            
            return selected;
        }
        internal static TEnum SelectGUI<TEnum>(TEnum selected, GUIContent title, string[] selectionContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            string[] selectionGUIContent = Enum.GetNames(typeof(TEnum));
            int temp = Array.IndexOf(selectionGUIContent, selected.ToString());
            int index = GUILayout.SelectionGrid(temp, selectionContent, selectionContent.Length, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            if (index == temp)
                return selected;
            string selectedname = selectionGUIContent.GetValue(index).ToString();
            selected = (TEnum)Enum.Parse(typeof(TEnum), selectedname);
            return selected;
        }
        internal static void SelectGUI<TEnum>(ref TEnum selected, GUIContent title)
        {
            selected = SelectGUI(selected, title);
        }
        internal static TEnum SelectGUI<TEnum>(TEnum selected, GUIContent title)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            string[] selectionGUIContent = Enum.GetNames(typeof(TEnum));
            int temp = Array.IndexOf(selectionGUIContent, selected.ToString());
            int index = GUILayout.SelectionGrid(temp, selectionGUIContent, selectionGUIContent.Length, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            if (index == temp)
                return selected;
            string selectedname = selectionGUIContent.GetValue(index).ToString();
            selected = (TEnum)Enum.Parse(typeof(TEnum), selectedname);
            return selected;
        }
        internal static void SelectGUI<TEnum>(ref TEnum selected, GUIContent title, int xCount, Action<TEnum> action)
        {
            var temp = SelectGUI(selected, title, xCount);
            if (Equals(temp,selected))
                return;
            selected = temp;
            action(selected);

        }
        internal static void SelectGUI<TEnum>(ref TEnum selected, GUIContent title, int xCount)
        {
            selected = SelectGUI(selected, title, xCount);
        }
        internal static TEnum SelectGUI<TEnum>(TEnum selected, GUIContent title, int xCount)
        {
            GUILayout.Label(title, labelstyle);
            string[] selectionGUIContent = Enum.GetNames(typeof(TEnum));
            int index = Array.IndexOf(selectionGUIContent, selected.ToString());

            if(xCount == 0)
                index = GUILayout.SelectionGrid(index, selectionGUIContent, selectionGUIContent.Length, selectstyle);
            else
                index = GUILayout.SelectionGrid(index, selectionGUIContent, xCount, selectstyle);

            string selectedname = selectionGUIContent.GetValue(index).ToString();
            selected = (TEnum)Enum.Parse(typeof(TEnum), selectedname);
            return selected;
        }
        internal static int SelectGUI(int selected, GUIContent title, string[] selections)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            GUIContent[] selectionGUIContent = new GUIContent[selections.Length];
            uint num = 0;
            foreach (string s in selections)
            {
                selectionGUIContent[num] = new GUIContent(s, title.tooltip);
                num++;
            }
            selected = GUILayout.SelectionGrid(selected, selectionGUIContent, selections.Length, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            return selected;
        }
        internal static void SelectGUI(ref int selected, GUIContent title, string[] selections, Action<int> Action)
        {
            selected = SelectGUI(selected, title, selections, Action);
        }

        internal static int SelectGUI(int selected, GUIContent title, string[] selections, Action<int> Action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            GUIContent[] selectionGUIContent= new GUIContent[selections.Length];
            uint num = 0; 
            foreach(string s in selections)
            {
                selectionGUIContent[num] = new GUIContent(s, title.tooltip);
                num++;
            }
            int temp = GUILayout.SelectionGrid(selected, selectionGUIContent, selections.Length, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            if (temp == selected)
            {
                return selected;
            }
            else
            {
                Action(temp);
                return temp;
            }
        }
        internal static bool ToggleButton(bool toggle, GUIContent label, Action<bool> Action)
        {
            if (GUILayout.Button(label, (toggle ? toggleButtonOn : toggleButtonOff)))
            {
                toggle = !toggle;
                Action(toggle);
            }
            return toggle;
        }
        internal static bool ToggleButton(bool toggle, GUIContent label)
        {
            if(GUILayout.Button(label, (toggle ? toggleButtonOn : toggleButtonOff)))
            {
                toggle = !toggle;
            }
            return toggle;
        }
        internal static bool ToggleGUI(bool toggle, GUIContent title)
        {
            return ToggleGUI(toggle, title, GUIStrings.Disable_vs_Enable);
        }

        internal static void ToggleGUI(ref bool toggle, GUIContent title)
        {
            toggle = ToggleGUI(toggle, title, GUIStrings.Disable_vs_Enable);
        }
        internal static void ToggleGUI(ref bool toggle, GUIContent title, string[] switches)
        {
            toggle = ToggleGUI(toggle, title, switches);
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
        internal static void ToggleGUI(ref bool toggle, GUIContent title, string[] switches, Action<bool> Action)
        {
            toggle = ToggleGUI(toggle, title, switches, Action);
        }
        internal static void ToggleGUI(ref bool toggle, GUIContent title, Action<bool> Action)
        {
            toggle = ToggleGUI(toggle, title, GUIStrings.Disable_vs_Enable, Action);
        }
        internal static bool ToggleGUI(bool toggle, GUIContent title, Action<bool> Action)
        {
            return ToggleGUI(toggle, title, GUIStrings.Disable_vs_Enable, Action);
        }
        internal static bool ToggleGUI(bool toggle, GUIContent title, string[] switches, Action<bool> Action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, labelstyle);
            GUILayout.FlexibleSpace();
            int temp = Convert.ToInt32(toggle); 
            temp = GUILayout.SelectionGrid(temp, new GUIContent[]
            {
                new GUIContent(switches[0], title.tooltip),
                new GUIContent(switches[1], title.tooltip)
            }, 2, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            if((temp != 0) == toggle)
            {
                return toggle;
            }
            else if (temp == 0)
            {
                Action(false);
                return false;
            }            
            Action(true);
            return true;                     
        }
        internal static bool ToggleGUI(bool toggle, string[] switches, Action<bool> Action)
        {
            int temp = Convert.ToInt32(toggle);
            temp = GUILayout.SelectionGrid(temp, new GUIContent[]
            {
                new GUIContent(switches[0]),
                new GUIContent(switches[1])
            }, 2, selectstyle, GUILayout.ExpandWidth(false));
            if ((temp != 0) == toggle)
            {
                return toggle;
            }
            else if (temp == 0)
            {
                Action(false);
                return false;
            }
            Action(true);
            return true;
        }

        internal static void ToggleGUITitle(ref bool toggle, GUIContent title, Action<bool> Action)
        {
            toggle = ToggleGUITitle(toggle, title, GUIStrings.Disable_vs_Enable, Action);
        }
        internal static void ToggleGUITitle(ref bool toggle, GUIContent title, string[] switches, Action<bool> Action)
        {
            toggle = ToggleGUITitle(toggle, title, switches, Action);
        }
        internal static bool ToggleGUITitle(bool toggle, GUIContent title, Action<bool> Action)
        {
            return ToggleGUITitle(toggle, title, GUIStrings.Disable_vs_Enable, Action);
        }
        internal static bool ToggleGUITitle(bool toggle, GUIContent title, string[] switches, Action<bool> Action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, titlestyle2);
            GUILayout.FlexibleSpace();
            int temp = Convert.ToInt32(toggle);
            temp = GUILayout.SelectionGrid(temp, new GUIContent[]
            {
                new GUIContent(switches[0], title.tooltip),
                new GUIContent(switches[1], title.tooltip)
            }, 2, selectstyle, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            if ((temp != 0) == toggle)
            {
                return toggle;
            }
            else if (temp == 0)
            {
                Action(false);
                return false;
            }
            Action(true);
            return true;
        }

        internal static void ToggleGUITitle(ref bool toggle, GUIContent title)
        {
            toggle = ToggleGUITitle(toggle, title, GUIStrings.Disable_vs_Enable);
        }
        internal static void ToggleGUITitle(ref bool toggle, GUIContent title, string[] switches)
        {
            toggle = ToggleGUITitle(toggle, title, switches);
        }
        internal static bool ToggleGUITitle(bool toggle, GUIContent title)
        {
            return ToggleGUITitle(toggle, title, GUIStrings.Disable_vs_Enable);
        }

        internal static bool ToggleGUITitle(bool toggle, GUIContent title, string[] switches)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, titlestyle2);
            GUILayout.FlexibleSpace();
            int temp = Convert.ToInt32(toggle);
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
        internal static void EmptyPage(GUIContent content)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(content, titlestyle2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        internal static class Screen
        {
            internal static float width = 3840;
            internal static float height = 2160;
        }

        internal static int hsvcolorpicker = 0;
        internal static float customscale;
        internal static Vector3 scale;
        private static Vector2 screenBound;
        static Font myfont;
        internal static GUIStyle windowtitle;
        static Color selected = new Color(0.1f, 0.75f, 1f);
        static Color selectedOnHover = new Color(0.1f, 0.6f, 0.8f);
        internal static float minwidth;
        internal static Rect TooltipRect = new Rect(Screen.width * 0.35f, Screen.height * 0.64f, Screen.width * 0.15f, Screen.height * 0.45f);
        internal static Rect windowRect = new Rect(Screen.width * 0.5f, Screen.height * 0.64f, Screen.width * 0.33f, Screen.height * 0.45f);
        internal static Rect warningRect = new Rect(Screen.width * 0.425f, Screen.height * 0.45f, Screen.width * 0.15f, Screen.height * 0.1f);
        internal static GUIStyle selectstyle;
        internal static GUIStyle buttonstyleNoStretch;
        internal static GUIStyle sliderstyle;
        internal static GUIStyle thumbstyle;
        internal static GUIStyle labelstyle2;
        internal static GUIStyle titlestyle;
        internal static GUIStyle titlestyle2;
        internal static GUIStyle labelstyle;
        internal static GUIStyle buttonstyleStrechWidth;
        internal static GUIStyle toggleButtonOn;
        internal static GUIStyle toggleButtonOff;
        internal static GUIStyle windowstyle;
        internal static Vector2[] scrollPosition = new Vector2[7];
        internal static bool styleInitialized = false;
        internal static float space;
        internal static GUIStyle labelstyle3;
        internal static GUIStyle boxstyle;
    }
}
