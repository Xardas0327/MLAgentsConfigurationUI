using System.Collections.Generic;

namespace Xardas.MLAgents.Yaml
{
    public abstract class YamlElement
    {
        public string name;
    }

    public enum YamlObjectType { Simple, List}

    public class YamlObject : YamlElement
    {
        public YamlObject parent;
        public int deep;
        public List<YamlElement> elements = new();
        public YamlObjectType type;
    }

    public class YamlValue : YamlElement
    {
        public string value;

        public YamlValue() { }
        public YamlValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
        public YamlValue(string name, bool value)
        {
            this.name = name;
            this.value = value ? "true" : "false";
        }

        public YamlValue(string name, object value) 
        {
            this.name = name;
            this.value = value.ToString();
        }
    }
}