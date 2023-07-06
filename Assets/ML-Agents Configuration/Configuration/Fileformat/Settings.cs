using UnityEngine;
using Xardas.MLAgents.Configuration.SettingParameter;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public abstract class Settings<T> : ScriptableObject, IConfigFile where T : ISettings
    {
        public T settings;

        public bool isUse => settings == null ? false : settings.IsUse;

        public abstract void LoadData(YamlObject yaml);

        public YamlObject ToYaml()
        {
            if (!isUse)
                return null;

            return settings.ToYaml();
        }

        public bool IsValid()
        {
            return settings != null;
        }
    }
}