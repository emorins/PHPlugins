using System;
using System.Collections.Generic;
using IllusionPlugin;
using UnityEngine;
using System.IO;

namespace BoneModHarmony
{

    public static class Prefs
    {
        //ModPrefs
        private const string section = "BoneManager";

        public static KeyCode WindowToggle { get; private set; }
        public static SaveFormat SaveFormat { get; private set; }
        static readonly string[] separator = { "," };

        internal static readonly string defaultBMPath = Path.GetFullPath(Path.Combine(Application.dataPath, @"..\UserData\BoneModifiers.txt"));
        internal static readonly string defaultMaleBMPath = Path.GetFullPath(Path.Combine(Application.dataPath, @"..\UserData\BoneModifiersMale.txt"));
        public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
        {
            if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
                return defaultValue;

            return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        }
        public static void Init()
        {
            WindowToggle = ToEnum(ModPrefs.GetString(section, "WindowToggle", "B", true), KeyCode.B);
            SaveFormat = ToEnum(ModPrefs.GetString(section, "SaveFormat", "Compressed", true), SaveFormat.Compressed);
        }
        public static string GetCharaPathMsgPack(string charaName, Character.SEX sex)
        {
            if(sex == Character.SEX.MALE)
                return Path.GetFullPath(Path.Combine(Application.dataPath, @"..\UserData\Chara\male\" + charaName + ".bonemod"));
            else
                return Path.GetFullPath(Path.Combine(Application.dataPath, @"..\UserData\Chara\female\" + charaName + ".bonemod"));
        }
        public static Dictionary<int, BoneModifier> LoadBoneModifiers(string charaName, Character.SEX sex)
        {
            string path;
            if (sex == Character.SEX.MALE)
            {
                path = defaultMaleBMPath;
                if (charaName != null || charaName != "")
                {
                    var temppath = Path.GetFullPath(Path.Combine(Application.dataPath, @"..\UserData\Chara\male\" + charaName + ".png.bmm.txt"));
                    if (File.Exists(temppath))
                        path = temppath;
                }
            }
            else
            {
                path = defaultBMPath;
                if (charaName != null || charaName != "")       
                {
                    var temppath = Path.GetFullPath(Path.Combine(Application.dataPath, @"..\UserData\Chara\female\" + charaName + ".png.bmm.txt"));
                    if (File.Exists(temppath))
                        path = temppath;
                }
            }
            return _LoadBoneModifiers(path);
        }

        static Dictionary<int, BoneModifier> _LoadBoneModifiers(string path)
        {
            if (!File.Exists(path))
                return null;

            Dictionary<int, BoneModifier> dictBoneModifiers = new Dictionary<int, BoneModifier>();

            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                string[] sArray;
                while ((s = sr.ReadLine()) != null)
                {
                    sArray = s.Split(separator, StringSplitOptions.None);
                    if (sArray != null && sArray.Length >= 5)
                    {

                        string boneName = sArray[1];
                        Vector3 value = new Vector3(float.Parse(sArray[2]), float.Parse(sArray[3]), float.Parse(sArray[4]));

                        switch (sArray[0].ToLower())
                        {
                            case "s":
                                if (!dictBoneModifiers.ContainsKey(Animator.StringToHash(boneName)))
                                {
                                    dictBoneModifiers.Add(Animator.StringToHash(boneName), new BoneModifier(boneName));
                                }
                                dictBoneModifiers[Animator.StringToHash(boneName)].Scale = value;
                                dictBoneModifiers[Animator.StringToHash(boneName)].isScale = true;
                                break;
                            case "r":
                                if (!dictBoneModifiers.ContainsKey(Animator.StringToHash(boneName)))
                                {
                                    dictBoneModifiers.Add(Animator.StringToHash(boneName), new BoneModifier(boneName));
                                }
                                dictBoneModifiers[Animator.StringToHash(boneName)].Rotation = value;
                                dictBoneModifiers[Animator.StringToHash(boneName)].isRotate = true;
                                break;
                            case "p":
                                if (!dictBoneModifiers.ContainsKey(Animator.StringToHash(boneName)))
                                {
                                    dictBoneModifiers.Add(Animator.StringToHash(boneName), new BoneModifier(boneName));
                                }
                                dictBoneModifiers[Animator.StringToHash(boneName)].Position = value;
                                dictBoneModifiers[Animator.StringToHash(boneName)].isPosition = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return dictBoneModifiers;
        }

    }
}
