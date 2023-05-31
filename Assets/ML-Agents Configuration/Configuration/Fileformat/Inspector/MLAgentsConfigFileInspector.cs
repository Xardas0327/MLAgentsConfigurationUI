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
        private static bool showHyperparameters;
        private static bool showNetworkSettings;
        private static bool showRewardSignals;

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
                            DrawHyperparameters(iterator, configFile);
                        else if (iterator.type == typeof(NetworkSettings).Name)
                            DrawNetworkSettings(iterator, configFile);
                        else if (iterator.type == typeof(RewardSignals).Name)
                            DrawRewardSignals(iterator, configFile);
                        else
                            EditorGUILayout.PropertyField(iterator, true);
                    }

                    enterChildren = false;
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }

        void DrawHyperparameters(SerializedProperty iterator, MLAgentsConfigFile configFile)
        {
            showHyperparameters = EditorGUILayout.Foldout(showHyperparameters, iterator.displayName);

            if(showHyperparameters)
            {
                int numberOfFields = typeof(Hyperparameters).GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        bool isPpoAndPocaSpecific = Hyperparameters.OnlyPpoAndPocaFields.Contains(iterator.name);
                        bool isSacSpecific = Hyperparameters.OnlySacFields.Contains(iterator.name);

                        //PPO OR POCA
                        if (((configFile.trainerType == TrainerType.ppo || configFile.trainerType == TrainerType.poca)
                            && isPpoAndPocaSpecific)
                            // SAC
                            || (configFile.trainerType == TrainerType.sac && isSacSpecific)
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

        void DrawNetworkSettings(SerializedProperty iterator, MLAgentsConfigFile configFile)
        {
            showNetworkSettings = EditorGUILayout.Foldout(showNetworkSettings, iterator.displayName);

            if (showNetworkSettings)
            {
                int numberOfFields = typeof(NetworkSettings).GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        if (iterator.name != nameof(configFile.networkSettings.memory)
                            || configFile.networkSettings.isUseMemory)
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

        void DrawRewardSignals(SerializedProperty iterator, MLAgentsConfigFile configFile)
        {
            showRewardSignals = EditorGUILayout.Foldout(showRewardSignals, iterator.displayName);

            if (showRewardSignals)
            {
                int numberOfFields = typeof(RewardSignals).GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        //extrinsic
                        if ((iterator.name == nameof(configFile.rewardSignals.extrinsic)
                            && configFile.rewardSignals.isUseExtrinsic)
                            //curiosity
                            || (iterator.name == nameof(configFile.rewardSignals.curiosity)
                            && configFile.rewardSignals.isUseCuriosity)
                            //gail
                            || (iterator.name == nameof(configFile.rewardSignals.gail)
                            && configFile.rewardSignals.isUseGail)
                            //rnd
                            || (iterator.name == nameof(configFile.rewardSignals.rnd)
                            && configFile.rewardSignals.isUseRnd)
                            || iterator.name.StartsWith("isUse"))
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