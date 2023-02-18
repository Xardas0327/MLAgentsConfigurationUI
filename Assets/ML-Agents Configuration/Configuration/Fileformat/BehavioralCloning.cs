using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class BehavioralCloning
    {
        public string demoPath;
        public float strength = 1f;
        public int step = 0;
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
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
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
                            Int32.TryParse(value, out step);
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
        }
    }
}