using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter.Reward
{
    [Serializable]
    public class RndIntrinsicReward : CuriosityIntrinsicReward
    {
        public RndIntrinsicReward() { }

        public RndIntrinsicReward(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.rndReward || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.rndReward} is not right.");

            // it is same like CuriosityIntrinsicReward
            Init(yaml);
        }
        public override YamlObject ToYaml()
        {
            var yaml = base.ToYaml();
            yaml.name = ConfigText.rndReward;

            return yaml;
        }
    }
}