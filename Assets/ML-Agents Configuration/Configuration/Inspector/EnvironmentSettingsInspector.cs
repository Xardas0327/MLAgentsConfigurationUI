#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
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
                var pathWrapper = new PathWrapper<Fileformat.SettingParameter.EnvironmentSettings>(
                    settings, 
                    (s) => s.envPath, 
                    (s, path) => s.envPath = path
                );
                DrawFolderPanelProperty(ref settings.isUseEnvPath, property, pathWrapper, "Select a build");
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
