using Xardas.MLAgents.Configuration.Fileformat.SettingParameter;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public abstract class Settings<T> : ConfigFile where T : ISettings
    {
        public T settings;

        public bool isUse => settings == null ? false : settings.IsUse;

        public override YamlObject ToYaml()
        {
            if (!isUse)
                return null;

            return settings.ToYaml();
        }

        public override bool IsValid()
        {
            return settings != null;
        }
    }
}