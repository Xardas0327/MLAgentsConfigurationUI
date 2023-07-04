using System.Globalization;
using System.Linq;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{
    public static class SampleFactory
    {
        public static Sampler GetSampler(YamlObject yaml)
        {
            var samplerType = yaml.elements.Find(x => x.name == ConfigText.samplerType);
            var samplerParameters = yaml.elements.Find(x => x.name == ConfigText.samplerParameters);
            if (samplerType == null || samplerParameters == null)
                throw new System.Exception("The sampler has to have sampler_type and sampler_parameters.");

            string type = ((YamlValue)samplerType).value.ToLower();
            var parameters = (YamlObject)samplerParameters;

            Sampler sampler = null;

            switch (type)
            {
                case "uniform":
                    sampler = CreateUniformSampler(parameters);
                    break;
                case "multirangeuniform":
                    sampler = CreateMultiUniformSampler(parameters);
                    break;
                case "gaussian":
                    sampler = CreateGaussianSampler(parameters);
                    break;
            }

            if(sampler != null)
                sampler.name = yaml.name;

            return sampler;
        }

        private static UniformSampler CreateUniformSampler(YamlObject parameters)
        {
            var sampler = new UniformSampler();
            foreach (var element in parameters.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    switch (yamlValue.name)
                    {
                        case ConfigText.minValue:
                            if (!float.TryParse(
                                yamlValue.value,
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out sampler.minValue
                            ))
                            {
                                throw new System.Exception("A Uniform Sampler has unvalid min value");
                            }
                            break;
                        case ConfigText.maxValue:
                            if(!float.TryParse(
                                yamlValue.value,
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out sampler.maxValue
                            ))
                            {
                                throw new System.Exception("A Uniform Sampler has unvalid max value");
                            }
                            break;
                    }
                }
            }
            return sampler;
        }

        private static MultiUniformSampler CreateMultiUniformSampler(YamlObject parameters)
        {
            var sampler = new MultiUniformSampler();
            var intervals = parameters.elements.Find(x => x.name == ConfigText.intervals);

            if (intervals != null && (intervals is YamlValue yamlValue))
            {
                var valuesText = yamlValue.value;
                if (valuesText != null && valuesText[0] == '[' && valuesText[valuesText.Length - 1] == ']')
                {
                    var values = valuesText.Substring(1, valuesText.Length - 2).Split(',').Select(x => x.Trim()).ToList();
                    if (values.Count > 0 || values.Count % 2 == 0)
                    {
                        bool isWrong = false;
                        for(int i = 0; i < values.Count; i+=2)
                        {
                            if (values[i][0] != '[' || values[i + 1][values[i+1].Length -1] != ']')
                            {
                                isWrong = true;
                                break;
                            }

                            float minValue, maxValue;
                            var first = values[i].Substring(1);
                            var second = values[i + 1].Substring(0, values[i + 1].Length - 1);

                            if (!float.TryParse(first, NumberStyles.Any, CultureInfo.InvariantCulture, out minValue))
                            {
                                isWrong = true;
                                break;
                            }
                            if (!float.TryParse(second, NumberStyles.Any, CultureInfo.InvariantCulture, out maxValue))
                            {
                                isWrong = true;
                                break;
                            }
                            sampler.values.Add(new MultiUniformSamplerValue(minValue, maxValue));
                        }

                        if (!isWrong)
                            return sampler;
                    }
                }
            }

            throw new System.Exception("A multirangeuniform sampler has to have valid intervals.");
        }

        private static GaussianSampler CreateGaussianSampler(YamlObject parameters)
        {
            var sampler = new GaussianSampler();
            foreach (var element in parameters.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    switch (yamlValue.name)
                    {
                        case ConfigText.mean:
                            if(!float.TryParse(
                                yamlValue.value,
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out sampler.mean
                            ))

                            {
                                throw new System.Exception("A Gaussian Sampler has unvalid mean value");
                            }
                            break;
                        case ConfigText.stDev:
                            if (!float.TryParse(
                                yamlValue.value,
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out sampler.stDev
                            ))
                            {
                                throw new System.Exception("A Gaussian Sampler has unvalid stDev value");
                            }
                            break;
                    }
                }
            }
            return sampler;
        }
    }
}