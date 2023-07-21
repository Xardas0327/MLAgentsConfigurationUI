#if UNITY_EDITOR_WIN
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Configuration.SettingParameter;

namespace Xardas.MLAgents.Cli
{
    public class CliWindow : EditorWindow
    {
        private const float depthSize = 15f;

        private string yamlFilePath;
        private static Dictionary<string, bool> showPropertiesOfObject = new();
        private static CliSettings cliSettings = new();

        [MenuItem("Window/ML-Agents/CLI")]
        public static void ShowWindow()
        {
            GetWindow<CliWindow>("ML-Agents CLI");
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            if (GUILayout.Button("Clear"))
                Clear();

            var pythoEnvtext = !string.IsNullOrEmpty(ConfigurationSettings.Instance.PythonVirtualEnvironment)
                ? "A Python Virtual Environment has set in Project Settings."
                : "A Python Virtual Environment has not set in Project Settings.";
            GUILayout.Label(pythoEnvtext);
            GUILayout.Space(25);

            DrawYamlFileDialog();
            GUILayout.Space(5);

            DrawCliSettingsField();
            GUILayout.Space(5);

            if (GUILayout.Button("Run"))
                Run();
        }

        private void Clear()
        {
            yamlFilePath = "";
            showPropertiesOfObject = new();
            cliSettings = new();
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

            arguments.Append(GetMLagentsLearnArguments());

            arguments.Append("\"");

            return arguments.ToString();
        }

        private string GetMLagentsLearnArguments()
        {
            var arguments = new StringBuilder();

            if(cliSettings.isUseDeterministic && cliSettings.deterministic)
                arguments.Append($"--deterministic ");

            if (cliSettings.isUseNumAreas)
                arguments.Append($"--num-areas {cliSettings.numAreas} ");

            if (cliSettings.isUseDebug && cliSettings.debug)
                arguments.Append($"--debug ");

            if (cliSettings.isUseResultsDir && !string.IsNullOrEmpty(cliSettings.resultsDir))
                arguments.Append($"--results-dir \"{cliSettings.resultsDir}\"");

            return arguments.ToString();
        }

        private void DrawYamlFileDialog()
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

        private void DrawCliSettingsField()
        {
            if (DrawFoldout("CLI Settings", typeof(CliSettings).Name))
            {
                DrawFieldWithTickBox(
                    ref cliSettings.isUseDeterministic,
                    new GUIContent("Deterministic", ConfigTooltip.deterministic),
                    ref cliSettings.deterministic,
                    1);

                DrawFieldWithTickBox(
                    ref cliSettings.isUseNumAreas,
                    new GUIContent("Num Areas", ConfigTooltip.numAreas),
                    ref cliSettings.numAreas,
                    1,
                    NumAreasCondition);

                DrawFieldWithTickBox(
                    ref cliSettings.isUseDebug,
                    new GUIContent("Debug", ConfigTooltip.debug),
                    ref cliSettings.debug,
                    1);

                DrawFieldWithTickBox(
                    ref cliSettings.isUseResultsDir,
                    new GUIContent("Result Directory", ConfigTooltip.resultsDir),
                    ref cliSettings.resultsDir,
                    1);
            }


            GUILayout.Space(5);
        }

        private bool DrawFoldout(string label, string index)
        {
            if (!showPropertiesOfObject.ContainsKey(index))
            {
                showPropertiesOfObject.Add(index, false);
            }

            showPropertiesOfObject[index] =
                    EditorGUILayout.Foldout(showPropertiesOfObject[index], label);

            return showPropertiesOfObject[index];
        }

        private void DrawFieldWithTickBox(ref bool active, GUIContent label, ref bool value, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if(deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            value = EditorGUILayout.Toggle(label, value);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFieldWithTickBox(ref bool active, GUIContent label, ref uint value, uint deep = 0, Func<int, uint> condition = null)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            var temp = EditorGUILayout.IntField(label, (int)value);
            if (condition == null)
            {
                if (temp < 0)
                    temp = 0;
                value = (uint)temp;
            }
            else
                value = condition(temp);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFieldWithTickBox(ref bool active, GUIContent label, ref string value, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            value = EditorGUILayout.TextField(label, value);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private uint NumAreasCondition(int v)
        {
            if (v < 1)
                v = 1;

            return (uint)v;
        }
    }
}
#endif