#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Cli;
using Xardas.MLAgents.Property;

namespace Xardas.MLAgents
{
    class ConfigurationSettings : ScriptableObject
    {
        private const string settingsFileName = "ML-Agents Configuration.asset";

        [SerializeField]
        [Tooltip("You can modify this in Project Settings!")]
        [ReadOnly]
        private string pythonVirtualEnvironment;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on Windows!")]
        [ReadOnly]
        private string windowsCLI;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on MAC!")]
        [ReadOnly]
        private string macCLI;

        private static string filePath => Path.Combine(Paths.SettingsPath, settingsFileName);

        public string PythonVirtualEnvironment
        {
            get { return pythonVirtualEnvironment; }
            set
            {
                if (pythonVirtualEnvironment == value)
                    return;

                Instance.pythonVirtualEnvironment = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string WindowsCLI
        {
            get { return windowsCLI; }
            set
            {
                if (windowsCLI == value)
                    return;

                Instance.windowsCLI = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string MacCLI
        {
            get { return macCLI; }
            set
            {
                if (macCLI == value)
                    return;

                Instance.macCLI = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        private static ConfigurationSettings instance;

        internal static ConfigurationSettings Instance
        {
            get
            {
                if (File.Exists(filePath) && instance != null)
                    return instance;

                if (!Directory.Exists(Paths.SettingsPath))
                    Directory.CreateDirectory(Paths.SettingsPath);

                instance = AssetDatabase.LoadAssetAtPath<ConfigurationSettings>(filePath);
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<ConfigurationSettings>();
                    instance.pythonVirtualEnvironment = "";
                    instance.macCLI = CliExtensions.defaultMacCLI;
                    instance.windowsCLI = CliExtensions.defaultWindowsCLI;
                    AssetDatabase.CreateAsset(instance, filePath);
                    AssetDatabase.SaveAssets();
                }
                return instance;
            }
        }
    }
}
#endif