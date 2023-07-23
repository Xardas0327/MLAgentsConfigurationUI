#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Xardas.MLAgents.Configuration.Inspector
{
    public abstract class SettingsInspector : Editor
    {
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

        protected void DrawPropertyWithTickBox(ref bool active, SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.PropertyField(property, true);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        protected void DrawFilePanelProperty<T>(ref bool active, SerializedProperty property, PathWrapper<T> pathObject, string title, string extension)
        {
            EditorGUILayout.BeginHorizontal();
            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFilePanel(title, Application.dataPath, extension);

                    if (newPath != pathObject.Path && !string.IsNullOrEmpty(newPath))
                        pathObject.Path = newPath;
                };
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        protected void DrawFolderPanelProperty<T>(ref bool active, SerializedProperty property, PathWrapper<T> pathObject, string title)
        {
            EditorGUILayout.BeginHorizontal();
            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFolderPanel(title, Application.dataPath, "");

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