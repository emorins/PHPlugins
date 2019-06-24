// Utility scripts for the post processing stack
// https://github.com/keijiro/PostProcessingUtilities

using UnityEngine;

namespace PHIBL.PostProcessing.Utilities
{
    
    [RequireComponent(typeof(PostProcessingBehaviour))]
    public class PostProcessingController : MonoBehaviour
    {
        #region Public structs

        public bool controlAntialiasing = true;
        public bool enableAntialiasing;
        public AntialiasingModel.Settings antialiasing;
        public bool controlAmbientOcclusion = true;
        public bool enableAmbientOcclusion;
        public AmbientOcclusionModel.Settings ambientOcclusion;
        public bool controlScreenSpaceReflection = true;
        public bool enableScreenSpaceReflection;
        public ScreenSpaceReflectionModel.Settings screenSpaceReflection;
        public bool controlDepthOfField = true;
        public bool enableDepthOfField;
        public DepthOfFieldModel.Settings depthOfField;
        public bool controlMotionBlur = true;
        public bool enableMotionBlur;
        public MotionBlurModel.Settings motionBlur;
        public bool controlEyeAdaptation = true;
        public bool enableEyeAdaptation;
        public EyeAdaptationModel.Settings eyeAdaptation;
        public bool controlBloom = true;
        public bool enableBloom;
        public BloomModel.Settings bloom;
        public bool controlColorGrading = true;
        public bool enableColorGrading;
        public ColorGradingModel.Settings colorGrading;
        public bool controlUserLut = true;
        public bool enableUserLut;
        public UserLutModel.Settings userLut;
        public bool controlChromaticAberration = true;
        public bool enableChromaticAberration;
        public ChromaticAberrationModel.Settings chromaticAberration;
        public bool controlGrain = true;
        public bool enableGrain;
        public GrainModel.Settings grain;
        public bool controlVignette = true;
        public bool enableVignette;
        public VignetteModel.Settings vignette;
        public bool enableDither = true;
        public bool controlFog = true;
        public bool enableFog = false;
        public FogModel.Settings fog;

        #endregion

        #region Private members

        // Cloned profile
        internal PostProcessingProfile _profile;
        #endregion

