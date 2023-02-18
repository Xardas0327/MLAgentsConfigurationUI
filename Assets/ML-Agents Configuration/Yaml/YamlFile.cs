using System;
using System.IO;
using System.Text;

namespace Xardas.MLAgents.Yaml
{
    public static class YamlFile
    {
        public static YamlElement ConvertFileToObject(string filePath)
        {
            YamlObject file = null;
            YamlObject currentParent = null;

            var lines = File.ReadLines(filePath);
            foreach (var line in lines)
            {
                var element = ConvertToElement(line);
                var deep = GetDeep(line);

                if (file == null)
                {
                    if(element is YamlObject)
                        file = element as YamlObject;
                    else
                    {
                        var o = new YamlObject();
                        o.elements.Add(element);

                        file = o;
                    }

                    currentParent = file;
                    currentParent.deep = deep;
                    continue;
                }

                while(deep <= currentParent.deep)
                {
                    currentParent = currentParent.parent;
                }

                if (currentParent == null)
                    throw new Exception("Incorrect file.");

                if(currentParent.deep <= deep)
                {
                    currentParent.elements.Add(element);

                    var newParent = element as YamlObject;
                    if(newParent != null)
                    {
                        newParent.deep = deep;
                        newParent.parent = currentParent;
                        currentParent = newParent;
                    }
                }
            }

            return file;
        }

        private static int GetDeep(string text)
        {
            int deep = 0;
            foreach (var c in text) 
            {
                if (c == ' ' || c == '\t')
                    ++deep;
                else
                    break;
            }

            return deep;
        }

        private static YamlElement ConvertToElement(string text)
        {
            YamlElement element = null;
            var parts = text.Trim().Split(':');
            if(parts.Length == 1 || (parts.Length > 1 && string.IsNullOrEmpty(parts[1])))
            {
                element = new YamlObject()
                {
                    name = parts[0]
                };
            }
            else
            {
                element = new YamlValue()
                {
                    name = parts[0]
                };

                var value = new StringBuilder();
                value.Append(parts[1]);

                for (int i = 2; i < parts.Length; ++i)
                {
                    value.Append(':');
                    value.Append(parts[i]);
                }

                ((YamlValue)element).value = value.ToString().Trim();
            }

            return element;
        }
    }
}