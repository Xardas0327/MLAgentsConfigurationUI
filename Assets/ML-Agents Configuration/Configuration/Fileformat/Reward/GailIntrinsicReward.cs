using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.Reward
{
    public class GailIntrinsicReward : CuriosityIntrinsicReward
    {
        public string demoPath;
        public bool useActions = false;
        public bool useVail = false;

        public GailIntrinsicReward() { }

        public GailIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.gailRewardText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.gailRewardText} is not right.");

            Init(yaml);

            if (string.IsNullOrEmpty(demoPath))
                throw new System.Exception($"The {ConfigText.demoPathText} can't be empty in {ConfigText.gailRewardText}.");
        }

        protected override void Init(YamlObject yaml)
        {
            base.Init(yaml);
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
                        case ConfigText.useActionsText:
                            if (value == "true")
                                useActions = true;
                            break;
                        case ConfigText.useVailText:
                            if (value == "true")
                                useVail = true;
                            break;
                    }
                }
            }
        }
    }
}