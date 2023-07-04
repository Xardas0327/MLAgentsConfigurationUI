using UnityEngine;
using Xardas.MLAgents.Yaml;
using TorchSetting = Xardas.MLAgents.Configuration.SettingParameter.TorchSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class TorchSettings : ScriptableObject, IConfigFile
    {
        public TorchSetting torchSettings = new();

        public bool isUse => torchSettings.IsUse;

        public void LoadData(YamlObject yaml)
        {
            torchSettings = new TorchSetting(yaml);
        }

        public YamlObject ToYaml()
        {
            if (!torchSettings.isUseDevice)
                return null;

            return torchSettings.ToYaml();
        }

        public bool IsValid()
        {
            return true;
        }
    }
}