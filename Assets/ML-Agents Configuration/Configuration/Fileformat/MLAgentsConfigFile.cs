using System;
using System.Collections.Generic;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat.EnvParameters;
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
        public string behaviorName;
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

        public bool isUseBehavioralCloning = false;
        public BehavioralCloning behavioralCloning = null;
        public bool isUseSelfPlay = false;
        public SelfPlay selfPlay = null;

        [Header("Environment Parameters")]
        public List<SimpleValue> simpleValues = new();
        public List<UniformSampler> uniformSamplers = new();
        public List<MultiUniformSampler> multiUniformSamplers = new();
        public List<GaussianSampler> gaussianSamplers = new();
        public List<Curriculum> curriculums = new();

        private int EnvParamCount
        {
            get
            {
                int length = 0;
                length += simpleValues != null ? simpleValues.Count : 0;
                length += uniformSamplers != null ? uniformSamplers.Count : 0;
                length += multiUniformSamplers != null ? multiUniformSamplers.Count : 0;
                length += gaussianSamplers != null ? gaussianSamplers.Count : 0;
                length += curriculums != null ? curriculums.Count : 0;

                return length;
            }
        }

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
                        case ConfigText.behaviorsText:
                            LoadBehaviors(yamlObject);
                            break;
                        case ConfigText.environmentParametersText:
                            LoadEnvParameters(yamlObject);
                            break;
                    }
                }
            }


        }

        protected void LoadBehaviors(YamlObject yaml)
        {
            if (yaml.name != ConfigText.behaviorsText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.behaviorsText} is not right.");

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
            foreach (var element in yaml.elements)
            {
                if (element is YamlObject yamlObject)
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
                            isUseBehavioralCloning = true;
                            behavioralCloningYamlObject = yamlObject;
                            break;
                        case ConfigText.selfPlayText:
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

        protected void LoadEnvParameters(YamlObject yaml)
        {
            if (yaml.name != ConfigText.environmentParametersText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.environmentParametersText} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    simpleValues.Add(new SimpleValue()
                    {
                        name = yamlValue.name,
                        value = yamlValue.value
                    });

                    continue;
                }

                if (element is YamlObject yamlObject)
                {
                    if (yamlObject.elements.Find(x => x.name == ConfigText.samplerTypeText) != null)
                    {
                        var sampler = SampleFactory.GetSampler(yamlObject);
                        if (sampler != null)
                        {
                            if (sampler is UniformSampler uniformSampler)
                                uniformSamplers.Add(uniformSampler);
                            else if (sampler is MultiUniformSampler multiUniformSampler)
                                multiUniformSamplers.Add(multiUniformSampler);
                            else if (sampler is GaussianSampler gaussianSampler)
                                gaussianSamplers.Add(gaussianSampler);
                        }
                    }
                    else if (yamlObject.elements.Find(x => x.name == ConfigText.curriculumText) != null)
                        curriculums.Add(new Curriculum(yamlObject));
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();

            var behaviorsYaml = ConvertBehaviorsToYaml();
            behaviorsYaml.parent = yaml;
            yaml.elements.Add(behaviorsYaml);

            if (EnvParamCount > 0)
            {
                var envParametersYaml = ConvertEnvParametersToYaml();
                envParametersYaml.parent = yaml;
                yaml.elements.Add(envParametersYaml);
            }

            return yaml;
        }

        protected YamlObject ConvertBehaviorsToYaml()
        {
            var behaviors = new YamlObject();
            behaviors.name = ConfigText.behaviorsText;

            var mlName = new YamlObject()
            {
                name = behaviorName,
                parent = behaviors
            };
            behaviors.elements.Add(mlName);

            mlName.elements.Add(new YamlValue(ConfigText.trainerTypeText, trainerType));
            mlName.elements.Add(new YamlValue(ConfigText.summaryFreqText, summaryFreq));
            mlName.elements.Add(new YamlValue(ConfigText.timeHorizonText, timeHorizon));
            mlName.elements.Add(new YamlValue(ConfigText.maxStepsText, maxSteps));
            mlName.elements.Add(new YamlValue(ConfigText.keepCheckpointsText, keepCheckpoints));
            mlName.elements.Add(new YamlValue(ConfigText.checkpointIntervalText, checkpointInterval));
            mlName.elements.Add(new YamlValue(ConfigText.threadedText, threaded));

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

        protected YamlObject ConvertEnvParametersToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.environmentParametersText;

            foreach (var item in simpleValues)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in uniformSamplers)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in multiUniformSamplers)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in gaussianSamplers)
                AddEnvParamToYaml(yaml, item);

            foreach (var item in curriculums)
                AddEnvParamToYaml(yaml, item);

            return yaml;
        }

        protected void AddEnvParamToYaml(YamlObject yaml, EnvParam item)
        {
            var yamlElement = item.ToYaml();

            if (yamlElement is YamlObject yamlObject)
                yamlObject.parent = yaml;

            yaml.elements.Add(yamlElement);
        }
    }
}