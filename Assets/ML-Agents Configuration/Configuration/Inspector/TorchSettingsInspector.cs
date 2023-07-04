#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(TorchSettings))]
    public class TorchSettingsInspector : SettingsInspector
    {
        protected override void DrawProperty(SerializedProperty property)
        {
            TorchSettings settings = (TorchSettings)target;
            if (property.name == nameof(settings.torchSettings.device))
            {
                DrawPropertyWithTickBox(ref settings.torchSettings.isUseDevice, property);
            }
        }
    }
}
#endif