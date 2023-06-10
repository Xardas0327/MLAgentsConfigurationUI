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

            foreach (var element in yamlFile.elements)
            {
                if (element is YamlObject yamlObject)
                {
                    switch (yamlObject.name)
                    {
                        case ConfigText.behaviors:
                            if(yamlFile.elements.Count < 1)
                                throw new System.Exception("The yaml file has to have a behavior.");

                            var behavior = ScriptableObject.CreateInstance<Behavior>();
                            behavior.LoadData(yamlObject);

                            var behaviorFileName = yamlObject.elements[0].name + fileExtension;
                            string behaviorFilePath = Path.Combine(path, behaviorFileName);
                            CreateAsset(behavior, behaviorFilePath);
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
        }

        private static void CreateAsset(ScriptableObject asset, string path)
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            Debug.Log("Asset is created: " + path);
        }
    }
}