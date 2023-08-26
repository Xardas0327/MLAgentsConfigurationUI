using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat.EnvParameter;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [CreateAssetMenu(fileName = "EnvironmentParameters", menuName = "ML-Agents Config files/Environment Parameters")]
    public class EnvironmentParameters : ConfigFile
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

        public override void LoadData(YamlObject yaml)
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

        public override YamlObject ToYaml()
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

        public override bool IsValid()
        {
            bool isValid = CheckNames() 
                && CheckUniformSampler()
                && CheckMultiUniformSampler()
                && CheckCurriculums();

            if (isValid)
                Debug.Log("It looks a valid Environment Parameters file.");

            return isValid;
        }

        bool CheckNames()
        {
            bool isValid = true;

            var names = new Dictionary<string, int>();
            CollectNames(names, simpleValues.Cast<EnvParam>());
            CollectNames(names, uniformSamplers.Cast<EnvParam>());
            CollectNames(names, multiUniformSamplers.Cast<EnvParam>());
            CollectNames(names, gaussianSamplers.Cast<EnvParam>());
            CollectNames(names, curriculums.Cast<EnvParam>());
            foreach (var item in names)
            {
                if (item.Value > 1)
                {
                    Debug.LogError($"Every environment parameters have to have a unique name, but there are the {item.Key} {item.Value} times.");
                    isValid = false;
                }
            }

            return isValid;
        }

        bool CheckUniformSampler()
        {
            bool isValid = true;
            foreach (var item in uniformSamplers)
            {
                if (!CheckNumber(item.minValue, item.maxValue))
                {
                    Debug.LogError($"The {item.name} uniform sampler's min value ({item.minValue}) is not smaller than the max value ({item.maxValue}).");
                    isValid = false;
                }
            }
            return isValid;
        }

        bool CheckMultiUniformSampler()
        {
            bool isValid = true;
            foreach (var item in multiUniformSamplers)
            {
                foreach (var array in item.values)
                {
                    if (!CheckNumber(array.minValue, array.maxValue))
                    {
                        Debug.LogError($"The {item.name} multi uniform sampler have an invalid array. The min value ({array.minValue}) is not smaller than the max value ({array.maxValue})");
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        bool CheckCurriculums()
        {
            foreach (var curriculum in curriculums)
            {
                if(curriculum.lessons.Count < 2)
                {
                    Debug.LogWarning($"The {curriculum.name} curriculum has {curriculum.lessons.Count} lesson(s). It should have 2 or more.");
                }
                else
                {
                    var completionCriteria = curriculum.lessons[curriculum.lessons.Count - 1].completionCriteria;
                    if (completionCriteria != null && !string.IsNullOrEmpty(completionCriteria.behavior))
                    {
                        Debug.LogWarning($"The completion criteria of last lesson is useless. But the behavior is not empty in {curriculum.name} curriculum.");
                    }
                }
            }
            return true;
        }

        void CollectNames(Dictionary<string, int> names, IEnumerable<EnvParam> list)
        {
            foreach(var item in list)
            {
                if(!names.ContainsKey(item.name))
                    names.Add(item.name, 0);

                ++names[item.name];
            }
        }

        bool CheckNumber(float smaller, float higher)
        {
            return smaller < higher;
        }
    }
}