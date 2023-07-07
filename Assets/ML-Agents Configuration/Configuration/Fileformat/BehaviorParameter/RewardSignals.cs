using System;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter.Reward;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter
{
    [Serializable]
    public class RewardSignals
    {
        public bool isUseExtrinsic;
        public ExtrinsicReward extrinsic = null;
        public bool isUseCuriosity;
        public CuriosityIntrinsicReward curiosity = null;
        public bool isUseGail;
        public GailIntrinsicReward gail = null;
        public bool isUseRnd;
        public RndIntrinsicReward rnd = null;

        public RewardSignals() 
        {
            extrinsic = new ExtrinsicReward();
        }

        public RewardSignals(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.rewardSignals || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.rewardSignals} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.extrinsicReward:
                            isUseExtrinsic = true;
                            extrinsic = new ExtrinsicReward(yamlObject);
                            break;
                        case ConfigText.curiosityReward:
                            isUseCuriosity = true;
                            curiosity = new CuriosityIntrinsicReward(yamlObject);
                            break;
                        case ConfigText.gailReward:
                            isUseGail = true;
                            gail = new GailIntrinsicReward(yamlObject);
                            break;
                        case ConfigText.rndReward:
                            isUseRnd = true;
                            rnd = new RndIntrinsicReward(yamlObject);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.rewardSignals;

            if(isUseExtrinsic && extrinsic != null)
            {
                var e = extrinsic.ToYaml();
                e.parent = yaml;
                yaml.elements.Add(e);
            }

            if (isUseCuriosity && curiosity != null)
            {
                var c = curiosity.ToYaml();
                c.parent = yaml;
                yaml.elements.Add(c);
            }

            if (isUseGail && gail != null)
            {
                var g = gail.ToYaml();
                g.parent = yaml;
                yaml.elements.Add(g);
            }

            if (isUseRnd && rnd != null)
            {
                var r = rnd.ToYaml();
                r.parent = yaml;
                yaml.elements.Add(r);
            }


            return yaml;
        }

        public bool IsValid()
        {
            bool isValid = true;

            //ExtrinsicReward doesn't have isvalid

            if(isUseCuriosity)
            {
                isValid &= curiosity.IsValid();
            }

            if (isUseGail)
            {
                isValid &= gail.IsValid();
            }

            if (isUseRnd)
            {
                isValid &= rnd.IsValid();
            }

            return isValid;
        }
    }
}