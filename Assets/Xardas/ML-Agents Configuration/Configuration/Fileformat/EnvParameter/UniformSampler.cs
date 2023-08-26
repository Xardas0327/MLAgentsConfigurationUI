using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{

    [Serializable]
    public class UniformSampler : Sampler
    {
        public float minValue;
        public float maxValue;

        public UniformSampler()
        {
            type = SamplerType.uniform;
        }

        public override YamlElement ToYaml()
        {
            var yaml = (YamlObject) base.ToYaml();

            var samplerParameters = new YamlObject();
            samplerParameters.name = ConfigText.samplerParameters;
            samplerParameters.parent = yaml;
            yaml.elements.Add(samplerParameters);

            samplerParameters.elements.Add(new YamlValue(ConfigText.minValue, minValue));
            samplerParameters.elements.Add(new YamlValue(ConfigText.maxValue, maxValue));

            return yaml;
        }
    }
}