#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationImporterWindow : EditorWindow
    {
        const string fileExtension = "yaml";
        //For MacOS
        const string fileExtension2 = "yml";
        string fileName;
        bool isEditableFileName = false;
        string filePath = null;
        string fileData = null;
        Vector2 fileDataScrollPos;

        //DropDown
        const string everythingText = "Everything";
        HashSet<string> selectedImportTypes = new HashSet<string>();
        readonly HashSet<string> importTypes = new HashSet<string>()
        {
            ConfigText.behaviors,
            ConfigText.checkpointSettings,
            ConfigText.engineSettings,
            ConfigText.environmentParameters,
            ConfigText.environmentSettings,
            ConfigText.torchSettings
        };

        bool IsLoaded => !string.IsNullOrEmpty(filePath);

        [MenuItem("Window/ML-Agents Configuration/Config Importer")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationImporterWindow>("ML-Agents Config Importer");
        }

        private void OnGUI()
        {
            GUILayout.Space(30);

            EditorGUI.BeginDisabledGroup(!isEditableFileName);
            fileName = EditorGUILayout.TextField("File", fileName);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open"))
                OpenFile();

            EditorGUI.BeginDisabledGroup(!IsLoaded);
            if (GUILayout.Button("Copy"))
                Copy();
            if (GUILayout.Button("Delete"))
                Delete();
            if (GUILayout.Button("Clear"))
                Clear();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            EditorGUI.BeginDisabledGroup(!IsLoaded);
            CreateDropDown();
            GUILayout.Space(5);

            if (GUILayout.Button("Create Asset file(s)"))
                CreateAssets();
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

            extension = "." + fileExtension2;
            if (fileName.EndsWith(extension))
                fileName = fileName.Substring(0, fileName.Length - extension.Length);
        }

        private void Clear()
        {
            fileName = "";
            isEditableFileName = false;
            filePath = null;
            fileData = null;
            selectedImportTypes.Clear();
        }

        private void Copy()
        {
            isEditableFileName = true;
        }

        private void CreateAssets()
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
            ConfigFileCreater.CreateFiles(folderPath, yaml, selectedImportTypes);
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
                    Debug.Log("File is deleted: " + filePath);
                    Clear();
                }
            }
        }

        private void CreateDropDown()
        {
            GUILayout.Label("Import");
            var label = selectedImportTypes.Count > 0 ? String.Join(", ", selectedImportTypes) : everythingText;
            if (EditorGUILayout.DropdownButton(new GUIContent(label), FocusType.Passive))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent(everythingText), selectedImportTypes.Count == 0, DropDownClick, everythingText);
                foreach (var type in importTypes)
                {
                    menu.AddItem(new GUIContent(type), selectedImportTypes.Contains(type), DropDownClick, type);
                }
                menu.ShowAsContext();
            }
        }

        private void DropDownClick(object userData)
        {
            var text = (string)userData;
            if(text == everythingText)
            {
                selectedImportTypes.Clear();
                return;
            }

            if(selectedImportTypes.Contains(text))
                selectedImportTypes.Remove(text);
            else
                selectedImportTypes.Add(text);

            if (selectedImportTypes.Count == importTypes.Count)
                selectedImportTypes.Clear();
        }
    }
}
#endif