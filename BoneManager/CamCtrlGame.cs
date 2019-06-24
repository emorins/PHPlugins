using UnityEngine;
using System.Collections;

namespace BoneModHarmony
{
    class CamCtrl : MonoBehaviour
    {
        void Start()
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
                if (GetComponent<BoneManager>().windowdragflag)
                {
                    Illcam.Lock = true;
                }
                else if (previouswindowdragflag)
                {
                    Illcam.Lock = false;
                }
                previouswindowdragflag = GetComponent<BoneManager>().windowdragflag;
            }
        }
        bool previouswindowdragflag = false;
        IllusionCamera Illcam;
    }
}
