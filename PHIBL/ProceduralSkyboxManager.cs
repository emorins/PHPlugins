using UnityEngine;
using UnityEngine.Rendering;
using MessagePack;
namespace PHIBL
{
    [MessagePackObject(true)]
    public struct ProceduralSkyboxParams
    {
        public float exposure;
        public float sunsize;
        public float atmospherethickness;
        public Color skytint;
        public Color groundcolor;
        public ProceduralSkyboxParams(float exposure, float sunSize, float atmosphereThickness, Color skyTint, Color groundColor)
        {
            this.exposure = exposure;
            sunsize = sunSize;
            atmospherethickness = atmosphereThickness;
            skytint = skyTint;
            groundcolor = groundColor;
        }
    };

    class ProceduralSkyboxManager
    {
        public void ApplySkybox()
        {
            RenderSettings.skybox = proceduralsky;
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
            Camera.main.GetOrAddComponent<Skybox>().enabled = false;
        }
        public void ApplySkyboxParams()
        {
            Exposure = skyboxParams.exposure;
            AtmosphereThickness = skyboxParams.atmospherethickness;
            GroundColor = skyboxParams.groundcolor;
            SkyTint= skyboxParams.skytint;
            SunSize = skyboxParams.sunsize;
        }
        public void SaveSkyboxParams()
        {
            skyboxParams.exposure = Exposure;
            skyboxParams.atmospherethickness = AtmosphereThickness;
            skyboxParams.groundcolor = GroundColor;
            skyboxParams.skytint = SkyTint;
            skyboxParams.sunsize = SunSize;
        }
        Material proceduralsky;
        public ProceduralSkyboxParams skyboxParams = new ProceduralSkyboxParams(1f, 0.1f, 1f, Color.gray, Color.gray);
        public Material Proceduralsky { get => proceduralsky; set => proceduralsky = value; }
        public float Exposure
        {
            get => proceduralsky.GetFloat(_Exposure);
            set
            {
                proceduralsky.SetFloat(_Exposure, value);
                skyboxParams.exposure = value;
            }
        }
        public Color SkyTint
        {
            get => proceduralsky.GetColor(_SkyTint);
            set
            {
                proceduralsky.SetColor(_SkyTint, value);
                skyboxParams.skytint = value;
            }
        }
        public float SunSize
        {
            get => proceduralsky.GetFloat(_SunSize);
            set
            {
                proceduralsky.SetFloat(_SunSize, value);
                skyboxParams.sunsize = value;
            }
        }
        public Color GroundColor
        {
            get => proceduralsky.GetColor(_GroundColor);
            set
            {
                proceduralsky.SetColor(_GroundColor, value);
                skyboxParams.groundcolor = value;
            }
        }
        public float AtmosphereThickness
        {
            get => proceduralsky.GetFloat(_AtmosphereThickness);
            set
            {
                proceduralsky.SetFloat(_AtmosphereThickness, value);
                skyboxParams.atmospherethickness = value;
            }
        }

        static readonly int _Exposure = Shader.PropertyToID("_Exposure");
        static readonly int _SkyTint = Shader.PropertyToID("_SkyTint");
        static readonly int _SunSize = Shader.PropertyToID("_SunSize");
        static readonly int _GroundColor = Shader.PropertyToID("_GroundColor");
        static readonly int _AtmosphereThickness = Shader.PropertyToID("_AtmosphereThickness");
    }


}
