using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.Reward
{
    public class CuriosityIntrinsicReward : ExtrinsicReward
    {
        public NetworkSettings networkSettings = new();
        public float learningRate = 0.0003f;
        public CuriosityIntrinsicReward() { }

        public CuriosityIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.curiosityRewardText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.curiosityRewardText} is not right.");

            Init(yaml);
        }

        protected override void Init(YamlObject yaml)
        {
            base.Init(yaml);
            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.learningRateText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out learningRate);
                            break;
                    }
                    continue;
                }

                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.networkSettingsText:
                            networkSettings = new NetworkSettings(yamlObject);
                            break;
                    }
                }
            }
        }
        public override YamlObject ToYaml()
        {
            var yaml = base.ToYaml();
            yaml.name = ConfigText.curiosityRewardText;

            yaml.elements.Add(new YamlValue(ConfigText.learningRateText, learningRate));

            var ns = networkSettings.ToYaml();
            ns.parent = yaml;
            yaml.elements.Add(ns);

            return yaml;
        }
    }
}