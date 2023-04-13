using System.Collections.Generic;
using System.Xml.Linq;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
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
                var yamlObject = element as YamlObject;
                if (yamlObject != null)
                {
                    items.Add(new CurriculumItem(yamlObject));
                }
            }
        }
    }
}