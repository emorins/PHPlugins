using UnityEngine;
using System.Collections;

namespace PlayHomeMirrorHelper
{
    class CamCtrl : MonoBehaviour
    {
        CamCtrl()
        {
            Illcam = Camera.main.GetComponent<IllusionCamera>();
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
                    Illcam.Lock = true;
                }
                else if (previouswindowdragflag)
                {
                    Illcam.Lock = false;
                }
                previouswindowdragflag = GetComponent<MirrorHelper>().windowdragflag;
            }
        }

        bool previouswindowdragflag = false;
        internal IllusionCamera Illcam;
    }
}
