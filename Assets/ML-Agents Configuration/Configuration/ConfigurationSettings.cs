#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Property;

namespace Xardas.MLAgents.Configuration
{
    class ConfigurationSettings : ScriptableObject
    {
        private const string settingsFileName = "ML-Agents Configuration.asset";

        [SerializeField]
        [Tooltip("You have to modify this in Project Settings! This won't be save here!")]
        [ReadOnly]
        private string yamlFolderPath;

        private static string filePath => Path.Combine(Paths.SettingsPath, settingsFileName);

        //TODO: it is useless, it should be removed
        public string YamlFolderPath
        {
            get { return yamlFolderPath; }
            set
            {
                if(yamlFolderPath == value)
                    return;

                AssetDatabase.DeleteAsset(filePath);
                instance = CreateObject();
                instance.yamlFolderPath = value;
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
                    instance.yamlFolderPath = "";
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