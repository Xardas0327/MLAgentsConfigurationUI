using Xardas.MLAgents.Yaml;
using System.Globalization;
using System;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{

    [Serializable]
    public class CurriculumItem
    {
        public string name;
        public bool isUseCompletionCriteria;
        public CompletionCriteria completionCriteria;
        public float value;

        public CurriculumItem() { }

        public CurriculumItem(YamlObject yaml)
        {
            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    switch(yamlValue.name)
                    {
                        case ConfigText.name:
                            name = yamlValue.value;
                            break;
                        case ConfigText.value:
                            float.TryParse(yamlValue.value, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                            break;
                    }
                    continue;
                }

                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.completionCriteria:
                            isUseCompletionCriteria = true;
                            completionCriteria = new CompletionCriteria(yamlObject);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = YamlFile.ArrayItemName;

            yaml.elements.Add(new YamlValue(ConfigText.name, name));

            if(isUseCompletionCriteria && completionCriteria != null)
                yaml.elements.Add(completionCriteria.ToYaml());

            yaml.elements.Add(new YamlValue(ConfigText.value, value));

            return yaml;
        }
    }
}