using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using IllusionPlugin;
using PHPE.AMModules;
using UILib;
using ToolBox;
using ToolBox.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Studio;

namespace PHPE
{
    internal class PHPEPlugin : GenericPlugin, IPlugin, IEnhancedPlugin
    {
        internal const string _name = "PHPE";
        internal const string _guid = "com.joan6694.illusionplugins.poseeditor";
        internal const string _versionNum = "2.12.0";

        public override string Name => "PHPE";
        public override string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public override string[] Filter => new string[]
        {
            "PlayHome64bit",
            "PlayHomeStudio64bit"
        };

        protected override void Awake()
        {
            base.Awake();
            HarmonyExtensions.CreateInstance(_guid).PatchAllSafe();
        }

        protected override void LevelLoaded(int level)
        {
            base.LevelLoaded(level);
            //if (level == 1)
            if (!GameObject.Find("PHPE"))
            {
                this.gameObject.AddComponent<PHPE>();
            }
        }
    }
}