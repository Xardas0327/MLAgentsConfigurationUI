namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public class UniformSampler : Sampler
    {
        public float minValue;
        public float maxValue;

        public UniformSampler()
        {
            type = SamplerType.uniform;
        }
    }
}