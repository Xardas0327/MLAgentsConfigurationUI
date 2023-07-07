using System;
using System.Collections.Generic;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{

    [Serializable]
    public class Curriculum : EnvParam
    {
        public List<CurriculumItem> lessons = new();

        public Curriculum() { }

        public Curriculum(YamlObject yaml)
        {
            if (yaml == null || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.curriculum} is not right.");

            var curriculum = yaml.elements.Find(x => x.name == ConfigText.curriculum);
            if (curriculum == null)
                throw new System.Exception($"The {ConfigText.curriculum} is not right.");

            name = yaml.name;

            foreach(var element in ((YamlObject)curriculum).elements)
            {
                if (element is YamlObject yamlObject)
                {
                    lessons.Add(new CurriculumItem(yamlObject));
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

            for(int i = 0; i < lessons.Count; ++i)
            {
                //last elem shouldn't have completion criteria
                if (i == lessons.Count - 1)
                {
                    var cc = lessons[i].completionCriteria;
                    lessons[i].completionCriteria = null;

                    curriculum.elements.Add(lessons[i].ToYaml());

                    lessons[i].completionCriteria = cc;
                }
                else
                    curriculum.elements.Add(lessons[i].ToYaml());
            }

            return yaml;
        }
    }
}