using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public enum ScheduleType { linear, constant}

    public class Hyperparameters
    {
        public float learningRate = 0.0003f;
        //This should always be multiple times smaller than buffer_size
        public int batchSize = 512;
        public int bufferSize = 10240;
        public ScheduleType learningRateSchedule = ScheduleType.linear;

        //PPO and POCA specific Configurations
        public float beta = 0.005f;
        public float epsilon = 0.2f;
        public ScheduleType betaSchedule = ScheduleType.linear; //The default should be the learningRateSchedule
        public ScheduleType epsilonSchedule = ScheduleType.linear; //The default should be the learningRateSchedule
        public float lambd = 0.95f;
        public int numEpoch = 3;

        //SAC-specific Configurations
        public int bufferInitSteps = 0;
        public float initEntcoef = 1f;
        public bool saveReplayBuffer = false;
        public float tau = 0.005f;
        public float stepsPerUpdate = 1;
        public float rewardSignalNumUpdate = 1;

        const int defaultPOOBufferSize = 10240;
        const int defaultSACBufferSize = 50000;

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
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
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


            return yaml;
        }
    }
}