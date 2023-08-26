using System;
using System.Globalization;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter.Reward
{
    [Serializable]
    public class CuriosityIntrinsicReward
    {
        [Tooltip(ConfigTooltip.curiosityrndStrength)]
        [Min(0f)]
        public float strength = 1f;
        [Tooltip(ConfigTooltip.curiosityrndGamma)]
        [Range(0f, 0.9999f)]
        public float gamma = 0.99f;
        public NetworkSettings networkSettings = new();
        [Tooltip(ConfigTooltip.curiosityrndLearningRate)]
        [Min(0)]
        public float learningRate = 0.0003f;
        public CuriosityIntrinsicReward() { }

        public CuriosityIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.curiosityReward || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.curiosityReward} is not right.");

            Init(yaml);
        }

        protected virtual void Init(YamlObject yaml)
        {
            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.strength:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out strength);
                            break;
                        case ConfigText.gamma:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out gamma);
                            break;
                        case ConfigText.learningRate:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out learningRate);
                            break;
                    }
                    continue;
                }

                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.networkSettings:
                            networkSettings = new NetworkSettings(yamlObject);
                            break;
                    }
                }
            }
        }
        public virtual YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.curiosityReward;

            yaml.elements.Add(new YamlValue(ConfigText.strength, strength));
            yaml.elements.Add(new YamlValue(ConfigText.gamma, gamma));
            yaml.elements.Add(new YamlValue(ConfigText.learningRate, learningRate));

            var ns = networkSettings.ToYaml();
            ns.parent = yaml;
            yaml.elements.Add(ns);

            return yaml;
        }

        public virtual bool IsValid()
        {
            return networkSettings.IsValid();
        }
    }
}