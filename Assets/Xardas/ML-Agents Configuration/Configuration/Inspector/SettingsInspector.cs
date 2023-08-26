#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using DeviceType = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.DeviceType;

namespace Xardas.MLAgents.Configuration.Inspector
{
    public abstract class SettingsInspector : Editor
    {
        private const float depthSize = 15;

        public override void OnInspectorGUI()
        {
            DrawInspector();
        }

        protected void DrawInspector()
        {
            using (new LocalizationGroup(target))
            {
                EditorGUI.BeginChangeCheck();
                serializedObject.UpdateIfRequiredOrScript();
                SerializedProperty iterator = serializedObject.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        if (iterator.displayName == "Script")
                        {
                            EditorGUILayout.PropertyField(iterator, true);
                            EditorGUILayout.Space();
                        }
                        else
                            DrawProperty(iterator);
                    }
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }

        protected abstract void DrawProperty(SerializedProperty property);

        public static void DrawFieldWithTickBox(ref bool active, SerializedProperty property, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.PropertyField(property, true);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFieldWithTickBox(ref bool active, GUIContent label, ref bool value, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            value = EditorGUILayout.Toggle(label, value);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFieldWithTickBox(ref bool active, GUIContent label, ref uint value, uint deep = 0, Func<int, uint> condition = null)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);

            var temp = EditorGUILayout.IntField(label, (int)value);
            if (condition == null)
            {
                if (temp < 0)
                    temp = 0;
                value = (uint)temp;
            }
            else
                value = condition(temp);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFieldWithTickBox(ref bool active, GUIContent label, ref int value, uint deep = 0, Func<int, int> condition = null)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);

            var temp = EditorGUILayout.IntField(label, (int)value);
            if (condition != null)
                value = condition(temp);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFieldWithTickBox(ref bool active, GUIContent label, ref string value, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            value = EditorGUILayout.TextField(label, value);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFieldWithTickBox(ref bool active, GUIContent label, ref DeviceType value, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            value = (DeviceType)EditorGUILayout.EnumPopup(label, value);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFilePanelProperty<T>(ref bool active, GUIContent label, PathWrapper<T> pathObject, string filePanelTitle, string extension, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.TextField(label, pathObject.Path);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFilePanel(filePanelTitle, Application.dataPath, extension);

                    if (newPath != pathObject.Path && !string.IsNullOrEmpty(newPath))
                        pathObject.Path = newPath;
                };
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawFolderPanelProperty<T>(ref bool active, GUIContent label, PathWrapper<T> pathObject, string folderPanelTitle, uint deep = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (deep > 0)
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * deep));

            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.TextField(label, pathObject.Path);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFolderPanel(folderPanelTitle, Application.dataPath, "");

                    if (newPath != pathObject.Path && !string.IsNullOrEmpty(newPath))
                        pathObject.Path = newPath;
                };
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif