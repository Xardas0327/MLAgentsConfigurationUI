#if UNITY_EDITOR_WIN
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Property;

namespace Xardas.MLAgents
{
    class ConfigurationSettings : ScriptableObject
    {
        private const string settingsFileName = "ML-Agents Configuration.asset";

        [SerializeField]
        [Tooltip("You have to modify this in Project Settings!")]
        [ReadOnly]
        private string pythonVirtualEnvironment;

        private static string filePath => Path.Combine(Paths.SettingsPath, settingsFileName);

        public string PythonVirtualEnvironment
        {
            get { return pythonVirtualEnvironment; }
            set
            {
                if (pythonVirtualEnvironment == value)
                    return;

                AssetDatabase.DeleteAsset(filePath);
                instance = CreateObject();
                instance.pythonVirtualEnvironment = value;
                AssetDatabase.CreateAsset(instance, filePath);
                AssetDatabase.SaveAssets();
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
                    instance = CreateObject();
                    instance.pythonVirtualEnvironment = "";
                    AssetDatabase.CreateAsset(instance, filePath);
                    AssetDatabase.SaveAssets();
                }
                return instance;
            }
        }

        private static ConfigurationSettings CreateObject()
        {
            return ScriptableObject.CreateInstance<ConfigurationSettings>();
        }
    }
}
#endif