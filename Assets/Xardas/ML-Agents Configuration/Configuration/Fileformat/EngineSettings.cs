using UnityEngine;
using Xardas.MLAgents.Yaml;
using EngineSetting = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.EngineSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [CreateAssetMenu(fileName = "EngineSettings", menuName = "ML-Agents Config files/Engine Settings")]
    public class EngineSettings : Settings<EngineSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new EngineSetting(yaml);
        }
    }
}
