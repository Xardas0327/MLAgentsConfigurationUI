#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat.SettingParameter;

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
                DrawEnvPathProperty(ref settings.isUseEnvPath, property, settings);
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
        protected void DrawEnvPathProperty(ref bool active, SerializedProperty property, IEnvPathObject envPathObject)
        {
            EditorGUILayout.BeginHorizontal();
            active = EditorGUILayout.Toggle(active, GUILayout.MaxWidth(15));
            EditorGUI.BeginDisabledGroup(!active);
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFolderPanel("Select a build", Application.dataPath, "");

                    if (newPath != envPathObject.EnvPath && !string.IsNullOrEmpty(newPath))
                        envPathObject.EnvPath = newPath;
                };
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
