using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationCreatorWindow : EditorWindow
    {
        const string fileExtension = ".yaml";
        string[] filesInTheFolder;
        int selectedFileIndex = 0;
        string fileName;
        bool isLoaded = false;
        bool isEditableFileName = false;
        string fileData = null;
        Vector2 fileDataScrollPos;

        [MenuItem("Window/ML-Agents/Config Creator")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationCreatorWindow>("ML-Agents Config Creator");
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
            GUILayout.Space(5);

            if (GUILayout.Button("Create Asset file"))
                CreateAsset();

            if (fileData != null)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("File's data:");
                fileDataScrollPos = EditorGUILayout.BeginScrollView(fileDataScrollPos, GUILayout.Height(600));
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextArea(fileData);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndScrollView();
            }
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

            fileData = File.ReadAllText(
                Path.Combine(ConfigurationSettings.Instance.YamlFolderPath, fileName + fileExtension)
                );

            isEditableFileName = false;
            isLoaded = true;
        }

        private void CreateNewFile()
        {
            selectedFileIndex = 0;
            fileName = "";
            isLoaded = false;
            isEditableFileName = true;
            fileData = null;
        }

        private void Copy()
        {
            isEditableFileName = true;
        }

        private void CreateAsset()
        {
            if (string.IsNullOrEmpty(fileName))
                throw new System.Exception("It has to have a file name.");

            string folderPath = Path.Combine(Paths.FilesPath, fileName);

            if (Directory.Exists(folderPath))
                throw new System.Exception("The folder exists. Please remove it or move it in another folder");
            else
                Directory.CreateDirectory(folderPath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (isLoaded)
            {
                var yaml = YamlFile.ConvertStringToObject(fileData);
                ConfigFileCreater.CreateFiles(folderPath, yaml);
            }
            else
            {
                ConfigFileCreater.CreateBasicBehavior(folderPath);
            }
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
                    Debug.Log("File is deleted: " + filePath);
                }
            }
        }
    }
}
