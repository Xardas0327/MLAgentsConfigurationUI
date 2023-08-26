using System;
using System.Globalization;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter.Reward
{
    [Serializable]
    public class GailIntrinsicReward
    {
        [Tooltip(ConfigTooltip.gailStrength)]
        [Min(0f)]
        public float strength = 1f;
        [Tooltip(ConfigTooltip.gailGamma)]
        [Range(0f, 0.9999f)]
        public float gamma = 0.99f;
        [Tooltip(ConfigTooltip.gailDemoPath)]
        public string demoPath;
        public NetworkSettings networkSettings = new();
        [Tooltip(ConfigTooltip.gailLearningRate)]
        [Min(0)]
        public float learningRate = 0.0003f;
        [Tooltip(ConfigTooltip.gailUseActions)]
        public bool useActions = false;
        [Tooltip(ConfigTooltip.gailUseVail)]
        public bool useVail = false;

        public GailIntrinsicReward() { }

        public GailIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.gailReward || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.gailReward} is not right.");

            Init(yaml);

            if (string.IsNullOrEmpty(demoPath))
                Debug.LogWarning($"The {ConfigText.demoPath} shouldn't be empty in {ConfigText.gailReward}.");
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
                        case ConfigText.demoPath:
                            demoPath = yamlValue.value; // we have to have the original string.
                            break;
                        case ConfigText.useActions:
                            if (value == "true")
                                useActions = true;
                            break;
                        case ConfigText.useVail:
                            if (value == "true")
                                useVail = true;
                            break;
                    }
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
            yaml.name = ConfigText.gailReward;

            yaml.elements.Add(new YamlValue(ConfigText.strength, strength));
            yaml.elements.Add(new YamlValue(ConfigText.gamma, gamma));
            yaml.elements.Add(new YamlValue(ConfigText.demoPath, demoPath));
            yaml.elements.Add(new YamlValue(ConfigText.learningRate, learningRate));
            yaml.elements.Add(new YamlValue(ConfigText.useActions, useActions));
            yaml.elements.Add(new YamlValue(ConfigText.useVail, useVail));

            var ns = networkSettings.ToYaml();
            ns.parent = yaml;
            yaml.elements.Add(ns);

            return yaml;
        }

        public bool IsValid()
        {
            bool isValid = networkSettings.IsValid();

            if(string.IsNullOrEmpty(demoPath))
            {
                Debug.LogError("The demo path is required, it can't be empty.");
                isValid = false;
            }

            return isValid;
        }
    }
}