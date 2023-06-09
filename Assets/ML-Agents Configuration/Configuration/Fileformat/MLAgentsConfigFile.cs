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

        public Behavior behavior = new();
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
                            behavior = new Behavior(yamlObject);
                            break;
                        case ConfigText.environmentParameters:
                            environmentParameters = new EnvironmentParameters(yamlObject);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();

            var behaviorsYaml = behavior.ToYaml();
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
    }
}