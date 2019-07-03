using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.IO;
using IllusionPlugin;
using System.Collections.Generic;
using PHIBL.PostProcessing.Utilities;
using PHIBL.PostProcessing;
using PHIBL.Utilities;
using UnityEngine.Rendering;
using static PHIBL.DeferredShadingUtils;
using static PHIBL.UIUtils;
using MessagePack;
using MessagePack.Formatters;

namespace PHIBL
{
    public partial class PHIBL : MonoBehaviour
    {
        PHIBL()
        {
            disableLightMap = ModPrefs.GetBool("PHIBL", "DisableLightMap", false, true);
            dollyZoom = ModPrefs.GetBool("PHIBL", "DollyZoom", true, true);
            shortcut = EnumUtils.ToEnum(ModPrefs.GetString("PHIBL", "Shortcut", "F5", true), KeyCode.F5);
            autoSetting = ModPrefs.GetBool("PHIBL", "AutoSetting", false, true);
            asyncLoad = ModPrefs.GetBool("PHIBL", "AsyncLoad", true, true);
            vSyncCount = ModPrefs.GetInt("PHIBL", "VSync", 1, true);
            phong = ModPrefs.GetFloat("PHIBL", "Tessellation.Phong", 0, true);
            edgelength = ModPrefs.GetFloat("PHIBL", "Tessellation.EdgeLength", 30, true);
            nippleSSS = ModPrefs.GetFloat("PHIBL", "NippleSSS", 0.5f, true);
            phong = Mathf.Clamp01(phong);
            nippleSSS = Mathf.Clamp01(nippleSSS);
            edgelength = Mathf.Clamp(edgelength, 2f, 50f);
            deferredShading = DeferredShadingUtils.Instance;
            deferredShading.InitDeferredShading(Camera.main);
            SetTessellation(phong, edgelength);
            Shader.SetGlobalFloat(_AlphaSSS, nippleSSS);
            var cubemapbundle = AssetBundle.LoadFromFile(Application.dataPath + "/../plugins/PHIBL/procedural.cube");
            ProceduralSkybox.Proceduralsky = cubemapbundle.LoadAsset<Material>("Procedural Skybox");
            cubemapbundle.Unload(false);
            probeGameObject = new GameObject("RealtimeReflectionProbe");
            probeComponent = probeGameObject.AddComponent<ReflectionProbe>();
            probeComponent.mode = ReflectionProbeMode.Realtime;
            probeComponent.boxProjection = false;
            probeComponent.intensity = 1f;
            probeComponent.importance = 100;
            probeComponent.resolution = 512;
            probeComponent.hdr = true;
            probeComponent.cullingMask = 1 | ~Camera.main.cullingMask;
            probeComponent.clearFlags = ReflectionProbeClearFlags.Skybox;
            probeComponent.size = new Vector3(50, 20, 50);
            probeGameObject.transform.position = new Vector3(0, 1.6f, 0);
            probeComponent.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            probeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
            SceneFolder = new FolderAssist();
            SceneFolder.CreateFolderInfo(Application.dataPath + "/../Scenes/", "*.scene", true, true);
            SceneFileNames = new string[SceneFolder.GetFileCount()];
            SceneFileNames = SceneFolder.GetFilenames();

            CubemapFolder = new FolderAssist();
            CubemapFolder.CreateFolderInfo(Application.dataPath + "/../Cubemaps/", "*.cube", true, true);
            CubemapFolder.lstFile.Insert(0, new FolderAssist.FileInfo(Application.dataPath + "/../plugins/PHIBL/procedural.cube"));
            CubemapFileNames = CubemapFolder.GetFilenames();

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Studio")
            {
                StudioMode = true;
                Singleton<Studio.Info>.Instance.dicLightLoadInfo[0] = new Studio.Info.LightLoadInfo()
                {
                    no = 0,
                    name = "Directional Light",
                    manifest = "studio00",
                    bundlePath = "lightsource",
                    fileName = "Directional light",
                    target = 0
                };
                Singleton<Studio.Info>.Instance.dicLightLoadInfo[1] = new Studio.Info.LightLoadInfo()
                {
                    no = 1,
                    name = "Point Light",
                    manifest = "studio00",
                    bundlePath = "lightsource",
                    fileName = "Point light",
                    target = 0
                };
                Singleton<Studio.Info>.Instance.dicLightLoadInfo[2] = new Studio.Info.LightLoadInfo()
                {
                    no = 2,
                    name = "Spot Light",
                    manifest = "studio00",
                    bundlePath = "lightsource",
                    fileName = "Spot light",
                    target = 0
                };
            }
            else
                StudioMode = false;
        }

