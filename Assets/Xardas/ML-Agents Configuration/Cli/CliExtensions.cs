using System.Text;
using Xardas.MLAgents.Configuration.Fileformat.SettingParameter;

namespace Xardas.MLAgents.Cli
{
    public static class CliExtensions
    {
        public const string defaultWindowsCLI = "cmd.exe";
        public const string defaultWindowsArguments = "/K \"{{commands}}\"";

        public const string defaultMacCLI = @"/System/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
        public const string defaultMacArguments = "{{commands}}";

        public const string defaultLinuxCLI = "gnome-terminal";
        public const string defaultLinuxArguments = "-e \" {{commands}} \"";

        public const string shellScriptFileName = "mlAgentsCommand.sh";

        public static string GetCliArguments(this CliSettings cliSettings)
        {
            if (!cliSettings.IsUse)
                return "";

            var arguments = new StringBuilder();

            if (cliSettings.isUseDeterministic && cliSettings.deterministic)
                arguments.Append($"--{CliText.deterministic} ");

            if (cliSettings.isUseNumAreas)
                arguments.Append($"--{CliText.numAreas} {cliSettings.numAreas} ");

            if (cliSettings.isUseDebug && cliSettings.debug)
                arguments.Append($"--{CliText.debug} ");

            if (cliSettings.isUseResultsDir && !string.IsNullOrEmpty(cliSettings.resultsDir))
                arguments.Append($"--{CliText.resultsDir} \"{cliSettings.resultsDir}\" ");

            return arguments.ToString();
        }

        public static string GetCliArguments(this EnvironmentSettings environmentSettings)
        {
            if (!environmentSettings.IsUse)
                return "";

            var arguments = new StringBuilder();

            if (environmentSettings.isUseEnvPath && !string.IsNullOrEmpty(environmentSettings.envPath))
                arguments.Append($"--{CliText.envPath} \"{environmentSettings.envPath}\" ");

            if (environmentSettings.isUseEnvArgs && !string.IsNullOrEmpty(environmentSettings.envArgs))
                arguments.Append($"--{CliText.envArgs} {environmentSettings.envArgs} ");

            if (environmentSettings.isUseBasePort)
                arguments.Append($"--{CliText.basePort} {environmentSettings.basePort} ");

            if (environmentSettings.isUseNumEnvs)
                arguments.Append($"--{CliText.numEnvs} {environmentSettings.numEnvs} ");

            if (environmentSettings.isUseTimeoutWait)
                arguments.Append($"--{CliText.timeoutWait} {environmentSettings.timeoutWait} ");

            if (environmentSettings.isUseSeed)
                arguments.Append($"--{CliText.seed} {environmentSettings.seed} ");

            if (environmentSettings.isUseMaxLifetimeRestarts)
                arguments.Append($"--{CliText.maxLifetimeRestarts} {environmentSettings.maxLifetimeRestarts} ");

            if (environmentSettings.isUseRestartsRateLimitN)
                arguments.Append($"--{CliText.restartsRateLimitN} {environmentSettings.restartsRateLimitN} ");

            if (environmentSettings.isUseRestartsRateLimitPeriodS)
                arguments.Append($"--{CliText.restartsRateLimitPeriodS} {environmentSettings.restartsRateLimitPeriodS} ");

            return arguments.ToString();
        }

        public static string GetCliArguments(this EngineSettings engineSettings)
        {
            if (!engineSettings.IsUse)
                return "";

            var arguments = new StringBuilder();

            if (engineSettings.isUseWidth)
                arguments.Append($"--{CliText.width} {engineSettings.width} ");

            if (engineSettings.isUseHeight)
                arguments.Append($"--{CliText.height} {engineSettings.height} ");

            if (engineSettings.isUseQualityLevel)
                arguments.Append($"--{CliText.qualityLevel} {engineSettings.qualityLevel} ");

            if (engineSettings.isUseTimeScale)
                arguments.Append($"--{CliText.timeScale} {engineSettings.timeScale} ");

            if (engineSettings.isUseTargetFrameRate)
                arguments.Append($"--{CliText.targetFrameRate} {engineSettings.targetFrameRate} ");

            if (engineSettings.isUseCaptureFrameRate)
                arguments.Append($"--{CliText.captureFrameRate} {engineSettings.captureFrameRate} ");

            if (engineSettings.isUseNoGraphics && engineSettings.noGraphics)
                arguments.Append($"--{CliText.noGraphics} ");

            return arguments.ToString();
        }

        public static string GetCliArguments(this CheckpointSettings checkpointSettings)
        {
            if (!checkpointSettings.IsUse)
                return "";

            var arguments = new StringBuilder();

            if (checkpointSettings.isUseRunId && !string.IsNullOrEmpty(checkpointSettings.runId))
                arguments.Append($"--{CliText.runId} {checkpointSettings.runId} ");

            if (checkpointSettings.isUseInitializeFrom)
                arguments.Append($"--{CliText.initializeFrom} \"{checkpointSettings.initializeFrom}\" ");

            if (checkpointSettings.isUseResume && checkpointSettings.resume)
                arguments.Append($"--{CliText.resume} ");

            if (checkpointSettings.isUseForce && checkpointSettings.force)
                arguments.Append($"--{CliText.force} ");

            if (checkpointSettings.isUseInference && checkpointSettings.inference)
                arguments.Append($"--{CliText.inference} ");

            return arguments.ToString();
        }

        public static string GetCliArguments(this TorchSettings torchSettings)
        {
            if (!torchSettings.IsUse)
                return "";

            var arguments = new StringBuilder();

            if (torchSettings.isUseDevice)
            {
                string deviceText = torchSettings.device == DeviceType.cuda0 ? TorchSettings.Cuda0Text : torchSettings.device.ToString();
                arguments.Append($"--{CliText.device} {deviceText} ");
            }

            return arguments.ToString();
        }
    }
}