using UnityEngine;
using System.Collections;
using Studio;

namespace PlayHomeMirrorHelper
{
    class CamCtrlStudio : MonoBehaviour
    {
        CamCtrlStudio()
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
                if (GetComponent<MirrorHelper>().windowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                else if (previouswindowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                previouswindowdragflag = GetComponent<MirrorHelper>().windowdragflag;
            }
        }

        bool CameraCtrlOff()
        {
            return GetComponent<MirrorHelper>().windowdragflag;
        }

        bool previouswindowdragflag = false;
        internal CameraControl StudioCameraControl;
    }
}
