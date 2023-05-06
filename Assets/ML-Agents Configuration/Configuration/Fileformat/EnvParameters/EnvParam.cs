using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public abstract class EnvParam
    {
        public string name;

        public abstract YamlElement ToYaml();
    }
}