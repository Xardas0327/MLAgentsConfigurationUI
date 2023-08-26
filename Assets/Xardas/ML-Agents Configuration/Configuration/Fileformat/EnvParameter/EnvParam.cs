using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{
    public abstract class EnvParam
    {
        public string name;

        public abstract YamlElement ToYaml();
    }
}