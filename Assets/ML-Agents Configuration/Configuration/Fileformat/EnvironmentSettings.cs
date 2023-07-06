using UnityEngine;
using Xardas.MLAgents.Configuration.SettingParameter;
using Xardas.MLAgents.Yaml;
using EnvironmentSetting = Xardas.MLAgents.Configuration.SettingParameter.EnvironmentSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class EnvironmentSettings : Settings<EnvironmentSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new EnvironmentSetting(yaml);
        }
    }
}