        public LightsSerializationData LightsSerializ()
        {
            LightsSerializationData lightsSerializationData = new LightsSerializationData();

            Light[] allLights = UnityEngine.Object.FindObjectsOfType<Light>();
            foreach (Light light in allLights)
            {
                AlloyAreaLight alloyAreaLight = light.GetComponent<AlloyAreaLight>();
                lightsSerializationData.Serializ(light, alloyAreaLight);
            }
            return lightsSerializationData;
        }

        public IEnumerator LightsDeserializ(string json)
        {
            yield return new WaitForSeconds(1f);
            var scene = Singleton<Studio.Scene>.Instance;
            while (scene.isNowLoading)
            {
                yield return new WaitForEndOfFrame();
            }

            LightsSerializationData lightsSerializationData = JsonUtility.FromJson<LightsSerializationData>(json);

            Light[] allLights = UnityEngine.Object.FindObjectsOfType<Light>();

            /*
            foreach (LightSerializationData data in lightsSerializationData.lights)
            {
                foreach (Light light in allLights)
                {
                    this.LightDeserializ(light, data);
                }
            }
            */
        }
        /*
        public Light LightDeserializ (Light light, LightSerializationData data)
        {
            AlloyAreaLight alloyAreaLight = light.GetOrAddComponent<AlloyAreaLight>();

            light.range = float.Parse(data.lightData["range"]);
            light.spotAngle = float.Parse(data.lightData["spotAngle"]);
            light.cookieSize = float.Parse(data.lightData["cookieSize"]);
            light.renderMode = (LightRenderMode)(int.Parse(data.lightData["renderMode"]));
            light.bakedIndex = int.Parse(data.lightData["bakedIndex"]);
            light.cullingMask = int.Parse(data.lightData["cullingMask"]);
            light.shadowNearPlane = float.Parse(data.lightData["shadowNearPlane"]);
            light.shadowBias = float.Parse(data.lightData["shadowBias"]);
            light.shadowNormalBias = float.Parse(data.lightData["shadowNormalBias"]);
            light.color = new Color(float.Parse(data.lightData["color_r"]), float.Parse(data.lightData["color_g"]), float.Parse(data.lightData["color_b"]));
            light.intensity = float.Parse(data.lightData["intensity"]);
            light.bounceIntensity = float.Parse(data.lightData["bounceIntensity"]);
            light.type = (LightType)(int.Parse(data.lightData["type"]));
            light.shadowStrength = float.Parse(data.lightData["shadowStrength"]);
            light.shadowResolution = (LightShadowResolution)(int.Parse(data.lightData["shadowResolution"]));
            light.shadowCustomResolution = int.Parse(data.lightData["shadowCustomResolution"]);
            light.shadows = (LightShadows)(int.Parse(data.lightData["shadows"]));

            alloyAreaLight.Radius = float.Parse(data.lightData["alloy_Radius"]);
            alloyAreaLight.Length = float.Parse(data.lightData["alloy_Length"]);
            alloyAreaLight.Intensity = float.Parse(data.lightData["alloy_Intensity"]);
            alloyAreaLight.Color = new Color(float.Parse(data.lightData["alloy_Color_r"]), float.Parse(data.lightData["alloy_Color_g"]), float.Parse(data.lightData["alloy_Color_b"]));
            alloyAreaLight.HasSpecularHighlight = (int.Parse(data.lightData["alloy_HasSpecularHighlight"]) == 1);
            alloyAreaLight.IsAnimatedByClip = (int.Parse(data.lightData["alloy_IsAnimatedByClip"]) == 1);

            return light;
        }
        */

