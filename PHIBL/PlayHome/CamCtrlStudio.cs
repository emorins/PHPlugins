using UnityEngine;
using System.Collections;
using Studio;

namespace PHIBL
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
                if (GetComponent<PHIBL>().windowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                else if (previouswindowdragflag)
                {
                    StudioCameraControl.noCtrlCondition = new CameraControl.NoCtrlFunc(CameraCtrlOff);
                }
                previouswindowdragflag = GetComponent<PHIBL>().windowdragflag;
            }
        }

        bool CameraCtrlOff()
        {
            return GetComponent<PHIBL>().windowdragflag;
        }



        bool previouswindowdragflag = false;
        internal CameraControl StudioCameraControl;
    }
}
