using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.Reward
{
    [Serializable]
    public class ExtrinsicReward
    {
        public float strength = 1f;
        public float gamma = 0.99f;

        public ExtrinsicReward() { }

        public ExtrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.extrinsicReward || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.extrinsicReward} is not right.");

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
                    }
                }
            }
        }

        public virtual YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.extrinsicReward;

            yaml.elements.Add(new YamlValue(ConfigText.strength, strength));
            yaml.elements.Add(new YamlValue(ConfigText.gamma, gamma));

            return yaml;
        }
    }
}