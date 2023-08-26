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
                DrawFieldWithTickBox(ref settings.isUseWidth, property);
            }
            else if (property.name == nameof(settings.height))
            {
                DrawFieldWithTickBox(ref settings.isUseHeight, property);
            }
            else if (property.name == nameof(settings.qualityLevel))
            {
                DrawFieldWithTickBox(ref settings.isUseQualityLevel, property);
            }
            else if (property.name == nameof(settings.timeScale))
            {
                DrawFieldWithTickBox(ref settings.isUseTimeScale, property);
            }
            else if (property.name == nameof(settings.targetFrameRate))
            {
                DrawFieldWithTickBox(ref settings.isUseTargetFrameRate, property);
            }
            else if (property.name == nameof(settings.captureFrameRate))
            {
                DrawFieldWithTickBox(ref settings.isUseCaptureFrameRate, property);
            }
            else if (property.name == nameof(settings.noGraphics))
            {
                DrawFieldWithTickBox(ref settings.isUseNoGraphics, property);
            }
        }
    }
}
#endif