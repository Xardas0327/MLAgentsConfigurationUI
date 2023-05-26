#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [CustomEditor(typeof(MLAgentsConfigFile))]
    public class MLAgentsConfigFileInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);
            if (GUILayout.Button("Save Into File"))
                Save();
        }

        private void Save()
        {
            var config = (MLAgentsConfigFile)target;

            YamlFile.SaveObjectToFile(config.ToYaml(), config.YamlFolderPath);
            Debug.Log("File is saved: " + config.YamlFolderPath);
        }
    }
}
#endif