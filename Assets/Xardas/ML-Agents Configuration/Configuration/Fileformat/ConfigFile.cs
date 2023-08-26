using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public abstract class ConfigFile : ScriptableObject
    {
        public abstract void LoadData(YamlObject yaml);

        public abstract YamlObject ToYaml();

        public abstract bool IsValid();
    }
}
