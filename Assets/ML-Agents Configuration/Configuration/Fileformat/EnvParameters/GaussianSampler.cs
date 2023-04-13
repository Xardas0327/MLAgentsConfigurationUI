namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public class GaussianSampler : Sampler
    {
        public float mean;
        public float stDev;

        public GaussianSampler()
        {
            type = SamplerType.gaussian;
        }
    }
}