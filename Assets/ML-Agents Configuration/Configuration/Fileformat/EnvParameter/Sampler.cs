using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{
    public enum SamplerType { uniform, multirangeuniform, gaussian }

    public abstract class Sampler: EnvParam
    {
        [HideInInspector]
        public SamplerType type;

        public override YamlElement ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = name;

            yaml.elements.Add(new YamlValue(ConfigText.samplerType, type));

            return yaml;
        }
    }
}