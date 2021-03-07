using IllusionPlugin;

namespace ToolBox
{
    public class GenericConfig
    {
        private readonly string _name;

        public GenericConfig(string name, GenericPlugin plugin = null)
        {
            this._name = name;
        }

        public string AddString(string key, string defaultValue, bool autoSave, string description = null)
        {
            return ModPrefs.GetString(this._name, key, defaultValue, autoSave);
        }

        public void SetString(string key, string value)
        {
            ModPrefs.SetString(this._name, key, value);
        }

        public int AddInt(string key, int defaultValue, bool autoSave, string description = null)
        {
            return ModPrefs.GetInt(this._name, key, defaultValue, autoSave);
        }

        public void SetInt(string key, int value)
        {
            ModPrefs.SetInt(this._name, key, value);
        }

        public bool AddBool(string key, bool defaultValue, bool autoSave, string description = null)
        {
            return ModPrefs.GetBool(this._name, key, defaultValue, autoSave);
        }

        public void SetBool(string key, bool value)
        {
            ModPrefs.SetBool(this._name, key, value);
        }

        public float AddFloat(string key, float defaultValue, bool autoSave, string description = null)
        {
            return ModPrefs.GetFloat(this._name, key, defaultValue, autoSave);
        }

        public void SetFloat(string key, float value)
        {
            ModPrefs.SetFloat(this._name, key, value);
        }

        public void Save()
        {
        }
    }
}
