using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.SettingParameter
{
    [Serializable]
    public class EnvironmentSettings : ISettings
    {
        public bool isUseEnvPath;
        [Tooltip(ConfigTooltip.envPath)]
        public string envPath;

        public bool isUseEnvArgs;
        [Tooltip(ConfigTooltip.envArgs)]
        public string envArgs;

        public bool isUseBasePort;
        [Tooltip(ConfigTooltip.basePort)]
        public uint basePort = 5005;

        public bool isUseNumEnvs;
        [Tooltip(ConfigTooltip.numEnvs)]
        [Min(1)]
        public uint numEnvs = 1;

        public bool isUseTimeoutWait;
        [Tooltip(ConfigTooltip.timeoutWait)]
        public uint timeoutWait = 60;

        public bool isUseSeed;
        [Tooltip(ConfigTooltip.seed)]
        public int seed = -1;

        public bool isUseMaxLifetimeRestarts;
        [Tooltip(ConfigTooltip.maxLifetimeRestarts)]
        [Min(-1)]
        public int maxLifetimeRestarts = 10;

        public bool isUseRestartsRateLimitN;
        [Tooltip(ConfigTooltip.restartsRateLimitN)]
        [Min(-1)]
        public int restartsRateLimitN = 1;

        public bool isUseRestartsRateLimitPeriodS;
        [Tooltip(ConfigTooltip.restartsRateLimitPeriodS)]
        public uint restartsRateLimitPeriodS = 60;

        public bool IsUse => 
                isUseEnvPath || isUseEnvArgs
            || isUseBasePort || isUseNumEnvs || isUseTimeoutWait
            || isUseSeed || isUseMaxLifetimeRestarts
            || isUseRestartsRateLimitN || isUseRestartsRateLimitPeriodS;

        public EnvironmentSettings() { }
        public EnvironmentSettings(YamlObject yaml) 
        {
            if (yaml == null || yaml.name != ConfigText.environmentSettings || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.environmentSettings} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.envPath:
                            envPath = yamlValue.value; // we have to have the original string.
                            isUseEnvPath = true;
                            break;
                        case ConfigText.envArgs:
                            envArgs = yamlValue.value; // we have to have the original string.
                            isUseEnvArgs = true;
                            break;
                        case ConfigText.basePort:
                            UInt32.TryParse(value, out basePort);
                            isUseBasePort = true;
                            break;
                        case ConfigText.numEnvs:
                            UInt32.TryParse(value, out numEnvs);
                            isUseNumEnvs = true;
                            break;
                        case ConfigText.timeoutWait:
                            UInt32.TryParse(value, out timeoutWait);
                            isUseTimeoutWait = true;
                            break;
                        case ConfigText.seed:
                            Int32.TryParse(value, out seed);
                            isUseSeed = true;
                            break;
                        case ConfigText.maxLifetimeRestarts:
                            Int32.TryParse(value, out maxLifetimeRestarts);
                            isUseMaxLifetimeRestarts = true;
                            break;
                        case ConfigText.restartsRateLimitN:
                            Int32.TryParse(value, out restartsRateLimitN);
                            isUseRestartsRateLimitN = true;
                            break;
                        case ConfigText.restartsRateLimitPeriodS:
                            UInt32.TryParse(value, out restartsRateLimitPeriodS);
                            isUseRestartsRateLimitPeriodS = true;
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.environmentSettings;

            if (isUseEnvPath)
                yaml.elements.Add(new YamlValue(ConfigText.envPath, envPath));

            if (isUseEnvArgs)
                yaml.elements.Add(new YamlValue(ConfigText.envArgs, envArgs));

            if (isUseBasePort)
                yaml.elements.Add(new YamlValue(ConfigText.basePort, basePort));

            if (isUseNumEnvs)
                yaml.elements.Add(new YamlValue(ConfigText.numEnvs, numEnvs));

            if (isUseSeed)
                yaml.elements.Add(new YamlValue(ConfigText.seed, seed));

            if (isUseTimeoutWait)
                yaml.elements.Add(new YamlValue(ConfigText.timeoutWait, timeoutWait));

            if (isUseMaxLifetimeRestarts)
                yaml.elements.Add(new YamlValue(ConfigText.maxLifetimeRestarts, maxLifetimeRestarts));

            if (isUseRestartsRateLimitN)
                yaml.elements.Add(new YamlValue(ConfigText.restartsRateLimitN, restartsRateLimitN));

            if (isUseRestartsRateLimitPeriodS)
                yaml.elements.Add(new YamlValue(ConfigText.restartsRateLimitPeriodS, restartsRateLimitPeriodS));

            return yaml;
        }
    }
}