#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Yaml;
using System.Linq;

namespace Xardas.MLAgents.Configuration.Fileformat.Inspector
{
    [CustomEditor(typeof(MLAgentsConfigFile))]
    public class MLAgentsConfigFileInspector : Editor
    {
        private bool showHyperparameters;

        public override void OnInspectorGUI()
        {
            DrawIspector();

            GUILayout.Space(20);
            if (GUILayout.Button("Save Into File"))
                Save();
        }

        private void Save()
        {
            var configFile = (MLAgentsConfigFile)target;

            YamlFile.SaveObjectToFile(configFile.ToYaml(), configFile.YamlFolderPath);
            Debug.Log("File is saved: " + configFile.YamlFolderPath);
        }

        void DrawIspector()
        {
            var configFile = (MLAgentsConfigFile)target;

            using (new LocalizationGroup(target))
            {
                EditorGUI.BeginChangeCheck();
                serializedObject.UpdateIfRequiredOrScript();
                SerializedProperty iterator = serializedObject.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        if (iterator.type == typeof(Hyperparameters).Name)
                            DrawHyperparameters(iterator, configFile.trainerType);
                        else
                            EditorGUILayout.PropertyField(iterator, true);
                    }

                    enterChildren = false;
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }

        void DrawHyperparameters(SerializedProperty iterator, TrainerType trainerType)
        {
            //Draw the Hyperparameters
            showHyperparameters = EditorGUILayout.Foldout(showHyperparameters, iterator.displayName);

            if(showHyperparameters)
            {
                int numberOfFields = typeof(Hyperparameters).GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                while (iterator.NextVisible(true))
                {
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        bool isPpoAndPocaSpecific = Hyperparameters.OnlyPpoAndPocaFields.Contains(iterator.name);
                        bool isSacSpecific = Hyperparameters.OnlySacFields.Contains(iterator.name);

                        //PPO OR POCA
                        if (((trainerType == TrainerType.ppo || trainerType == TrainerType.poca)
                            && isPpoAndPocaSpecific)
                            // SAC
                            || (trainerType == TrainerType.sac && isSacSpecific)
                            //Not specific field, so it should be render always
                            || (!isPpoAndPocaSpecific && !isSacSpecific))
                        {
                            EditorGUILayout.PropertyField(iterator, true);
                        }

                    }
                    ++index;
                    if (index >= numberOfFields)
                        break;
                }
                GUILayout.Space(10);
            }
        }
    }
}
#endif