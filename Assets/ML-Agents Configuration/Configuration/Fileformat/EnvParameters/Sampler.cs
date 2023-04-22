using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public enum SamplerType { uniform, multirangeuniform, gaussian }

    public abstract class Sampler: EnvParam
    {
        public SamplerType type;

        public override YamlElement ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = name;

            yaml.elements.Add(new YamlValue(ConfigText.samplerTypeText, type));

            return yaml;
        }
    }
}