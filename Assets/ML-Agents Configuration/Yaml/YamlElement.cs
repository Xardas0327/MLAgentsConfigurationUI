using System.Collections.Generic;

namespace Xardas.MLAgents.Yaml
{
    public abstract class YamlElement
    {
        public string name;

        public abstract YamlElement Copy();
    }

    public enum YamlObjectType { Object, List}

    public class YamlObject : YamlElement
    {
        public YamlObject parent;
        public int deep;
        public List<YamlElement> elements = new();
        public YamlObjectType type;

        public override YamlElement Copy()
        {
            var newYamlObject = new YamlObject();
            newYamlObject.name = name;
            newYamlObject.parent = parent;
            newYamlObject.deep = deep;
            newYamlObject.type = type;

            newYamlObject.elements = new List<YamlElement>();
            foreach (var element in elements)
            {
                var newElement = element.Copy();
                if (newElement is YamlObject yamlObject)
                {
                    yamlObject.parent = newYamlObject;
                }
                newYamlObject.elements.Add(newElement);
            }
            return newYamlObject;
        }

        public static YamlObject Merge(YamlObject defaultObject, YamlObject overwriterObject)
        {
            if (overwriterObject == null)
                return defaultObject;

            if (defaultObject == null)
                return overwriterObject;

            var newYamlObject = (YamlObject) defaultObject.Copy();
            newYamlObject.name = overwriterObject.name;
            newYamlObject.parent = overwriterObject.parent;
            newYamlObject.deep = overwriterObject.deep;
            newYamlObject.type = overwriterObject.type;

            foreach (var overwriterElement in overwriterObject.elements)
            {
                for(int i = 0; i < newYamlObject.elements.Count; ++i)
                {
                    if (overwriterElement.name == newYamlObject.elements[i].name)
                    {
                        if (overwriterElement is YamlValue
                            && newYamlObject.elements[i] is YamlValue)
                        {
                            newYamlObject.elements[i] = overwriterElement.Copy();
                            break;
                        }
                        else if (overwriterElement is YamlObject overwriterElementYamlObject
                            && newYamlObject.elements[i] is YamlObject elementYamlObject)
                        {
                            newYamlObject.elements[i] = Merge(elementYamlObject, overwriterElementYamlObject);
                            break;
                        }

                        throw new System.Exception($"Both YamlObjects has {overwriterElement.name} element, but they are not the same type.");
                    }
                }
            }
            return newYamlObject;
        }
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

        public YamlValue(string name, float value)
        {
            this.name = name;
            this.value = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public YamlValue(string name, object value) 
        {
            this.name = name;
            this.value = value.ToString();
        }

        public override YamlElement Copy()
        {
            return new YamlValue(name, value);
        }
    }
}