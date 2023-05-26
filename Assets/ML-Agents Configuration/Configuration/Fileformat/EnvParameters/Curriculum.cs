using System;
using System.Collections.Generic;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{

    [Serializable]
    public class Curriculum : EnvParam
    {
        public List<CurriculumItem> items = new();

        public Curriculum() { }

        public Curriculum(YamlObject yaml)
        {
            if (yaml == null || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.curriculumText} is not right.");

            var curriculum = yaml.elements.Find(x => x.name == ConfigText.curriculumText);
            if (curriculum == null)
                throw new System.Exception($"The {ConfigText.curriculumText} is not right.");

            name = yaml.name;

            foreach(var element in ((YamlObject)curriculum).elements)
            {
                if (element is YamlObject yamlObject)
                {
                    items.Add(new CurriculumItem(yamlObject));
                }
            }
        }

        public override YamlElement ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = name;

            var curriculum = new YamlObject();
            curriculum.name = "curriculum";
            curriculum.parent = yaml;
            curriculum.type = YamlObjectType.List;
            yaml.elements.Add(curriculum);

            foreach (var item in items)
                curriculum.elements.Add(item.ToYaml());

            return yaml;
        }
    }
}