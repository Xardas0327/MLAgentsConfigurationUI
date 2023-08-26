using System;
using System.IO;
using System.Text;

namespace Xardas.MLAgents.Yaml
{
    public static class YamlFile
    {
        public const string ArrayItemName = "ArrayItem";

        public static YamlElement ConvertFileToObject(string filePath)
        {
            return ConvertStringToObject(File.ReadAllText(filePath));
        }

        public static YamlElement ConvertStringToObject(string fileData)
        {
            var root = new YamlObject();
            root.deep = -1;
            YamlObject currentParent = root;

            var lines = fileData.Split('\n');
            foreach (var line in lines)
            {
                var element = ConvertToElement(line);
                if(element == null)
                    continue;

                var deep = GetDeep(line);

                while(deep <= currentParent.deep)
                {
                    currentParent = currentParent.parent;
                }

                if (currentParent == null)
                    throw new Exception("Incorrect file.");

                if(currentParent.deep <= deep)
                {
                    if (element.name[0] == '-')
                    {
                        if(currentParent.type != YamlObjectType.List && currentParent.elements.Count > 0)
                            throw new Exception("Incorrect file.");

                        currentParent.type = YamlObjectType.List;

                        var arrayItem = new YamlObject();
                        arrayItem.name = ArrayItemName;
                        arrayItem.deep = deep;
                        arrayItem.parent = currentParent;
                        currentParent.elements.Add(arrayItem);
                        currentParent = arrayItem;

                        string newName = element.name.Substring(1);
                        deep += GetDeep(newName) + 1;

                        element.name = newName.TrimStart();
                    }

                    currentParent.elements.Add(element);

                    if(element is YamlObject newParent)
                    {
                        newParent.deep = deep;
                        newParent.parent = currentParent;
                        currentParent = newParent;
                    }
                }
            }

            return root;
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
            if (string.IsNullOrEmpty(text))
                return null;

            var hashTagIndex = text.IndexOf("#");
            text = hashTagIndex > -1 ? text.Substring(0, hashTagIndex).Trim() : text.Trim();

            //now it can be empty, if it was a comment
            if (string.IsNullOrEmpty(text))
                return null;

            YamlElement element = null;
            var parts = text.Split(':');
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

        public static void SaveObjectToFile(YamlElement yaml, string filePath)
        {
            File.WriteAllText(filePath, ConvertToText(yaml));
        }

        private static string ConvertToText(YamlElement yaml)
        {
            var text = new StringBuilder();
            ConvertToText(yaml, text, 0);
            return text.ToString();
        }

        private static void ConvertToText(YamlElement yaml, StringBuilder text, int deep)
        {
            if(yaml is YamlObject yamlObject)
            {
                if (yamlObject.name != null)
                {
                    WriteSpaces(text, deep);
                    text.Append(yamlObject.name);
                    text.Append(":");
                    text.Append(Environment.NewLine);
                    deep += 2;
                }

                switch(yamlObject.type)
                {
                    case YamlObjectType.Object:
                        foreach (var element in yamlObject.elements)
                        {
                            ConvertToText(element, text, deep);
                        }
                        break;
                    case YamlObjectType.List:
                        foreach (var arrayItem in yamlObject.elements)
                        {
                            if (arrayItem.name == ArrayItemName)
                                arrayItem.name = null;

                            var arrayItemText = new StringBuilder();
                            ConvertToText(arrayItem, arrayItemText, deep + 2);
                            arrayItemText[deep] = '-';
                            text.Append(arrayItemText);
                        }
                        break;
                }
            }
            else if (yaml is YamlValue yamlValue)
            {
                WriteSpaces(text, deep);
                text.Append(yamlValue.name);
                text.Append(": ");
                text.Append(yamlValue.value);
                text.Append(Environment.NewLine);
            }
        }

        private static void WriteSpaces(StringBuilder text, int deep)
        {
            for (int i = 0; i < deep; ++i)
            {
                text.Append(" ");
            }
        }
    }
}