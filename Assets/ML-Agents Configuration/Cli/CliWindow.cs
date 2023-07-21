#if UNITY_EDITOR_WIN
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Configuration.Fileformat.SettingParameter;
using EnvironmentSettings = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.EnvironmentSettings;
using EngineSettings = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.EngineSettings;
using CheckpointSettings = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.CheckpointSettings;
using TorchSettings = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.TorchSettings;
using DeviceType = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.DeviceType;

namespace Xardas.MLAgents.Cli
{
    public class CliWindow : EditorWindow
    {
        private const float depthSize = 15;

        private string yamlFilePath = "";
        private CliSettings cliSettings = new();
        private EnvironmentSettings environmentSettings = new();
        private EngineSettings engineSettings = new();
        private CheckpointSettings checkpointSettings = new();
        private TorchSettings torchSettings = new();

        //Foldout
        bool showCliSettings;
        bool showEnvironmentSettings;
        bool showEngineSettings;
        bool showCheckpointSettings;
        bool showTorchSettings;

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
            GUILayout.Label("The CLI will start in the current unity project main folder.");
            GUILayout.Space(25);

            DrawYamlFileDialog();
            GUILayout.Space(5);

            DrawCliSettingsField();
            DrawEnvironmentSettingsField();
            DrawEngineSettingsField();
            DrawCheckpointSettingsField();
            DrawTorchSettingsField();
            GUILayout.Space(5);

            if (GUILayout.Button("Run"))
                Run();
        }

        private void Clear()
        {
            yamlFilePath = "";
            cliSettings = new();
            environmentSettings = new();
            engineSettings = new();
            checkpointSettings = new();
            torchSettings = new();

            showCliSettings = false;
            showEnvironmentSettings = false;
            showEngineSettings = false;
            showCheckpointSettings = false;
            showTorchSettings = false;
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
                arguments.Append($"\"{ConfigurationSettings.Instance.PythonVirtualEnvironment}\" && ");

            arguments.Append($"mlagents-learn \"{yamlFilePath}\" ");

            arguments.Append(GetMLagentsLearnArguments());

            arguments.Append("\"");

            return arguments.ToString();
        }

        private string GetMLagentsLearnArguments()
        {
            var arguments = new StringBuilder();

            arguments.Append(cliSettings.GetCliArguments());
            arguments.Append(environmentSettings.GetCliArguments());
            arguments.Append(engineSettings.GetCliArguments());
            arguments.Append(checkpointSettings.GetCliArguments());
            arguments.Append(torchSettings.GetCliArguments());

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
            showCliSettings = EditorGUILayout.Foldout(showCliSettings, new GUIContent("CLI Settings"));
            if (showCliSettings)
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
                    Min1Condition);

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

