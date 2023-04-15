using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat.Reward;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class RewardSignals
    {
        public ExtrinsicReward extrinsic = null;
        public CuriosityIntrinsicReward curiosity = null;
        public GailIntrinsicReward gail = null;
        public RndIntrinsicReward rnd = null;

        public RewardSignals() { }

        public RewardSignals(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.rewardSignalsText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.rewardSignalsText} is not right.");

            foreach (var element in yaml.elements)
            {
                var yamlObject = element as YamlObject;
                if (yamlObject != null)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.extrinsicRewardText:
                            extrinsic = new ExtrinsicReward(yamlObject);
                            break;
                        case ConfigText.curiosityRewardText:
                            curiosity = new CuriosityIntrinsicReward(yamlObject);
                            break;
                        case ConfigText.gailRewardText:
                            gail = new GailIntrinsicReward(yamlObject);
                            break;
                        case ConfigText.rndRewardText:
                            rnd = new RndIntrinsicReward(yamlObject);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.rewardSignalsText;


            return yaml;
        }
    }
}