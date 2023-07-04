using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter
{
    public enum VisEncodeType { simple, nature_cnn, resnet, match3, fully_connected }
    public enum ConditioningType { none, hyper }

    [Serializable]
    public class NetworkSettings
    {
        [Tooltip(ConfigTooltip.hiddenUnits)]
        public uint hiddenUnits = 128;
        [Tooltip(ConfigTooltip.numLayers)]
        public uint numLayers = 2;
        [Tooltip(ConfigTooltip.normalize)]
        public bool normalize = false;
        [Tooltip(ConfigTooltip.visEncodeType)]
        public VisEncodeType visEncodeType = VisEncodeType.simple;
        [Tooltip(ConfigTooltip.conditioningType)]
        public ConditioningType conditioningType = ConditioningType.hyper;
        public bool isUseMemory = false;
        public Memory memory = null;

        public NetworkSettings() { }

        public NetworkSettings(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.networkSettings || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.networkSettings} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.hiddenUnits:
                            UInt32.TryParse(value, out hiddenUnits);
                            break;
                        case ConfigText.numLayers:
                            UInt32.TryParse(value, out numLayers);
                            break;
                        case ConfigText.normalize:
                            if (value == "true")
                                normalize = true;
                            break;
                        case ConfigText.visEncodeType:
                            // default is the simple
                            switch(value)
                            {
                                case "nature_cnn":
                                    visEncodeType = VisEncodeType.nature_cnn;
                                    break;
                                case "resnet":
                                    visEncodeType = VisEncodeType.resnet;
                                    break;
                                case "match3":
                                    visEncodeType = VisEncodeType.match3;
                                    break;
                                case "fully_connected":
                                    visEncodeType = VisEncodeType.fully_connected;
                                    break;
                            }
                            break;
                        case ConfigText.conditioningType:
                            if (value == "none")
                                conditioningType = ConditioningType.none;
                            break;
                    }
                    continue;
                }

                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.memory:
                            isUseMemory = true;
                            memory = new Memory(yamlObject);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.networkSettings;

            yaml.elements.Add(new YamlValue(ConfigText.hiddenUnits, hiddenUnits));
            yaml.elements.Add(new YamlValue(ConfigText.numLayers, numLayers));
            yaml.elements.Add(new YamlValue(ConfigText.normalize, normalize));
            yaml.elements.Add(new YamlValue(ConfigText.visEncodeType, visEncodeType));
            yaml.elements.Add(new YamlValue(ConfigText.conditioningType, conditioningType));

            if(isUseMemory && memory != null)
            {
                var m = memory.ToYaml();
                m.parent = yaml;
                yaml.elements.Add(new YamlValue(ConfigText.conditioningType, m));
            }
            return yaml;
        }

        public bool IsValid()
        {
            if (isUseMemory && memory.memorySize % 2 != 0)
            {
                Debug.LogError("The memory size must be divisible by 2.");
                return false;
            }

            return true;
        }
    }
}