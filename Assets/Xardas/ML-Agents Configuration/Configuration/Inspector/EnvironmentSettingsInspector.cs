#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(Fileformat.EnvironmentSettings))]
    public class EnvironmentSettingsInspector : SettingsInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);
            if (GUILayout.Button("Validation"))
            {
                ((Fileformat.EnvironmentSettings)target).IsValid();
            }
        }

        protected override void DrawProperty(SerializedProperty property)
        {
            var settings = ((Fileformat.EnvironmentSettings)target).settings;
            if (property.name == nameof(settings.envPath))
            {
                var pathWrapper = new PathWrapper<Fileformat.SettingParameter.EnvironmentSettings>(
                    settings, 
                    (s) => s.envPath, 
                    (s, path) => s.envPath = path
                );
                DrawFolderPanelProperty(
                    ref settings.isUseEnvPath,
                    new GUIContent(property.displayName, property.tooltip), 
                    pathWrapper, 
                    "Select a build folder"
               );
            }
            else if (property.name == nameof(settings.envArgs))
            {
                DrawFieldWithTickBox(ref settings.isUseEnvArgs, property);
            }
            else if (property.name == nameof(settings.basePort))
            {
                DrawFieldWithTickBox(ref settings.isUseBasePort, property);
            }
            else if (property.name == nameof(settings.numEnvs))
            {
                DrawFieldWithTickBox(ref settings.isUseNumEnvs, property);
            }
            else if (property.name == nameof(settings.timeoutWait))
            {
                DrawFieldWithTickBox(ref settings.isUseTimeoutWait, property);
            }
            else if (property.name == nameof(settings.seed))
            {
                DrawFieldWithTickBox(ref settings.isUseSeed, property);
            }
            else if (property.name == nameof(settings.maxLifetimeRestarts))
            {
                DrawFieldWithTickBox(ref settings.isUseMaxLifetimeRestarts, property);
            }
            else if (property.name == nameof(settings.restartsRateLimitN))
            {
                DrawFieldWithTickBox(ref settings.isUseRestartsRateLimitN, property);
            }
            else if (property.name == nameof(settings.restartsRateLimitPeriodS))
            {
                DrawFieldWithTickBox(ref settings.isUseRestartsRateLimitPeriodS, property);
            }
        }
    }
}
#endif
