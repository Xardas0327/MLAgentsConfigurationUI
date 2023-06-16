using System.Collections.Generic;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat.EnvParameters;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public class EnvironmentParameters : ScriptableObject, ConfigFile
    {
        public List<SimpleValue> simpleValues = new();
        public List<UniformSampler> uniformSamplers = new();
        public List<MultiUniformSampler> multiUniformSamplers = new();
        public List<GaussianSampler> gaussianSamplers = new();
        public List<Curriculum> curriculums = new();

        public int ParamCount
        {
            get
            {
                int length = 0;
                length += simpleValues != null ? simpleValues.Count : 0;
                length += uniformSamplers != null ? uniformSamplers.Count : 0;
                length += multiUniformSamplers != null ? multiUniformSamplers.Count : 0;
                length += gaussianSamplers != null ? gaussianSamplers.Count : 0;
                length += curriculums != null ? curriculums.Count : 0;

                return length;
            }
        }

        public void LoadData(YamlObject yaml)
        {
            if (yaml.name != ConfigText.environmentParameters || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.environmentParameters} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    simpleValues.Add(new SimpleValue()
                    {
                        name = yamlValue.name,
                        value = yamlValue.value
                    });

                    continue;
                }

                if (element is YamlObject yamlObject)
                {
                    if (yamlObject.elements.Find(x => x.name == ConfigText.samplerType) != null)
                    {
                        var sampler = SampleFactory.GetSampler(yamlObject);
                        if (sampler != null)
                        {
                            if (sampler is UniformSampler uniformSampler)
                                uniformSamplers.Add(uniformSampler);
                            else if (sampler is MultiUniformSampler multiUniformSampler)
                                multiUniformSamplers.Add(multiUniformSampler);
                            else if (sampler is GaussianSampler gaussianSampler)
                                gaussianSamplers.Add(gaussianSampler);
                        }
                    }
                    else if (yamlObject.elements.Find(x => x.name == ConfigText.curriculum) != null)
                        curriculums.Add(new Curriculum(yamlObject));
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.environmentParameters;

            foreach (var item in simpleValues)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in uniformSamplers)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in multiUniformSamplers)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in gaussianSamplers)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in curriculums)
                AddEnvParamToYaml(yaml, item);

            return yaml;
        }

        protected void AddEnvParamToYaml(YamlObject yaml, EnvParam item)
        {
            var yamlElement = item.ToYaml();

            if (yamlElement is YamlObject yamlObject)
                yamlObject.parent = yaml;

            yaml.elements.Add(yamlElement);
        }
    }
}