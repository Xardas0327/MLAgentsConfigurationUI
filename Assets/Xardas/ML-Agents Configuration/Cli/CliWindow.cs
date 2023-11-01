#if UNITY_EDITOR
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
using Xardas.MLAgents.Configuration.Inspector;
using System.IO;

namespace Xardas.MLAgents.Cli
{
    public class CliWindow : EditorWindow
    {
        private string yamlFilePath = "";
        private CliSettings cliSettings = new();
        private EnvironmentSettings environmentSettings = new();
        private EngineSettings engineSettings = new();
        private CheckpointSettings checkpointSettings = new();
        private TorchSettings torchSettings = new();

        //Foldout
        private bool showCliSettings;
        private bool showEnvironmentSettings;
        private bool showEngineSettings;
        private bool showCheckpointSettings;
        private bool showTorchSettings;

        [MenuItem("Window/ML-Agents Configuration/Command Line Interface")]
        public static void ShowWindow()
        {
            GetWindow<CliWindow>("ML-Agents CLI");
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            if (GUILayout.Button("Clear"))
                Clear();

            GUILayout.Label("The CLI will start in the current unity project main folder.");
            GUILayout.Label("Virtual Environment: " + ConfigurationSettings.Instance.VirtualEnvType.ToString());
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
                RunCLI();
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

        private void RunCLI()
        {
            if (string.IsNullOrEmpty(yamlFilePath))
                throw new System.Exception("There is no selected Yaml file.");

            ProcessStartInfo startInfo = new ProcessStartInfo();
            string commands = "";
#if UNITY_EDITOR_WIN
            startInfo.FileName = ConfigurationSettings.Instance.WindowsCLI;
            startInfo.Arguments = ConfigurationSettings.Instance.WindowsArguments;
            commands = GetWindowsCmdCommand();
#elif UNITY_EDITOR_OSX
            startInfo.FileName = ConfigurationSettings.Instance.MacCLI;
            startInfo.Arguments = ConfigurationSettings.Instance.MacArguments;
            startInfo.UseShellExecute = false;
            commands = CreateShellScriptForMac();
#elif UNITY_EDITOR_LINUX
            startInfo.FileName = ConfigurationSettings.Instance.LinuxCLI;
            startInfo.Arguments = ConfigurationSettings.Instance.LinuxArguments;
            commands = "./" + CreateShellScriptForLinux();
#endif
            startInfo.Arguments = startInfo.Arguments.Replace("{{commands}}", commands);
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.CreateNoWindow = false;

            Process.Start(startInfo);
        }

#if UNITY_EDITOR_WIN
        private string GetWindowsCmdCommand()
        {
            var arguments = new StringBuilder();
            switch(ConfigurationSettings.Instance.VirtualEnvType)
            {
                case PythonVirtualEnvironmentType.BasicPython:
                    arguments.Append($"\"{ConfigurationSettings.Instance.BasicPythonVirtualEnvPath}\" && ");
                    break;
                case PythonVirtualEnvironmentType.Anaconda:
                    arguments.Append($"\"{ConfigurationSettings.Instance.AnacondaVirtualEnvPath}\" {ConfigurationSettings.Instance.AnacondaVirtualEnvName} && ");
                    break;
            }

            arguments.Append($"mlagents-learn \"{yamlFilePath}\" ");

            arguments.Append(GetMLagentsLearnArguments());

            return arguments.ToString();
        }
#endif

#if UNITY_EDITOR_OSX
        /// <summary>
        /// 
        /// </summary>
        /// <returns>File path</returns>
        private string CreateShellScriptForMac()
        {
            var shellScriptPath = Application.dataPath + "/../" + CliExtensions.shellScriptFileName;
            CreateShellScript(shellScriptPath, "#!/bin/sh");

            return shellScriptPath;
        }
#endif

#if UNITY_EDITOR_LINUX
        /// <summary>
        /// 
        /// </summary>
        /// <returns>File name</returns>
        private string CreateShellScriptForLinux()
        {
            var shellScriptPath = Application.dataPath + "/../" + CliExtensions.shellScriptFileName;
            CreateShellScript(shellScriptPath, "#!/bin/bash");

            return CliExtensions.shellScriptFileName;
        }
#endif

#if UNITY_EDITOR_LINUX || UNITY_EDITOR_OSX
        private void CreateShellScript(string path, string firstLine = null)
        {
            using (StreamWriter file = new StreamWriter(path, false))
            {
                if(!string.IsNullOrEmpty(firstLine))
                    file.WriteLine(firstLine);

                file.WriteLine($"cd {Application.dataPath}/../");
                switch (ConfigurationSettings.Instance.VirtualEnvType)
                {
                    case PythonVirtualEnvironmentType.BasicPython:
                        file.WriteLine("source " + ConfigurationSettings.Instance.BasicPythonVirtualEnvPath);
                        break;
                    case PythonVirtualEnvironmentType.Anaconda:
                        file.WriteLine("source " + ConfigurationSettings.Instance.AnacondaVirtualEnvPath + " " + ConfigurationSettings.Instance.AnacondaVirtualEnvName);
                        break;
                }

                file.WriteLine($"mlagents-learn \"{yamlFilePath}\" " + GetMLagentsLearnArguments());
            }

            Process chmod = new Process
            {
                StartInfo = {
                    FileName = @"/bin/bash",
                    Arguments = string.Format("-c \"chmod 755 {0}\"", path),
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            chmod.Start();
            chmod.WaitForExit();
        }
#endif

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
                SettingsInspector.DrawFieldWithTickBox(
                    ref cliSettings.isUseDeterministic,
                    new GUIContent("Deterministic", ConfigTooltip.deterministic),
                    ref cliSettings.deterministic,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref cliSettings.isUseNumAreas,
                    new GUIContent("Num Areas", ConfigTooltip.numAreas),
                    ref cliSettings.numAreas,
                    1,
                    Min1Condition);

                SettingsInspector.DrawFieldWithTickBox(
                    ref cliSettings.isUseDebug,
                    new GUIContent("Debug", ConfigTooltip.debug),
                    ref cliSettings.debug,
                    1);

                var pathWrapper = new PathWrapper<CliSettings>(
                    cliSettings, 
                    (cs) => cs.resultsDir, 
                    (cs, path) => cs.resultsDir = path
                );

                SettingsInspector.DrawFolderPanelProperty(
                    ref cliSettings.isUseResultsDir,
                    new GUIContent("Result Directory", ConfigTooltip.resultsDir),
                    pathWrapper,
                    "Select result folder",
                    1
                );
            }
            GUILayout.Space(5);
        }

        private void DrawEnvironmentSettingsField()
        {
            showEnvironmentSettings = EditorGUILayout.Foldout(showEnvironmentSettings, new GUIContent("Environment Settings"));
            if (showEnvironmentSettings)
            {
                var pathWrapper = new PathWrapper<EnvironmentSettings>(
                    environmentSettings,
                    (es) => es.envPath,
                    (es, path) => es.envPath = path
                );

                SettingsInspector.DrawFolderPanelProperty(
                    ref environmentSettings.isUseEnvPath,
                    new GUIContent("Env Path", ConfigTooltip.envPath),
                    pathWrapper,
                    "Select a build folder",
                    1
                );

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseEnvArgs,
                    new GUIContent("Env Args", ConfigTooltip.envArgs),
                    ref environmentSettings.envArgs,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseBasePort,
                    new GUIContent("Base Port", ConfigTooltip.basePort),
                    ref environmentSettings.basePort,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseNumEnvs,
                    new GUIContent("Num Envs", ConfigTooltip.numEnvs),
                    ref environmentSettings.numEnvs,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseTimeoutWait,
                    new GUIContent("Timeout Wait", ConfigTooltip.timeoutWait),
                    ref environmentSettings.timeoutWait,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseSeed,
                    new GUIContent("Seed", ConfigTooltip.seed),
                    ref environmentSettings.seed,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseMaxLifetimeRestarts,
                    new GUIContent("Max Lifetime Restarts", ConfigTooltip.maxLifetimeRestarts),
                    ref environmentSettings.maxLifetimeRestarts,
                    1,
                    MinMinus1Condition);

                SettingsInspector.DrawFieldWithTickBox(
                    ref environmentSettings.isUseRestartsRateLimitN,
                    new GUIContent("Restarts Rate Limit N", ConfigTooltip.restartsRateLimitN),
                    ref environmentSettings.restartsRateLimitN,
                    1,
                    MinMinus1Condition);

                SettingsInspector.DrawFieldWithTickBox(
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
                SettingsInspector.DrawFieldWithTickBox(
                    ref engineSettings.isUseWidth,
                    new GUIContent("Width", ConfigTooltip.width),
                    ref engineSettings.width,
                    1,
                    Min1Condition);

                SettingsInspector.DrawFieldWithTickBox(
                    ref engineSettings.isUseHeight,
                    new GUIContent("Height", ConfigTooltip.height),
                    ref engineSettings.height,
                    1,
                    Min1Condition);

                SettingsInspector.DrawFieldWithTickBox(
                    ref engineSettings.isUseQualityLevel,
                    new GUIContent("Quality Level", ConfigTooltip.qualityLevel),
                    ref engineSettings.qualityLevel,
                    1,
                    Min1Condition);

                SettingsInspector.DrawFieldWithTickBox(
                    ref engineSettings.isUseTimeScale,
                    new GUIContent("Time Scale", ConfigTooltip.timeScale),
                    ref engineSettings.timeScale,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref engineSettings.isUseTargetFrameRate,
                    new GUIContent("Target Frame Rate", ConfigTooltip.targetFrameRate),
                    ref engineSettings.targetFrameRate,
                    1,
                    MinMinus1Condition);

                SettingsInspector.DrawFieldWithTickBox(
                    ref engineSettings.isUseCaptureFrameRate,
                    new GUIContent("Capture Frame Rate", ConfigTooltip.captureFrameRate),
                    ref engineSettings.captureFrameRate,
                    1,
                    Min1Condition);

                SettingsInspector.DrawFieldWithTickBox(
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
                SettingsInspector.DrawFieldWithTickBox(
                    ref checkpointSettings.isUseRunId,
                    new GUIContent("Run id", ConfigTooltip.runId),
                    ref checkpointSettings.runId,
                    1);

                var pathWrapper = new PathWrapper<CheckpointSettings>(
                    checkpointSettings,
                    (cs) => cs.initializeFrom,
                    (cs, path) => cs.initializeFrom = path
                );

                SettingsInspector.DrawFilePanelProperty(
                    ref checkpointSettings.isUseInitializeFrom,
                    new GUIContent("Initialize From", ConfigTooltip.initializeFrom),
                    pathWrapper,
                    "Select a checkpoint file", 
                    "pt",
                    1
                );


                SettingsInspector.DrawFieldWithTickBox(
                    ref checkpointSettings.isUseResume,
                    new GUIContent("Resume", ConfigTooltip.resume),
                    ref checkpointSettings.resume,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
                    ref checkpointSettings.isUseForce,
                    new GUIContent("Force", ConfigTooltip.force),
                    ref checkpointSettings.force,
                    1);

                SettingsInspector.DrawFieldWithTickBox(
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
                SettingsInspector.DrawFieldWithTickBox(
                    ref torchSettings.isUseDevice,
                    new GUIContent("Device", ConfigTooltip.device),
                    ref torchSettings.device,
                    1);
            }
            GUILayout.Space(5);
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