        private void DrawEnvironmentSettingsField()
        {
            showEnvironmentSettings = EditorGUILayout.Foldout(showEnvironmentSettings, new GUIContent("Environment Settings"));
            if (showEnvironmentSettings)
            {
                DrawFieldWithTickBox(
                    ref environmentSettings.isUseEnvPath,
                    new GUIContent("Env Path", ConfigTooltip.envPath),
                    ref environmentSettings.envPath,
                    1);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseEnvArgs,
                    new GUIContent("Env Args", ConfigTooltip.envArgs),
                    ref environmentSettings.envArgs,
                    1);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseBasePort,
                    new GUIContent("Base Port", ConfigTooltip.basePort),
                    ref environmentSettings.basePort,
                    1);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseNumEnvs,
                    new GUIContent("Num Envs", ConfigTooltip.numEnvs),
                    ref environmentSettings.numEnvs,
                    1);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseSeed,
                    new GUIContent("Seed", ConfigTooltip.seed),
                    ref environmentSettings.seed,
                    1);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseMaxLifetimeRestarts,
                    new GUIContent("Max Lifetime Restarts", ConfigTooltip.maxLifetimeRestarts),
                    ref environmentSettings.maxLifetimeRestarts,
                    1,
                    MinMinus1Condition);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseRestartsRateLimitN,
                    new GUIContent("Restarts Rate Limit N", ConfigTooltip.restartsRateLimitN),
                    ref environmentSettings.restartsRateLimitN,
                    1,
                    MinMinus1Condition);

                DrawFieldWithTickBox(
                    ref environmentSettings.isUseRestartsRateLimitPeriodS,
                    new GUIContent("Restarts Rate Limit Period", ConfigTooltip.restartsRateLimitPeriodS),
                    ref environmentSettings.restartsRateLimitPeriodS,
                    1);
            }
            GUILayout.Space(5);
        }

        private void DrawEngineSettingsField()
        {
            showEngineSettings = EditorGUILayout.Foldout(showEngineSettings, new GUIContent("Engine Settings"));
            if (showEngineSettings)
            {
                DrawFieldWithTickBox(
                    ref engineSettings.isUseWidth,
                    new GUIContent("Width", ConfigTooltip.width),
                    ref engineSettings.width,
                    1,
                    Min1Condition);

                DrawFieldWithTickBox(
                    ref engineSettings.isUseHeight,
                    new GUIContent("Height", ConfigTooltip.height),
                    ref engineSettings.height,
                    1,
                    Min1Condition);

                DrawFieldWithTickBox(
                    ref engineSettings.isUseQualityLevel,
                    new GUIContent("Quality Level", ConfigTooltip.qualityLevel),
                    ref engineSettings.qualityLevel,
                    1,
                    Min1Condition);

                DrawFieldWithTickBox(
                    ref engineSettings.isUseTimeScale,
                    new GUIContent("Time Scale", ConfigTooltip.timeScale),
                    ref engineSettings.timeScale,
                    1);

                DrawFieldWithTickBox(
                    ref engineSettings.isUseTargetFrameRate,
                    new GUIContent("Target Frame Rate", ConfigTooltip.targetFrameRate),
                    ref engineSettings.targetFrameRate,
                    1,
                    MinMinus1Condition);

                DrawFieldWithTickBox(
                    ref engineSettings.isUseCaptureFrameRate,
                    new GUIContent("Capture Frame Rate", ConfigTooltip.captureFrameRate),
                    ref engineSettings.captureFrameRate,
                    1,
                    Min1Condition);

                DrawFieldWithTickBox(
                    ref engineSettings.isUseNoGraphics,
                    new GUIContent("No Graphics", ConfigTooltip.noGraphics),
                    ref engineSettings.noGraphics,
                    1);
            }
            GUILayout.Space(5);
        }

        private void DrawCheckpointSettingsField()
        {
            showCheckpointSettings = EditorGUILayout.Foldout(
                showCheckpointSettings, 
                new GUIContent("Checkpoint Settings", "Load Model and the Train Model don't have CLI version.")
            );

            if (showCheckpointSettings)
            {
                DrawFieldWithTickBox(
                    ref checkpointSettings.isUseRunId,
                    new GUIContent("Run id", ConfigTooltip.runId),
                    ref checkpointSettings.runId,
                    1);

                DrawFieldWithTickBox(
                    ref checkpointSettings.isUseInitializeFrom,
                    new GUIContent("Initialize From", ConfigTooltip.initializeFrom),
                    ref checkpointSettings.initializeFrom,
                    1);

                DrawFieldWithTickBox(
                    ref checkpointSettings.isUseResume,
                    new GUIContent("Resume", ConfigTooltip.resume),
                    ref checkpointSettings.resume,
                    1);

                DrawFieldWithTickBox(
                    ref checkpointSettings.isUseForce,
                    new GUIContent("Force", ConfigTooltip.force),
                    ref checkpointSettings.force,
                    1);

                DrawFieldWithTickBox(
                    ref checkpointSettings.isUseInference,
                    new GUIContent("Inference", ConfigTooltip.inference),
                    ref checkpointSettings.inference,
                    1);
            }
            GUILayout.Space(5);
        }

        private void DrawTorchSettingsField()
        {
            showTorchSettings = EditorGUILayout.Foldout(showTorchSettings, new GUIContent("Torch Settings"));
            if (showTorchSettings)
            {
                DrawFieldWithTickBox(
                    ref torchSettings.isUseDevice,
                    new GUIContent("Device", ConfigTooltip.device),
                    ref torchSettings.device,
                    1);
            }
            GUILayout.Space(5);
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

        private void DrawFieldWithTickBox(ref bool active, GUIContent label, ref int value, uint deep = 0, Func<int, int> condition = null)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);

            var temp = EditorGUILayout.IntField(label, (int)value);
            if (condition != null)
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

        private void DrawFieldWithTickBox(ref bool active, GUIContent label, ref DeviceType value, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            value = (DeviceType)EditorGUILayout.EnumPopup(label, value);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private uint Min1Condition(int v)
        {
            if (v < 1)
                v = 1;

            return (uint)v;
        }

        private int MinMinus1Condition(int v)
        {
            if (v < -1)
                v = -1;

            return v;
        }
    }
}
#endif