        #region MonoBehaviour functions
        //public void Reset()
        //{
        //    OnEnable();
        //}
        internal void SetProfile(PostProcessingProfile __profile)
        {
            enableAntialiasing = __profile.antialiasing.enabled;
            antialiasing = __profile.antialiasing.settings;

            enableAmbientOcclusion = __profile.ambientOcclusion.enabled;
            ambientOcclusion = __profile.ambientOcclusion.settings;

            enableScreenSpaceReflection = __profile.screenSpaceReflection.enabled;
            screenSpaceReflection = __profile.screenSpaceReflection.settings;

            enableDepthOfField = __profile.depthOfField.enabled;
            depthOfField = __profile.depthOfField.settings;

            enableMotionBlur = __profile.motionBlur.enabled;
            motionBlur = __profile.motionBlur.settings;

            enableEyeAdaptation = __profile.eyeAdaptation.enabled;
            eyeAdaptation = __profile.eyeAdaptation.settings;

            enableBloom = __profile.bloom.enabled;
            bloom = __profile.bloom.settings;

            enableColorGrading = __profile.colorGrading.enabled;
            colorGrading = __profile.colorGrading.settings;

            enableUserLut = __profile.userLut.enabled;
            userLut = __profile.userLut.settings;

            enableChromaticAberration = __profile.chromaticAberration.enabled;
            chromaticAberration = __profile.chromaticAberration.settings;

            enableGrain = __profile.grain.enabled;
            grain = __profile.grain.settings;

            enableVignette = __profile.vignette.enabled;
            vignette = __profile.vignette.settings;

            enableDither = __profile.dithering.enabled;

            enableFog = __profile.fog.enabled;
            fog = __profile.fog.settings;

        }
        void OnEnable()
        {          
            var postfx = GetComponent<PostProcessingBehaviour>();

            // Replace the profile with its clone.
            _profile = Instantiate(postfx.profile);
            postfx.profile = _profile;

            // Initialize the public structs with the current profile.
            SetProfile(_profile);
        }
        void Update()
        {

        }
        void LateUpdate()
        {
            if (controlAntialiasing)
            {
                if (enableAntialiasing != _profile.antialiasing.enabled)
                    _profile.antialiasing.enabled = enableAntialiasing;

                if (enableAntialiasing)
                    _profile.antialiasing.settings = antialiasing;
            }

            if (controlAmbientOcclusion)
            {
                if (enableAmbientOcclusion != _profile.ambientOcclusion.enabled)
                    _profile.ambientOcclusion.enabled = enableAmbientOcclusion;

                if (enableAmbientOcclusion)
                    _profile.ambientOcclusion.settings = ambientOcclusion;
            }

            if (controlScreenSpaceReflection)
            {
                if (enableScreenSpaceReflection != _profile.screenSpaceReflection.enabled)
                    _profile.screenSpaceReflection.enabled = enableScreenSpaceReflection;

                if (enableScreenSpaceReflection)
                    _profile.screenSpaceReflection.settings = screenSpaceReflection;
            }

            if (controlDepthOfField)
            {
                if (enableDepthOfField != _profile.depthOfField.enabled)
                    _profile.depthOfField.enabled = enableDepthOfField;

                if (enableDepthOfField)
                    _profile.depthOfField.settings = depthOfField;
            }

            if (controlMotionBlur)
            {
                if (enableMotionBlur != _profile.motionBlur.enabled)
                    _profile.motionBlur.enabled = enableMotionBlur;

                if (enableMotionBlur)
                    _profile.motionBlur.settings = motionBlur;
            }

            if (controlEyeAdaptation)
            {
                if (enableEyeAdaptation != _profile.eyeAdaptation.enabled)
                    _profile.eyeAdaptation.enabled = enableEyeAdaptation;

                if (enableEyeAdaptation)
                    _profile.eyeAdaptation.settings = eyeAdaptation;
            }

            if (controlBloom)
            {
                if (enableBloom != _profile.bloom.enabled)
                    _profile.bloom.enabled = enableBloom;

                if (enableBloom)
                    _profile.bloom.settings = bloom;
            }

            if (controlColorGrading)
            {
                if (enableColorGrading != _profile.colorGrading.enabled)
                    _profile.colorGrading.enabled = enableColorGrading;

                if (enableColorGrading)
                    _profile.colorGrading.settings = colorGrading;
            }

            if (controlUserLut)
            {
                if (enableUserLut != _profile.userLut.enabled)
                    _profile.userLut.enabled = enableUserLut;

                if (enableUserLut)
                    _profile.userLut.settings = userLut;
            }

            if (controlChromaticAberration)
            {
                if (enableChromaticAberration != _profile.chromaticAberration.enabled)
                    _profile.chromaticAberration.enabled = enableChromaticAberration;

                if (enableChromaticAberration)
                    _profile.chromaticAberration.settings = chromaticAberration;
            }

            if (controlGrain)
            {
                if (enableGrain != _profile.grain.enabled)
                    _profile.grain.enabled = enableGrain;

                if (enableGrain)
                    _profile.grain.settings = grain;
            }

            if (controlVignette)
            {
                if (enableVignette != _profile.vignette.enabled)
                    _profile.vignette.enabled = enableVignette;

                if (enableVignette)
                    _profile.vignette.settings = vignette;
            }

            if (enableDither != _profile.dithering.enabled)
                _profile.dithering.enabled = enableDither;

            
            if (enableFog != _profile.fog.enabled)
                _profile.fog.enabled = enableFog;
            if (enableFog)
                _profile.fog.settings = fog;
        }

        #endregion
    }
}
