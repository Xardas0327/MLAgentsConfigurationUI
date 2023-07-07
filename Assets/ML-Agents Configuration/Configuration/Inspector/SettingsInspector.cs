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

    }
}
#endif