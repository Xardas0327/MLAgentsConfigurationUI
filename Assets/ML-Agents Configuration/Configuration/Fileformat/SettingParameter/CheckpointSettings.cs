using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.SettingParameter
{
    [Serializable]
    public class CheckpointSettings : ISettings
    {
        public bool isUseRunId;
        [Tooltip(ConfigTooltip.runId)]
        public string runId = "ppo";

        public bool isUseInitializeFrom;
        [Tooltip(ConfigTooltip.initializeFrom)]
        public string initializeFrom = "";

        public bool isUseLoadModel;
        public bool loadModel = false;

        public bool isUseResume;
        [Tooltip(ConfigTooltip.resume)]
        public bool resume = false;

        public bool isUseForce;
        [Tooltip(ConfigTooltip.force)]
        public bool force = false;

        public bool isUseTrainModel;
        public bool trainModel = false;

        public bool isUseInference;
        [Tooltip(ConfigTooltip.inference)]
        public bool inference = false;

        public bool IsUse => 
                isUseRunId
            || isUseInitializeFrom || isUseLoadModel
            || isUseResume || isUseForce
            || isUseTrainModel || isUseInference;

        public CheckpointSettings() { }

        public CheckpointSettings(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.checkpointSettings || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.checkpointSettings} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.runId:
                            runId = yamlValue.value; // we have to have the original string.
                            isUseRunId = true;
                            break;
                        case ConfigText.initializeFrom:
                            initializeFrom = yamlValue.value; // we have to have the original string.
                            isUseInitializeFrom = true;
                            break;
                        case ConfigText.loadModel:
                            if (value == "true")
                                loadModel = true;

                            isUseLoadModel = true;
                            break;
                        case ConfigText.resume:
                            if (value == "true")
                                resume = true;

                            isUseResume = true;
                            break;
                        case ConfigText.force:
                            if (value == "true")
                                force = true;

                            isUseForce = true;
                            break;
                        case ConfigText.trainModel:
                            if (value == "true")
                                trainModel = true;

                            isUseTrainModel = true;
                            break;
                        case ConfigText.inference:
                            if (value == "true")
                                inference = true;

                            isUseInference = true;
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.checkpointSettings;

            if (isUseRunId)
                yaml.elements.Add(new YamlValue(ConfigText.runId, runId));

            if (isUseInitializeFrom)
                yaml.elements.Add(new YamlValue(ConfigText.initializeFrom, initializeFrom));

            if (isUseLoadModel)
                yaml.elements.Add(new YamlValue(ConfigText.loadModel, loadModel));

            if (isUseResume)
                yaml.elements.Add(new YamlValue(ConfigText.resume, resume));

            if (isUseForce)
                yaml.elements.Add(new YamlValue(ConfigText.force, force));

            if (isUseTrainModel)
                yaml.elements.Add(new YamlValue(ConfigText.trainModel, trainModel));

            if (isUseInference)
                yaml.elements.Add(new YamlValue(ConfigText.inference, inference));

            return yaml;
        }
    }
}