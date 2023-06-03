using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [Serializable]
    public class BehavioralCloning
    {
        public string demoPath;
        public float strength = 1f;
        public int steps = 0;
        public uint batchSize;//if not specified, it will default to the batch_size of the trainer.
        public uint numEpoch;//if not specified, it will default to the numEpoch of the trainer.
        public int samplesPerUpdate = 0;

        public BehavioralCloning(uint defaultBatchSize, uint defaultNumEpoch) 
        {
            batchSize = defaultBatchSize;
            numEpoch = defaultNumEpoch;
        }

        public BehavioralCloning(YamlObject yaml, uint defaultBatchSize, uint defaultNumEpoch)
        {
            if (yaml == null || yaml.name != ConfigText.behavioralCloning || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.behavioralCloning} is not right.");

            batchSize = defaultBatchSize;
            numEpoch = defaultNumEpoch;

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.demoPath:
                            demoPath = yamlValue.value; // we have to have the original string.
                            break;
                        case ConfigText.strength:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out strength);
                            break;
                        case ConfigText.steps:
                            Int32.TryParse(value, out steps);
                            break;
                        case ConfigText.batchSize:
                            UInt32.TryParse(value, out batchSize);
                            break;
                        case ConfigText.numEpoch:
                            UInt32.TryParse(value, out numEpoch);
                            break;
                        case ConfigText.samplesPerUpdate:
                            Int32.TryParse(value, out samplesPerUpdate);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(demoPath))
                throw new System.Exception($"The {ConfigText.demoPath} can't be empty in {ConfigText.behavioralCloning}.");
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.behavioralCloning;

            yaml.elements.Add(new YamlValue(ConfigText.demoPath, demoPath));
            yaml.elements.Add(new YamlValue(ConfigText.strength, strength));
            yaml.elements.Add(new YamlValue(ConfigText.steps, steps));
            yaml.elements.Add(new YamlValue(ConfigText.batchSize, batchSize));
            yaml.elements.Add(new YamlValue(ConfigText.numEpoch, numEpoch));
            yaml.elements.Add(new YamlValue(ConfigText.samplesPerUpdate, samplesPerUpdate));

            return yaml;
        }
    }
}