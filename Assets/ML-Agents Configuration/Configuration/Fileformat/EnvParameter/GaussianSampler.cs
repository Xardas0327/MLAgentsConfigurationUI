using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{

    [Serializable]
    public class GaussianSampler : Sampler
    {
        public float mean;
        [Tooltip(ConfigTooltip.stDev)]
        public float stDev;

        public GaussianSampler()
        {
            type = SamplerType.gaussian;
        }

        public override YamlElement ToYaml()
        {
            var yaml = (YamlObject)base.ToYaml();

            var samplerParameters = new YamlObject();
            samplerParameters.name = ConfigText.samplerParameters;
            samplerParameters.parent = yaml;
            yaml.elements.Add(samplerParameters);

            samplerParameters.elements.Add(new YamlValue(ConfigText.mean, mean));
            samplerParameters.elements.Add(new YamlValue(ConfigText.stDev, stDev));

            return yaml;
        }
    }
}