using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.SettingParameter
{
    public interface ISettings
    {
        bool IsUse { get; }

        YamlObject ToYaml();
    }
}