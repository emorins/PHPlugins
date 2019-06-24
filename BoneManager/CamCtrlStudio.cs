using UnityEngine;
using System.Collections;
using Studio;

namespace BoneModHarmony
{
    class CamCtrlStudio : MonoBehaviour
    {
        void Start()
        {
            StudioCameraControl = Singleton<Studio.Studio>.Instance.cameraCtrl;
        }

        void OnEnable()
        {
            StartCoroutine(CameraLock());
        }
        void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator CameraLock()
        {
            while (true)
            {
                yield return null;
                if (GetComponent<BoneManager>().windowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                else if (previouswindowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                previouswindowdragflag = GetComponent<BoneManager>().windowdragflag;
            }
        }

        bool CameraCtrlOff()
        {
            return GetComponent<BoneManager>().windowdragflag;
        }

        bool previouswindowdragflag = false;
        CameraControl StudioCameraControl;
    }
}
