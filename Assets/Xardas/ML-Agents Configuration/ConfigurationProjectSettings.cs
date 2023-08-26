#if UNITY_EDITOR
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

                    GUILayout.BeginHorizontal();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField(
                        new GUIContent("Python Virtual Env", "Please add the activate file from your python virtual environment"), 
                        ConfigurationSettings.Instance.PythonVirtualEnvironment
                    );
                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
                        OpenFileBrower();
                    GUILayout.EndHorizontal();

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

        private static void OpenFileBrower()
        {
            string newPath = EditorUtility.OpenFilePanel("Select Python Virtual Environment", Application.dataPath, "");

            if (newPath != ConfigurationSettings.Instance.PythonVirtualEnvironment && !string.IsNullOrEmpty(newPath))
                ConfigurationSettings.Instance.PythonVirtualEnvironment = newPath;
        }
    }
}
#endif