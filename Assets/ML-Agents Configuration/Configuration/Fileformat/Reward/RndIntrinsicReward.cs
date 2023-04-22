using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.Reward
{
    public class RndIntrinsicReward : CuriosityIntrinsicReward
    {
        public RndIntrinsicReward() { }

        public RndIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.rndRewardText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.rndRewardText} is not right.");

            // it is same like CuriosityIntrinsicReward
            Init(yaml);
        }
        public override YamlObject ToYaml()
        {
            var yaml = base.ToYaml();
            yaml.name = ConfigText.rndRewardText;

            return yaml;
        }
    }
}