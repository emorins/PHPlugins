using IllusionInjector;
using System;
using UnityEngine;
using System.Reflection;

namespace ToolBox
{
    public abstract class GenericPlugin
    {
        public Binary binary { get; private set; }
        public int level { get; private set; } = -1;
        private static PluginComponent _pluginComponent;
        private Component _onGUIDispatcher = null;

        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract string[] Filter { get; }
        public GameObject gameObject
        {
            get
            {
                if (_pluginComponent == null)
                    _pluginComponent = UnityEngine.Object.FindObjectOfType<PluginComponent>();
                return _pluginComponent.gameObject;
            }
        }

        public void OnApplicationStart()
        {
            this.Awake();
        }

        public void OnApplicationQuit()
        {
            this.OnDestroy();
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnLevelWasLoaded(int level)
        {
            this.level = level;
            this.LevelLoaded(level);
        }

        public void OnUpdate()
        {
            this.Update();
        }

        public void OnFixedUpdate()
        {
            this.FixedUpdate();
        }

        public void OnLateUpdate()
        {
            this.LateUpdate();
        }

        protected virtual void Awake()
        {
            switch (Application.productName)
            {
                case "PlayHome":
                    this.binary = Binary.Game;
                    break;
                case "PlayHomeStudio":
                    this.binary = Binary.Studio;
                    break;
            }

            Component[] components = this.gameObject.GetComponents<Component>();
            foreach (Component c in components)
            {
                if (c.GetType().Name == nameof(OnGUIDispatcher))
                {
                    this._onGUIDispatcher = c;
                    break;
                }
            }
            if (this._onGUIDispatcher == null)
                this._onGUIDispatcher = this.gameObject.gameObject.AddComponent<OnGUIDispatcher>();
            this._onGUIDispatcher.GetType().GetMethod(nameof(OnGUIDispatcher.AddListener), BindingFlags.Instance | BindingFlags.Public).Invoke(this._onGUIDispatcher, new object[]{new Action(this.OnGUI)});

        }

        protected virtual void OnDestroy()
        {

            if (this._onGUIDispatcher != null)
            this._onGUIDispatcher.GetType().GetMethod(nameof(OnGUIDispatcher.RemoveListener), BindingFlags.Instance | BindingFlags.Public).Invoke(this._onGUIDispatcher, new object[]{new Action(this.OnGUI)});

        }

        protected virtual void LevelLoaded(int l) { }


        protected virtual void Update() { }

        protected virtual void LateUpdate() { }

        protected virtual void FixedUpdate() { }

        protected virtual void OnGUI() { }
    }


    internal class OnGUIDispatcher : MonoBehaviour
    {
        private event Action _onGUI;

        public void AddListener(Action listener)
        {
            this._onGUI += listener;
        }

        public void RemoveListener(Action listener)
        {
            this._onGUI -= listener;
        }

        private void OnGUI()
        {
            if (this._onGUI != null)
                this._onGUI();
        }
    }


    public enum Binary
    {
        Game,
        Studio
    }
}
