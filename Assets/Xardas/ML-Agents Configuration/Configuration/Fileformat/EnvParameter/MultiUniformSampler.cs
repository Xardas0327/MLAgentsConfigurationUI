using System;
using System.Collections.Generic;
using System.Text;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameter
{
    [Serializable]
    public struct MultiUniformSamplerValue
    {
        public float minValue;
        public float maxValue;

        public MultiUniformSamplerValue(float min, float max)
        {
            minValue = min; 
            maxValue = max;
        }
    }


    [Serializable]
    public class MultiUniformSampler : Sampler
    {
        public List<MultiUniformSamplerValue> values = new();

        public MultiUniformSampler()
        {
            type = SamplerType.multirangeuniform;
        }

        public override YamlElement ToYaml()
        {
            var yaml = (YamlObject)base.ToYaml();

            var samplerParameters = new YamlObject();
            samplerParameters.name = ConfigText.samplerParameters;
            samplerParameters.parent = yaml;
            yaml.elements.Add(samplerParameters);

            var sb = new StringBuilder();
            sb.Append("[");
            for(var i = 0; i < values.Count; ++i)
            {
                if(i > 0)
                    sb.Append(", ");

                sb.Append($"[{values[i].minValue}, {values[i].maxValue}]");
            }
            sb.Append("]");

            samplerParameters.elements.Add(new YamlValue(ConfigText.intervals, sb.ToString()));

            return yaml;
        }
    }
}