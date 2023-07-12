#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationImporterWindow : EditorWindow
    {
        const string fileExtension = "yaml";
        string fileName;
        bool isEditableFileName = false;
        string filePath = null;
        string fileData = null;
        Vector2 fileDataScrollPos;

        bool IsLoaded => filePath != null;

        [MenuItem("Window/ML-Agents/Config Importer")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationImporterWindow>("ML-Agents Config Importer");
        }

        private void OnGUI()
        {
            GUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(!isEditableFileName);
            fileName = EditorGUILayout.TextField("File's name", fileName);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Open", GUILayout.Width(100)))
                OpenFile();

            EditorGUI.BeginDisabledGroup(!IsLoaded);
            if (GUILayout.Button("Copy", GUILayout.Width(100)))
                Copy();
            if (GUILayout.Button("Delete", GUILayout.Width(100)))
                Delete();
            if (GUILayout.Button("Clear", GUILayout.Width(100)))
                Clear();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(!IsLoaded);
            if (GUILayout.Button("Create Asset file"))
                CreateAsset();
            EditorGUI.EndDisabledGroup();

            if (!string.IsNullOrEmpty(fileData))
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("File's data");
                fileDataScrollPos = EditorGUILayout.BeginScrollView(fileDataScrollPos, GUILayout.Height(600));
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextArea(fileData);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndScrollView();
            }
        }

        private void OpenFile()
        {
            var newFilePath = EditorUtility.OpenFilePanel("Select YAML file", Application.dataPath, fileExtension);
            if (!string.IsNullOrEmpty(newFilePath) && newFilePath != filePath)
            {
                filePath = newFilePath;
                SetFileName(filePath);
                fileData = File.ReadAllText(filePath);

                isEditableFileName = false;
            }
        }

        private void SetFileName(string path)
        {
            fileName = Path.GetFileName(path);
            var extension = "." + fileExtension;
            if (fileName.EndsWith(extension))
                fileName = fileName.Substring(0, fileName.Length - extension.Length);
        }

        private void Clear()
        {
            fileName = "";
            isEditableFileName = true;
            filePath = null;
            fileData = null;
        }

        private void Copy()
        {
            isEditableFileName = true;
        }

        private void CreateAsset()
        {
            if (!IsLoaded)
                throw new System.Exception("There is no loaded file.");

            if (string.IsNullOrEmpty(fileName))
                throw new System.Exception("It has to have a file name.");

            string folderPath = Path.Combine(Paths.FilesPath, fileName);

            if (Directory.Exists(folderPath))
            {
                if (EditorUtility.DisplayDialog("Warning!",
                $"The {fileName} folder already exists. If you continue maybe some files will be overwritten.", "Continue", "Stop"))
                {
                    CreateFiles(folderPath);
                }
            }
            else
            {
                Directory.CreateDirectory(folderPath);
                CreateFiles(folderPath);
            }

        }

        private void CreateFiles(string folderPath)
        {
            var yaml = YamlFile.ConvertStringToObject(fileData);
            ConfigFileCreater.CreateFiles(folderPath, yaml);
        }

        private void Delete()
        {
            var fullFileName = fileName + "." + fileExtension;
            if(EditorUtility.DisplayDialog("Delete config file",
                $"Are you sure you want to delete the {fullFileName} config file?", "Yes", "No"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Clear();
                    Debug.Log("File is deleted: " + filePath);
                }
            }
        }
    }
}
#endif