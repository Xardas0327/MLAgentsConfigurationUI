#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(EnvironmentSettings))]
    public class EnvironmentSettingsInspector : SettingsInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);
            if (GUILayout.Button("Validation"))
            {
                ((EnvironmentSettings)target).IsValid();
            }
        }

        protected override void DrawProperty(SerializedProperty property)
        {
            var settings = ((EnvironmentSettings)target).settings;
            if (property.name == nameof(settings.envPath))
            {
                DrawPropertyWithTickBox(ref settings.isUseEnvPath, property);
            }
            else if (property.name == nameof(settings.envArgs))
            {
                DrawPropertyWithTickBox(ref settings.isUseEnvArgs, property);
            }
            else if (property.name == nameof(settings.basePort))
            {
                DrawPropertyWithTickBox(ref settings.isUseBasePort, property);
            }
            else if (property.name == nameof(settings.numEnvs))
            {
                DrawPropertyWithTickBox(ref settings.isUseNumEnvs, property);
            }
            else if (property.name == nameof(settings.seed))
            {
                DrawPropertyWithTickBox(ref settings.isUseSeed, property);
            }
            else if (property.name == nameof(settings.maxLifetimeRestarts))
            {
                DrawPropertyWithTickBox(ref settings.isUseMaxLifetimeRestarts, property);
            }
            else if (property.name == nameof(settings.restartsRateLimitN))
            {
                DrawPropertyWithTickBox(ref settings.isUseRestartsRateLimitN, property);
            }
            else if (property.name == nameof(settings.restartsRateLimitPeriodS))
            {
                DrawPropertyWithTickBox(ref settings.isUseRestartsRateLimitPeriodS, property);
            }
        }
    }
}
#endif
