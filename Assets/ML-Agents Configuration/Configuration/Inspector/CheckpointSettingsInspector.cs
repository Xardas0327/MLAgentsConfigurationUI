#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;

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
                DrawFieldWithTickBox(ref settings.isUseRunId, property);
            }
            else if (property.name == nameof(settings.initializeFrom))
            {
                var pathWrapper = new PathWrapper<Fileformat.SettingParameter.CheckpointSettings>(
                    settings, 
                    (cs) => cs.initializeFrom, 
                    (cs, path) => cs.initializeFrom = path
                );
                DrawFilePanelProperty(
                    ref settings.isUseInitializeFrom,
                    new GUIContent(property.displayName, property.tooltip), 
                    pathWrapper, 
                    "Select a checkpoint file", 
                    "pt"
               );
            }
            else if (property.name == nameof(settings.loadModel))
            {
                DrawFieldWithTickBox(ref settings.isUseLoadModel, property);
            }
            else if (property.name == nameof(settings.resume))
            {
                DrawFieldWithTickBox(ref settings.isUseResume, property);
            }
            else if (property.name == nameof(settings.force))
            {
                DrawFieldWithTickBox(ref settings.isUseForce, property);
            }
            else if (property.name == nameof(settings.trainModel))
            {
                DrawFieldWithTickBox(ref settings.isUseTrainModel, property);
            }
            else if (property.name == nameof(settings.inference))
            {
                DrawFieldWithTickBox(ref settings.isUseInference, property);
            }
        }
    }

}
#endif