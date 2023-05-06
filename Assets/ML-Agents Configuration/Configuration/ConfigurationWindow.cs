using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat;
using System.Linq;
using UnityEngine.UIElements;

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
            if (GUILayout.Button("Delete", GUILayout.Width(100)))
                Delete();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            if(configFile != null)
            {
                GUILayout.Space(10);
                configFile.name = EditorGUILayout.TextField("Name:", configFile.name, GUILayout.Width(400));
            }

            EditorGUI.BeginDisabledGroup(configFile == null);
            if (GUILayout.Button("Save"))
                Save();
            EditorGUI.EndDisabledGroup();
        }

        private void LoadFileNames()
        {
            var fileNames = GetFileNames();
            if(filesInTheFolder == null || filesInTheFolder.Length != fileNames.Length)
            {
                selectedFileIndex = GetIndex(fileNames, fileName + fileExtension);
                if(selectedFileIndex < 0)
                    CreateNewFile();
            }
            filesInTheFolder = fileNames;
        }

        private int GetIndex(string[] fileNames, string fileName)
        {
            for(int i = 0; i < fileNames.Length; ++i)
            {
                if (fileNames[i] == fileName)
                    return i;
            }

            return -1;
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
            Debug.Log("File is loaded");
        }

        private void CreateNewFile()
        {
            selectedFileIndex = 0;
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
            Debug.Log("File is saved.");
        }

        private void Delete()
        {
            var fullFileName = fileName + fileExtension;
            if(EditorUtility.DisplayDialog("Delete config file",
                $"Are you sure you want to delete the {fullFileName} config file?", "Yes", "No"))
            {
                string filePath = ConfigurationSettings.Instance.YamlFolderPath
                    + Path.DirectorySeparatorChar + fullFileName;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    CreateNewFile();
                    Debug.Log("File is deleted");
                }
            }
        }
    }
}
