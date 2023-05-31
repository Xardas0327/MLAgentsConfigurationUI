#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Configuration.Fileformat.Reward;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(MLAgentsConfigFile))]
    public class MLAgentsConfigFileInspector : Editor
    {
        private static Dictionary<string, bool> showPropertiesOfObject = new();

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
                            DrawObject(iterator, typeof(Hyperparameters), configFile,  DrawHyperparameters);
                        else if (iterator.type == typeof(NetworkSettings).Name)
                            DrawObject(iterator, typeof(NetworkSettings), configFile.networkSettings, DrawNetworkSettings);
                        else if (iterator.type == typeof(RewardSignals).Name)
                            DrawObject(iterator, typeof(RewardSignals), configFile, DrawRewardSignals);
                        else
                            EditorGUILayout.PropertyField(iterator, true);
                    }

                    enterChildren = false;
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }

        void DrawObject<T>(
            SerializedProperty iterator,
            Type objectType,
            T objectForDraw,
            Action<SerializedProperty, T> drawFunction)
        {
            if(!showPropertiesOfObject.ContainsKey(iterator.propertyPath))
            {
                showPropertiesOfObject.Add(iterator.propertyPath, false);
            }

            showPropertiesOfObject[iterator.propertyPath] = 
                EditorGUILayout.Foldout(showPropertiesOfObject[iterator.propertyPath], iterator.displayName);

            if (showPropertiesOfObject[iterator.propertyPath])
            {
                int numberOfFields = objectType.GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    {
                        drawFunction(iterator, objectForDraw);
                    }
                    ++index;
                    if (index >= numberOfFields)
                        break;
                }
                GUILayout.Space(10);
            }
        }

        void DrawHyperparameters(SerializedProperty iterator, MLAgentsConfigFile configFile)
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

        void DrawNetworkSettings(SerializedProperty iterator, NetworkSettings networkSettings)
        {
            if (iterator.name != nameof(networkSettings.memory)
                            || networkSettings.isUseMemory)
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        void DrawRewardSignals(SerializedProperty iterator, MLAgentsConfigFile configFile)
        {
            if(iterator.name.StartsWith("isUse"))
            {
                EditorGUILayout.PropertyField(iterator, true);
            }
            //Extrinsic
            else if(iterator.name == nameof(configFile.rewardSignals.extrinsic) 
                && configFile.rewardSignals.isUseExtrinsic)
            {
                DrawObject(iterator, typeof(ExtrinsicReward), (NetworkSettings)null,  DrawReward);
            }
            //Curiosity
            else if (iterator.name == nameof(configFile.rewardSignals.curiosity) 
                && configFile.rewardSignals.isUseCuriosity)
            {
                DrawObject(
                    iterator, 
                    typeof(CuriosityIntrinsicReward),
                    configFile.rewardSignals.curiosity.networkSettings, 
                    DrawReward);
            }
            //Gail
            else if (iterator.name == nameof(configFile.rewardSignals.gail) 
                && configFile.rewardSignals.isUseGail)
            {
                DrawObject(
                    iterator, 
                    typeof(GailIntrinsicReward),
                    configFile.rewardSignals.gail.networkSettings,
                    DrawReward);
            }
            //Rnd
            else if (iterator.name == nameof(configFile.rewardSignals.rnd) 
                && configFile.rewardSignals.isUseRnd)
            {
                DrawObject(
                    iterator, 
                    typeof(RndIntrinsicReward),
                    configFile.rewardSignals.rnd.networkSettings,
                    DrawReward);
            }
        }

        void DrawReward(SerializedProperty iterator, NetworkSettings networkSettings)
        {
            if (iterator.type == typeof(NetworkSettings).Name)
                DrawObject(iterator, typeof(NetworkSettings), networkSettings, DrawNetworkSettings);
            else
                EditorGUILayout.PropertyField(iterator, true);
        }
    }
}
#endif