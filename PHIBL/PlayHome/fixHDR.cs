using UnityEngine;


class fixHDR : Singleton<fixHDR>
{
    private void LateUpdate()
    {
        if (QualitySettings.antiAliasing != 0)
            QualitySettings.antiAliasing = 0;
        if (!Camera.main.hdr)
            Camera.main.hdr = true;
    }
}

