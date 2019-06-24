using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace PHIBL
{
    public class DeferredShadingUtils : Singleton<DeferredShadingUtils>
    {
        static Texture2D DiffuseScatteringOnRing;
        internal static Shader AlloyDeferredSkinShader;
        static Shader AlloyBlurNormal;
        static Shader AlloyTransmissionBlit;
        static Shader AlloyAlpha;
        public AlloyDeferredRendererPlus SSSSS { get; private set; }

        static List<Material> skinMaterials;
        public static Texture2D DefaultSpotCookie;
        internal static readonly int _AlphaSSS = Shader.PropertyToID("_AlphaSSS");
        internal static readonly int _Phong = Shader.PropertyToID("_Phong");
        internal static readonly int _EdgeLength = Shader.PropertyToID("_EdgeLength");

        public DeferredShadingUtils()
        {
            AssetBundle AB = AssetBundle.LoadFromFile(Application.dataPath + "/../plugins/PHIBL/deferredskin");
            if (AB == null)
            {
                Console.WriteLine("DeferredShadingUtil: \"deferredskin\" file missing!");
            }
            DiffuseScatteringOnRing = AB.LoadAsset<Texture2D>("DiffuseScatteringOnRing");
            DefaultSpotCookie = AB.LoadAsset<Texture2D>("DefaultSpotCookie");
            AlloyBlurNormal = AB.LoadAsset<Shader>("Assets/Alloy/Scripts/DeferredRendererPlus/Shaders/BlurNormals.shader");
            AlloyTransmissionBlit = AB.LoadAsset<Shader>("Assets/Alloy/Scripts/DeferredRendererPlus/Shaders/TransmissionBlit.shader");
            GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, BuiltinShaderMode.UseCustom);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, BuiltinShaderMode.UseCustom);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseCustom);
            GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, AB.LoadAsset<Shader>("Assets/Alloy/Shaders/Alloy Deferred Skin.shader"));
            GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, AB.LoadAsset<Shader>("Assets/Alloy/Shaders/Alloy Deferred Reflections.shader"));
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, AB.LoadAsset<Shader>("Assets/Psychose Interactive/NGSS/Internal-ScreenSpaceShadows.shader"));
            AlloyDeferredSkinShader = AB.LoadAsset<Material>("skin").shader;
            AlloyAlpha = AB.LoadAsset<Material>("nip").shader;
            if (AlloyDeferredSkinShader == null)
            {
                Console.WriteLine("DeferredShadingUtil: Can't find deferred skin shader!");
            }
            AB.Unload(false);
            skinMaterials = new List<Material>();
        }

        public void InitDeferredShading(Camera camera)
        {
            if (camera.actualRenderingPath != RenderingPath.DeferredShading)
            {
                camera.renderingPath = RenderingPath.DeferredShading;
            }
            if (camera.actualRenderingPath == RenderingPath.DeferredShading)
            {
                SSSSS = camera.GetOrAddComponent<AlloyDeferredRendererPlus>();
                SSSSS.DeferredBlurredNormals = AlloyBlurNormal;
                SSSSS.DeferredTransmissionBlit = AlloyTransmissionBlit;
                SSSSS.SkinLut = DiffuseScatteringOnRing;
                SSSSS.TransmissionSettings = JSON.Utilities.LoadSettings<AlloyDeferredRendererPlus.TransmissionSettingsData>(JSON.Utilities.TransmissionSettings);
                SSSSS.SkinSettings = JSON.Utilities.LoadSettings<AlloyDeferredRendererPlus.SkinSettingsData>(JSON.Utilities.SkinSettings);
                SSSSS.SkinSettings.Lut = DiffuseScatteringOnRing;
                SSSSS.Reset();
                Refresh();
            }
            else
            {
                Console.WriteLine("DeferredShadingUtil: Fail to enable deferred shading! ");
            }
        }

        public static void SetTessellation(float phong, float edgelength)
        {
            Shader.SetGlobalFloat(_Phong, phong);
            Shader.SetGlobalFloat(_EdgeLength, edgelength);
        }
        
        public static void GetTessellation(out float phong, out float edgelength)
        {
            phong = Shader.GetGlobalFloat(_Phong);
            edgelength = Shader.GetGlobalFloat(_EdgeLength);
        }
        public static void FixNips(Renderer[] renderers)
        {
            foreach (var renderer in renderers)
            {
                if (renderer.name.Contains("cf_O_tikubi"))
                {
                    if (renderer.sharedMaterial.renderQueue != -1)
                    {
                        renderer.sharedMaterial.renderQueue = -1;
                        renderer.material.shader = AlloyAlpha;
                    }
                }
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public static void Refresh()
        {
            FindSkinMaterials();
            skinMaterials.ForEach(new Action<Material>(ReplaceSkinShader));
            //FixNips(FindObjectsOfType<SkinnedMeshRenderer>());
        }

        static void FindSkinMaterials()
        {
            skinMaterials.Clear();
            Material[] allmaterials = FindObjectsOfType<Material>();
            foreach (Material M in allmaterials)
            {
                if(M.shader.name == "Alloy/Human/Skin (Forward)")
                {
                    skinMaterials.Add(M);
                }
            }
        }

        internal static void ReplaceSkinShader(Material material)
        {
            material.renderQueue = (int)RenderQueue.Geometry;
            material.shader = AlloyDeferredSkinShader;
            material.EnableKeyword("_TESSELLATIONMODE_PHONG");
            material.EnableKeyword("_DETAIL_MULX2");
            material.EnableKeyword("EFFECT_BUMP");
            material.DisableKeyword("_RIM_ON");
        }
    }
}
