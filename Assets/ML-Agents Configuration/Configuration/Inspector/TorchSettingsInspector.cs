#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(TorchSettings))]
    public class TorchSettingsInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawIspector((TorchSettings)target);
        }

        void DrawIspector(TorchSettings settings)
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
                        else if(iterator.name == nameof(settings.torchSettings.device))
                        {
                            EditorGUILayout.BeginHorizontal();
                            settings.torchSettings.isUseDevice = EditorGUILayout.Toggle(settings.torchSettings.isUseDevice, GUILayout.MaxWidth(15));
                            EditorGUI.BeginDisabledGroup(!settings.torchSettings.isUseDevice);
                            EditorGUILayout.PropertyField(iterator, true);
                            EditorGUI.EndDisabledGroup();
                            EditorGUILayout.EndHorizontal();
                        } 
                    }
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }
    }
}
#endif