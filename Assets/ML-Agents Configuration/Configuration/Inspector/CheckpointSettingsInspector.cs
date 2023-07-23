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
                DrawPropertyWithTickBox(ref settings.isUseRunId, property);
            }
            else if (property.name == nameof(settings.initializeFrom))
            {
                var pathWrapper = new PathWrapper<Fileformat.SettingParameter.CheckpointSettings>(
                    settings, 
                    (cs) => cs.initializeFrom, 
                    (cs, path) => cs.initializeFrom = path
                );
                DrawFilePanelProperty(
                    ref settings.isUseInitializeFrom, property, pathWrapper, "Select init file", "pt"
               );
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
    }

}
#endif