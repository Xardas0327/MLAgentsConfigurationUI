using UnityEngine;
using Xardas.MLAgents.Yaml;
using TorchSetting = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.TorchSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [CreateAssetMenu(fileName = "TorchSettings", menuName = "ML-Agents Config files/Torch Settings")]
    public class TorchSettings : Settings<TorchSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new TorchSetting(yaml);
        }
    }
}