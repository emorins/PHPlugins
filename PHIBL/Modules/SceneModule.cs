using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.IO;
using static PHIBL.UIUtils;
namespace PHIBL
{
    partial class PHIBL : MonoBehaviour
    {
        private int selectedScene = -1;

        void SceneModule()
        {
            scrollPosition[1] = GUILayout.BeginScrollView(scrollPosition[1]);
            int newSelection = GUILayout.SelectionGrid(selectedScene, SceneFileNames, 1, buttonstyleStrechWidth);
            GUILayout.EndScrollView();
            if (selectedScene == newSelection)
                return;
            StartCoroutine(LoadScene(SceneFolder.lstFile[newSelection], selectedScene != -1));
            selectedScene = newSelection;
        }
        void SwitchScene(int n)
        {
            StartCoroutine(LoadSceneVariant(n));
        }
        void FogAction(bool enable)
        {
            RenderSettings.fog = enable;
            PPCtrl.enableFog = enable;
        }
        void SceneCustomModule()
        {
            GUILayout.Label("Scene", titlestyle2);
            GUILayout.Space(space);
            if(scenePaths.Length > 1)
                SelectGUI(ref SceneVariant, new GUIContent(" Scene Variant "), ScenePathToName(scenePaths), new Action<int>(SwitchScene));
            SliderGUI(DynamicGI.indirectScale, 0f, 5f, 1f, x => DynamicGI.indirectScale = x, " Indirect Lighting Scale: ");
            SliderGUI(DynamicGI.updateThreshold, 0f, 100f, 1f, x => DynamicGI.updateThreshold = x, " GI Update Threshold: ");
            ToggleGUITitle(RenderSettings.fog, new GUIContent("Fog"), new Action<bool>(FogAction));
            if(RenderSettings.fog)
            {
                SliderGUI(RenderSettings.fogColor, Color.gray, x => RenderSettings.fogColor = x," Fog Color ");
                RenderSettings.fogMode = SelectGUI(RenderSettings.fogMode, new GUIContent(" Fog Mode "), 0);
                if (RenderSettings.fogMode == FogMode.Linear)
                {
                    SliderGUI(RenderSettings.fogStartDistance, 0f, 100f, x => RenderSettings.fogStartDistance = x, " Fog Start Distance ", "", "N1");
                    SliderGUI(RenderSettings.fogEndDistance, 0f, 1000f, x => RenderSettings.fogEndDistance = x, " Fog End Distance ", "", "N1");
                }
                else
                {
                    SliderExGUI(value: RenderSettings.fogDensity, reset: 0.1f, power: 0.33f, SetOutput: x => RenderSettings.fogDensity = x, labeltext: " Fog Density ");
                }
                ToggleGUI(ref PPCtrl.fog.excludeSkybox, new GUIContent(" Exclude Skybox "));
            }
        }

        public GameObject[] RootGOs { get; private set; }

        public AssetBundle Scenebundle { get; private set; }

