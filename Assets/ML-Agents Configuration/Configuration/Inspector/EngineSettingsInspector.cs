#if UNITY_EDITOR
using UnityEditor;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(EngineSettings))]
    public class EngineSettingsInspector : SettingsInspector
    {
        protected override void DrawProperty(SerializedProperty property)
        {
            var settings = ((EngineSettings)target).settings;
            if (property.name == nameof(settings.width))
            {
                DrawPropertyWithTickBox(ref settings.isUseWidth, property);
            }
            else if (property.name == nameof(settings.height))
            {
                DrawPropertyWithTickBox(ref settings.isUseHeight, property);
            }
            else if (property.name == nameof(settings.qualityLevel))
            {
                DrawPropertyWithTickBox(ref settings.isUseQualityLevel, property);
            }
            else if (property.name == nameof(settings.timeScale))
            {
                DrawPropertyWithTickBox(ref settings.isUseTimeScale, property);
            }
            else if (property.name == nameof(settings.targetFrameRate))
            {
                DrawPropertyWithTickBox(ref settings.isUseTargetFrameRate, property);
            }
            else if (property.name == nameof(settings.captureFrameRate))
            {
                DrawPropertyWithTickBox(ref settings.isUseCaptureFrameRate, property);
            }
            else if (property.name == nameof(settings.noGraphics))
            {
                DrawPropertyWithTickBox(ref settings.isUseNoGraphics, property);
            }
        }
    }
}
#endif