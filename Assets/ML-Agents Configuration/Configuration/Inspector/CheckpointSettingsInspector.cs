#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(CheckpointSettings))]
    public class CheckpointSettingsInspector : SettingsInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);
            if (GUILayout.Button("Validation"))
            {
                ((CheckpointSettings)target).IsValid();
            }
        }

        protected override void DrawProperty(SerializedProperty property)
        {
            var settings = ((CheckpointSettings)target).settings;
            if (property.name == nameof(settings.runId))
            {
                DrawPropertyWithTickBox(ref settings.isUseRunId, property);
            }
            else if (property.name == nameof(settings.initializeFrom))
            {
                DrawInitPathProperty(ref settings.isUseInitializeFrom, property, settings);
            }
            else if (property.name == nameof(settings.loadModel))
            {
                DrawPropertyWithTickBox(ref settings.isUseLoadModel, property);
            }
            else if (property.name == nameof(settings.resume))
            {
                DrawPropertyWithTickBox(ref settings.isUseResume, property);
            }
            else if (property.name == nameof(settings.force))
            {
                DrawPropertyWithTickBox(ref settings.isUseForce, property);
            }
            else if (property.name == nameof(settings.trainModel))
            {
                DrawPropertyWithTickBox(ref settings.isUseTrainModel, property);
            }
            else if (property.name == nameof(settings.inference))
            {
                DrawPropertyWithTickBox(ref settings.isUseInference, property);
            }
        }

        protected void DrawInitPathProperty(ref bool active, SerializedProperty property, IInitPathObject initPathObject)
        {
            EditorGUILayout.BeginHorizontal();
            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFilePanel("Select init file", Application.dataPath, "pt");

                    if (newPath != initPathObject.InitPath && !string.IsNullOrEmpty(newPath))
                        initPathObject.InitPath = newPath;
                };
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }
    }

}
#endif