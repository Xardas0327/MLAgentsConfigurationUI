using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public class GaussianSampler : Sampler
    {
        public float mean;
        public float stDev;

        public GaussianSampler()
        {
            type = SamplerType.gaussian;
        }

        public override YamlElement ToYaml()
        {
            var yaml = (YamlObject)base.ToYaml();

            var samplerParameters = new YamlObject();
            samplerParameters.name = ConfigText.samplerParametersText;
            samplerParameters.parent = yaml;
            yaml.elements.Add(samplerParameters);

            samplerParameters.elements.Add(new YamlValue(ConfigText.meanText, mean));
            samplerParameters.elements.Add(new YamlValue(ConfigText.stDevText, stDev));

            return yaml;
        }
    }
}