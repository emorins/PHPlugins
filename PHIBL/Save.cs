using MessagePack;
using System;
using System.IO;
using PHIBL.PostProcessing;
using MessagePack.Formatters;

namespace PHIBL
{
    public class CustomCompositeResolver : IFormatterResolver
    {
        public static IFormatterResolver Instance = new CustomCompositeResolver();

        static readonly IFormatterResolver[] resolvers = new[]
        {
        // resolver custom types first
        MessagePack.Unity.UnityResolver2.Instance,
        // finaly use standard resolver
        MessagePackSerializer.DefaultResolver
        };

        CustomCompositeResolver() { }

        public IMessagePackFormatter<T> GetFormatter<T>() => FormatterCache<T>.formatter;

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                foreach (var item in resolvers)
                {
                    var f = item.GetFormatter<T>();
                    if (f != null)
                    {
                        formatter = f;
                        return;
                    }
                }
            }
        }
    }
    [MessagePackObject(keyAsPropertyName: true)]
    public class Profile
    {
        public FogModel fog = new FogModel();
        public AntialiasingModel antialiasing = new AntialiasingModel();
        public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();
        public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();
        public DepthOfFieldModel depthOfField = new DepthOfFieldModel();
        public MotionBlurModel motionBlur = new MotionBlurModel();
        public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();
        public BloomModel bloom = new BloomModel();
        public ColorGradingModel colorGrading = new ColorGradingModel();
        public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();
        public GrainModel grain = new GrainModel();
        public VignetteModel vignette = new VignetteModel();
        public AlloyDeferredRendererPlus.SkinSettingsData SkinSettings = new AlloyDeferredRendererPlus.SkinSettingsData();
        public AlloyDeferredRendererPlus.TransmissionSettingsData TransmissionSettings = new AlloyDeferredRendererPlus.TransmissionSettingsData();
        public bool deferred = false;
        public bool autoFocus = true;
        public float phong = 0.2f;
        public float edgeLength = 32f;
        public float focusSpeed = 5f;
        public string selectedCubemap;
        public string selectedScene;
        public int selectedSceneVariant = 0;
        public SkyboxParams SkyboxParams;
        public ProceduralSkyboxParams ProceduralSkyboxParams;

        public static void Save(string path)
        {
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();            
            var bin = LZ4MessagePackSerializer.Serialize(phibl.Snapshot(), CustomCompositeResolver.Instance);
            File.WriteAllBytes(path, bin);
            Console.WriteLine(LZ4MessagePackSerializer.ToJson(bin));
        }

        public static void Load(string path)
        {
            var phibl = UnityEngine.Object.FindObjectOfType<PHIBL>();
            var bin = File.ReadAllBytes(path);
            Console.WriteLine(LZ4MessagePackSerializer.ToJson(bin));
            var profile = LZ4MessagePackSerializer.Deserialize<Profile>(bin, CustomCompositeResolver.Instance);
            phibl.StartCoroutine(phibl.RestoreStat(profile));
        }

    }

}
