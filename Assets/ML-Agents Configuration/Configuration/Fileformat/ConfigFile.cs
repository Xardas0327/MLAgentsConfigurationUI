using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public interface ConfigFile
    {
        void LoadData(YamlObject yaml);

        YamlObject ToYaml();

        bool IsValid();
    }
}