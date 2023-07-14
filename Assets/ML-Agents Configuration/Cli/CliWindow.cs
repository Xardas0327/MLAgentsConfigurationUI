#if UNITY_EDITOR_WIN
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Xardas.MLAgents.Cli
{
    public class CliWindow : EditorWindow
    {
        private string yamlFilePath;
        private string resultDirectory = "MLAgents Results";

        [MenuItem("Window/ML-Agents/CLI")]
        public static void ShowWindow()
        {
            GetWindow<CliWindow>("ML-Agents CLI");
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            var pythoEnvtext = !string.IsNullOrEmpty(ConfigurationSettings.Instance.PythonVirtualEnvironment)
                ? "A Python Virtual Environment has set in Project Settings."
                : "A Python Virtual Environment has not set in Project Settings.";
            GUILayout.Label(pythoEnvtext);
            GUILayout.Space(25);

            CreateYamlFileDialog();
            resultDirectory = EditorGUILayout.TextField("Result Directory", resultDirectory);

            if (GUILayout.Button("Run"))
                Run();
        }

        private void Run()
        {
            if(string.IsNullOrEmpty(yamlFilePath))
                throw new System.Exception("There is no selected Yaml file.");

            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = GetCmdArguments();

            Process.Start(startInfo);
        }

        private string GetCmdArguments()
        {
            var arguments = new StringBuilder();
            arguments.Append("/K \"");
            if (!string.IsNullOrEmpty(ConfigurationSettings.Instance.PythonVirtualEnvironment))
                arguments.Append($"\"{ConfigurationSettings.Instance.PythonVirtualEnvironment}\" &&");

            arguments.Append($"mlagents-learn \"{yamlFilePath}\" ");

            if(!string.IsNullOrEmpty(resultDirectory))
                arguments.Append($"--results-dir \"{resultDirectory}\"");

            arguments.Append("\"");

            return arguments.ToString();
        }

        private void CreateYamlFileDialog()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Config File", yamlFilePath);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFilePanel("Select a yaml file", Application.dataPath, "yaml");

                    if (newPath != yamlFilePath && !string.IsNullOrEmpty(newPath))
                        yamlFilePath = newPath;
                };
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif