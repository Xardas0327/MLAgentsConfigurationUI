#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Yaml;
using EnvironmentParameters = Xardas.MLAgents.Configuration.Fileformat.EnvironmentParameters;

namespace Xardas.MLAgents.Configuration
{
    public static class ConfigFileCreater
    {
        const string fileExtension = ".asset";

        public static void CreateBasicBehavior(string path)
        {
            var file = ScriptableObject.CreateInstance<Behavior>();
            var fileName = "Behavior" + fileExtension;
            string filePath = Path.Combine(path, fileName);

            CreateAsset(file, filePath);
        }

        public static void CreateFiles(string path, YamlElement yaml)
        {
            var yamlFile = yaml as YamlObject;
            if (yamlFile == null
                || yamlFile.elements.Count < 1 || !(yamlFile.elements[0] is YamlObject))
                throw new System.Exception("The yaml file is not a valid MLAgents config file.");

            List<YamlElement> behaviorYamls = null;
            YamlObject defaultBehavior = null;
            foreach (var element in yamlFile.elements)
            {
                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.torchSettings:
                            var torchSettings = ScriptableObject.CreateInstance<TorchSettings>();
                            torchSettings.LoadData(yamlObject);

                            var torchSettingsFileName = "TorchSettings" + fileExtension;
                            string torchSettingsFilePath = Path.Combine(path, torchSettingsFileName);
                            CreateAsset(torchSettings, torchSettingsFilePath);
                            break;
                        case ConfigText.defaultSettings:
                            defaultBehavior = yamlObject;
                            break;
                        case ConfigText.behaviors:
                            if(yamlObject.elements.Count < 1)
                                throw new System.Exception("The yaml file has to have a behavior.");

                            behaviorYamls = yamlObject.elements;
                            break;
                        case ConfigText.environmentParameters:
                            var envParams = ScriptableObject.CreateInstance<EnvironmentParameters>();
                            envParams.LoadData(yamlObject);

                            var envParamsFileName = "EnvironmentParameters" + fileExtension;
                            string envParamsFilePath = Path.Combine(path, envParamsFileName);
                            CreateAsset(envParams, envParamsFilePath);
                            break;
                    }
                }
            }

            if(behaviorYamls == null)
                throw new System.Exception("The yaml file has to have a behavior.");

            foreach (var behaviorYaml in behaviorYamls)
            {
                if (behaviorYaml is YamlObject behaviorYamlObject)
                {
                    var behavior = ScriptableObject.CreateInstance<Behavior>();
                    behavior.LoadData(YamlObject.Merge(defaultBehavior, behaviorYamlObject));

                    var behaviorFileName = behaviorYamlObject.name + fileExtension;
                    string behaviorFilePath = Path.Combine(path, behaviorFileName);
                    CreateAsset(behavior, behaviorFilePath);
                }
            }
        }

        private static void CreateAsset(ScriptableObject asset, string path)
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            Debug.Log("Asset is created: " + path);
        }
    }
}
#endif