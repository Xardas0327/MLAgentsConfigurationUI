using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public enum VisEncodeType { simple, nature_cnn, resnet, match3, fully_connected }
    public enum ConditioningType { none, hyper }

    public class NetworkSettings
    {
        public int hiddenUnits = 128;
        public int numLayers = 2;
        public bool normalize = false;
        public VisEncodeType visEncodeType = VisEncodeType.simple;
        public ConditioningType conditioningType = ConditioningType.hyper;
        public Memory memory = null;

        public NetworkSettings() { }

        public NetworkSettings(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.networkSettingsText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.networkSettingsText} is not right.");

            foreach (var element in yaml.elements)
            {
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.hiddenUnitsText:
                            Int32.TryParse(value, out hiddenUnits);
                            break;
                        case ConfigText.numLayersText:
                            Int32.TryParse(value, out numLayers);
                            break;
                        case ConfigText.normalizeText:
                            if (value == "true")
                                normalize = true;
                            break;
                        case ConfigText.visEncodeTypeText:
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
                        case ConfigText.conditioningTypeText:
                            if (value == "none")
                                conditioningType = ConditioningType.none;
                            break;
                    }
                    continue;
                }

                var yamlObject = element as YamlObject;
                if (yamlObject != null)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.memoryText:
                            memory = new Memory(yamlObject);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.networkSettingsText;

            yaml.elements.Add(new YamlValue(ConfigText.hiddenUnitsText, hiddenUnits));

            yaml.elements.Add(new YamlValue(ConfigText.numLayersText, numLayers));

            yaml.elements.Add(new YamlValue(ConfigText.normalizeText, normalize));

            yaml.elements.Add(new YamlValue(ConfigText.visEncodeTypeText, visEncodeType));

            yaml.elements.Add(new YamlValue(ConfigText.conditioningTypeText, conditioningType));

            if(memory != null)
            {
                var m = memory.ToYaml();
                m.parent = yaml;
                yaml.elements.Add(new YamlValue(ConfigText.conditioningTypeText, m));
            }
            return yaml;
        }
    }
}