using UnityEngine;

namespace PHIBL
{
    public class GUIStrings : Utility.Xml.Data
    {
        public GUIStrings(string elementName):base(elementName) { }
        internal override void Init()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.ChineseTraditional:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.Chinese:
                    close_window = " 关闭窗口 ";
                    reset = " 重置 ";
                    vsync = " 垂直同步 ";
                    vsynctooltips = "重启游戏后生效";
                    disable_vs_enable_0 = " 禁用 ";
                    disable_vs_enable_1 = " 启用 ";
                    dolly_zoom = "滑动变焦";
                    depth_of_view = "景深";
                    field_of_view = "视场";
                    vignette = "渐晕";
                    filters = "滤镜";
                    chromatic_aberration = "色差";
                    global_settings = "全局设定";
                    auto_refresh = " 自动刷新 ";
                    async_load = " 异步读取 ";
                    async_load_tootips = "开启后可以消除 Cubemap 载入时的卡顿，但会增加载入时间。";
                    force_experimental_features = " 自动开启实验性功能 ";
                    force_experimental_features_tooltips = "自动开启延期着色、曲面细分和PCSS。";
                    auto_setting = " 自动最佳设置 ";
                    auto_refresh_tooltips = "搜索所有的蒙皮网格渲染器并对其使用反射探针, 如果延期着色功能开启也会搜索并替换所有的皮肤着色器。可能会影响性能。";
                    auto_setting_tooltips = "场景切换时自动载入最佳设置。会自动禁用 DHH 画质调整模块";
                    tessellation = "曲面细分";
                    skin_scattering = "皮肤散射";
                    transmission = "透射";
                    custom_window = " 自定义窗口";
                    custom_window_remember = " 记住大小与位置 ";
                    optimal_settings = " 载入最佳设置 ";
                    experimental_features = "开启实验性功能";
                    experimental_features_tooltips = "开启延期着色和曲面细分。一旦开启直到场景切换前不能关闭。";
                    dithering = "颜色抖动";
                    ditheringtooltips = "故意增加噪点使量化误差随机化，有助于防止图像出现大面积色带。";
                    reflection = "反射";
                    directional_light = "平行光";
                    refresh_v2 = " 刷新 ";
                    eye_adaptation = "自动曝光";
                    eye_adaptation_tooltips = "根据图像亮度范围自动调整曝光值";
                    exposure_value = " 曝光值：(EV)";
                    exposure_value_tooltips = "以EV为单位的曝光值调整";
                    tonemapping = "色调映射";
                    tonemapping_tooltips = "把高动态范围的图像映射到适合屏幕显示的范围内";
                    refresh_v2_tooltips = "搜索所有光源，并修复可能的问题，刷新环境和反射探针";
                    titlebar_0 = "光照";
                    titlebar_1 = "镜头";
                    titlebar_2 = "视觉";
                    titlebar_3 = "其他";
                    bloom = "过度曝光溢出";
                    bloom_tooltip = "用于模拟真实相机成像时的瑕疵。在使用高动态范围渲染时应该把阈值设在低动态范围之上（也就是大于1）";
                    window_height = " 窗口高度 ";
                    window_width = " 窗口宽度 ";
                    light_intensity = " 光强：";
                    colorpicker_blue = " 蓝色通道： ";
                    colorpicker_green = " 绿色通道： ";
                    colorpicker_red = " 红色通道： ";
                    colorpicker_saturation = " 饱和度： ";
                    colorpicker_value = " 亮度： ";
                    colorpicker_hue = " 色相： ";
                    loading = " 载入中... ";
                    load_cubemap = " 载入 Cubemap： ";
                    break;
                case SystemLanguage.Japanese:
                    reset = " リセット ";
                    disable_vs_enable_0 = "無効にする";
                    disable_vs_enable_1 = "有効にする";
                    refresh_v2 = "リフレッシュする";
                    titlebar_0 = "照明";
                    titlebar_1 = "レンズ";
                    titlebar_2 = "視覚";
                    titlebar_3 = "その他";
        
