using System;
using UnityEngine;

namespace PHPE
{
    public class CameraEventsDispatcher : MonoBehaviour
    {
        public event Action onPreRender;
        private void OnPreRender()
        {
            if (this.onPreRender != null)
                this.onPreRender();
        }
    }
}