        List<Light> defaultLights;
        List<Light> directionalLights = new List<Light>();
        List<Light> pointLights = new List<Light>();
        List<Light> spotLights = new List<Light>();
        //List<ReflectionProbe> reflectionProbes= new List<ReflectionProbe>();
        private void LightsInit()
        {
            allLights = FindObjectsOfType<Light>();
            directionalLights.Clear();
            pointLights.Clear();
            spotLights.Clear();
            foreach (Light l in allLights)
            {
                if (l.isBaked)
                    continue;
                //if (Camera.main.actualRenderingPath == RenderingPath.Forward)
                //    l.renderMode = LightRenderMode.ForcePixel;
                if (!allLights.Contains(l))
                    l.cullingMask = Camera.main.cullingMask;

                var lightshafts = l.GetComponent<LightShafts.LightShafts>();
                if (lightshafts != null)
                {
                    lightshafts.m_CullingMask &= Camera.main.cullingMask;
                }
                if (l.type == LightType.Spot)
                {
                    if (l.cookie == null)
                        l.cookie = DefaultSpotCookie;
                    spotLights.Add(l);
                }
                else if (l.type == LightType.Point)
                {
                    pointLights.Add(l);
                }
                else if(l.type == LightType.Directional)
                {
                    directionalLights.Add(l);
                }
                l.GetOrAddComponent<AlloyAreaLight>().UpdateBinding();
            }
        }
        internal void ResetIBL()
        {
            if (selectedCubemap > 0)
            {
                Skybox.ApplySkybox();
                Skybox.ApplySkyboxParams();
            }
            else if(selectedCubemap == 0)
            {
                ProceduralSkybox.ApplySkybox();
                ProceduralSkybox.ApplySkyboxParams();
            }
            probeComponent.enabled = (selectedScene < 0);
            EnvironmentUpdateFlag = true;
            LightsInit();
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled = false;
            Camera.main.GetOrAddComponent<PostProcessingBehaviour>().profile = (PostProcessingProfile)ScriptableObject.CreateInstance(typeof(PostProcessingProfile));
            PPCtrl = Camera.main.GetOrAddComponent<PostProcessingController>();
            
        }
        void Update()
        {
            if (!DHHCompatibleResolved)
            {
                mainWindow = false;
                return;
            }
            //if (forceDeferred)
            //{
                if (Camera.main.actualRenderingPath != RenderingPath.DeferredShading)
                {
                    deferredShading.InitDeferredShading(Camera.main);
                }
            //}
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EnvironmentUpdateFlag = true;
            }
            if (Input.GetKeyDown(shortcut))
            {
                mainWindow = !mainWindow;
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Console.WriteLine("Camera culling mask: " + Camera.main.cullingMask);
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {

                }
            }

