using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using IllusionPlugin;
using HSPE.AMModules;
using UILib;
using ToolBox;
using ToolBox.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Studio;

namespace HSPE
{
    internal class HSPE : GenericPlugin
    , IEnhancedPlugin
    {
        internal const string _name = "PHPE";
        internal const string _guid = "com.joan6694.illusionplugins.poseeditor";
        internal const string _versionNum = "2.12.0";

        public override string Name { get { return _name; } }
        public override string Version { get { return _versionNum; } }
        public override string[] Filter { get { return new[] { "PlayHomeStudio32bit", "PlayHomeStudio64bit" }; } }

        protected override void Awake()
        {
            base.Awake();
            HarmonyExtensions.CreateInstance(_guid).PatchAllSafe();
        }

        protected override void LevelLoaded(int level)
        {
            base.LevelLoaded(level);
            if (level == 1)
                this.gameObject.AddComponent<MainWindow>();
        }
    }
}