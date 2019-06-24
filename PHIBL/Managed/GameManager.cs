using UnityEngine;
using PHIBL.PostProcessing;
using PHIBL.PostProcessing.Utilities;
using System;
namespace PHIBL
{
    public class GameManager : MonoBehaviour
    {
        public ShadowQuality shadows = ShadowQuality.All;
        public ShadowResolution ShadowResolution = ShadowResolution.VeryHigh;
        public ShadowProjection ShadowProjection = ShadowProjection.CloseFit;
        public int shadowCascades = 4;
        public float shadowNearPlaneOffset = 0.5f;
        public float shadowDistance = 100f;
        public float shadowCascade2Split = 0.33f;
        public Vector3 shadowCascade4Split = new Vector3(0.0667f, 0.133f, 0.267f);
        public PostProcessingProfile ppp;

        //public bool enableAntialiasing;
        //public AntialiasingModel.Settings antialiasing;
        //public bool enableAmbientOcclusion;
        //public AmbientOcclusionModel.Settings ambientOcclusion;
        //public bool enableScreenSpaceReflection;
        //public ScreenSpaceReflectionModel.Settings screenSpaceReflection;
        //public bool enableDepthOfField;
        //public DepthOfFieldModel.Settings depthOfField;
        //public bool enableMotionBlur;
        //public MotionBlurModel.Settings motionBlur;
        //public bool enableEyeAdaptation;
        //public EyeAdaptationModel.Settings eyeAdaptation;
        //public bool enableBloom;
        //public BloomModel.Settings bloom;
        //public bool enableColorGrading;
        //public ColorGradingModel.Settings colorGrading;
        //public bool enableUserLut;
        //public UserLutModel.Settings userLut;
        //public bool enableChromaticAberration;
        //public ChromaticAberrationModel.Settings chromaticAberration;
        //public bool enableGrain;
        //public GrainModel.Settings grain;
        //public bool enableVignette;
        //public VignetteModel.Settings vignette;
        //public bool enableDither = true;
        //public bool enableFog = false;
        //public FogModel.Settings fog;

        //private PostProcessingController PPCtrl;

        void UpdateGlobalShadowSettings()
        {
            QualitySettings.shadows = shadows;
            QualitySettings.shadowResolution = ShadowResolution;
            QualitySettings.shadowNearPlaneOffset = shadowNearPlaneOffset;
            QualitySettings.shadowCascades = shadowCascades;
            QualitySettings.shadowProjection = ShadowProjection;
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.shadowCascade2Split = shadowCascade2Split;
            QualitySettings.shadowCascade4Split = shadowCascade4Split; 
        }
        //void UpdatePostProcessingProfile()
        //{

        //    if (ppp != null)
        //    {
        //        SetProfile();
        //    }
        //    else
        //    {
        //        Console.WriteLine("No post processing profile found! ");
        //    }
        //}

        void Awake()
        {
            FindObjectOfType<PHIBL>().PPCtrl.SetProfile(ppp);
            UpdateGlobalShadowSettings();
            //UpdatePostProcessingProfile();
        }

        //void SetProfile()
        //{
        //    Console.WriteLine("antialiasing: " + JsonUtility.ToJson(antialiasing,true));
        //    Console.WriteLine("ambientOcclusion: " + JsonUtility.ToJson(ambientOcclusion, true));
        //    Console.WriteLine("screenSpaceReflection: " + JsonUtility.ToJson(screenSpaceReflection, true));
        //    Console.WriteLine("eyeAdaptation: " + JsonUtility.ToJson(eyeAdaptation, true));
        //    Console.WriteLine("bloom: " + JsonUtility.ToJson(bloom, true));
        //    Console.WriteLine("vignette: " + JsonUtility.ToJson(vignette, true));
        //    Console.WriteLine("colorGrading: " + JsonUtility.ToJson(colorGrading, true));
        //    PPCtrl.enableAntialiasing = enableAntialiasing;
        //    PPCtrl.antialiasing = antialiasing;
        //    PPCtrl.enableAmbientOcclusion = enableAmbientOcclusion;
        //    PPCtrl.ambientOcclusion = ambientOcclusion;
        //    PPCtrl.enableScreenSpaceReflection = enableScreenSpaceReflection;
        //    PPCtrl.screenSpaceReflection = screenSpaceReflection;
        //    PPCtrl.enableDepthOfField = enableDepthOfField;
        //    PPCtrl.depthOfField = depthOfField;
        //    PPCtrl.enableMotionBlur = enableMotionBlur;
        //    PPCtrl.motionBlur = motionBlur;
        //    PPCtrl.enableEyeAdaptation = enableEyeAdaptation;
        //    PPCtrl.eyeAdaptation = eyeAdaptation;
        //    PPCtrl.enableBloom = enableBloom;
        //    PPCtrl.bloom = bloom;
        //    PPCtrl.enableColorGrading = enableColorGrading;
        //    PPCtrl.colorGrading = colorGrading;
        //    PPCtrl.enableUserLut = enableUserLut;
        //    PPCtrl.userLut = userLut;
        //    PPCtrl.enableChromaticAberration = enableChromaticAberration;
        //    PPCtrl.chromaticAberration = chromaticAberration;
        //    PPCtrl.enableGrain = enableGrain;
        //    PPCtrl.grain = grain;
        //    PPCtrl.enableVignette = enableVignette;
        //    PPCtrl.vignette = vignette;
        //    PPCtrl.enableDither = enableDither;
        //    PPCtrl.enableFog = enableFog;
        //    PPCtrl.fog = fog;
        //}

    }
}
