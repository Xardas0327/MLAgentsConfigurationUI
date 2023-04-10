namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public enum SamplerType { uniform, multirangeuniform, gaussian }

    public abstract class Sampler: EnvParam
    {
        public SamplerType type;
    }
}