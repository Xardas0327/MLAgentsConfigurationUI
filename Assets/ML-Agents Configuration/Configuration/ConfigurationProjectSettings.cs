#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xardas.MLAgents.Configuration
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

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField("Yaml Folder Path", ConfigurationSettings.Instance.YamlFolderPath);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Browse"))
                        OpenFileBrower();

                    GUI.enabled = !IsEmptyYamlFolderPath();
                    if (GUILayout.Button("Copy Path"))
                        CopyYamlFolderPathToClipBoard();
                    GUI.enabled = true;
                    GUILayout.EndHorizontal();
                },

                keywords = new HashSet<string>() { "ML-Agents", "Configuration", "folderPath", "yaml" }
            };

            return provider;
        }

        private static void OpenFileBrower()
        {
            string newPath = EditorUtility.OpenFolderPanel("Select Yaml folder", "", "");

            if (newPath != ConfigurationSettings.Instance.YamlFolderPath && !string.IsNullOrEmpty(newPath))
                ConfigurationSettings.Instance.YamlFolderPath = newPath;

            GUIUtility.systemCopyBuffer = "I want to put this string on the clipboard.";
        }

        private static void CopyYamlFolderPathToClipBoard()
        {
            GUIUtility.systemCopyBuffer = ConfigurationSettings.Instance.YamlFolderPath;
        }

        private static bool IsEmptyYamlFolderPath()
        {
            return string.IsNullOrEmpty(ConfigurationSettings.Instance.YamlFolderPath);
        }
    }
}
#endif