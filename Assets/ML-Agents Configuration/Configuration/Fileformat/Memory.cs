using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class Memory
    {
        public int memorySize = 128;
        public int sequenceLength = 64;

        public Memory() { }

        public Memory(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.memoryText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.memoryText} is not right.");

            foreach (var element in yaml.elements)
            {
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.memorySizeText:
                            Int32.TryParse(value, out memorySize);
                            break;
                        case ConfigText.sequenceLengthText:
                            Int32.TryParse(value, out sequenceLength);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.memoryText;

            yaml.elements.Add(new YamlValue(ConfigText.memorySizeText, memorySize));

            yaml.elements.Add(new YamlValue(ConfigText.sequenceLengthText, sequenceLength));

            return yaml;
        }
    }
}