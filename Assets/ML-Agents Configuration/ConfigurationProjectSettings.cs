#if UNITY_EDITOR_WIN
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
                    EditorGUILayout.TextField("Python Virtual Env", ConfigurationSettings.Instance.PythonVirtualEnvironment);
                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
                        OpenFileBrower();
                    GUILayout.EndHorizontal();
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