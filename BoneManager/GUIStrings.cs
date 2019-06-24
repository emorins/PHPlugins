using UnityEngine;

namespace BoneModHarmony
{
    public class GUIStrings : Studio.Config.BaseSystem
    {
        public GUIStrings(string elementName):base(elementName) { }
        public override void Init()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.ChineseTraditional:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.Chinese:
                    close_window = " 关闭窗口 ";
                    reset = " 重置 ";
                    disable_vs_enable_0 = " 禁用 ";
                    disable_vs_enable_1 = " 启用 ";
                    custom_window = " 自定义窗口";
                    custom_window_remember = " 记住大小与位置 ";
                    window_height = " 窗口高度 ";
                    window_width = " 窗口宽度 ";
                    save = " 保存 ";
                    break;
                case SystemLanguage.Japanese:
                    reset = " リセット ";
                    save = " セーブ ";
                    disable_vs_enable_0 = "無効にする";
                    disable_vs_enable_1 = "有効にする";        
                    break;
                default:
                    break;
            }
        }
        static string save = " Save ";
        static string low_vs_high_0 = "High";
        static string low_vs_high_1 = "Low";
        static string close_window = " Close Window ";
        static string reset = " Reset ";
        static string disable_vs_enable_0 = " Disable ";
        static string disable_vs_enable_1 = " Enable ";
        static string flip = " Flip ";
        static string custom_window = " Customize Window ";
        static string custom_window_remember = " Remember size and position ";
        static string refresh = " Refresh ";

        static string window_width = " Window width ";
        static string window_height = " Window height ";

        static string color = " Color: ";
        static string colorpicker_red = " Red: ";
        static string colorpicker_green = " Green: ";
        static string colorpicker_blue = " Blue: ";
        static string colorpicker_hue = " Hue: ";
        static string colorpicker_saturation = " Saturation: ";
        static string colorpicker_value = " Value: ";


        public static string Reset => reset;

        public static string[] Disable_vs_Enable => new string[] { disable_vs_enable_0, disable_vs_enable_1 };

        public static string Custom_Window => custom_window;
        public static string Custom_Window_Remember => custom_window_remember;
        public static GUIContent Refresh => new GUIContent(refresh);

        public static string Window_Height => window_height;
        public static string Window_Width => window_width;

        public static string Color => color;


        public static string Colorpicker_red => colorpicker_red; 
        public static string Colorpicker_green => colorpicker_green; 
        public static string Colorpicker_blue => colorpicker_blue; 
        public static string Colorpicker_hue => colorpicker_hue; 
        public static string Colorpicker_saturation => colorpicker_saturation; 
        public static string Colorpicker_value => colorpicker_value;

        public static string Close_Window => close_window;

        public static string[] Low_vs_High => new string[] { low_vs_high_0, low_vs_high_1 };
        public static string Save => save;

        public static string Flip => flip; 
    }
}
