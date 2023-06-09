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
        private const float depthSize = 15f;

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
                        if (iterator.type == typeof(Behavior).Name)
                        {
                            DrawObject(iterator, typeof(Behavior), configFile.behavior, DrawBehaviorProperties);
                        }
                        else
                            DrawProperty(iterator);
                    }

                    enterChildren = false;
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }

        void DrawProperty(SerializedProperty property)
        {
            if (property.depth == 0)
            {
                EditorGUILayout.PropertyField(property, true);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * property.depth));
                EditorGUILayout.PropertyField(property, true);
                EditorGUILayout.EndHorizontal();
            }
        }

        bool DrawFoldout(SerializedProperty property)
        {
            if (!showPropertiesOfObject.ContainsKey(property.propertyPath))
            {
                showPropertiesOfObject.Add(property.propertyPath, false);
            }

            if(property.depth == 0)
            {
                showPropertiesOfObject[property.propertyPath] =
                    EditorGUILayout.Foldout(showPropertiesOfObject[property.propertyPath], property.displayName);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * property.depth));

                showPropertiesOfObject[property.propertyPath] =
                    EditorGUILayout.Foldout(showPropertiesOfObject[property.propertyPath], property.displayName);
                EditorGUILayout.EndHorizontal();
            }

            return showPropertiesOfObject[property.propertyPath];
        }

        void DrawObject<T>(
            SerializedProperty property,
            Type objectType,
            T objectForDraw,
            Action<SerializedProperty, T> drawPropertyFunction)
        {
            if (DrawFoldout(property))
            {
                int numberOfFields = objectType.GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                bool enterChildren = true;
                while (property.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    using (new EditorGUI.DisabledScope("m_Script" == property.propertyPath))
                    {
                        drawPropertyFunction(property, objectForDraw);
                    }
                    ++index;
                    if (index >= numberOfFields)
                        break;
                }
            }
        }

        void DrawBehaviorProperties(SerializedProperty property, Behavior behavior)
        {
            if (property.type == typeof(Hyperparameters).Name)
            {
                DrawObject(property, typeof(Hyperparameters), behavior, DrawHyperparametersProperties);
            }
            else if (property.type == typeof(NetworkSettings).Name)
            {
                DrawObject(property, typeof(NetworkSettings), behavior.networkSettings, DrawNetworkSettingsProperties);
            }
            else if (property.type == typeof(RewardSignals).Name)
            {
                DrawObject(property, typeof(RewardSignals), behavior, DrawRewardSignalsProperties);
            }
            else if (property.type == typeof(BehavioralCloning).Name)
            {
                if (behavior.isUseBehavioralCloning)
                    DrawProperty(property);
            }
            else if (property.type == typeof(SelfPlay).Name)
            {
                if (behavior.isUseSelfPlay)
                    DrawProperty(property);
            }
            else
                DrawProperty(property);
        }

        void DrawHyperparametersProperties(SerializedProperty property, Behavior behavior)
        {
            bool isPpoAndPocaSpecific = Hyperparameters.OnlyPpoAndPocaFields.Contains(property.name);
            bool isSacSpecific = Hyperparameters.OnlySacFields.Contains(property.name);

            //PPO OR POCA
            if (((behavior.trainerType == TrainerType.ppo || behavior.trainerType == TrainerType.poca)
                && isPpoAndPocaSpecific)
                // SAC
                || (behavior.trainerType == TrainerType.sac && isSacSpecific)
                //Not specific field, so it should be render always
                || (!isPpoAndPocaSpecific && !isSacSpecific))
            {
                DrawProperty(property);
            }
        }

        void DrawNetworkSettingsProperties(SerializedProperty property, NetworkSettings networkSettings)
        {
            if (property.name != nameof(networkSettings.memory)
                            || networkSettings.isUseMemory)
            {
                DrawProperty(property);
            }
        }

        void DrawRewardSignalsProperties(SerializedProperty property, Behavior behavior)
        {
            //Extrinsic
            if(property.name == nameof(behavior.rewardSignals.extrinsic))
            {
                if (behavior.rewardSignals.isUseExtrinsic)
                    DrawObject(property, typeof(ExtrinsicReward), (NetworkSettings)null,  DrawRewardProperties);
            }
            //Curiosity
            else if (property.name == nameof(behavior.rewardSignals.curiosity))
            {
                if(behavior.rewardSignals.isUseCuriosity)
                    DrawObject(
                        property,
                        typeof(CuriosityIntrinsicReward),
                        behavior.rewardSignals.curiosity.networkSettings,
                        DrawRewardProperties);
            }
            //Gail
            else if (property.name == nameof(behavior.rewardSignals.gail))
            {
                if(behavior.rewardSignals.isUseGail)
                    DrawObject(
                        property, 
                        typeof(GailIntrinsicReward),
                        behavior.rewardSignals.gail.networkSettings,
                        DrawRewardProperties);
            }
            //Rnd
            else if (property.name == nameof(behavior.rewardSignals.rnd))
            {
                if(behavior.rewardSignals.isUseRnd)
                    DrawObject(
                        property, 
                        typeof(RndIntrinsicReward),
                        behavior.rewardSignals.rnd.networkSettings,
                        DrawRewardProperties);
            }
            else
            {
                DrawProperty(property);
            }
        }

        void DrawRewardProperties(SerializedProperty property, NetworkSettings networkSettings)
        {
            if (property.type == typeof(NetworkSettings).Name)
                DrawObject(property, typeof(NetworkSettings), networkSettings, DrawNetworkSettingsProperties);
            else
                DrawProperty(property);
        }
    }
}
#endif