                    break;
                default:
                    break;
            }
        }
        static string save = " Save ";
        static string focus_speed = " Focus Speed ";
        static string ssr = "Sreen-Space Reflection";
        static string ssr_tooltips = "Screen Space Reflection can be used to obtain more detailed reflections than other methods such as Cubemaps or Reflection Probes. Objects using Cubemaps for reflection are unable to obtain self reflection and Reflection Probe reflections are limited in their accuracy.";
        static string ssr_resolution = " Resolution ";
        static string ssr_resolution_tooltips = "The size of the buffer used for resolve. Half resolution SSR is much faster, but less accurate.";
        static string ssr_reflectBackfaces = " Reflect Backfaces";
        static string ssr_reflectBackfaces_tooltips = "Renders the scene by culling all front faces and uses the resulting texture for estimating what the backfaces might look like when a point on the depth map is hit from behind.";
        static string low_vs_high_0 = "High";
        static string low_vs_high_1 = "Low";
        static string ssao_ambientonly = " Ambient Only ";
        static string ssao_ambientonly_tooltips = "Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.";
        static string ssao_radius = " Radius: ";
        static string ssao_radius_tooltips = "Radius of sample points, which affects extent of darkened areas.";
        static string ssao_intensity = " Intensity: ";
        static string ssao_intensity_tooltips = "Degree of darkness produced by the effect.";
        static string ssao = "Ambient Occlusion";
        static string ssao_tooltips = "SSAO effect provided by post processing stack, disable SSAOpro before trying this one";
        static string ssao_forceForwardCompatibility = " Force Forward Compatibility ";
        static string ssao_forceForwardCompatibility_tooltips = "Forces compatibility with Forward rendered objects when working with the Deferred rendering path.";
        static string ssao_downsampling = " Downsampling: ";
        static string ssao_downsampling_tooltips = "Halves the resolution of the effect to increase performance at the cost of visual quality.";
        static string ssao_samplecount = " SampleCount: ";
        static string ssao_samplecount_tooltips = "Number of sample points, which affects quality and performance.";
        private static string ssao_highPrecision = " High Precision (Forward) ";
        private static string ssao_highPrecision_tooltips = "Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.";
        static string disableLightMap = " Disable Lightmap ";
        static string disableLightMap_tooltips = "Disable Lightmap and enable shadow cast on static objects";
        static string close_window = " Close Window ";
        static string dolly_zoom = "Dolly Zoom";
        static string procedural_skybox = "Procedural Skybox";
        static string skybox = "Skybox";
        static string skybox_exposure = " Skybox Exposure: ";
        static string sun_size = " Sun Size : ";
        static string atmosphere_thickness = " Atmosphere Thickness: ";
        static string sky_tint = " Sky Tint: ";
        static string ground_color = " Gound Color: ";
        static string ambient_intensity = " Ambient Intensity: ";
        static string skybox_rotation = " Skybox Rotation: ";
        static string reflection_probe_refresh_rate = " Reflection Probe Refresh Rate: ";
        static string reflection_probe_refresh = " Refresh ";
        static string reflection_refresh_rate_array_0 = "On Demand";
        static string reflection_refresh_rate_array_1 = "Low";
        static string reflection_refresh_rate_array_2 = "High";
        static string reflection_refresh_rate_array_3 = "Extreme";
        static string reflection_probe_resolution = " Reflection Probe Resolution: ";
        static string loading = " Loading... ";
        static string load_cubemap = " Load Cubemaps: ";
        static string bloom_intensity = " Intensity: ";
        static string bloom_intensity_tooltips = "Blend factor of the result image.";
        static string bloom_threshold = " Threshold: ";
        static string bloom_threshold_tooltips = "Filters out pixels under this level of brightness.";
        static string bloom_softknee = " Softknee: ";
        static string bloom_softknee_tooltips = "Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).";
        static string bloom_radius = " Radius: ";
        static string bloom_radius_tooltips = "Changes extent of veiling effects in a screen resolution-independent fashion.";
        static string bloom_antiflicker = " AntiFlicker ";
        static string bloom_antiflicker_tooltips = "Reduces flashing noise with an additional filter.";
        static string bloom = "Bloom";
        static string bloom_tooltip = "Bloom is an effect used to reproduce an imaging artifact of real-world cameras. In HDR rendering a Bloom effect should only affects areas of brightness above LDR range (above 1) by setting the Threshold parameter just above this value.";
        static string tonemapping = "Tone Mapping";
        static string tonemapping_tooltips = "Remap HDR values of an image into a range suitable to be displayed on screen";
        static string tonemapping_none = "None";
        static string tonemapping_none_tooltips = "Not Recommended. ";
        static string tonemapping_ACES = "Filmic (ACES)";
        static string tonemapping_ACES_tooltips = "Recommended. A close approximation of the reference ACES tonemapper for a more filmic look. ";
        static string tonemapping_neutral = "Neutral";
        static string tonemapping_neutral_tooltips = "Game Default. Only does range-remapping. ";
        static string tonemapping_neutralBlackIn = " Black In: ";
        static string tonemapping_neutralBlackOut = " Black Out: ";
        static string tonemapping_neutralWhiteIn = " White In: ";
        static string tonemapping_neutralWhiteOut = " White Out: ";
        static string tonemapping_neutralWhiteLevel = " White Level: ";
        static string tonemapping_neutralWhiteClip = " White Clip: ";
        static string reflection = "Reflection Probe";
        static string reflection_bounces = " Reflection Bounces ";
        static string transmission = "Transmission";
        static string skin_scattering = "Skin Scattering";
        static string reset = " Reset ";
        static string vsync = " VSync ";
        static string vsynctooltips = "Apply on next run. ";
        static string disable_vs_enable_0 = " Disable ";
        static string disable_vs_enable_1 = " Enable ";
        static string dithering = "Dithering";
        static string ditheringtooltips = "Intentionally applying noise as to randomize quantization error. This prevents large-scale patterns such as color banding in images.";
        static string depth_of_view = "Depth of View";
        static string dof_focal_distance = " Focal Distance: ";
        static string dof_focal_distance_tooltips = "Distance to the point of focus.";
        static string auto_focus = "Auto Focus";
        static string auto_focus_tooltips = "Focus on mouse. ";
        static string dof_aperture = " Aperture: ";
        static string dof_aperture_tooltips = "Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.";
        static string dof_focallength = " Focal Length: ";
        static string dof_focallength_tooltips = "Distance between the lens and the film. The larger the value is, the shallower the depth of field is.";
        static string dof_auto_calc = "Auto Calculate";
        static string dof_auto_calc_tooltips = "Calculate focal length from camera field-of-view";
        static string dof_kernel = " Kernel Size: ";
        static string dof_kernel_tooltips = "Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).";
        static string dof_kernel_size_0 = "Small";
        static string dof_kernel_size_1 = "Medium";
        static string dof_kernel_size_2 = "Large";
        static string dof_kernel_size_3 = "Very Large";
        static string field_of_view = "Field of View";
        static string vignette = "Vignette";
        static string vignette_mode = " Mode: ";
        static string vignette_mode_tooltips = "Use the \"Classic\" mode for parametric controls. Use \"Round\" to get a perfectly round vignette no matter what the aspect ratio is.";
        static string vignette_mode_selection_0 = "Classic";
        static string vignette_mode_selection_1 = "Round";
        static string vignette_intensity = " Intensity: ";
        static string vignette_roundness = " Roundness: ";
        static string vignette_smoothness = " Smoothness: ";
        static string vignette_color = " Vignette Color: ";
        static string chromatic_aberration = "Chromatic Aberration";
        static string chromatic_aberration_intensity = " Intensity: ";
        static string filters = "Filters";
        static string filter_temperature = " Temperature ";
        static string filter_temperature_tooltips = "Sets the white balance to a custom color temperature.";
        static string filter_tint = " Tint ";
        static string filter_tint_tooltips = "Sets the white balance to compensate for a green or magenta tint.";
        static string filter_hue = " Hue Shift ";
        static string filter_hue_tooltips = "Shift the hue of all colors.";
        static string filter_saturation = " Saturation ";
        static string filter_saturation_tooltips = "Pushes the intensity of all colors.";
        static string filter_contrast = " Contrast ";
        static string filter_contrast_tooltips = "Expands or shrinks the overall range of tonal values.";
        static string global_settings = "Global Settings";
        static string auto_refresh = " Auto Refresh ";
        static string auto_refresh_tooltips = "Automatically find all SkinnedMeshRenderer and enable ReflectionProbe usage, also replace all skin shader when deferred shading enabled. This may have performance impact.";
        static string async_load = " Async Load ";
        static string async_load_tootips = "Enable it to eliminate performance impact during cubemap loading while increasing the loading time.";
        static string force_experimental_features = " Force Enable Experimental Features ";
        static string force_experimental_features_tooltips = "Automatically enable deferred shading ,tessallation and PCSS. ";
        static string auto_setting = " Auto Setting ";
        static string auto_setting_tooltips = "Auto load optimal setting at scene change. This will disable DHH graphicsetting module silently. ";

        static string custom_window = " Customize Window ";
        static string custom_window_remember = " Remember size and position ";
        static string optimal_settings = " Load Optimal Settings ";
        static string experimental_features = " Experimental Features ";
        static string experimental_features_tooltips = "Enable deferred shading and tessallation. Once enabled there's no way to disable it until scene change. ";
        static string refresh_v2 = " Refresh ";
        static string refresh_v2_tooltips = "Find all light sources correct possible issues, update environment and reflection probe. When something don't look right just refresh. ";

        static string window_width = " Window width ";
        static string window_height = " Window height ";
        static string titlebar_0 = "Lighting";
        static string titlebar_1 = "Lens";
        static string titlebar_2 = "Perception";
        static string titlebar_3 = "Misc";
        static string exposure_value = " Exposure Value: ";
        static string exposure_value_tooltips = "Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.";
        static string eye_adaptation = "Eye Adaptation";
        static string eye_adaptation_tooltips = "This effect dynamically adjusts the exposure of the image according to the range of brightness levels it contains. ";
        static string dynamic_key = " Dynamic Key Value: ";
        static string dynamic_key_tooltips = "Set this to true to let Unity handle the key value automatically based on average luminance.";
        static string keyvalue = " Key Value: ";
        static string keyvalue_tooltips = "Exposure bias. Use this to control the global exposure of the scene.";
        static string histogram_filter = " Histogram filtering ";
        static string histogram_filter_tooltips = "These values are the lower and upper percentages of the histogram that will be used to find a stable average luminance. Values outside of this range will be discarded and won't contribute to the average luminance.";
        static string histogram_bound = " Histogram bound: ";
        static string histogram_bound_tooltips = "bound for the brightness range of the generated histogram (in EV).";
        static string lumination_range = " Luminosity range: ";
        static string lumination_range_tooltips = "average luminance to consider for auto exposure (in EV). ";
        static string light_adaptation_speed = " Light environment adatation speed: ";
        static string light_adaptation_speed_tooltips = "Adaptation speed from a dark to a light environment.";
        static string dark_adaptation_speed = " Dark environment adatation speed: ";
        static string dark_adaptation_speed_tooltips = "Adaptation speed from a light to a dark environment.";
        static string reflection_intensity = " Reflection Intensity: ";
        static string color = " Color: ";
        static string directional_light = "Directional Light";
        static string shadow = " Shadow ";
        static string shadow_none = " None ";
        static string shadow_hard = " Soft ";
        static string shadow_soft = " PCSS ";
        static string shadow_strength = " Shadow softness: ";
        static string shadow_bias = " Shadow bias: ";
        static string shadow_normal_bias = " Shadow normal bias: ";
        static string light_inspector = "Light Inspector";
        static string light_intensity = " Intensity: ";
        static string light_bounce = " Bounce Intensity: ";
        static string specular_highlight = " Specular Highlight ";
        static string specular_highlight_radius = " Radius: ";
        static string point_light = "Sphere Light";
        static string spot_light = "Spot Light";
        static string spot_angle = " Spot angle: ";
        static string specular_highlight_length = " Length: ";
        static string light_range = " Range: ";
        static string skin_scattering_weight = " Weight ";
        static string skin_scattering_weight_tooltips = "Weight of the scattering effect.";
        static string skin_scattering_mask_cutoff = " Mask cutoff ";
        static string skin_scattering_mask_cutoff_tooltips = "Threshold value above which transmission map is used to mask skin scattering. Values below act as a blend mask back to standard shading model.";
        static string skin_scattering_bias = " Bias ";
        static string skin_scattering_bias_tooltips = "Increases scattering effect.";
        static string skin_scattering_scale = " Scale ";
        static string skin_scattering_scale_tooltips = "Decreases scattering effect.";
        static string skin_bump_blur = " Bump blur ";
        static string skin_bump_blur_tooltips = "Amount that the normals are blurred for ambient and direct diffuse lighting.";
        static string skin_transmission_weight = " Weight ";
        static string skin_transmission_weight_tooltips = "The global intensity of the transmission effect.";
        static string skin_transmission_shadow_weight = " Shadow weight ";
        static string skin_transmission_shadow_weight_tooltips = "How much the light shadow attenuates the transmission effect for double-sided materials.";
        static string skin_bump_distortion = " Bump distortion ";
        static string skin_bump_distortion_tooltips = "Amount that the transmission is distorted by surface normals. A low bump distortion will result in a more even, geo-driven look, while a high bump distortion will result in a far more contrasty look.";
        static string skin_transmission_falloff = " Fall off ";
        static string skin_transmission_falloff_tooltips = "Controls how wide/tight the angular falloff is for the transmission effect.";

        static string tessellation = "Tessellation";
        static string tessellation_phong = " Phong ";
        static string tessellation_phong_tooltips = "Phong tessellation strength.";
        static string tessellation_edgelength = " Edge length ";
        static string tessellation_edgelength_tooltips = "Length of edge to split for tessellation. Lower values result in more tessellation.";
        static string colorpicker_red = " Red: ";
        static string colorpicker_green = " Green: ";
        static string colorpicker_blue = " Blue: ";
        static string colorpicker_hue = " Hue: ";
        static string colorpicker_saturation = " Saturation: ";
        static string colorpicker_value = " Value: ";
        private static string taa = " TAA ";
        private static string fxaa = " FXAA ";

        public static string Reset => reset;
        public static GUIContent Vsync => new GUIContent(vsync, vsynctooltips);

        public static string[] Disable_vs_Enable => new string[] { disable_vs_enable_0, disable_vs_enable_1 };
        public static GUIContent Dithering => new GUIContent(dithering, ditheringtooltips);
        public static string Depth_Of_View => depth_of_view;
        public static string Field_Of_View => field_of_view;
        public static string Vignette => vignette;
        public static string Filters => filters;
        public static string Chromatic_Aberration => chromatic_aberration;
        public static string Global_Settings => global_settings;
        public static GUIContent Auto_Refresh => new GUIContent(auto_refresh, auto_refresh_tooltips);
        public static GUIContent Async_Load => new GUIContent(async_load,async_load_tootips);
        public static GUIContent Force_Deferred => new GUIContent(force_experimental_features, force_experimental_features_tooltips);
        public static GUIContent Auto_Setting => new GUIContent(auto_setting, auto_setting_tooltips);

        public static string Skin_Scattering => skin_scattering;
        public static string Skin_Transmission => transmission;
        public static string Custom_Window => custom_window;
        public static string Custom_Window_Remember => custom_window_remember;
        public static string Optimal_Settings => optimal_settings;
        public static GUIContent Deferred_Shading => new GUIContent(experimental_features, experimental_features_tooltips);
        public static GUIContent Refresh => new GUIContent(refresh_v2, refresh_v2_tooltips);

        public static string Reflection => reflection;
        public static string Directional_Light => directional_light;
        public static string[] Titlebar => new string[] { titlebar_0, titlebar_1, titlebar_2, titlebar_3 };
        public static GUIContent Exposure_Value => new GUIContent(exposure_value, exposure_value_tooltips);
        public static GUIContent Eye_Adaptation => new GUIContent(eye_adaptation, eye_adaptation_tooltips);
        public static GUIContent Bloom => new GUIContent(bloom, bloom_tooltip);
        public static string Window_Height => window_height;
        public static string Window_Width => window_width;
        public static string Specular_Highlight => specular_highlight;
        public static string Color => color;
        public static string Light_Bounce => light_bounce;
        public static string Light_Intensity => light_intensity;
        public static string Light_Inspector => light_inspector;
        public static string Radius => specular_highlight_radius;
        public static string Reflection_Intensity => reflection_intensity;
        public static string Skin_Scattering_Weight_Tooltips => skin_scattering_weight_tooltips;
        public static string Skin_Scattering_Weight => skin_scattering_weight;
        public static string Skin_Scattering_Mask_Cutoff_Tooltips => skin_scattering_mask_cutoff_tooltips; 
        public static string Skin_Scattering_Mask_Cutoff => skin_scattering_mask_cutoff;
        public static string Skin_Scattering_Scale_Tooltips => skin_scattering_scale_tooltips;
        public static string Skin_Scattering_Scale => skin_scattering_scale; 
        public static string Skin_scattering_bias => skin_scattering_bias; 
        public static string Skin_scattering_bias_tooltips => skin_scattering_bias_tooltips;
        public static string Skin_bump_blur_tooltips => skin_bump_blur_tooltips; 
        public static string Skin_bump_blur => skin_bump_blur;
        public static string Skybox => skybox; 
        public static string Procedural_Skybox => procedural_skybox;
        public static string Skybox_Exposure => skybox_exposure; 
        public static string Sun_size => sun_size;
        public static string Atmosphere_thickness  => atmosphere_thickness; 
        public static string Sky_tint => sky_tint; 
        public static string Ground_color => ground_color; 
        public static string Ambient_intensity => ambient_intensity; 
        public static string Skybox_rotation => skybox_rotation;
        public static string Reflection_probe_refresh_rate => reflection_probe_refresh_rate;
        public static string Reflection_probe_refresh => reflection_probe_refresh;
        public static GUIContent Reflection_Bounces => new GUIContent(reflection_bounces);
        public static string[] Reflection_probe_refresh_rate_Array => new string[]
        {
            reflection_refresh_rate_array_0,
            reflection_refresh_rate_array_1,
            reflection_refresh_rate_array_2,
            reflection_refresh_rate_array_3
        };

        public static string Reflection_probe_resolution => reflection_probe_resolution;
        public static string Load_cubemap  => load_cubemap;
        public static string Loading  => loading;
        public static string Skin_transmission_weight => skin_transmission_weight; 
        public static string Skin_transmission_weight_tooltips => skin_transmission_weight_tooltips;
        public static string Skin_transmission_shadow_weight_tooltips => skin_transmission_shadow_weight_tooltips;
        public static string Skin_transmission_shadow_weight => skin_transmission_shadow_weight;
        public static string Skin_bump_distortion_tooltips => skin_bump_distortion_tooltips;
        public static string Skin_bump_distortion => skin_bump_distortion;
        public static string Skin_transmission_falloff_tooltips => skin_transmission_falloff_tooltips; 
        public static string Skin_transmission_falloff => skin_transmission_falloff;

        public static string Tessellation => tessellation;
        public static GUIContent Tessellation_edgelength => new GUIContent(tessellation_edgelength, tessellation_edgelength_tooltips); 
        public static GUIContent Tessellation_phong => new GUIContent(tessellation_phong, tessellation_phong_tooltips);
        //public static string Tessellation_save => tessellation_save;

        public static string Bloom_intensity_tooltips  => bloom_intensity_tooltips; 
        public static string Bloom_intensity => bloom_intensity; 
        public static string Bloom_threshold_tooltips  => bloom_threshold_tooltips;
        public static string Bloom_threshold => bloom_threshold; 
        public static string Bloom_softknee_tooltips => bloom_softknee_tooltips; 
        public static string Bloom_softknee => bloom_softknee;
        public static string Bloom_radius_tooltips => bloom_radius_tooltips; 
        public static string Bloom_radius  => bloom_radius; 
        public static GUIContent Bloom_antiflicker => new GUIContent(bloom_antiflicker, bloom_antiflicker_tooltips);

        public static GUIContent KeyValue => new GUIContent(keyvalue, keyvalue_tooltips);
        public static GUIContent DynamicKeyValue => new GUIContent(dynamic_key, dynamic_key_tooltips);
        public static string Histogram_filter_tooltips => histogram_filter_tooltips;
        public static string Histogram_filter  => histogram_filter; 
        public static string Histogram_bound_tooltips  => histogram_bound_tooltips; 
        public static string Histogram_bound  => histogram_bound; 
        public static string Lumination_range_tooltips => lumination_range_tooltips;
        public static string Lumination_range => lumination_range; 

        public static GUIContent Dark_adaptation_speed => new GUIContent(dark_adaptation_speed, dark_adaptation_speed_tooltips); 
        public static GUIContent Light_adaptation_speed  => new GUIContent(light_adaptation_speed, light_adaptation_speed_tooltips); 
        public static string[] Vignette_mode_selection => new string[] { vignette_mode_selection_0, vignette_mode_selection_1 };

        public static GUIContent Vignette_mode => new GUIContent(vignette_mode,vignette_mode_tooltips);
        public static string Vignette_color => vignette_color;

        public static GUIContent Tonemapping => new GUIContent(tonemapping, tonemapping_tooltips);
        public static GUIContent Tonemapping_none  => new GUIContent(tonemapping_none, tonemapping_none_tooltips); 
        public static GUIContent Tonemapping_ACES  => new GUIContent(tonemapping_ACES, tonemapping_ACES_tooltips); 
        public static GUIContent Tonemapping_neutral  => new GUIContent(tonemapping_neutral, tonemapping_neutral_tooltips); 


        public static string Tonemapping_neutralWhiteClip => tonemapping_neutralWhiteClip; 
        public static string Tonemapping_neutralWhiteLevel => tonemapping_neutralWhiteLevel; 
        public static string Tonemapping_neutralWhiteOut => tonemapping_neutralWhiteOut; 
        public static string Tonemapping_neutralWhiteIn => tonemapping_neutralWhiteIn; 
        public static string Tonemapping_neutralBlackOut => tonemapping_neutralBlackOut; 
        public static string Tonemapping_neutralBlackIn => tonemapping_neutralBlackIn;

        public static string Vignette_intensity => vignette_intensity; 
        public static string Vignette_roundness => vignette_roundness; 
        public static string Vignette_smoothness => vignette_smoothness; 
        public static string Chromatic_aberration_intensity => chromatic_aberration_intensity;

        public static string Dof_focal_distance => dof_focal_distance; 
        public static string Dof_focal_distance_tooltips => dof_focal_distance_tooltips; 
        public static string Dof_aperture_tooltips => dof_aperture_tooltips; 
        public static string Dof_aperture => dof_aperture; 
        public static string Dof_focallength => dof_focallength; 
        public static string Dof_focallength_tooltips  => dof_focallength_tooltips; 
        public static string Dof_kernel_tooltips => dof_kernel_tooltips; 
        public static string Dof_kernel => dof_kernel; 
        public static string[] Dof_kernel_size => new string[] { dof_kernel_size_0, dof_kernel_size_1, dof_kernel_size_2, dof_kernel_size_3 };
        public static GUIContent Dof_auto_calc => new GUIContent(dof_auto_calc, dof_auto_calc_tooltips);
        public static GUIContent Dof_auto_focus => new GUIContent(auto_focus, auto_focus_tooltips);

        public static GUIContent Filter_temperature => new GUIContent(filter_temperature, filter_temperature_tooltips); 
        public static GUIContent Filter_hue => new GUIContent(filter_hue, filter_hue_tooltips); 
        public static GUIContent Filter_tint => new GUIContent(filter_tint, filter_tint_tooltips); 
        public static GUIContent Filter_contrast => new GUIContent(filter_contrast, filter_contrast_tooltips); 
        public static GUIContent Filter_saturation => new GUIContent(filter_saturation, filter_saturation_tooltips);

        public static string Colorpicker_red => colorpicker_red; 
        public static string Colorpicker_green => colorpicker_green; 
        public static string Colorpicker_blue => colorpicker_blue; 
        public static string Colorpicker_hue => colorpicker_hue; 
        public static string Colorpicker_saturation => colorpicker_saturation; 
        public static string Colorpicker_value => colorpicker_value;

        public static string Dolly_zoom => dolly_zoom;

        public static string Shadow => shadow;
        public static string[] Shadow_Selections => new string[] { shadow_none, shadow_hard, shadow_soft };
        public static string Shadow_strength => shadow_strength;
        public static string Shadow_bias => shadow_bias;

        public static string Shadow_normal_bias => shadow_normal_bias;
        public static string Close_Window => close_window;

        public static string Light_Range => light_range; 
        public static string Light_point_length => specular_highlight_length; 
        public static string Point_Light => point_light;
        public static string Spot_light => spot_light;
        //public static string[] Lighting_Submenu => new string[] { lighting_submenu_0, lighting_submenu_1 };
        public static string Spot_angle => spot_angle;
        public static GUIContent Focus_Speed => new GUIContent(focus_speed);
        public static GUIContent DisableLightMap => new GUIContent(disableLightMap, disableLightMap_tooltips);
        public static GUIContent SSAO => new GUIContent(ssao, ssao_tooltips);
        public static GUIContent SSAO_intensity => new GUIContent(ssao_intensity, ssao_intensity_tooltips);
        public static GUIContent SSAO_radius => new GUIContent(ssao_radius, ssao_radius_tooltips);
        public static GUIContent SSAO_AmbientOnly => new GUIContent(ssao_ambientonly, ssao_ambientonly_tooltips);
        public static GUIContent SSAO_HighPrecision => new GUIContent(ssao_highPrecision, ssao_highPrecision_tooltips);
        public static GUIContent SSAO_ForceForwardCompatibility => new GUIContent(ssao_forceForwardCompatibility, ssao_forceForwardCompatibility_tooltips);
        public static GUIContent SSAO_Downsampling => new GUIContent(ssao_downsampling, ssao_downsampling_tooltips);
        public static GUIContent SSAO_SampleCount => new GUIContent(ssao_samplecount, ssao_samplecount_tooltips);
        public static GUIContent SSR  => new GUIContent(ssr, ssr_tooltips);
        public static GUIContent SSRResolution => new GUIContent(ssr_resolution, ssr_resolution_tooltips);
        public static GUIContent SSR_reflectBackfaces => new GUIContent(ssr_reflectBackfaces, ssr_reflectBackfaces_tooltips);
        public static string[] TAA_vs_FXAA => new string[] { fxaa, taa};
        public static string[] Low_vs_High => new string[] { low_vs_high_0, low_vs_high_1 };
        public static string Save => save;
    }
}
