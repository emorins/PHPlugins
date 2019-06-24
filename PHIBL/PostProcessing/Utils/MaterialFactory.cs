using System;
using System.Collections.Generic;
using UnityEngine;
namespace PHIBL.PostProcessing
{
    using UnityObject = UnityEngine.Object;

    public sealed class MaterialFactory : IDisposable
    {
        Dictionary<string, Material> m_Materials;

        public MaterialFactory()
        {
            m_Materials = new Dictionary<string, Material>();
        }
        string ShaderNameToPath(string shaderName)
        {
            switch (shaderName)
            {
                default:
                    return null;
                case "Hidden/Post FX/Ambient Occlusion":
                    return "Assets/PostProcessing/Resources/Shaders/AmbientOcclusion.shader";
                case "Hidden/Post FX/Blit":
                    return "Assets/PostProcessing/Resources/Shaders/Blit.shader";
                case "Hidden/Post FX/Bloom":
                    return "Assets/PostProcessing/Resources/Shaders/Bloom.shader";
                case "Hidden/Post FX/Depth Of Field":
                    return "Assets/PostProcessing/Resources/Shaders/DepthOfField.shader";
                case "Hidden/Post FX/Eye Adaptation":
                    return "Assets/PostProcessing/Resources/Shaders/EyeAdaptation.shader";
                case "Hidden/Post FX/FXAA":
                    return "Assets/PostProcessing/Resources/Shaders/FXAA.shader";
                case "Hidden/Post FX/Temporal Anti-aliasing":
                    return "Assets/PostProcessing/Resources/Shaders/TAA.shader";
                case "Hidden/Post FX/Uber Shader":
                    return "Assets/PostProcessing/Resources/Shaders/Uber.shader";
                case "Hidden/Post FX/Lut Generator":
                    return "Assets/PostProcessing/Resources/Shaders/LutGen.shader";
                case "Hidden/Post FX/Screen Space Reflection":
                    return "Assets/PostProcessing/Resources/Shaders/ScreenSpaceReflection.shader";
                case "Hidden/Post FX/Grain Generator":
                    return "Assets/PostProcessing/Resources/Shaders/GrainGen.shader";
                case "Hidden/Post FX/Fog":
                    return "Assets/PostProcessing/Resources/Shaders/Fog.shader";
                case "Hidden/Post FX/Builtin Debug Views":
                    return "Assets/PostProcessing/Resources/Shaders/BuiltinDebugViews.shader";
                case "Hidden/Post FX/Motion Blur":
                    return "Assets/PostProcessing/Resources/Shaders/MotionBlur.shader";
            }
        }
        public Material Get(string shaderName)
        {
            Material material;

            if (!m_Materials.TryGetValue(shaderName, out material))
            {
                var AB = AssetBundle.LoadFromFile(Application.dataPath + "/../plugins/PHIBL/postprocessingstack");
                var shaderpath = ShaderNameToPath(shaderName);
                Shader shader;
                if (shaderpath == null)
                    shader = Shader.Find(shaderName);
                shader = AB.LoadAsset<Shader>(shaderpath);
                AB.Unload(false);
                if (shader == null)
                    throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));

                material = new Material(shader)
                {
                    name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
                    hideFlags = HideFlags.DontSave
                };

                m_Materials.Add(shaderName, material);
            }

            return material;
        }

        public void Dispose()
        {
            var enumerator = m_Materials.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var material = enumerator.Current.Value;
                GraphicsUtils.Destroy(material);
            }

            m_Materials.Clear();
        }
    }
}
