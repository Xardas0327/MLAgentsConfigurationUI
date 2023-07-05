using UnityEngine;
using Xardas.MLAgents.Yaml;
using EnvironmentSetting = Xardas.MLAgents.Configuration.SettingParameter.EnvironmentSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class EnvironmentSettings : ScriptableObject, IConfigFile
    {
        public EnvironmentSetting environmentSettings = new();

        public bool isUse => environmentSettings.IsUse;

        public void LoadData(YamlObject yaml)
        {
            environmentSettings = new EnvironmentSetting(yaml);
        }

        public YamlObject ToYaml()
        {
            if (!isUse)
                return null;

            return environmentSettings.ToYaml();
        }

        public bool IsValid()
        {
            return true;
        }
    }
}