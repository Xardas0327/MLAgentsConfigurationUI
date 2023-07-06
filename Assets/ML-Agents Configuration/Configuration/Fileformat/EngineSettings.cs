using Xardas.MLAgents.Yaml;
using EngineSetting = Xardas.MLAgents.Configuration.SettingParameter.EngineSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class EngineSettings : Settings<EngineSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new EngineSetting(yaml);
        }
    }
}
