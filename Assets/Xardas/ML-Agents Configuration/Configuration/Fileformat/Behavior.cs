using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    public enum TrainerType { ppo, sac, poca }

    [CreateAssetMenu(fileName = "Behavior", menuName = "ML-Agents Config files/Behavior")]
    public class Behavior : ConfigFile
    {
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
        [Tooltip(ConfigTooltip.evenCheckpoints)]
        public bool evenCheckpoints = false;
        [Tooltip(ConfigTooltip.checkpointInterval)]
        public uint checkpointInterval = 500000;
        [Tooltip(ConfigTooltip.initPath)]
        public string initPath;
        [Tooltip(ConfigTooltip.threaded)]
        public bool threaded = false;
        public Hyperparameters hyperparameters = new Hyperparameters(TrainerType.ppo);
        public NetworkSettings networkSettings = new();
        public RewardSignals rewardSignals = new();

        public bool isUseBehavioralCloning = false;
        public BehavioralCloning behavioralCloning = null;
        public bool isUseSelfPlay = false;
        public SelfPlay selfPlay = null;

        public override void LoadData(YamlObject yaml)
        {
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
                        case ConfigText.evenCheckpoints:
                            if (value == "true")
                                evenCheckpoints = true;
                            break;
                        case ConfigText.checkpointInterval:
                            UInt32.TryParse(value, out checkpointInterval);
                            break;
                        case ConfigText.initPath:
                            initPath = yamlValue.value; // we have to have the original string.
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

        public override YamlObject ToYaml()
        {
            var yaml = new YamlObject()
            {
                name = behaviorName
            };

            yaml.elements.Add(new YamlValue(ConfigText.trainerType, trainerType));
            yaml.elements.Add(new YamlValue(ConfigText.summaryFreq, summaryFreq));
            yaml.elements.Add(new YamlValue(ConfigText.timeHorizon, timeHorizon));
            yaml.elements.Add(new YamlValue(ConfigText.maxSteps, maxSteps));
            yaml.elements.Add(new YamlValue(ConfigText.keepCheckpoints, keepCheckpoints));

            if (evenCheckpoints)
                yaml.elements.Add(new YamlValue(ConfigText.evenCheckpoints, evenCheckpoints));
            else
                yaml.elements.Add(new YamlValue(ConfigText.checkpointInterval, checkpointInterval));

            yaml.elements.Add(new YamlValue(ConfigText.threaded, threaded));

            if(!string.IsNullOrEmpty(initPath))
                yaml.elements.Add(new YamlValue(ConfigText.initPath, initPath));

            var hp = hyperparameters.ToYaml(trainerType);
            hp.parent = yaml;
            yaml.elements.Add(hp);

            var ns = networkSettings.ToYaml();
            ns.parent = yaml;
            yaml.elements.Add(ns);

            var rs = rewardSignals.ToYaml();
            rs.parent = yaml;
            yaml.elements.Add(rs);

            if (isUseBehavioralCloning && behavioralCloning != null)
            {
                var bc = behavioralCloning.ToYaml();
                bc.parent = yaml;
                yaml.elements.Add(bc);
            }

            if (isUseSelfPlay && selfPlay != null)
            {
                var sp = selfPlay.ToYaml();
                sp.parent = yaml;
                yaml.elements.Add(sp);
            }

            return yaml;
        }

        public override bool IsValid()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(behaviorName))
            {
                Debug.LogError("The behavior name can't be empty.");
                isValid = false;
            }

            isValid &= hyperparameters.IsValid(trainerType)
                && networkSettings.IsValid()
                && rewardSignals.IsValid();

            if(isUseBehavioralCloning)
                isValid &= behavioralCloning.IsValid();

            if(isUseSelfPlay)
                isValid &= selfPlay.IsValid();

            if (isValid)
                Debug.Log("It looks a valid behavior file.");

            return isValid;
        }
    }
}