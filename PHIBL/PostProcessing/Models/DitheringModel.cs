using System;
using UnityEngine;
using MessagePack;
namespace PHIBL.PostProcessing
{
    [MessagePackObject(keyAsPropertyName: true)]
    [Serializable]
    public class DitheringModel : PostProcessingModel
    {
        [MessagePackObject(keyAsPropertyName: true)]
        [Serializable]
        public struct Settings
        {
            public static Settings defaultSettings
            {
                get { return new Settings(); }
            }
        }

        [SerializeField]
        Settings m_Settings = Settings.defaultSettings;
        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}
