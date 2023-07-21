using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.SettingParameter
{
    public interface ISettings
    {
        bool IsUse { get; }

        YamlObject ToYaml();
    }
}