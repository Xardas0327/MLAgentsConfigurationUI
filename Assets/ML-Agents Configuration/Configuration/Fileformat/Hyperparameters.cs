using System;
using System.Globalization;
using System.Threading;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public enum ScheduleType { linear, constant}

    [Serializable]
    public class Hyperparameters
    {
        [Tooltip(ConfigTooltip.learningRate)]
        [Min(0)]
        public float learningRate = 0.0003f;
        [Tooltip(ConfigTooltip.batchSize)]
        public uint batchSize = 512;
        [Tooltip(ConfigTooltip.overwriteBufferSize)]
        public bool overwriteBufferSize = false;
        [Tooltip(ConfigTooltip.bufferSize)]
        public uint bufferSize = 10240; //default = 10240 for PPO and 50000 for SAC
        [Tooltip(ConfigTooltip.overwriteLearningRateSchedule)]
        public bool overwriteLearningRateSchedule = false;
        [Tooltip (ConfigTooltip.learningRateSchedule)]
        public ScheduleType learningRateSchedule = ScheduleType.linear; //default = linear for PPO and constant for SAC

        [Header("PPO/POCA specific")]
        [Tooltip(ConfigTooltip.beta)]
        [Min(0)]
        public float beta = 0.005f;
        [Tooltip(ConfigTooltip.epsilon)]
        [Min(0)]
        public float epsilon = 0.2f;
        [Tooltip(ConfigTooltip.overwriteBetaSchedule)]
        public bool overwriteBetaSchedule = false;
        [Tooltip(ConfigTooltip.betaSchedule)]
        public ScheduleType betaSchedule = ScheduleType.linear; //The default should be the learningRateSchedule
        [Tooltip(ConfigTooltip.overwriteEpsilonSchedule)]
        public bool overwriteEpsilonSchedule = false;
        [Tooltip(ConfigTooltip.epsilonSchedule)]
        public ScheduleType epsilonSchedule = ScheduleType.linear; //The default should be the learningRateSchedule
        [Tooltip(ConfigTooltip.lambd)]
        [Min(0)]
        public float lambd = 0.95f;
        [Tooltip(ConfigTooltip.numEpoch)]
        public uint numEpoch = 3;
        
        [Header("SAC specific")]
        [Tooltip(ConfigTooltip.bufferInitSteps)]
        public uint bufferInitSteps = 0;
        [Tooltip(ConfigTooltip.initEntcoef)]
        [Min(0)]
        public float initEntcoef = 1f;
        [Tooltip(ConfigTooltip.saveReplayBuffer)]
        public bool saveReplayBuffer = false;
        [Tooltip(ConfigTooltip.tau)]
        [Min(0)]
        public float tau = 0.005f;
        [Tooltip(ConfigTooltip.stepsPerUpdate)]
        public uint stepsPerUpdate = 1;
        [Tooltip(ConfigTooltip.overwriteRewardSignalNumUpdate)]
        public bool overwriteRewardSignalNumUpdate = false;
        [Tooltip(ConfigTooltip.rewardSignalNumUpdate)]
        public uint rewardSignalNumUpdate = 1; //The default should be the stepsPerUpdate

        const uint defaultPPOBufferSize = 10240;
        const uint defaultSACBufferSize = 50000;

        public static readonly string[] OnlyPpoAndPocaFields = 
        {
            nameof(beta), 
            nameof(epsilon),
            nameof(overwriteBetaSchedule),
            nameof(betaSchedule),
            nameof(overwriteEpsilonSchedule),
            nameof(epsilonSchedule),
            nameof(lambd),
            nameof(numEpoch),
        };

        public static readonly string[] OnlySacFields =
        {
            nameof(bufferInitSteps),
            nameof(initEntcoef),
            nameof(saveReplayBuffer),
            nameof(tau),
            nameof(stepsPerUpdate),
            nameof(overwriteRewardSignalNumUpdate),
            nameof(rewardSignalNumUpdate),
        };

        public Hyperparameters(TrainerType trainerType) 
        {
            bufferSize = trainerType == TrainerType.sac ? defaultSACBufferSize : defaultPPOBufferSize;
            learningRateSchedule = trainerType == TrainerType.sac ? ScheduleType.constant : ScheduleType.linear;
        }

        public Hyperparameters(TrainerType trainerType, YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.hyperparameters || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.hyperparameters} is not right.");

            bufferSize = trainerType == TrainerType.sac ? defaultSACBufferSize : defaultPPOBufferSize;
            learningRateSchedule = trainerType == TrainerType.sac ? ScheduleType.constant : ScheduleType.linear;

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch(yamlValue.name)
                    {
                        case ConfigText.learningRate:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out learningRate);
                            break;
                        case ConfigText.batchSize:
                            UInt32.TryParse(value, out batchSize);
                            break;
                        case ConfigText.bufferSize:
                            UInt32.TryParse(value, out bufferSize);
                            overwriteBufferSize = true;
                            break;
                        case ConfigText.learningRateSchedule:
                            overwriteLearningRateSchedule = true;
                            if (value == "constant")
                                learningRateSchedule = ScheduleType.constant;
                            break;

                        //PPO and POCA specific Configurations
                        case ConfigText.beta:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out beta);
                            break;
                        case ConfigText.epsilon:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out epsilon);
                            break;
                        case ConfigText.betaSchedule:
                            if (value == "constant")
                                betaSchedule = ScheduleType.constant;

                            overwriteBetaSchedule = true;
                            break;
                        case ConfigText.epsilonSchedule:
                            if (value == "constant")
                                epsilonSchedule = ScheduleType.constant;

                            overwriteEpsilonSchedule = true;
                            break;
                        case ConfigText.lambd:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out lambd);
                            break;
                        case ConfigText.numEpoch:
                            UInt32.TryParse(value, out numEpoch);
                            break;

                        //SAC-specific Configurations
                        case ConfigText.bufferInitSteps:
                            UInt32.TryParse(value, out bufferInitSteps);
                            break;
                        case ConfigText.initEntcoef:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out initEntcoef);
                            break;
                        case ConfigText.saveReplayBuffer:
                            if (value == "true")
                                saveReplayBuffer = true;
                            break;
                        case ConfigText.tau:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out tau);
                            break;
                        case ConfigText.stepsPerUpdate:
                            UInt32.TryParse(value, out stepsPerUpdate);
                            break;
                        case ConfigText.rewardSignalNumUpdate:
                            UInt32.TryParse(value, out rewardSignalNumUpdate);

                            overwriteRewardSignalNumUpdate = true;
                            break;
                    }
                }
            }

            //The default of betaSchedule should be the learningRateSchedule
            if (!overwriteBetaSchedule)
                betaSchedule = learningRateSchedule;

            //The default of epsilonSchedule should be the learningRateSchedule
            if (!overwriteEpsilonSchedule)
                epsilonSchedule = learningRateSchedule;

            //The default of rewardSignalNumUpdate should be the stepsPerUpdate
            if (!overwriteRewardSignalNumUpdate)
                rewardSignalNumUpdate = stepsPerUpdate;
        }

        public YamlObject ToYaml(TrainerType trainerType)
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.hyperparameters;

            yaml.elements.Add(new YamlValue(ConfigText.learningRate, learningRate));
            yaml.elements.Add(new YamlValue(ConfigText.batchSize, batchSize));

            if (overwriteBufferSize)
                yaml.elements.Add(new YamlValue(ConfigText.bufferSize, bufferSize));

            if (overwriteLearningRateSchedule)
                yaml.elements.Add(new YamlValue(ConfigText.learningRateSchedule, learningRateSchedule));

            switch(trainerType)
            {
                case TrainerType.ppo:
                case TrainerType.poca:
                    yaml.elements.Add(new YamlValue(ConfigText.beta, beta));
                    yaml.elements.Add(new YamlValue(ConfigText.epsilon, epsilon));

                    if(overwriteBetaSchedule)
                        yaml.elements.Add(new YamlValue(ConfigText.betaSchedule, betaSchedule));

                    if (overwriteEpsilonSchedule)
                        yaml.elements.Add(new YamlValue(ConfigText.epsilonSchedule, epsilonSchedule));

                    yaml.elements.Add(new YamlValue(ConfigText.lambd, lambd));
                    yaml.elements.Add(new YamlValue(ConfigText.numEpoch, numEpoch));
                    break;
                case TrainerType.sac:
                    yaml.elements.Add(new YamlValue(ConfigText.bufferInitSteps, bufferInitSteps));
                    yaml.elements.Add(new YamlValue(ConfigText.initEntcoef, initEntcoef));
                    yaml.elements.Add(new YamlValue(ConfigText.saveReplayBuffer, saveReplayBuffer));
                    yaml.elements.Add(new YamlValue(ConfigText.tau, tau));
                    yaml.elements.Add(new YamlValue(ConfigText.stepsPerUpdate, stepsPerUpdate));

                    if (overwriteRewardSignalNumUpdate)
                        yaml.elements.Add(new YamlValue(ConfigText.rewardSignalNumUpdate, rewardSignalNumUpdate));
                    break;
            }

            return yaml;
        }
    }
}