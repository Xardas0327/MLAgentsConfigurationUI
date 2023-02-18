using UnityEditor;

namespace Xardas.MLAgents.Configuration
{
    public class ConfigurationWindow : EditorWindow
    {
        [MenuItem("Window/ML-Agents/Configuration")]
        public static void ShowWindow()
        {
            GetWindow<ConfigurationWindow>("ML-Agents Configuration");
        }

        private void OnGUI()
        {
            
        }
    }
}
