using UnityEngine;
using System;

namespace PHIBL
{
    class DHHCompatible : MonoBehaviour
    {
        private void Awake()
        {
            if (!GameObject.Find("DHH_Base"))
            {
                enabled = false;
                Console.WriteLine("DHH is not loaded.");
            }
            else if (gameObject.GetComponent<PHIBL>().autoSetting)
            {
                ProjectHighHeel.HighHeelRuntime.instance.SystemActive[2] = false;
                ProjectHighHeel.HighHeelRuntime.instance.SystemLoaded[2] = false;
                enabled = false;
            }
        }
        private void Update()
        {
            if (ProjectHighHeel.HighHeelRuntime.instance.SystemActive[2])
            {
                gameObject.GetComponent<PHIBL>().DHHCompatibleResolved = false;
                if (Input.GetKeyDown(gameObject.GetComponent<PHIBL>().shortcut))
                    warningWindow = !warningWindow;
            }
        }
        private void OnEnable()
        {
        }
        private void OnGUI()
        {
            if (!warningWindow)
                return;
            if (!UIUtils.styleInitialized)
            {
                UIUtils.InitStyle();
            }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(UIUtils.scale.x * UIUtils.customscale, UIUtils.scale.y * UIUtils.customscale, 1f));

            if (ProjectHighHeel.HighHeelRuntime.instance.SystemActive[2])
            {
                UIUtils.warningRect = GUILayout.Window(warningID, UIUtils.warningRect, WarningWindow, "", UIUtils.windowstyle);
                return;
            }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        }
        private void WarningWindow(int id)
        {
            //UIUtils.CameraControlOffOnGUI();
            if (Event.current.type == EventType.MouseDown)
            {
                GUI.FocusWindow(warningID);
            }
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(" Warning! Not compatible with DHH graphic setting module. Disable it now? ", UIUtils.labelstyle3);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" Yes ", UIUtils.buttonstyleNoStretch))
            {
                Console.WriteLine("PHIBL: resolving compatible issues with DHH");
                ProjectHighHeel.HighHeelRuntime.instance.SystemActive[2] = false;
                //ProjectHighHeel.HighHeelRuntime.instance.SystemLoaded[2] = false;
                gameObject.GetComponent<PHIBL>().DHHCompatibleResolved = true;
                gameObject.GetComponent<PHIBL>().ResetIBL();
                warningWindow = false;
                gameObject.GetComponent<PHIBL>().mainWindow = true;
                //return;
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(" No ", UIUtils.buttonstyleNoStretch))
            {
                warningWindow = false;
                //return;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }


        internal const int warningID = 19854;
        internal bool warningWindow = false;

    }
}
