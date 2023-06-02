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
        public float learningRate = 0.0003f;
        //This should always be multiple times smaller than buffer_size
        public int batchSize = 512;
        public int bufferSize = 10240; //default = 10240 for PPO and 50000 for SAC
        public ScheduleType learningRateSchedule = ScheduleType.linear; //default = linear for PPO and constant for SAC

        [Header("PPO/POCA specific")]
        public float beta = 0.005f;
        public float epsilon = 0.2f;
        public ScheduleType betaSchedule = ScheduleType.linear; //The default should be the learningRateSchedule
        public ScheduleType epsilonSchedule = ScheduleType.linear; //The default should be the learningRateSchedule
        public float lambd = 0.95f;
        public int numEpoch = 3;
        
        [Header("SAC specific")]
        public int bufferInitSteps = 0;
        public float initEntcoef = 1f;
        public bool saveReplayBuffer = false;
        public float tau = 0.005f;
        public float stepsPerUpdate = 1;
        public float rewardSignalNumUpdate = 1;

        const int defaultPOOBufferSize = 10240;
        const int defaultSACBufferSize = 50000;

        public static readonly string[] OnlyPpoAndPocaFields = 
        {
            nameof(beta), 
            nameof(epsilon), 
            nameof(betaSchedule),
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
            nameof(rewardSignalNumUpdate),
        };

        public Hyperparameters(TrainerType trainerType) 
        {
            bufferSize = trainerType == TrainerType.sac ? defaultSACBufferSize : defaultPOOBufferSize;
            learningRateSchedule = trainerType == TrainerType.sac ? ScheduleType.constant : ScheduleType.linear;
        }

        public Hyperparameters(TrainerType trainerType, YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.hyperparametersText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.hyperparametersText} is not right.");

            bool wasBufferSize = false;

            bool wasLearningRateSchedule = false;
            bool wasBetaSchedule = false;
            bool wasEpsilonSchedule = false;

            bool wasStepsPerUpdate = false;
            bool wasRewardSignalNumUpdate = false;

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch(yamlValue.name)
                    {
                        case ConfigText.learningRateText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out learningRate);
                            break;
                        case ConfigText.batchSizeText:
                            Int32.TryParse(value, out batchSize);
                            break;
                        case ConfigText.bufferSizeText:
                            Int32.TryParse(value, out bufferSize);

                            wasBufferSize = true;
                            break;
                        case ConfigText.learningRateScheduleText:
                            if (value == "constant")
                                learningRateSchedule = ScheduleType.constant;

                            wasLearningRateSchedule = true;
                            break;

                        //PPO and POCA specific Configurations
                        case ConfigText.betaText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out beta);
                            break;
                        case ConfigText.epsilonText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out epsilon);
                            break;
                        case ConfigText.betaScheduleText:
                            if (value == "constant")
                                betaSchedule = ScheduleType.constant;

                            wasBetaSchedule = true;
                            break;
                        case ConfigText.epsilonScheduleText:
                            if (value == "constant")
                                epsilonSchedule = ScheduleType.constant;

                            wasEpsilonSchedule = true;
                            break;
                        case ConfigText.lambdText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out lambd);
                            break;
                        case ConfigText.numEpochText:
                            Int32.TryParse(value, out numEpoch);
                            break;

                        //SAC-specific Configurations
                        case ConfigText.bufferInitStepsText:
                            Int32.TryParse(value, out bufferInitSteps);
                            break;
                        case ConfigText.initEntcoefText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out initEntcoef);
                            break;
                        case ConfigText.saveReplayBufferText:
                            if (value == "true")
                                saveReplayBuffer = true;
                            break;
                        case ConfigText.tauText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out tau);
                            break;
                        case ConfigText.stepsPerUpdateText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out stepsPerUpdate);

                            wasStepsPerUpdate = true;
                            break;
                        case ConfigText.rewardSignalNumUpdateText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out rewardSignalNumUpdate);

                            wasRewardSignalNumUpdate = true;
                            break;
                    }
                }
            }

            if(!wasBufferSize)
                bufferSize = trainerType == TrainerType.sac ? defaultSACBufferSize : defaultPOOBufferSize;


            if (!wasLearningRateSchedule)
                learningRateSchedule = trainerType == TrainerType.sac ? ScheduleType.constant : ScheduleType.linear;

            //The default of betaSchedule should be the learningRateSchedule
            if (!wasBetaSchedule)
                betaSchedule = learningRateSchedule;

            //The default of epsilonSchedule should be the learningRateSchedule
            if (!wasEpsilonSchedule)
                epsilonSchedule = learningRateSchedule;

            //The default of rewardSignalNumUpdate should be the stepsPerUpdate
            if (wasStepsPerUpdate && !wasRewardSignalNumUpdate)
                rewardSignalNumUpdate = stepsPerUpdate;
        }

        public YamlObject ToYaml(TrainerType trainerType)
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.hyperparametersText;

            yaml.elements.Add(new YamlValue(ConfigText.learningRateText, learningRate));
            yaml.elements.Add(new YamlValue(ConfigText.batchSizeText, batchSize));
            yaml.elements.Add(new YamlValue(ConfigText.bufferSizeText, bufferSize));
            yaml.elements.Add(new YamlValue(ConfigText.learningRateScheduleText, learningRateSchedule));

            switch(trainerType)
            {
                case TrainerType.ppo:
                case TrainerType.poca:
                    yaml.elements.Add(new YamlValue(ConfigText.betaText, beta));
                    yaml.elements.Add(new YamlValue(ConfigText.epsilonText, epsilon));

                    if(learningRateSchedule != betaSchedule)
                        yaml.elements.Add(new YamlValue(ConfigText.betaScheduleText, betaSchedule));

                    if (learningRateSchedule != epsilonSchedule)
                        yaml.elements.Add(new YamlValue(ConfigText.epsilonScheduleText, epsilonSchedule));

                    yaml.elements.Add(new YamlValue(ConfigText.lambdText, lambd));
                    yaml.elements.Add(new YamlValue(ConfigText.numEpochText, numEpoch));
                    break;
                case TrainerType.sac:
                    yaml.elements.Add(new YamlValue(ConfigText.bufferInitStepsText, bufferInitSteps));
                    yaml.elements.Add(new YamlValue(ConfigText.initEntcoefText, initEntcoef));
                    yaml.elements.Add(new YamlValue(ConfigText.saveReplayBufferText, saveReplayBuffer));
                    yaml.elements.Add(new YamlValue(ConfigText.tauText, tau));
                    yaml.elements.Add(new YamlValue(ConfigText.stepsPerUpdateText, stepsPerUpdate));
                    yaml.elements.Add(new YamlValue(ConfigText.rewardSignalNumUpdateText, rewardSignalNumUpdate));
                    break;
            }

            return yaml;
        }
    }
}