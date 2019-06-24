using UnityEngine;
using System.IO;

namespace PHIBL.JSON
{
    static class Utilities
    {
        public static void SaveSettings(object settings, string filepath)
        {
            var jsonstring = JsonUtility.ToJson(settings,true);
            (new FileInfo(filepath)).Directory.Create();
            File.WriteAllText(filepath, jsonstring);
        }

        public static T LoadSettings<T>(string filepath)
        {
            string jsonstring = File.ReadAllText(filepath);
            return JsonUtility.FromJson<T>(jsonstring);
        }

        public static readonly string SkinSettings = UserData.Path + "PHIBL/Settings/SkinSettings.json";
        public static readonly string TransmissionSettings = UserData.Path + "PHIBL/Settings/TransmissionSettings.json";

    }
}
