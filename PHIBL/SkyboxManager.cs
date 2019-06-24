using UnityEngine;
using UnityEngine.Rendering;
using MessagePack;
namespace PHIBL
{
    [MessagePackObject(true)]
    public struct SkyboxParams
    {
        public float exposure;
        public float rotation;
        public Color tint;
        public SkyboxParams(float exposure, float rotation, Color tint)
        {
            this.exposure = exposure;
            this.rotation = rotation;
            this.tint = tint;
        }
    };
    class SkyboxManager
    {
        public void ApplySkybox()
        {
            RenderSettings.skybox = skybox;
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
            var sky = Camera.main.GetOrAddComponent<Skybox>();
            sky.enabled = true;
            sky.material = skyboxbg;
        }
        public void ApplySkyboxParams()
        {
            skyboxbg.SetFloat(_Exposure, skyboxParams.exposure);
            skyboxbg.SetFloat(_Rotation, skyboxParams.rotation);
            skyboxbg.SetColor(_Tint, skyboxParams.tint);
            skybox.SetFloat(_Exposure, skyboxParams.exposure);
            skybox.SetColor(_Tint, skyboxParams.tint);
            skybox.SetFloat(_Rotation, skyboxParams.rotation);
        }
        public void SaveSkyboxParams()
        {
            skyboxParams.exposure = Exposure;
            skyboxParams.rotation = Rotation;
            skyboxParams.tint = Tint;
        }
        public float Exposure
        {
            get => skybox.GetFloat(_Exposure);
            set
            {
                skybox.SetFloat(_Exposure, value);
                skyboxbg.SetFloat(_Exposure, value);
                skyboxParams.exposure = value;
            }
        }
        public Color Tint
        {
            get => skybox.GetColor(_Tint);
            set
            {
                skybox.SetColor(_Tint, value);
                skyboxbg.SetColor(_Tint, value);
                skyboxParams.tint = value;
            }
        }
        public float Rotation
        {
            get => Skybox.GetFloat(_Rotation);
            set
            {
                skyboxbg.SetFloat(_Rotation, value);
                skybox.SetFloat(_Rotation, value);
                skyboxParams.rotation = value;
            }
        }


        static readonly int _Exposure = Shader.PropertyToID("_Exposure");
        static readonly int _Rotation = Shader.PropertyToID("_Rotation");
        static readonly int _Tint = Shader.PropertyToID("_Tint");

        Material skybox;
        Material skyboxbg;
        public SkyboxParams skyboxParams = new SkyboxParams(1f,0f,Color.gray);
        public Material Skyboxbg { get => skyboxbg; set => skyboxbg = value; }
        public Material Skybox { get => skybox; set => skybox = value; }
    }

}
