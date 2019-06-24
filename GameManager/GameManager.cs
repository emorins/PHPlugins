using UnityEngine;
using PHIBL.PostProcessing;
namespace PHIBL
{
    [ExecuteInEditMode]
    public class GameManager : MonoBehaviour
    {
        public ShadowQuality shadows = ShadowQuality.All;
        public ShadowResolution ShadowResolution = ShadowResolution.VeryHigh;
        public ShadowProjection ShadowProjection = ShadowProjection.CloseFit;
        public int shadowCascades = 4;
        public float shadowNearPlaneOffset = 0.1f;
        public float shadowDistance = 50f;
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
        void CloneGlobalShadowSettings()
        {
            shadows = QualitySettings.shadows;
            ShadowResolution = QualitySettings.shadowResolution;
            shadowNearPlaneOffset = QualitySettings.shadowNearPlaneOffset;
            shadowCascades = QualitySettings.shadowCascades;
            ShadowProjection = QualitySettings.shadowProjection;
            shadowDistance = QualitySettings.shadowDistance;
            shadowCascade2Split = QualitySettings.shadowCascade2Split;
            shadowCascade4Split = QualitySettings.shadowCascade4Split;
        }
        void UpdatePostProcessingProfile()
        {
            ppp = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
        }
        void Reset()
        {
            CloneGlobalShadowSettings();
            UpdatePostProcessingProfile();
            //if (ppp != null)
            //    SetProfile(ppp);
        }
        void OnValidate()
        {
            UpdateGlobalShadowSettings();
            //if (ppp != null)
            //    SetProfile(ppp);
        }
        //void Update()
        //{
        //    if (ppp != null)
        //        SetProfile(ppp);
        //}
        //void SetProfile(PostProcessingProfile __profile)
        //{
        //    enableAntialiasing = __profile.antialiasing.enabled;
        //    antialiasing = __profile.antialiasing.settings;
        //    enableAmbientOcclusion = __profile.ambientOcclusion.enabled;
        //    ambientOcclusion = __profile.ambientOcclusion.settings;
        //    enableScreenSpaceReflection = __profile.screenSpaceReflection.enabled;
        //    screenSpaceReflection = __profile.screenSpaceReflection.settings;
        //    enableDepthOfField = __profile.depthOfField.enabled;
        //    depthOfField = __profile.depthOfField.settings;
        //    enableMotionBlur = __profile.motionBlur.enabled;
        //    motionBlur = __profile.motionBlur.settings;
        //    enableEyeAdaptation = __profile.eyeAdaptation.enabled;
        //    eyeAdaptation = __profile.eyeAdaptation.settings;
        //    enableBloom = __profile.bloom.enabled;
        //    bloom = __profile.bloom.settings;
        //    enableColorGrading = __profile.colorGrading.enabled;
        //    colorGrading = __profile.colorGrading.settings;
        //    enableUserLut = __profile.userLut.enabled;
        //    userLut = __profile.userLut.settings;
        //    enableChromaticAberration = __profile.chromaticAberration.enabled;
        //    chromaticAberration = __profile.chromaticAberration.settings;
        //    enableGrain = __profile.grain.enabled;
        //    grain = __profile.grain.settings;
        //    enableVignette = __profile.vignette.enabled;
        //    vignette = __profile.vignette.settings;
        //    enableDither = __profile.dithering.enabled;
        //    enableFog = __profile.fog.enabled;
        //    fog = __profile.fog.settings;
        //}

    }
}
