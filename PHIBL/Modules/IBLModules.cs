using UnityEngine;
using IllusionPlugin;
using System;
using System.Collections;
using static PHIBL.UIUtils;

namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {

        private void ProcedualSkyboxModule()
        {
            GUILayout.Label(GUIStrings.Procedural_Skybox, titlestyle2);
            GUILayout.Space(space);
            SliderGUI(ProceduralSkybox.Exposure, 0f, 8f, 1f, x => { ProceduralSkybox.Exposure = x; EnvironmentUpdateFlag = true; }, GUIStrings.Skybox_Exposure);
            SliderGUI(ProceduralSkybox.SunSize, 0f, 1f, 0.1f, x => { ProceduralSkybox.SunSize = x; EnvironmentUpdateFlag = true; }, GUIStrings.Sun_size);
            SliderGUI(ProceduralSkybox.AtmosphereThickness, 0f, 5f, 1f, x => { ProceduralSkybox.AtmosphereThickness = x; EnvironmentUpdateFlag = true; }, GUIStrings.Atmosphere_thickness);
            SliderGUI(ProceduralSkybox.SkyTint, Color.gray, x => { ProceduralSkybox.SkyTint = x; EnvironmentUpdateFlag = true; }, GUIStrings.Sky_tint);
            SliderGUI(ProceduralSkybox.GroundColor, Color.gray, x => { ProceduralSkybox.GroundColor = x; EnvironmentUpdateFlag = true; }, GUIStrings.Ground_color);
            SliderGUI(RenderSettings.ambientIntensity, 0f, 2f, 1f, x => RenderSettings.ambientIntensity = x, GUIStrings.Ambient_intensity);
        }
        private void SkyboxModule()
        {
            GUILayout.Label(GUIStrings.Skybox, titlestyle2);
			GUILayout.Space(space);
            SliderGUI(Skybox.Rotation, 0f, 360f, 0f, x => { Skybox.Rotation = x; EnvironmentUpdateFlag = true; }, GUIStrings.Skybox_rotation);
            SliderGUI(Skybox.Exposure, 0f, 8f, 1f, x => { Skybox.Exposure = x; EnvironmentUpdateFlag = true; }, GUIStrings.Skybox_Exposure);
            SliderGUI(Skybox.Tint, Color.gray, x => { Skybox.Tint = x; EnvironmentUpdateFlag = true; }, GUIStrings.Sky_tint);
            SliderGUI(RenderSettings.ambientIntensity, 0f, 2f, 1f, x => RenderSettings.ambientIntensity = x, GUIStrings.Ambient_intensity);
        } 

        private void CubeMapModule()
        {
            scrollPosition[0] = GUILayout.BeginScrollView(scrollPosition[0]);
            int newSelection = GUILayout.SelectionGrid(selectedCubemap, CubemapFileNames, 1, UIUtils.buttonstyleStrechWidth);
            GUILayout.EndScrollView();
            if (selectedCubemap == newSelection)
                return;
            if (newSelection == 0)
            {
                ProceduralSkybox.ApplySkybox();
                ProceduralSkybox.ApplySkyboxParams();
                EnvironmentUpdateFlag = true;
            }
            else
            {
                StartCoroutine(Loadcubemap(CubemapFolder.lstFile[newSelection]));                
            }
            if (StudioMode)
            {
                var bg = FindObjectOfType<Studio.BackgroundCtrl>();
                bg.isVisible = false;
                bg.enabled = false;
                Camera.main.clearFlags = CameraClearFlags.Skybox;
            }
            selectedCubemap = newSelection;
        }

        IEnumerator Loadcubemap(FolderAssist.FileInfo file)
        {
            Console.WriteLine("Load cubemap: " + file.FileName);
            yield return null;
            IsLoading = true;
  
            if (asyncLoad)
            {
                AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(file.FullPath);
                yield return assetBundleCreateRequest;
                AssetBundle cubemapbundle = assetBundleCreateRequest.assetBundle;
                AssetBundleRequest bundleRequest1 = assetBundleCreateRequest.assetBundle.LoadAssetAsync<Material>("skybox-bg");
                yield return bundleRequest1;
                Skybox.Skyboxbg = bundleRequest1.asset as Material;
                AssetBundleRequest bundleRequest2 = assetBundleCreateRequest.assetBundle.LoadAssetAsync<Material>("skybox");
                yield return bundleRequest2;
                Skybox.Skybox = bundleRequest2.asset as Material;
                cubemapbundle.Unload(false);
                cubemapbundle = null;
                bundleRequest1 = null;
                bundleRequest2 = null;
                assetBundleCreateRequest = null;
            }
            else
            {
                AssetBundle cubemapbundle = AssetBundle.LoadFromFile(file.FullPath);
                Skybox.Skyboxbg = cubemapbundle.LoadAsset<Material>("skybox-bg");
                Skybox.Skybox = cubemapbundle.LoadAsset<Material>("skybox");
                cubemapbundle.Unload(false);
                cubemapbundle = null;
            }
            if (Skybox.Skyboxbg == null)
                Skybox.Skyboxbg = Skybox.Skybox;

            Skybox.ApplySkybox();
            Skybox.ApplySkyboxParams();
            EnvironmentUpdateFlag = true;
            Resources.UnloadUnusedAssets();
            IsLoading = false;
            //ModPrefs.SetString("PHIBL", "LastLoadedCubeMap", file.FileName);
            yield break;
        }

        bool IsLoading = false;
        bool asyncLoad;

        public static string LoadRequest { get; internal set; }
    }
}