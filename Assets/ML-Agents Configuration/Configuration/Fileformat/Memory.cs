using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{

    [Serializable]
    public class Memory
    {
        public int memorySize = 128;
        public int sequenceLength = 64;

        public Memory() { }

        public Memory(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.memory || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.memory} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.memorySize:
                            Int32.TryParse(value, out memorySize);
                            break;
                        case ConfigText.sequenceLength:
                            Int32.TryParse(value, out sequenceLength);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.memory;

            yaml.elements.Add(new YamlValue(ConfigText.memorySize, memorySize));

            yaml.elements.Add(new YamlValue(ConfigText.sequenceLength, sequenceLength));

            return yaml;
        }
    }
}