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

        public static void CreateFiles(string path, YamlElement yaml, HashSet<string> filter = null)
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
                        case ConfigText.environmentSettings:
                            if(IsImportable(filter, ConfigText.environmentSettings))
                                CreateAsset<EnvironmentSettings>(path, "EnvironmentSettings", yamlObject);
                            break;
                        case ConfigText.engineSettings:
                            if(IsImportable(filter, ConfigText.engineSettings))
                                CreateAsset<EngineSettings>(path, "EngineSettings", yamlObject);
                            break;
                        case ConfigText.checkpointSettings:
                            if (IsImportable(filter, ConfigText.checkpointSettings))
                                CreateAsset<CheckpointSettings>(path, "CheckpointSettings", yamlObject);
                            break;
                        case ConfigText.torchSettings:
                            if (IsImportable(filter, ConfigText.torchSettings))
                                CreateAsset<TorchSettings>(path, "TorchSettings", yamlObject);
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
                            if (IsImportable(filter, ConfigText.environmentParameters))
                                CreateAsset<EnvironmentParameters>(path, "EnvironmentParameters", yamlObject);
                            break;
                    }
                }
            }

            if (IsImportable(filter, ConfigText.behaviors) && behaviorYamls != null)
            {
                foreach (var behaviorYaml in behaviorYamls)
                {
                    if (behaviorYaml is YamlObject behaviorYamlObject)
                    {
                        CreateAsset<Behavior>(
                            path,
                            behaviorYamlObject.name,
                            YamlObject.Merge(defaultBehavior, behaviorYamlObject)
                        );
                    }
                }
            }
        }

        private static bool IsImportable(HashSet<string> filter, string text)
        {
            return filter == null || filter.Count == 0 || filter.Contains(text);
        }

        private static void CreateAsset<T>(string path, string fileName, YamlObject yamlObject) where T : ConfigFile
        {
            var configFile = ScriptableObject.CreateInstance<T>();

            if(yamlObject != null)
                configFile.LoadData(yamlObject);

            string filePath = Path.Combine(path, fileName + fileExtension);
            AssetDatabase.CreateAsset(configFile, filePath);
            AssetDatabase.SaveAssets();

            Debug.Log("Asset is created: " + filePath);
        }
    }
}
#endif