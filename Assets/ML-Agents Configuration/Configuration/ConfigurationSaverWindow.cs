using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationSaverWindow : EditorWindow
    {
        const string fileExtension = ".yaml";

        string fileName;
        List<Behavior> behaviors = new();
        EnvironmentParameters environmentParameters;

        //Draw Behavior
        Vector2 behaviorScrollPos;
        bool showBehaviors;

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

            fileName = EditorGUILayout.TextField("File's name", fileName);
            GUILayout.Space(5);
            DrawBehaviors();
            GUILayout.Space(5);
            environmentParameters = 
                (EnvironmentParameters)EditorGUILayout.ObjectField("EnvironmentParameters", environmentParameters, typeof(EnvironmentParameters), false);
            GUILayout.Space(5);

            if (GUILayout.Button("Create Yaml file"))
                CreateYaml();

        }

        private void DrawBehaviors()
        {
            EditorGUILayout.BeginHorizontal();
            showBehaviors = EditorGUILayout.Foldout(showBehaviors, "Behaviors");
            int newCount = Mathf.Max(0, EditorGUILayout.IntField(behaviors.Count, GUILayout.MaxWidth(60f)));
            EditorGUILayout.EndHorizontal();

            while (newCount < behaviors.Count)
                behaviors.RemoveAt(behaviors.Count - 1);
            while (newCount > behaviors.Count)
                behaviors.Add(null);

            if(showBehaviors)
            {
                behaviorScrollPos = EditorGUILayout.BeginScrollView(behaviorScrollPos, GUILayout.MaxHeight(300));
                for (int i = 0; i < behaviors.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(15f));
                    EditorGUILayout.LabelField(i < 10 ? $" {i + 1}." : $"{i + 1}.", GUILayout.MaxWidth(30f));
                    behaviors[i] = (Behavior)EditorGUILayout.ObjectField(behaviors[i], typeof(Behavior), false);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void Clear()
        {
            fileName = "";
            behaviors = new List<Behavior>();
            environmentParameters = null;

            showBehaviors = false;
        }

        private void CreateYaml()
        {
            if (string.IsNullOrEmpty(fileName))
                throw new System.Exception("It has to have a file name.");

            if (behaviors == null || behaviors.Count < 1)
                throw new System.Exception("The behaviors can't be empty.");

            if (!Directory.Exists(ConfigurationSettings.Instance.YamlFolderPath))
                Directory.CreateDirectory(ConfigurationSettings.Instance.YamlFolderPath);

            string fullFilePath = Path.Combine(ConfigurationSettings.Instance.YamlFolderPath, fileName + fileExtension);

            if(File.Exists(fullFilePath))
            {
                if (EditorUtility.DisplayDialog("Warning!",
                $"The {fileName} file already exists. Do you want to overwrite it?", "Yes", "No"))
                {
                    CreateFile(fullFilePath);
                }
            }
            else
                CreateFile(fullFilePath);
        }

        private YamlElement GenerateYamlFromFiles()
        {
            var yaml = new YamlObject();

            var behaviorsYaml = new YamlObject()
            {
                name = ConfigText.behaviors,
                parent = yaml
            };
            yaml.elements.Add(behaviorsYaml);

            var isFound = false;
            var behaviorsName = new List<string>();
            foreach (var behavior in behaviors)
            {
                if(behavior != null)
                {
                    isFound = true;
                    var behaviorYaml = behavior.ToYaml();
                    if(behaviorsName.Contains(behaviorYaml.name))
                        throw new System.Exception("The name of every behaviors have to be unique.");

                    behaviorsName.Add(behaviorYaml.name);
                    behaviorYaml.parent = behaviorsYaml;
                    behaviorsYaml.elements.Add(behaviorYaml);
                }
            }

            if(!isFound)
                throw new System.Exception("Every behaviors are null.");

            if (environmentParameters != null && environmentParameters.ParamCount > 0)
            {
                var envParametersYaml = environmentParameters.ToYaml();
                envParametersYaml.parent = yaml;
                yaml.elements.Add(envParametersYaml);
            }

            return yaml;
        }

        private void CreateFile(string path)
        {
            var yaml = GenerateYamlFromFiles();

            YamlFile.SaveObjectToFile(yaml, path);
            Debug.Log("File is saved: " + path);
        }
    }
}