        string[] ScenePathToName(string[] paths)
        {
            string[] sceneName = new string[paths.Length];
            var i = 0;
            foreach (var path in paths)
            {
                sceneName[i] = Path.GetFileNameWithoutExtension(path);
                i++;
            }
            return sceneName;
        }
        IEnumerator LoadSceneVariant(int variant)
        {
            if (asyncLoad)
            {
                yield return null;
                var asyncOperation = SceneManager.LoadSceneAsync(scenePaths[variant], LoadSceneMode.Single);
                while (!asyncOperation.isDone)
                {
                    yield return null;                 
                }
            }
            else
            {
                SceneManager.LoadScene(scenePaths[variant], LoadSceneMode.Single);
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePaths[variant]));
            }
            PPCtrl.enableFog = RenderSettings.fog;
            CloneSceneRenderSettings();
            LightsInit();
            ReflectionProbeChangeMode(rpRate);
            //EnvironmentUpdateFlag = true;
        }
        IEnumerator LoadScene(FolderAssist.FileInfo file, bool unloadOldScene = false, int variant = 0)
        {
            IsLoading = true;
            //DisableLightMap(true);
            if (StudioMode)
            {
                var scene = Studio.Scene.Instance;

                while (true)
                {
                    Console.WriteLine("Added scene name: "+scene.addSceneName);
                    if (scene.addSceneName == string.Empty)
                        break;
                    scene.UnLoad();
                }
            }
            deferredShading.InitDeferredShading(Camera.main);
            AssetBundle AB;
            if (asyncLoad)
            {
                AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(file.FullPath);
                yield return assetBundleCreateRequest;
                AB = assetBundleCreateRequest.assetBundle;
            }
            else
            {
                AB = AssetBundle.LoadFromFile(file.FullPath);
            }
            scenePaths = AB.GetAllScenePaths();
            SceneVariant = 0;
            if (!unloadOldScene)
            {
                RootGOs = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (var GO in RootGOs)
                {
                    Console.WriteLine("Root Gameobject: " + GO.name);
                    DontDestroyOnLoad(GO);
                    //SceneManager.MoveGameObjectToScene(GO, newscene);
                }
            }

            if (asyncLoad)
            {
                var asyncOperation = SceneManager.LoadSceneAsync(scenePaths[variant], LoadSceneMode.Single);
                asyncOperation.allowSceneActivation = false;

                while (!asyncOperation.isDone)
                {
                    yield return null;
                    if (asyncOperation.progress > 0.899f && !asyncOperation.allowSceneActivation)
                    {
                        if (SceneManager.GetActiveScene().buildIndex != -1)
                        {
                            foreach (var l in defaultLights)
                            {
                                if (l.name == "CharaLight_back")
                                    l.GetComponent<AlloyAreaLight>().Intensity = 0f;
                                else
                                    l.enabled = false;
                            }
                            var map = FindObjectOfType<Map>();
                            if (null != map)
                            {
                                GlobalData.showMap = false;
                                map.ChangeShow();
                            }
                        }
                        asyncOperation.allowSceneActivation = true;
                    }
                }
            }
            else
            {
                if (SceneManager.GetActiveScene().buildIndex != -1)
                {
                    foreach (var l in defaultLights)
                    {
                        if (l.name == "CharaLight_back")
                            l.GetComponent<AlloyAreaLight>().Intensity = 0f;
                        else
                            l.enabled = false;
                    }
                }
                SceneManager.LoadScene(scenePaths[variant], LoadSceneMode.Single);
                var map = FindObjectOfType<Map>();
                if (null != map)
                {
                    GlobalData.showMap = false;
                    map.ChangeShow();
                }
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePaths[variant]));
            }


            if (unloadOldScene)
            {
                Scenebundle.Unload(true);
            }
            Scenebundle = AB;
            if (StudioMode)
            {
                var bg = FindObjectOfType<Studio.BackgroundCtrl>();
                bg.isVisible = false;
                bg.enabled = false;
                Camera.main.clearFlags = CameraClearFlags.Skybox;
            }
            Camera.main.GetOrAddComponent<FlareLayer>().enabled = true;
            Camera.main.GetComponent<Skybox>().enabled = false;
            probeComponent.enabled = false;
            RendererSetup.SetupProbes();
            //EnvironmentUpdateFlag = true;
            Resources.UnloadUnusedAssets();
            IsLoading = false;
            PPCtrl.enableFog = RenderSettings.fog;
            CloneSceneRenderSettings();
            LightsInit();
            ReflectionProbeChangeMode(rpRate);
            yield break;
        }
        CustomRenderSettings sceneRenderSettings;
        private int SceneVariant;
        private string[] scenePaths = new string[0];

        void RestoreSceneRenderSettings()
        {
            RenderSettings.ambientSkyColor = sceneRenderSettings.ambientSkyColor;
            RenderSettings.ambientEquatorColor = sceneRenderSettings.ambientEquatorColor;
            RenderSettings.ambientGroundColor = sceneRenderSettings.ambientGroundColor;
            RenderSettings.ambientIntensity = sceneRenderSettings.ambientIntensity;
            RenderSettings.ambientLight = sceneRenderSettings.ambientLight;
            RenderSettings.ambientMode = sceneRenderSettings.ambientMode;
            RenderSettings.skybox = sceneRenderSettings.skybox;
            EnvironmentUpdateFlag = true;
        }
        void CloneSceneRenderSettings()
        {
            sceneRenderSettings.ambientSkyColor = RenderSettings.ambientSkyColor;
            sceneRenderSettings.ambientEquatorColor = RenderSettings.ambientEquatorColor;
            sceneRenderSettings.ambientGroundColor = RenderSettings.ambientGroundColor;
            sceneRenderSettings.ambientIntensity = RenderSettings.ambientIntensity;
            sceneRenderSettings.ambientLight = RenderSettings.ambientLight;
            sceneRenderSettings.ambientMode = RenderSettings.ambientMode;
            sceneRenderSettings.skybox = RenderSettings.skybox;
        }
    }
    
    struct CustomRenderSettings
    {
        public Color ambientSkyColor;
        public Color ambientEquatorColor;
        public Color ambientGroundColor;
        public float ambientIntensity;
        public Color ambientLight;
        public AmbientMode ambientMode;
        public Material skybox;
    }
}
