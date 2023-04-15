using System;
using System.Collections.Generic;
using System.Linq;
using Xardas.MLAgents.Configuration.Fileformat.EnvParameters;
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
        public List<EnvParam> parameters = new();

        public MLAgentsConfigFile() { }

        public MLAgentsConfigFile(YamlElement yaml)
        {
            YamlObject yamlFile = yaml as YamlObject;
            if (yamlFile == null
                || yamlFile.elements.Count < 1 || !(yamlFile.elements[0] is YamlObject))
                throw new System.Exception("The yaml file is not a MLAgents config file.");

            foreach (var element in yamlFile.elements)
            {
                var yamlObject = element as YamlObject;
                if (yamlObject != null)
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
            name = yaml.name;
            //Firstly the YamlValue only
            foreach (var element in yaml.elements)
            {
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
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

        protected void LoadEnvParameters(YamlObject yaml)
        {
            if (yaml.name != ConfigText.environmentParametersText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.environmentParametersText} is not right.");

            foreach (var element in yaml.elements)
            {
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
                {
                    parameters.Add(new SimpleValue()
                    {
                        name = yamlValue.name,
                        value = yamlValue.value
                    });

                    continue;
                }

                var yamlObject = element as YamlObject;
                if (yamlObject != null)
                {
                    if (yamlObject.elements.Find(x => x.name == ConfigText.samplerTypeText) != null)
                    {
                        var sampler = SampleFactory.GetSampler(yamlObject);
                        if (sampler != null)
                            parameters.Add(sampler);
                    }

                    if (yamlObject.elements.Find(x => x.name == ConfigText.curriculumText) != null)
                    {
                        parameters.Add(new Curriculum(yamlObject));
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();

            var behaviorsYaml = ConvertBehaviorsToYaml();
            behaviorsYaml.parent = yaml;
            yaml.elements.Add(behaviorsYaml);

            if(parameters.Count > 0)
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

            var mlName = new YamlObject();
            mlName.name = name;
            mlName.parent = behaviors;
            behaviors.elements.Add(mlName);

            var tt = new YamlValue();
            tt.name = ConfigText.trainerTypeText;
            tt.value = trainerType.ToString();
            mlName.elements.Add(tt);

            var sf = new YamlValue();
            sf.name = ConfigText.summaryFreqText;
            sf.value = summaryFreq.ToString();
            mlName.elements.Add(sf);

            var th = new YamlValue();
            th.name = ConfigText.timeHorizonText;
            th.value = timeHorizon.ToString();
            mlName.elements.Add(th);

            var ms = new YamlValue();
            ms.name = ConfigText.maxStepsText;
            ms.value = maxSteps.ToString();
            mlName.elements.Add(ms);

            var kch = new YamlValue();
            kch.name = ConfigText.keepCheckpointsText;
            kch.value = keepCheckpoints.ToString();
            mlName.elements.Add(kch);

            var chi = new YamlValue();
            chi.name = ConfigText.checkpointIntervalText;
            chi.value = checkpointInterval.ToString();
            mlName.elements.Add(chi);

            var t = new YamlValue();
            t.name = ConfigText.threadedText;
            t.value = threaded ? "true" : "false";
            mlName.elements.Add(t);

            var hp = hyperparameters.ToYaml(trainerType);
            hp.parent = mlName;
            mlName.elements.Add(hp);

            var ns = networkSettings.ToYaml();
            ns.parent = mlName;
            mlName.elements.Add(ns);

            var rs = rewardSignals.ToYaml();
            rs.parent = mlName;
            mlName.elements.Add(rs);

            if(behavioralCloning != null)
            {
                var bc = behavioralCloning.ToYaml();
                bc.parent = mlName;
                mlName.elements.Add(bc);
            }

            if (selfPlay != null)
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


            return yaml;
        }
    }
}