            if (mainWindow)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    mainWindow = false;
                }
            }
            else
            {
                windowdragflag = false;
            }
        }

        private void Start()
        {
            defaultLights = FindObjectsOfType<Light>().ToList();
            LightsInit();
            if(StudioMode)
                GetComponent<CamCtrlStudio>().StudioCameraControl.fieldOfView = ModPrefs.GetFloat("PHIBL", "StudioFOV", 23.5f, true);
            PPCtrl.enableChromaticAberration = ModPrefs.GetBool("PHIBL", "ChromaticAberration", false, true);
            PPCtrl.enableVignette = ModPrefs.GetBool("PHIBL", "Vignette", true, true);
            PPCtrl.enableAmbientOcclusion = ModPrefs.GetBool("PHIBL", "AmbientOcclusion", true, true);
            PPCtrl.enableScreenSpaceReflection = ModPrefs.GetBool("PHIBL", "ScreenSpaceReflection", false, true);
            PPCtrl.enableDepthOfField = ModPrefs.GetBool("PHIBL", "DepthOfField", false, true);
            PPCtrl.antialiasing.method = EnumUtils.ToEnum(ModPrefs.GetString("PHIBL", "AntiAliasing", "Fxaa", true), AntialiasingModel.Method.Fxaa);
            PPCtrl.enableMotionBlur = ModPrefs.GetBool("PHIBL", "MotionBlur", false, true);
            PPCtrl.enableGrain = ModPrefs.GetBool("PHIBL", "Grain", false, true);
            PPCtrl.colorGrading.basic.postExposure = ModPrefs.GetFloat("PHIBL", "EV", 0.5f, true);
            autoFocus = ModPrefs.GetBool("PHIBL", "AutoFocus", true, true);
            FocusPuller = Camera.main.gameObject.AddComponent<FocusPuller>();
            FocusPuller.enabled = autoFocus && PPCtrl.enableDepthOfField;
            if (PPCtrl.enableAmbientOcclusion)
            {
                Camera.main.GetComponent<SSAOPro>().enabled = false;
                if (StudioMode)
                {
                    Singleton<Studio.Studio>.Instance.sceneInfo.enableSSAO = false;
                }
                else
                {
                    ConfigData.ssaoEnable = false;
                }
            }
            //if (autoSetting)
            //{
                LoadOptimal();
            //}
            probeComponent.RenderProbe();
        }

        private void OnEnable()
        {
            StartCoroutine(UpdateEnvironment());
        }
        private void OnDisable()
        {
            StopAllCoroutines();            
        }
        private void OnDestroy()
        {
            Destroy(probeComponent);
        }
        private void OnGUI()
        {
            if (!mainWindow)
            {
                return;
            }
           
            if (!styleInitialized)
            {
                InitStyle();
            }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(x: scale.x * customscale, y: scale.y * customscale, z: 1f));
            if (Event.current.type == EventType.repaint)
            {
                LimitWindowRect();
                MoveTooltipRect();
            }
            windowRect = GUILayout.Window(windowID, windowRect, PHIBLWindow, "", windowstyle);
            //Vector2 pos = new Vector2
            //{
            //    x = Input.mousePosition.x * UIUtils.Screen.width / UnityEngine.Screen.width,
            //    y = (UnityEngine.Screen.height - Input.mousePosition.y) * UIUtils.Screen.height / UnityEngine.Screen.height
            //};

            if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown) /*&& !windowRect.Contains(pos * customscale)*/)
            {
               windowdragflag = false;
            }
            if (lastTooltip != "")
            {
                GUILayout.BeginArea(TooltipRect);
                GUILayout.Box(lastTooltip, boxstyle);
                GUILayout.EndArea();
            }
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        }
        #endregion

        private void PHIBLWindow(int id)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                GUI.FocusWindow(windowID);
                windowdragflag = true;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                windowdragflag = false;
            }

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            using (var verticalScope = new GUILayout.VerticalScope("box", GUILayout.Width(windowRect.width * 0.3333f)))
            {
                GUI.enabled = !IsLoading;
                SelectGUI(ref selectMenuType, new Action<SelectMenuType>(SwitchMode));
                if (selectMenuType == SelectMenuType.Cubemap)
                    CubeMapModule();
                else if (selectMenuType == SelectMenuType.Scene)
                    SceneModule();
                else
                    LightModule();
                GUI.enabled = true;
            }
            GUILayout.BeginVertical();
            menutab = GUILayout.Toolbar(menutab, GUIStrings.Titlebar, titlestyle);
            GUILayout.Space(space);
            scrollPosition[menutab + 3] = GUILayout.BeginScrollView(scrollPosition[menutab + 3]);
            switch (menutab)
            {
                case 0:
                default:                    
                    if (selectMenuType != SelectMenuType.Light)
                    {
                        if (selectedScene != -1 && IsLoading == false)
                        {
                            using (var verticalScope = new GUILayout.VerticalScope("box"))
                            {
                                SceneCustomModule();
                            }
                        }
                        if (selectMenuType == SelectMenuType.Cubemap && selectedCubemap >= 0 && IsLoading == false)
                        {                        
                            using (var verticalScope = new GUILayout.VerticalScope("box"))
                            {
                                if (selectedCubemap == 0)
                                    ProcedualSkyboxModule();
                                else
                                    SkyboxModule();
                            }
                        }
                        using (var verticalScope = new GUILayout.VerticalScope("box"))
                        {
                            ReflectionProbeModule(); 
                            if (Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
                            {
                                SreenSpaceReflectionModule();
                            }
                        }
                        using (var verticalScope = new GUILayout.VerticalScope("box"))
                        {
                            AmbientOcclusionModule();
                        }
                    }
                    else
                    {
                        if(seletedLight)
                        {
                            LightInspector(seletedLight);
                        }
                        else
                        {
                            EmptyPage(new GUIContent("Select a light source on the left panel. "));
                        }
                    }
                    break;
                case 2:
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        TonemappingModule();
                    }
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        EyeAdaptationModule();
                    }
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        NoiseModule();
                    }
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        BloomModule();
                    }
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        MotionBlurModule();
                    }
                    break;
                case 1:
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        LensModule();
                    }
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        FilterModule();
                    }
                    break;
                case 3:

                        
                    //if (Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
                    //{
                        using (var verticalScope = new GUILayout.VerticalScope("box"))
                        {
                            DeferredSkinModule();
                        }
                        using (var verticalScope = new GUILayout.VerticalScope("box"))
                        {
                            TessellationModule();
                        }
                    //}
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        AAModule();
                    }
                    UserCustomModule();
                    break;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(GUIStrings.Close_Window, buttonstyleNoStretch))
            {
                mainWindow = false;
            }
            GUILayout.Space(space);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(GUIStrings.Refresh, buttonstyleNoStretch))
            {
                Refresh();
            }
            GUILayout.Space(space);
            if (GUILayout.Button(GUIStrings.Optimal_Settings, buttonstyleNoStretch))
            {
                LoadOptimal();
            }
            GUILayout.Space(space);

            //if (GUILayout.Button(GUIStrings.Deferred_Shading, (Camera.main.actualRenderingPath == RenderingPath.DeferredShading) ? toggleButtonOn : buttonstyleNoStretch))
            //{
            //    deferredShading.InitDeferredShading(Camera.main);
            //}
            //GUILayout.Space(space);
            GUILayout.EndHorizontal();

            if (Event.current.type == EventType.Repaint)
                lastTooltip = GUI.tooltip;
            GUILayout.EndVertical();
            GUI.DragWindow();          
        }

        private void SwitchMode(SelectMenuType selectedmode)
        {
            if(selectedmode == SelectMenuType.Cubemap)
            {
                Camera.main.GetComponent<Skybox>().enabled = true;
                RenderSettings.ambientMode = AmbientMode.Skybox;
                ResetIBL();
            }
            else if(selectedmode == SelectMenuType.Scene)
            {
                if (selectedScene != -1)
                {
                    probeComponent.enabled = false;
                    Camera.main.GetComponent<Skybox>().enabled = false;
                    RestoreSceneRenderSettings();
                }
            }
        }

        private void Refresh()
        {
            LightsInit();
            RendererSetup.SetupProbes();
            RenderSettings.ambientMode = AmbientMode.Skybox;
            EnvironmentUpdateFlag = true;
        }

        private void LoadOptimal()
        {
            RenderSettings.ambientIntensity = 1f;
            RenderSettings.reflectionIntensity = 1f;
            RenderSettings.ambientMode = AmbientMode.Skybox;
            probeComponent.intensity = 1f;

            LightsInit();
            foreach (Light l in allLights)
            {                
                if (l.type == LightType.Directional && (l.name == "CharaLight_back" || l.name == "MainLight" || l.name == "map&chara"))
                {
                    l.shadowStrength = 0.5f;
                    l.shadowBias = 0.004f;
                    l.shadowNormalBias = 0.6f;
                    l.GetComponent<AlloyAreaLight>().Radius = 0f;
                }
            }
            rpRate = ReflectionProbeRefreshRate.OnDemand;
            PPCtrl.enableBloom = true;
            PPCtrl.enableColorGrading = true;
            PPCtrl.enableEyeAdaptation = true;
            PPCtrl.enableAntialiasing = true;
            PPCtrl.enableDither = true;
            PPCtrl.ambientOcclusion.intensity = 0.45f;
            PPCtrl.ambientOcclusion.radius = 0.35f;
            PPCtrl.ambientOcclusion.highPrecision = true;
            PPCtrl.ambientOcclusion.forceForwardCompatibility = true;
            PPCtrl.ambientOcclusion.ambientOnly = true;
            PPCtrl.screenSpaceReflection = ScreenSpaceReflectionModel.Settings.defaultSettings;
            PPCtrl.screenSpaceReflection.reflection.widthModifier = 0.08f;
            PPCtrl.screenSpaceReflection.reflection.blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased;
            PPCtrl.screenSpaceReflection.intensity.reflectionMultiplier = 1f;
            PPCtrl.screenSpaceReflection.intensity.fresnelFade = 1f;
            PPCtrl.screenSpaceReflection.intensity.fresnelFadePower = 0.5f;
            PPCtrl.screenSpaceReflection.reflection.maxDistance = 100f;
            PPCtrl.screenSpaceReflection.reflection.iterationCount = 200;
            PPCtrl.screenSpaceReflection.intensity.fadeDistance = 100f;
            PPCtrl.screenSpaceReflection.screenEdgeMask.intensity = 0.1f;
            PPCtrl.antialiasing.fxaaSettings.preset = AntialiasingModel.FxaaPreset.ExtremeQuality;
            PPCtrl.eyeAdaptation.adaptationType = EyeAdaptationModel.EyeAdaptationType.Progressive;
            PPCtrl.eyeAdaptation.dynamicKeyValue = true;
            PPCtrl.eyeAdaptation.highPercent = 95f;
            PPCtrl.eyeAdaptation.lowPercent = 35f;
            PPCtrl.eyeAdaptation.logMax = 4;
            PPCtrl.eyeAdaptation.logMin = -8;
            PPCtrl.eyeAdaptation.maxLuminance = 2f;
            PPCtrl.eyeAdaptation.minLuminance = -5f;
            PPCtrl.eyeAdaptation.speedDown = 1.2f;
            PPCtrl.eyeAdaptation.speedUp = 2.5f;
            PPCtrl.bloom.bloom.intensity = 0.15f;
            PPCtrl.bloom.bloom.threshold = 1.7f;
            PPCtrl.bloom.bloom.radius = 4f;
            PPCtrl.bloom.bloom.softKnee = 0.6f;
            PPCtrl.bloom.bloom.antiFlicker = true;
            PPCtrl.colorGrading.tonemapping.tonemapper = ColorGradingModel.Tonemapper.ACES;
            //PPCtrl.colorGrading.basic = ColorGradingModel.BasicSettings.defaultSettings;
            PPCtrl.colorGrading.curves = ColorGradingModel.CurvesSettings.defaultSettings;
            PPCtrl.colorGrading.channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings;
            PPCtrl.colorGrading.colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings;
            PPCtrl.userLut = UserLutModel.Settings.defaultSettings;
            PPCtrl.depthOfField.useCameraFov = true;
            PPCtrl.depthOfField.kernelSize = DepthOfFieldModel.KernelSize.VeryLarge;
            PPCtrl.motionBlur.frameBlending = 0f;
            ReflectionProbeChangeMode(rpRate);
        }
        public Profile Snapshot()
        {
            var profile = SavePostProcessingProfile();
            //if (Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
            //{
                profile.deferred = true;
                profile.SkinSettings = deferredShading.SSSSS.SkinSettings;
                profile.TransmissionSettings = deferredShading.SSSSS.TransmissionSettings;
                DeferredShadingUtils.GetTessellation(out profile.phong, out profile.edgeLength);
            //}
            if (selectedCubemap == 0)
            {
                ProceduralSkybox.SaveSkyboxParams();
                profile.selectedCubemap = CubemapFileNames[selectedCubemap];
            }
            else if (selectedCubemap > 0)
            {
                Skybox.SaveSkyboxParams();
                profile.selectedCubemap = CubemapFileNames[selectedCubemap];
            }
            profile.ProceduralSkyboxParams = ProceduralSkybox.skyboxParams;
            profile.SkyboxParams = Skybox.skyboxParams;
            if (selectedScene >= 0)
            {
                profile.selectedScene = SceneFileNames[selectedScene];
                profile.selectedSceneVariant = SceneVariant;
            }
            return profile;
        }

        private Profile SavePostProcessingProfile()
        {
            return new Profile()
            {
                ambientOcclusion = PPCtrl._profile.ambientOcclusion,
                antialiasing = PPCtrl._profile.antialiasing,
                screenSpaceReflection = PPCtrl._profile.screenSpaceReflection,
                depthOfField = PPCtrl._profile.depthOfField,
                motionBlur = PPCtrl._profile.motionBlur,
                eyeAdaptation = PPCtrl._profile.eyeAdaptation,
                bloom = PPCtrl._profile.bloom,
                colorGrading = PPCtrl._profile.colorGrading,
                chromaticAberration = PPCtrl._profile.chromaticAberration,
                grain = PPCtrl._profile.grain,
                vignette = PPCtrl._profile.vignette,
                fog = PPCtrl._profile.fog,
                autoFocus = autoFocus,
                focusSpeed = FocusPuller.speed
            };
        }

        public IEnumerator RestoreStat(Profile profile)
        {
            yield return new WaitForSeconds(1f);
            var scene = Singleton<Studio.Scene>.Instance;
            while (scene.isNowLoading)
            {
                yield return new WaitForEndOfFrame();
            }

            if (profile.selectedScene != null && profile.selectedScene.Length != 0)
            {
                if (SceneFileNames.Contains(profile.selectedScene))
                {
                    var newselection = Array.FindIndex(SceneFileNames, x => x == profile.selectedScene);
                    if (selectedScene != newselection)
                    {
                        StartCoroutine(LoadScene(SceneFolder.lstFile[newselection], selectedScene != -1, profile.selectedSceneVariant));
                        selectedScene = newselection;
                        SceneVariant = profile.selectedSceneVariant;
                    }
                    else if (profile.selectedSceneVariant != SceneVariant)
                    {
                        StartCoroutine(LoadSceneVariant(profile.selectedSceneVariant));
                        SceneVariant = profile.selectedSceneVariant;
                    }
                }
            }
            if (profile.selectedCubemap != null && profile.selectedCubemap.Length != 0)
            {
                if (CubemapFileNames.Contains(profile.selectedCubemap))
                {
                    var newcubemap = Array.FindIndex(CubemapFileNames, x => x == profile.selectedCubemap);
                    //if (selectedCubemap != newcubemap)
                    //{
                    ProceduralSkybox.skyboxParams = profile.ProceduralSkyboxParams;
                    Skybox.skyboxParams = profile.SkyboxParams;

                    if (newcubemap == 0)
                    {
                        ProceduralSkybox.ApplySkybox();
                        ProceduralSkybox.ApplySkyboxParams();
                        EnvironmentUpdateFlag = true;
                    }
                    else
                    {
                        StartCoroutine(Loadcubemap(CubemapFolder.lstFile[newcubemap]));
                    }
                    if (StudioMode)
                    {
                        var bg = FindObjectOfType<Studio.BackgroundCtrl>();
                        bg.isVisible = false;
                        bg.enabled = false;
                        Camera.main.clearFlags = CameraClearFlags.Skybox;
                    }
                    selectedCubemap = newcubemap;
                    //}
                }
            }
            LoadPostProcessingProfile(profile);
            //if (profile.deferred && Camera.main.actualRenderingPath == RenderingPath.DeferredShading)
            //{
                deferredShading.SSSSS.TransmissionSettings = profile.TransmissionSettings;
                deferredShading.SSSSS.SkinSettings = profile.SkinSettings;
                deferredShading.SSSSS.Reset();
                DeferredShadingUtils.SetTessellation(profile.phong, profile.edgeLength);
            //}
        }

        private void LoadPostProcessingProfile(Profile profile)
        {
            PPCtrl.ambientOcclusion = profile.ambientOcclusion.settings;
            PPCtrl.antialiasing = profile.antialiasing.settings;
            PPCtrl.screenSpaceReflection = profile.screenSpaceReflection.settings;
            PPCtrl.depthOfField = profile.depthOfField.settings;
            PPCtrl.motionBlur = profile.motionBlur.settings;
            PPCtrl.eyeAdaptation = profile.eyeAdaptation.settings;
            PPCtrl.bloom = profile.bloom.settings;
            PPCtrl.colorGrading = profile.colorGrading.settings;
            PPCtrl.chromaticAberration = profile.chromaticAberration.settings;
            PPCtrl.grain = profile.grain.settings;
            PPCtrl.vignette = profile.vignette.settings;
            PPCtrl.fog = profile.fog.settings;
            PPCtrl.enableAmbientOcclusion = profile.ambientOcclusion.enabled;
            PPCtrl.enableAntialiasing = profile.antialiasing.enabled;
            PPCtrl.enableScreenSpaceReflection = profile.screenSpaceReflection.enabled;
            PPCtrl.enableDepthOfField = profile.depthOfField.enabled;
            PPCtrl.enableMotionBlur = profile.motionBlur.enabled;
            PPCtrl.enableEyeAdaptation = profile.eyeAdaptation.enabled;
            PPCtrl.enableBloom = profile.bloom.enabled;
            PPCtrl.enableColorGrading = profile.colorGrading.enabled;
            PPCtrl.enableChromaticAberration = profile.chromaticAberration.enabled;
            PPCtrl.enableGrain = profile.grain.enabled;
            PPCtrl.enableVignette = profile.vignette.enabled;
            PPCtrl.enableFog = profile.fog.enabled;
            AutoFocusAction(profile.autoFocus);
            FocusPuller.speed = profile.focusSpeed;
        }

        FocusPuller FocusPuller;
        DeferredShadingUtils deferredShading;
        internal bool autoSetting = false;
        //bool forceDeferred = false;
        internal bool DHHCompatibleResolved = true;
        internal PostProcessingController PPCtrl;
        public GameObject probeGameObject;
        ReflectionProbe probeComponent;
        FolderAssist SceneFolder;
        FolderAssist CubemapFolder;
        string[] CubemapFileNames;
        public static bool EnvironmentUpdateFlag = false;
        internal const int windowID = 19853;
        int selectedCubemap = -1;
        internal bool mainWindow = false;
        ReflectionProbeRefreshRate rpRate;
        int ReflectionProbeResolution;
        int menutab = 0;
        string lastTooltip = "";
        SkyboxManager Skybox = new SkyboxManager();
        ProceduralSkyboxManager ProceduralSkybox = new ProceduralSkyboxManager();
        internal KeyCode shortcut;
        Light[] allLights;
        bool dollyZoom;
        bool autoFocus = true;
        bool StudioMode;
        internal bool windowdragflag = false;
        string[] SceneFileNames;
        SelectMenuType selectMenuType = SelectMenuType.Cubemap;
        internal static ScreenShotSize screenShotSize = ScreenShotSize.FourThirds;
        IEnumerator UpdateEnvironment()
        {
            while (true)
            {
                yield return null;
                if (EnvironmentUpdateFlag)
                {
                    //if(selectedScene == -1)
                    DynamicGI.UpdateEnvironment();                    
                    var rp = FindObjectsOfType<ReflectionProbe>();
                    for(int i = 0; i < rp.Length; i++)
                    {
                        if (rp[i].refreshMode == ReflectionProbeRefreshMode.ViaScripting)
                        {
                            //yield return null;
                            rp[i].timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
                            Console.WriteLine(rp[i].name + " Render Probe!");
                            rp[i].RenderProbe();
                        }
                    }
                    EnvironmentUpdateFlag = false;
                }                
            }
        }

    }
    internal enum SelectMenuType
    {
        Cubemap,
        Scene,
        Light
    }
    internal enum ScreenShotSize
    {
        Original,
        FourThirds,
        ThreeHalves,
        Double
    }
    enum ReflectionProbeRefreshRate
    {
        OnDemand,
        Low,
        High,
        Extreme
    }
}
