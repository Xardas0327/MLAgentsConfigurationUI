#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR_LINUX
using Xardas.MLAgents.Cli;
#endif

namespace Xardas.MLAgents
{
    static class ConfigurationProjectSettings
    {
        [SettingsProvider]
        public static SettingsProvider CreateConfigurationSettingsProvider()
        {
            var provider = new SettingsProvider("Project/ML-Agents Configuration", SettingsScope.Project)
            {
                label = "ML-Agents Configuration",
                guiHandler = (searchContext) =>
                {
                    GUILayout.Space(10);

                    ConfigurationSettings.Instance.VirtualEnvType = 
                        (PythonVirtualEnvironmentType)EditorGUILayout.EnumPopup("Virtual Environment Type:", ConfigurationSettings.Instance.VirtualEnvType);

                    switch(ConfigurationSettings.Instance.VirtualEnvType)
                    {
                        case PythonVirtualEnvironmentType.BasicPython:
                            GUILayout.BeginHorizontal();
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.TextField(
                                new GUIContent("Basic Python Virtual Env", "Please add the activate file from your python virtual environment"),
                                ConfigurationSettings.Instance.BasicPythonVirtualEnvPath
                            );
                            EditorGUI.EndDisabledGroup();

                            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
                                OpenFileBrower("Select Basic Python Virtual Environment", (string newPath) =>
                                {
                                    if (newPath != ConfigurationSettings.Instance.BasicPythonVirtualEnvPath && !string.IsNullOrEmpty(newPath))
                                        ConfigurationSettings.Instance.BasicPythonVirtualEnvPath = newPath;
                                });
                            GUILayout.EndHorizontal();

                            break;
                        case PythonVirtualEnvironmentType.Anaconda:
                            GUILayout.BeginHorizontal();
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.TextField(
                                new GUIContent("Anaconda Virtual Env", "Please add the activate file from your Anaconda virtual environment"),
                                ConfigurationSettings.Instance.AnacondaVirtualEnvPath
                            );
                            EditorGUI.EndDisabledGroup();

                            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
                                OpenFileBrower("Select Anaconda Virtual Environment", (string newPath) =>
                                {
                                    if (newPath != ConfigurationSettings.Instance.AnacondaVirtualEnvPath && !string.IsNullOrEmpty(newPath))
                                        ConfigurationSettings.Instance.AnacondaVirtualEnvPath = newPath;
                                });
                            GUILayout.EndHorizontal();

                            ConfigurationSettings.Instance.AnacondaVirtualEnvName = EditorGUILayout.TextField(
                                new GUIContent("Anaconda Virtual Env Name", "Please add name of the Anaconda virtual environment."),
                                ConfigurationSettings.Instance.AnacondaVirtualEnvName
                            );
                            break;
                    }

                    GUILayout.Space(10);
#if UNITY_EDITOR_WIN
                    ConfigurationSettings.Instance.WindowsCLI = EditorGUILayout.TextField(
                        new GUIContent("Windows CMD", "Please add path of the CMD"),
                        ConfigurationSettings.Instance.WindowsCLI
                    );
                    ConfigurationSettings.Instance.WindowsArguments = EditorGUILayout.TextField(
                        new GUIContent("Windows CMD Arguments", "{{commands}} will be replaced with commands.\nWhich is separated by &&."),
                        ConfigurationSettings.Instance.WindowsArguments
                    );
#endif

#if UNITY_EDITOR_OSX
                    ConfigurationSettings.Instance.MacCLI = EditorGUILayout.TextField(
                        new GUIContent("Mac Terminal", "Please add path of the terinal"),
                        ConfigurationSettings.Instance.MacCLI
                    );
                    ConfigurationSettings.Instance.MacArguments = EditorGUILayout.TextField(
                        new GUIContent("Mac Terminal Arguments", "{{commands}} will be replaced with path of the generated Shell Script file."),
                        ConfigurationSettings.Instance.MacArguments
                    );
#endif

#if UNITY_EDITOR_LINUX
                    ConfigurationSettings.Instance.LinuxCLI = EditorGUILayout.TextField(
                        new GUIContent("Linux Terminal", "Please add path of the terinal"),
                        ConfigurationSettings.Instance.LinuxCLI
                    );
                    ConfigurationSettings.Instance.LinuxArguments = EditorGUILayout.TextField(
                        new GUIContent("Linux Terminal Arguments", 
                        "{{commands}} will be replaced with path of the generated Shell Script file.\n./" 
                        + CliExtensions.shellScriptFileName),
                        ConfigurationSettings.Instance.LinuxArguments
                    );
#endif
                },

                keywords = new HashSet<string>() { "ML-Agents", "Configuration" }
            };

            return provider;
        }

        private static void OpenFileBrower(string title, Action<string> Update)
        {
            string newPath = EditorUtility.OpenFilePanel(title, Application.dataPath, "");

            Update(newPath);
        }
    }
}
#endif