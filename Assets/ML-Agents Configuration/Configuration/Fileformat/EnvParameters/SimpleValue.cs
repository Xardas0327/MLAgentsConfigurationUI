using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{

    [Serializable]
    public class SimpleValue : EnvParam
    {
        public string value;

        public override YamlElement ToYaml()
        {
            return new YamlValue(name, value);
        }
    }
}