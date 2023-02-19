using System.Collections.Generic;

namespace Xardas.MLAgents.Yaml
{
    public abstract class YamlElement
    {
        public string name;
    }
    public class YamlObject : YamlElement
    {
        public YamlObject parent;
        public int deep;
        public List<YamlElement> elements = new();
    }
    public class YamlValue : YamlElement
    {
        public string value;
    }
}