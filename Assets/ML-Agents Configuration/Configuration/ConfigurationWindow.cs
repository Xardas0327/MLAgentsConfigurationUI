using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationWindow : EditorWindow
    {
        const string fileExtension = ".yaml";
        string[] filesInTheFolder;
        int selectedFileIndex = 0;
        string loadedFileName;
        bool isLoaded = false;
        bool isEditableFileName = true;

        MLAgentsConfigFile configFile;

        [MenuItem("Window/ML-Agents/Configuration")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationWindow>("ML-Agents Configuration");
        }

        private void OnGUI()
        {
            filesInTheFolder = GetFiles();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            selectedFileIndex = EditorGUILayout.Popup("Files:", selectedFileIndex, filesInTheFolder);
            if (GUILayout.Button("Load file", GUILayout.Width(100)))
                LoadFile();
            EditorGUILayout.EndHorizontal();
            if(isLoaded)
            {
                EditorGUILayout.LabelField("The file is loaded.");
                GUILayout.Space(10);
            }
            else
                GUILayout.Space(30);

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(!isEditableFileName);
            loadedFileName = EditorGUILayout.TextField("Loaded file:", loadedFileName);
            EditorGUI.EndDisabledGroup();

            if(GUILayout.Button("New", GUILayout.Width(100)))
                CreateNewFile();

            EditorGUI.BeginDisabledGroup(!isLoaded);
            if (GUILayout.Button("Copy", GUILayout.Width(100)))
                Copy();
            GUILayout.Button("Delete", GUILayout.Width(100));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Save"))
                Save();
        }

        private string[] GetFiles()
        {
            if (string.IsNullOrEmpty(ConfigurationSettings.Instance.YamlFolderPath))
            {
                var empty = new string[1];
                empty[0] = "";
                return empty;
            }


            var info = new DirectoryInfo(ConfigurationSettings.Instance.YamlFolderPath);
            var fileInfo = info.GetFiles("*" + fileExtension);

            var fileNames = new string[fileInfo.Length + 1];
            fileNames[0] = "";
            for (var i =0; i < fileInfo.Length; ++i)
            {
                fileNames[i+1] = fileInfo[i].Name;
            }

            return fileNames;
        }

        private void LoadFile()
        {
            loadedFileName = filesInTheFolder[selectedFileIndex];
            if (loadedFileName.EndsWith(fileExtension))
                loadedFileName = loadedFileName.Substring(0, loadedFileName.Length - fileExtension.Length);

            isEditableFileName = false;
            isLoaded = true;

            string filePath = ConfigurationSettings.Instance.YamlFolderPath
                + Path.DirectorySeparatorChar + loadedFileName + fileExtension;

            var yaml = YamlFile.ConvertFileToObject(filePath);
            configFile = new MLAgentsConfigFile(yaml);
        }

        private void CreateNewFile()
        {
            loadedFileName = "";
            isLoaded = false;
            isEditableFileName = true;
            configFile = new MLAgentsConfigFile();
        }

        private void Copy()
        {
            isEditableFileName = true;
        }

        private void Save()
        {
        }
    }
}
