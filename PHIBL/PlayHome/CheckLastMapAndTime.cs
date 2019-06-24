using UnityEngine;

namespace PHIBL
{
    class CheckLastMapAndTime : MonoBehaviour
    {
        void Update()
        {
            //map or time change
            if (lastselectedtime != GlobalData.PlayData.lastSelectTimeZone || lastselectedmap != GlobalData.PlayData.lastSelectMap)
            {
                gameObject.GetComponent<PHIBL>().ResetIBL();
                lastselectedtime = GlobalData.PlayData.lastSelectTimeZone;
                lastselectedmap = GlobalData.PlayData.lastSelectMap;
            }
        }

        void OnEnable()
        {
            lastselectedtime = GlobalData.PlayData.lastSelectTimeZone;
            lastselectedmap = GlobalData.PlayData.lastSelectMap;
        }
        private int lastselectedmap;
        private int lastselectedtime;
    }
}
