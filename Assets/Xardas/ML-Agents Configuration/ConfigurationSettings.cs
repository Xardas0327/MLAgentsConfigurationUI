#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Cli;
using Xardas.MLAgents.Property;

namespace Xardas.MLAgents
{
    enum PythonVirtualEnvironmentType
    {
        None,
        BasicPython,
        Anaconda
    }

    class ConfigurationSettings : ScriptableObject
    {
        private const string settingsFileName = "ML-Agents Configuration.asset";

        [SerializeField]
        [Tooltip("You can modify this in Project Settings!")]
        [ReadOnly]
        private PythonVirtualEnvironmentType virtualEnvType;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings!")]
        [ReadOnly]
        private string basicPythonVirtualEnvPath;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings!")]
        [ReadOnly]
        private string anacondaVirtualEnvPath;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings!")]
        [ReadOnly]
        private string anacondaVirtualEnvName;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on Windows!")]
        [ReadOnly]
        private string windowsCLI;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on Windows!")]
        [ReadOnly]
        private string windowsArguments;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on MAC!")]
        [ReadOnly]
        private string macCLI;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on MAC!")]
        [ReadOnly]
        private string macArguments;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on Linux!")]
        [ReadOnly]
        private string linuxCLI;
        [SerializeField]
        [Tooltip("You can modify this in Project Settings on Linux!")]
        [ReadOnly]
        private string linuxArguments;

        private static string filePath => Path.Combine(Paths.SettingsPath, settingsFileName);

        public PythonVirtualEnvironmentType VirtualEnvType
        {
            get { return virtualEnvType; }
            set
            {
                if (virtualEnvType == value)
                    return;

                Instance.virtualEnvType = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string BasicPythonVirtualEnvPath
        {
            get { return basicPythonVirtualEnvPath; }
            set
            {
                if (basicPythonVirtualEnvPath == value)
                    return;

                Instance.basicPythonVirtualEnvPath = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string AnacondaVirtualEnvPath
        {
            get { return anacondaVirtualEnvPath; }
            set
            {
                if (anacondaVirtualEnvPath == value)
                    return;

                Instance.anacondaVirtualEnvPath = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string AnacondaVirtualEnvName
        {
            get { return anacondaVirtualEnvName; }
            set
            {
                if (anacondaVirtualEnvName == value)
                    return;

                Instance.anacondaVirtualEnvName = value;
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

        public string WindowsArguments
        {
            get { return windowsArguments; }
            set
            {
                if (windowsArguments == value)
                    return;

                Instance.windowsArguments = value;
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

        public string MacArguments
        {
            get { return macArguments; }
            set
            {
                if (macArguments == value)
                    return;

                Instance.macArguments = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string LinuxCLI
        {
            get { return linuxCLI; }
            set
            {
                if (linuxCLI == value)
                    return;

                Instance.linuxCLI = value;
                EditorUtility.SetDirty(Instance);
            }
        }

        public string LinuxArguments
        {
            get { return linuxArguments; }
            set
            {
                if (linuxArguments == value)
                    return;

                Instance.linuxArguments = value;
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
                    instance.virtualEnvType = PythonVirtualEnvironmentType.None;
                    instance.basicPythonVirtualEnvPath = "";
                    instance.anacondaVirtualEnvPath = "";
                    instance.anacondaVirtualEnvName = "";
                    instance.windowsCLI = CliExtensions.defaultWindowsCLI;
                    instance.windowsArguments = CliExtensions.defaultWindowsArguments;
                    instance.macCLI = CliExtensions.defaultMacCLI;
                    instance.macArguments = CliExtensions.defaultMacArguments;
                    instance.linuxCLI = CliExtensions.defaultLinuxCLI;
                    instance.linuxArguments = CliExtensions.defaultLinuxArguments;

                    AssetDatabase.CreateAsset(instance, filePath);
                    AssetDatabase.SaveAssets();
                }
                return instance;
            }
        }
    }
}
#endif