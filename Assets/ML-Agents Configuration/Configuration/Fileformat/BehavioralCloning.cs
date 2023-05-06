using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class BehavioralCloning
    {
        public string demoPath;
        public float strength = 1f;
        public int steps = 0;
        public int batchSize;
        public int numEpoch;
        public int samplesPerUpdate = 0;

        public BehavioralCloning(int defaultBatchSize, int defaultNumEpoch) 
        {
            batchSize = defaultBatchSize;
            numEpoch = defaultNumEpoch;
        }

        public BehavioralCloning(YamlObject yaml, int defaultBatchSize, int defaultNumEpoch)
        {
            if (yaml == null || yaml.name != ConfigText.behavioralCloningText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.behavioralCloningText} is not right.");

            batchSize = defaultBatchSize;
            numEpoch = defaultNumEpoch;

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.demoPathText:
                            demoPath = yamlValue.value; // we have to have the original string.
                            break;
                        case ConfigText.strengthText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out strength);
                            break;
                        case ConfigText.stepsText:
                            Int32.TryParse(value, out steps);
                            break;
                        case ConfigText.batchSizeText:
                            Int32.TryParse(value, out batchSize);
                            break;
                        case ConfigText.numEpochText:
                            Int32.TryParse(value, out numEpoch);
                            break;
                        case ConfigText.samplesPerUpdateText:
                            Int32.TryParse(value, out samplesPerUpdate);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(demoPath))
                throw new System.Exception($"The {ConfigText.demoPathText} can't be empty in {ConfigText.behavioralCloningText}.");
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.behavioralCloningText;

            yaml.elements.Add(new YamlValue(ConfigText.demoPathText, demoPath));
            yaml.elements.Add(new YamlValue(ConfigText.strengthText, strength));
            yaml.elements.Add(new YamlValue(ConfigText.stepsText, steps));
            yaml.elements.Add(new YamlValue(ConfigText.batchSizeText, batchSize));
            yaml.elements.Add(new YamlValue(ConfigText.numEpochText, numEpoch));
            yaml.elements.Add(new YamlValue(ConfigText.samplesPerUpdateText, samplesPerUpdate));

            return yaml;
        }
    }
}