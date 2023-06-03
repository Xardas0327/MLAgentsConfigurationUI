using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.Reward
{
    [Serializable]
    public class GailIntrinsicReward : CuriosityIntrinsicReward
    {
        public string demoPath;
        public bool useActions = false;
        public bool useVail = false;

        public GailIntrinsicReward() { }

        public GailIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.gailReward || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.gailReward} is not right.");

            Init(yaml);

            if (string.IsNullOrEmpty(demoPath))
                throw new System.Exception($"The {ConfigText.demoPath} can't be empty in {ConfigText.gailReward}.");
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
            }
        }
        public override YamlObject ToYaml()
        {
            var yaml = base.ToYaml();
            yaml.name = ConfigText.gailReward;

            yaml.elements.Add(new YamlValue(ConfigText.learningRate, learningRate));
            yaml.elements.Add(new YamlValue(ConfigText.demoPath, demoPath));
            yaml.elements.Add(new YamlValue(ConfigText.useActions, useActions));
            yaml.elements.Add(new YamlValue(ConfigText.useVail, useVail));

            return yaml;
        }
    }
}