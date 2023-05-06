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

        public RewardSignals() 
        {
            extrinsic = new ExtrinsicReward();
        }

        public RewardSignals(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.rewardSignalsText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.rewardSignalsText} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlObject yamlObject)
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

            if(extrinsic != null)
            {
                var e = extrinsic.ToYaml();
                e.parent = yaml;
                yaml.elements.Add(e);
            }

            if (curiosity != null)
            {
                var c = curiosity.ToYaml();
                c.parent = yaml;
                yaml.elements.Add(c);
            }

            if (gail != null)
            {
                var g = gail.ToYaml();
                g.parent = yaml;
                yaml.elements.Add(g);
            }

            if (rnd != null)
            {
                var r = rnd.ToYaml();
                r.parent = yaml;
                yaml.elements.Add(r);
            }


            return yaml;
        }
    }
}