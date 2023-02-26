using System;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public enum TrainerType { ppo, sac, poca}

    public class MLAgentsConfigFile
    {
        public string name;
        public TrainerType trainerType = TrainerType.ppo;
        public int summaryFreq = 50000;
        public int timeHorizon = 64;
        public int maxSteps = 500000;
        public int keepCheckpoints = 5;
        public int checkpointInterval = 500000;
        // We don't care with it, because we can use it as CLI param.
        //public string init_path
        public bool threaded = false;
        public Hyperparameters hyperparameters = new Hyperparameters(TrainerType.ppo);
        public NetworkSettings networkSettings = new();
        public RewardSignals rewardSignals = new();
        public BehavioralCloning behavioralCloning = null;
        public SelfPlay selfPlay = null;

        public MLAgentsConfigFile() { }

        public MLAgentsConfigFile(YamlElement yaml)
        {
            YamlObject yamlFile = yaml as YamlObject;
            if (yamlFile == null
                || yamlFile.elements.Count < 1 || !(yamlFile.elements[0] is YamlObject))
                throw new System.Exception("The yaml file is not a MLAgents config file.");

            //TODO: this one is temp only
            yamlFile = yamlFile.elements[0] as YamlObject;
            if(yamlFile.name != ConfigText.behaviorsText)
                throw new System.Exception($"The {ConfigText.behaviorsText} is not right.");

            yamlFile = yamlFile.elements[0] as YamlObject;
            name = yamlFile.name;
            //Firstly the YamlValue only
            foreach (var element in yamlFile.elements)
            {
                var yamlValue = element as YamlValue;
                if(yamlValue != null)
                {
                    string value = yamlValue.value.ToLower();
                    switch(yamlValue.name)
                    {
                        case ConfigText.trainerTypeText:
                            if (value == "ppo")
                                trainerType = TrainerType.ppo;
                            else if (value == "sac")
                                trainerType = TrainerType.sac;
                            else if (value == "poca")
                                trainerType = TrainerType.poca;
                            break;
                        case ConfigText.summaryFreqText:
                            Int32.TryParse(value, out summaryFreq);
                            break;
                        case ConfigText.timeHorizonText:
                            Int32.TryParse(value, out timeHorizon);
                            break;
                        case ConfigText.maxStepsText:
                            Int32.TryParse(value, out maxSteps);
                            break;
                        case ConfigText.keepCheckpointsText:
                            Int32.TryParse(value, out keepCheckpoints);
                            break;
                        case ConfigText.checkpointIntervalText:
                            Int32.TryParse(value, out checkpointInterval);
                            break;
                        case ConfigText.threadedText:
                            if (value == "true")
                                threaded = true;
                            break;
                    }
                }
            }

            //Secondly YamlObject, because know we have to know the TrainerType
            YamlObject behavioralCloningYamlObject = null;
            foreach (var element in yamlFile.elements)
            {
                var yamlObject = element as YamlObject;
                if (yamlObject != null)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.hyperparametersText:
                            hyperparameters = new Hyperparameters(trainerType, yamlObject);
                            break;
                        case ConfigText.networkSettingsText:
                            networkSettings = new NetworkSettings(yamlObject);
                            break;
                        case ConfigText.rewardSignalsText:
                            rewardSignals = new RewardSignals(yamlObject);
                            break;
                        case ConfigText.behavioralCloningText:
                            behavioralCloningYamlObject = yamlObject;
                            break;
                        case ConfigText.selfPlayText:
                            selfPlay = new SelfPlay(yamlObject);
                            break;
                    }
                }
            }

            //The hyperparameters has to be inited before behavioralCloning
            if (behavioralCloningYamlObject != null)
                behavioralCloning = new BehavioralCloning(
                    behavioralCloningYamlObject,
                    hyperparameters.batchSize,
                    hyperparameters.numEpoch);
        }
    }
}