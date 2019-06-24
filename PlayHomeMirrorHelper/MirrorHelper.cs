using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Reflection;


namespace PlayHomeMirrorHelper
{
    class MirrorHelper : MonoBehaviour
    {
        public MirrorHelper()
        {
            shortcut = ModPrefs.GetString("MirrorHelper", "Shortcut", "F6", true);
            texturesize = ModPrefs.GetInt("MirrorHelper", "Resolution", 2048, true);
            clipplaneoffset = ModPrefs.GetFloat("MirrorHelper", "ClipPlaneOffset", 0, true);
            if (texturesize <= 1024)
                texturesize = 1024;
            else if (texturesize <= 2048)
                texturesize = 2048;
            else
                texturesize = 4096;
            ModPrefs.SetInt("MirrorHelper", "Resolution", texturesize);
        }
        private void OnGUI()
        {
            if (!window)
            {
                return;
            }

            if (!UIUtils.styleInitialized)
            {
                UIUtils.InitStyle();
            }
            UIUtils.windowRect = GUILayout.Window(windowID, UIUtils.windowRect, MirrorHelperwindow, "Mirror Helper", UIUtils.windowstyle);
            Vector2 pos = new Vector2
            {
                x = Input.mousePosition.x,
                y = (Screen.height - Input.mousePosition.x)
            };
            if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown) && !UIUtils.windowRect.Contains(pos))
            {
                windowdragflag = false;
            }
        }

        private void MirrorHelperwindow(int id)
        {

            if (Event.current.type == EventType.MouseDown)
            {
                GUI.FocusWindow(windowID);
                windowdragflag = true;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                windowdragflag = false;
            }

            GUILayout.BeginVertical();
            if (Mirror == null)
            {
                window = false;
                return;
            }
            texturesize = Mirror.m_TextureSize;
            int selected;
            if (texturesize <= 1024)
                selected = 0;
            else if (texturesize <= 2048)
                selected = 1;
            else
                selected = 2;
            GUILayout.Label("Mirror Resolution", UIUtils.labelstyle);
            GUILayout.Space(UIUtils.space);
            selected = GUILayout.SelectionGrid(selected, new string[] { "1024", "2048", "4096" }, 3, UIUtils.selectstyle);
            switch (selected)
            {
                default:
                case 0:
                    texturesize = 1024;
                    break;
                case 1:
                    texturesize = 2048;
                    break;
                case 2:
                    texturesize = 4096;
                    break;
            }
            Mirror.m_TextureSize = texturesize;
            clipplaneoffset  = UIUtils.SliderGUI(clipplaneoffset, 0f, 1f, 0.01f, "Clip plane offset", "N3");
            Mirror.m_ClipPlaneOffset = clipplaneoffset;
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close Window", UIUtils.buttonstyle))
            {
                window = false;
            }
            if (GUILayout.Button("Save", UIUtils.buttonstyle))
            {
                ModPrefs.SetInt("MirrorHelper", "Resolution", texturesize);
                ModPrefs.SetFloat("MirrorHelper", "ClipPlaneOffset", clipplaneoffset);
            }
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }


        private void Update()
        {
            if (Input.GetKeyDown(shortcut.ToLower()))
            {                
                if (!window)
                    Mirror = FindObjectOfType<MirrorReflectionPlus>();
                window = !window;
            }
        }


        internal const int windowID = 19253;
        MirrorReflectionPlus Mirror;
        private bool window = false;

        private string shortcut;
        private int texturesize;
        private float clipplaneoffset;
        internal bool windowdragflag = false;
    }
}
