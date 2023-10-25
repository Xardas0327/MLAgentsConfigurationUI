namespace Xardas.MLAgents.Cli
{
    public static class CliText
    {
        //CLI Settings
        public const string deterministic = "deterministic";
        public const string numAreas = "num-areas";
        public const string debug = "debug";
        public const string resultsDir = "results-dir";

        //Environment Settings
        public const string envPath = "env";
        public const string envArgs = "env-args";
        public const string basePort = "base-port";
        public const string numEnvs = "num-envs";
        public const string timeoutWait = "timeout-wait";
        public const string seed = "seed";
        public const string maxLifetimeRestarts = "max-lifetime-restarts";
        public const string restartsRateLimitN = "restarts-rate-limit-n";
        public const string restartsRateLimitPeriodS = "restarts-rate-limit-period-s";

        //Engine Settings
        public const string width = "width";
        public const string height = "height";
        public const string qualityLevel = "quality-level";
        public const string timeScale = "time-scale";
        public const string targetFrameRate = "target-frame-rate";
        public const string captureFrameRate = "capture-frame-rate";
        public const string noGraphics = "no-graphics";

        //Checkpoint Settings
        public const string runId = "run-id";
        public const string initializeFrom = "initialize-from";
        public const string resume = "resume";
        public const string force = "force";
        public const string inference = "inference";

        //TorchSettings
        public const string device = "torch-device";
    }
}