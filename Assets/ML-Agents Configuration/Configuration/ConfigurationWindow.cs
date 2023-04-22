using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat;
using System.Linq;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationWindow : EditorWindow
    {
        const string fileExtension = ".yaml";
        string[] filesInTheFolder;
        int selectedFileIndex = 0;
        string fileName;
        bool isLoaded = false;
        bool isEditableFileName = false;

        MLAgentsConfigFile configFile = null;

        [MenuItem("Window/ML-Agents/Configuration")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationWindow>("ML-Agents Configuration");
        }

        private void OnGUI()
        {
            LoadFileNames();

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
            fileName = EditorGUILayout.TextField("File's name:", fileName);
            EditorGUI.EndDisabledGroup();

            if(GUILayout.Button("New", GUILayout.Width(100)))
                CreateNewFile();

            EditorGUI.BeginDisabledGroup(!isLoaded);
            if (GUILayout.Button("Copy", GUILayout.Width(100)))
                Copy();
            GUILayout.Button("Delete", GUILayout.Width(100));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            if(configFile != null)
            {
                GUILayout.Space(10);
                configFile.name = EditorGUILayout.TextField("Name:", configFile.name, GUILayout.Width(400));
            }

            if (GUILayout.Button("Save"))
                Save();
        }

        private void LoadFileNames()
        {
            var fileNames = GetFileNames();
            if(filesInTheFolder == null || filesInTheFolder.Length != fileNames.Length)
            {
                selectedFileIndex = 0;
                CreateNewFile();
            }
            filesInTheFolder = fileNames;
        }

        private string[] GetFileNames()
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
            if (selectedFileIndex >= filesInTheFolder.Length || selectedFileIndex == 0)
                return;

            fileName = filesInTheFolder[selectedFileIndex];
            if (fileName.EndsWith(fileExtension))
                fileName = fileName.Substring(0, fileName.Length - fileExtension.Length);

            isEditableFileName = false;
            isLoaded = true;

            string filePath = ConfigurationSettings.Instance.YamlFolderPath
                + Path.DirectorySeparatorChar + fileName + fileExtension;

            var yaml = YamlFile.ConvertFileToObject(filePath);
            configFile = new MLAgentsConfigFile(yaml);
        }

        private void CreateNewFile()
        {
            fileName = "";
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
            if (string.IsNullOrEmpty(fileName))
                throw new System.Exception("It has to have a file name.");

            var fullFileName = fileName + fileExtension;

            if (isEditableFileName && filesInTheFolder != null && filesInTheFolder.Contains(fullFileName))
                throw new System.Exception("This file name is used in the folder.");

            var yaml = configFile.ToYaml();
            string filePath = ConfigurationSettings.Instance.YamlFolderPath
                + Path.DirectorySeparatorChar + fullFileName;

            YamlFile.SaveObjectToFile(yaml, filePath);
        }
    }
}