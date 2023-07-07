using Xardas.MLAgents.Yaml;
using CheckpointSetting = Xardas.MLAgents.Configuration.SettingParameter.CheckpointSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class CheckpointSettings : Settings<CheckpointSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new CheckpointSetting(yaml);
        }
    }
}