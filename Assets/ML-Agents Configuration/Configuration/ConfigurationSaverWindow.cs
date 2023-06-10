using System;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationSaverWindow : EditorWindow
    {
        const string fileExtension = ".yaml";

        string fileName;
        Behavior behavior;
        EnvironmentParameters environmentParameters;

        [MenuItem("Window/ML-Agents/Config Saver")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationSaverWindow>("ML-Agents Config Saver");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Clear"))
                Clear();

            GUILayout.Space(20);

            fileName = EditorGUILayout.TextField("File's name:", fileName);
            GUILayout.Space(5);

            behavior = (Behavior)EditorGUILayout.ObjectField("Behavior:", behavior, typeof(Behavior), false);
            environmentParameters = 
                (EnvironmentParameters)EditorGUILayout.ObjectField("EnvironmentParameters:", environmentParameters, typeof(EnvironmentParameters), false);
            GUILayout.Space(5);

            if (GUILayout.Button("Create Yaml file"))
                CreateYaml();
        }

        private void Clear()
        {
            fileName = "";
            behavior = null;
            environmentParameters = null;
        }

        private void CreateYaml()
        {
            if (string.IsNullOrEmpty(fileName))
                throw new System.Exception("It has to have a file name.");

            if (behavior == null)
                throw new System.Exception("The behavior can't be null.");

            if (!Directory.Exists(ConfigurationSettings.Instance.YamlFolderPath))
                Directory.CreateDirectory(ConfigurationSettings.Instance.YamlFolderPath);

            string fullFilePath = Path.Combine(ConfigurationSettings.Instance.YamlFolderPath, fileName + fileExtension);

            if(File.Exists(fullFilePath))
                throw new System.Exception("This file name is used.");

            var yaml = GenerateYamlFromFiles();

            YamlFile.SaveObjectToFile(yaml, fullFilePath);
            Debug.Log("File is saved: " + fullFilePath);
        }

        private YamlElement GenerateYamlFromFiles()
        {
            var yaml = new YamlObject();

            var behaviorsYaml = behavior.ToYaml();
            behaviorsYaml.parent = yaml;
            yaml.elements.Add(behaviorsYaml);

            if (environmentParameters != null && environmentParameters.ParamCount > 0)
            {
                var envParametersYaml = environmentParameters.ToYaml();
                envParametersYaml.parent = yaml;
                yaml.elements.Add(envParametersYaml);
            }

            return yaml;
        }
    }
}