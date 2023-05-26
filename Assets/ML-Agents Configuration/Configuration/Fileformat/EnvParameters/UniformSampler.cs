using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
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
            samplerParameters.name = ConfigText.samplerParametersText;
            samplerParameters.parent = yaml;
            yaml.elements.Add(samplerParameters);

            samplerParameters.elements.Add(new YamlValue(ConfigText.minValueText, minValue));
            samplerParameters.elements.Add(new YamlValue(ConfigText.maxValueText, maxValue));

            return yaml;
        }
    }
}