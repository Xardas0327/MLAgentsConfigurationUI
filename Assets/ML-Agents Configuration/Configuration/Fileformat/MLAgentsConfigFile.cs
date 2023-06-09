using System;
using UnityEngine;
using Xardas.MLAgents.Property;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public enum TrainerType { ppo, sac, poca }
    public class MLAgentsConfigFile : ScriptableObject
    {
        [SerializeField]
        [ReadOnly]
        private string yamlFolderPath;

        public string YamlFolderPath => yamlFolderPath;

        [Header("Behavior")]
        [Tooltip(ConfigTooltip.behaviorName)]
        public string behaviorName;
        public TrainerType trainerType = TrainerType.ppo;
        [Tooltip(ConfigTooltip.summaryFreq)]
        public uint summaryFreq = 50000;
        [Tooltip(ConfigTooltip.timeHorizon)]
        public uint timeHorizon = 64;
        [Tooltip(ConfigTooltip.maxSteps)]
        public uint maxSteps = 500000;
        [Tooltip(ConfigTooltip.keepCheckpoints)]
        public uint keepCheckpoints = 5;
        [Tooltip(ConfigTooltip.checkpointInterval)]
        public uint checkpointInterval = 500000;
        // We don't care with it, because we can use it as CLI param.
        //public string init_path
        [Tooltip(ConfigTooltip.threaded)]
        public bool threaded = false;
        public Hyperparameters hyperparameters = new Hyperparameters(TrainerType.ppo);
        public NetworkSettings networkSettings = new();
        public RewardSignals rewardSignals = new();

        public bool isUseBehavioralCloning = false;
        public BehavioralCloning behavioralCloning = null;
        public bool isUseSelfPlay = false;
        public SelfPlay selfPlay = null;

        public EnvironmentParameters environmentParameters = new();

        public void LoadData(string path)
        {
            yamlFolderPath = path;
        }

        public void LoadData(string path, YamlElement yaml)
        {
            LoadData(path);

            var yamlFile = yaml as YamlObject;
            if (yamlFile == null
                || yamlFile.elements.Count < 1 || !(yamlFile.elements[0] is YamlObject))
                throw new System.Exception("The yaml file is not a MLAgents config file.");

            foreach (var element in yamlFile.elements)
            {
                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.behaviors:
                            LoadBehaviors(yamlObject);
                            break;
                        case ConfigText.environmentParameters:
                            environmentParameters = new EnvironmentParameters(yamlObject);
                            break;
                    }
                }
            }
        }

        protected void LoadBehaviors(YamlObject yaml)
        {
            if (yaml.name != ConfigText.behaviors || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.behaviors} is not right.");

            yaml = yaml.elements[0] as YamlObject;
            behaviorName = yaml.name;
            //Firstly the YamlValue only
            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.trainerType:
                            if (value == "ppo")
                                trainerType = TrainerType.ppo;
                            else if (value == "sac")
                                trainerType = TrainerType.sac;
                            else if (value == "poca")
                                trainerType = TrainerType.poca;
                            break;
                        case ConfigText.summaryFreq:
                            UInt32.TryParse(value, out summaryFreq);
                            break;
                        case ConfigText.timeHorizon:
                            UInt32.TryParse(value, out timeHorizon);
                            break;
                        case ConfigText.maxSteps:
                            UInt32.TryParse(value, out maxSteps);
                            break;
                        case ConfigText.keepCheckpoints:
                            UInt32.TryParse(value, out keepCheckpoints);
                            break;
                        case ConfigText.checkpointInterval:
                            UInt32.TryParse(value, out checkpointInterval);
                            break;
                        case ConfigText.threaded:
                            if (value == "true")
                                threaded = true;
                            break;
                    }
                }
            }

            //Secondly YamlObject, because know we have to know the TrainerType
            YamlObject behavioralCloningYamlObject = null;
            foreach (var element in yaml.elements)
            {
                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.hyperparameters:
                            hyperparameters = new Hyperparameters(trainerType, yamlObject);
                            break;
                        case ConfigText.networkSettings:
                            networkSettings = new NetworkSettings(yamlObject);
                            break;
                        case ConfigText.rewardSignals:
                            rewardSignals = new RewardSignals(yamlObject);
                            break;
                        case ConfigText.behavioralCloning:
                            isUseBehavioralCloning = true;
                            behavioralCloningYamlObject = yamlObject;
                            break;
                        case ConfigText.selfPlay:
                            isUseSelfPlay = true;
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

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();

            var behaviorsYaml = ConvertBehaviorsToYaml();
            behaviorsYaml.parent = yaml;
            yaml.elements.Add(behaviorsYaml);

            if (environmentParameters.ParamCount > 0)
            {
                var envParametersYaml = environmentParameters.ToYaml();
                envParametersYaml.parent = yaml;
                yaml.elements.Add(envParametersYaml);
            }

            return yaml;
        }

        protected YamlObject ConvertBehaviorsToYaml()
        {
            var behaviors = new YamlObject();
            behaviors.name = ConfigText.behaviors;

            var mlName = new YamlObject()
            {
                name = behaviorName,
                parent = behaviors
            };
            behaviors.elements.Add(mlName);

            mlName.elements.Add(new YamlValue(ConfigText.trainerType, trainerType));
            mlName.elements.Add(new YamlValue(ConfigText.summaryFreq, summaryFreq));
            mlName.elements.Add(new YamlValue(ConfigText.timeHorizon, timeHorizon));
            mlName.elements.Add(new YamlValue(ConfigText.maxSteps, maxSteps));
            mlName.elements.Add(new YamlValue(ConfigText.keepCheckpoints, keepCheckpoints));
            mlName.elements.Add(new YamlValue(ConfigText.checkpointInterval, checkpointInterval));
            mlName.elements.Add(new YamlValue(ConfigText.threaded, threaded));

            var hp = hyperparameters.ToYaml(trainerType);
            hp.parent = mlName;
            mlName.elements.Add(hp);

            var ns = networkSettings.ToYaml();
            ns.parent = mlName;
            mlName.elements.Add(ns);

            var rs = rewardSignals.ToYaml();
            rs.parent = mlName;
            mlName.elements.Add(rs);

            if (isUseBehavioralCloning && behavioralCloning != null)
            {
                var bc = behavioralCloning.ToYaml();
                bc.parent = mlName;
                mlName.elements.Add(bc);
            }

            if (isUseSelfPlay && selfPlay != null)
            {
                var sp = selfPlay.ToYaml();
                sp.parent = mlName;
                mlName.elements.Add(sp);
            }

            return behaviors;
        }
    }
}