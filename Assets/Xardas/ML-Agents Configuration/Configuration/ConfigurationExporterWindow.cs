#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationExporterWindow : EditorWindow
    {
        const string fileExtension = "yaml";

        EnvironmentSettings environmentSettings;
        EngineSettings engineSettings;
        CheckpointSettings checkpointSettings;
        TorchSettings torchSettings;
        List<Behavior> behaviors = new();
        EnvironmentParameters environmentParameters;

        //Draw Behavior
        Vector2 behaviorScrollPos;
        bool showBehaviors;

        [MenuItem("Window/ML-Agents Configuration/Config Exporter")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationExporterWindow>("ML-Agents Config Exporter");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Clear"))
                Clear();

            GUILayout.Space(20);

            EditorGUILayout.LabelField("Settings");
            environmentSettings =
                (EnvironmentSettings)EditorGUILayout.ObjectField("Environment Settings", environmentSettings, typeof(EnvironmentSettings), false);
            engineSettings =
                (EngineSettings)EditorGUILayout.ObjectField("Engine Settings", engineSettings, typeof(EngineSettings), false);
            checkpointSettings =
                (CheckpointSettings)EditorGUILayout.ObjectField("Checkpoint Settings", checkpointSettings, typeof(CheckpointSettings), false);
            torchSettings =
                (TorchSettings)EditorGUILayout.ObjectField("Torch Settings", torchSettings, typeof(TorchSettings), false);

            GUILayout.Space(5);
            DrawBehaviors();
            GUILayout.Space(5);
            environmentParameters = 
                (EnvironmentParameters)EditorGUILayout.ObjectField("Environment Parameters", environmentParameters, typeof(EnvironmentParameters), false);
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
            environmentSettings = null;
            engineSettings = null;
            checkpointSettings = null;
            torchSettings = null;
            behaviors = new List<Behavior>();
            environmentParameters = null;

            showBehaviors = false;
        }

        private void CreateYaml()
        {
            if (behaviors == null || behaviors.Count < 1)
                throw new System.Exception("The behaviors can't be empty.");

            var path =EditorUtility.SaveFilePanel("Save YAML file", Application.dataPath, "", fileExtension);
            if(string.IsNullOrEmpty(path))
                return;

            CreateFile(path);
        }

        private YamlElement GenerateYamlFromFiles()
        {
            var yaml = new YamlObject();

            if (environmentSettings != null && environmentSettings.isUse)
            {
                var environmentSettingsYaml = environmentSettings.ToYaml();
                environmentSettingsYaml.parent = yaml;
                yaml.elements.Add(environmentSettingsYaml);
            }

            if (engineSettings != null && engineSettings.isUse)
            {
                var engineSettingsYaml = engineSettings.ToYaml();
                engineSettingsYaml.parent = yaml;
                yaml.elements.Add(engineSettingsYaml);
            }

            if (checkpointSettings != null && checkpointSettings.isUse)
            {
                var checkpointSettingsYaml = checkpointSettings.ToYaml();
                checkpointSettingsYaml.parent = yaml;
                yaml.elements.Add(checkpointSettingsYaml);
            }

            if (torchSettings != null && torchSettings.isUse)
            {
                var torchSettingsYaml = torchSettings.ToYaml();
                torchSettingsYaml.parent = yaml;
                yaml.elements.Add(torchSettingsYaml);
            }

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
#endif