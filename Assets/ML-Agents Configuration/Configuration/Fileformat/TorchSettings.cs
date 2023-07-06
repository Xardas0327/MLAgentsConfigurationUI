using Xardas.MLAgents.Yaml;
using TorchSetting = Xardas.MLAgents.Configuration.SettingParameter.TorchSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class TorchSettings : Settings<TorchSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new TorchSetting(yaml);
        }
    }
}