using System;
using System.Globalization;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter
{
    [Serializable]
    public class BehavioralCloning
    {
        [Tooltip(ConfigTooltip.behavioralCloningDemoPath)]
        public string demoPath;
        [Tooltip(ConfigTooltip.behavioralCloningStrength)]
        [Min(0)]
        public float strength = 1f;
        [Tooltip(ConfigTooltip.behavioralCloningSteps)]
        public uint steps = 0;
        [Tooltip(ConfigTooltip.behavioralCloningOverwriteBatchSize)]
        public bool overwriteBatchSize = false;
        [Tooltip(ConfigTooltip.behavioralCloningBatchSize)]
        public uint batchSize;//if not specified, it will default to the batch_size of the trainer.
        [Tooltip(ConfigTooltip.behavioralCloningOverwriteNumEpoch)]
        public bool overwriteNumEpoch = false;
        [Tooltip(ConfigTooltip.behavioralCloningNumEpoch)]
        public uint numEpoch;//if not specified, it will default to the numEpoch of the trainer.
        [Tooltip(ConfigTooltip.behavioralCloningSamplesPerUpdate)]
        public uint samplesPerUpdate = 0;

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
                            UInt32.TryParse(value, out steps);
                            break;
                        case ConfigText.batchSize:
                            UInt32.TryParse(value, out batchSize);
                            overwriteBatchSize = true;
                            break;
                        case ConfigText.numEpoch:
                            UInt32.TryParse(value, out numEpoch);
                            overwriteNumEpoch = true;
                            break;
                        case ConfigText.samplesPerUpdate:
                            UInt32.TryParse(value, out samplesPerUpdate);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(demoPath))
                Debug.LogWarning($"The {ConfigText.demoPath} shouldn't be empty in {ConfigText.behavioralCloning}.");
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.behavioralCloning;

            yaml.elements.Add(new YamlValue(ConfigText.demoPath, demoPath));
            yaml.elements.Add(new YamlValue(ConfigText.strength, strength));
            yaml.elements.Add(new YamlValue(ConfigText.steps, steps));

            if(overwriteBatchSize)
                yaml.elements.Add(new YamlValue(ConfigText.batchSize, batchSize));

            if (overwriteNumEpoch)
                yaml.elements.Add(new YamlValue(ConfigText.numEpoch, numEpoch));
            yaml.elements.Add(new YamlValue(ConfigText.samplesPerUpdate, samplesPerUpdate));

            return yaml;
        }

        public bool IsValid()
        {
            bool isValid = !string.IsNullOrEmpty(demoPath);

            if (!isValid)
                Debug.LogError("The demo path is required, it can't be empty.");

            return isValid;
        }
    }
}