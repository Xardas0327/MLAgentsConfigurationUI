#if UNITY_EDITOR
using UnityEditor;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(TorchSettings))]
    public class TorchSettingsInspector : SettingsInspector
    {
        protected override void DrawProperty(SerializedProperty property)
        {
            var settings = (TorchSettings)target;
            if (property.name == nameof(settings.settings.device))
            {
                DrawFieldWithTickBox(ref settings.settings.isUseDevice, property);
            }
        }
    }
}
#endif