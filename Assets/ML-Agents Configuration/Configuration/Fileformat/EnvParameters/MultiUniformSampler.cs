using System.Collections.Generic;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public class MultiUniformSampler : Sampler
    {
        public List<(float minValue, float maxValue)> values = new();

        public MultiUniformSampler()
        {
            type = SamplerType.multirangeuniform;
        }
    }
}