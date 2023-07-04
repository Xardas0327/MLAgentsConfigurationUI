using System;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.SettingParameter
{
    public enum DeviceType { cpu, cuda, cuda0 /*cuda:0*/}

    [Serializable]
    public class TorchSettings
    {
        public bool isUseDevice;
        [Tooltip(ConfigTooltip.device)]
        public DeviceType device;

        public bool IsUse => isUseDevice;

        public TorchSettings() { }

        public TorchSettings(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.torchSettings || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.torchSettings} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.device:
                            isUseDevice = true;
                            if (value == "cpu")
                                device = DeviceType.cpu;
                            else if (value == "cuda")
                                device = DeviceType.cuda;
                            else if (value == "cuda:0")
                                device = DeviceType.cuda0;
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.torchSettings;

            string deviceText = device == DeviceType.cuda0 ? "cuda:0" : device.ToString();

            yaml.elements.Add(new YamlValue(ConfigText.device, deviceText));

            return yaml;
        }
    }
}