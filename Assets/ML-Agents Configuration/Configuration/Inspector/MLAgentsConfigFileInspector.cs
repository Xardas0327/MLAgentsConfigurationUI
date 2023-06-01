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
                        if (iterator.type == typeof(Hyperparameters).Name)
                        {
                            DrawObject(iterator, typeof(Hyperparameters), configFile, DrawHyperparameters);
                        }
                        else if (iterator.type == typeof(NetworkSettings).Name)
                        {
                            DrawObject(iterator, typeof(NetworkSettings), configFile.networkSettings, DrawNetworkSettings);
                        }
                        else if (iterator.type == typeof(RewardSignals).Name)
                        {
                            DrawObject(iterator, typeof(RewardSignals), configFile, DrawRewardSignals);
                        }
                        else
                            DefaultDrawProperty(iterator);
                    }

                    enterChildren = false;
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }
        }

        void DefaultDrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true);
        }

        void DrawPropertyWithDetph(SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * property.depth));
            EditorGUILayout.PropertyField(property, true);
            EditorGUILayout.EndHorizontal();
        }

        bool DrawObjectWithDetph(SerializedProperty property)
        {
            if (!showPropertiesOfObject.ContainsKey(property.propertyPath))
            {
                showPropertiesOfObject.Add(property.propertyPath, false);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * property.depth));

            showPropertiesOfObject[property.propertyPath] =
                EditorGUILayout.Foldout(showPropertiesOfObject[property.propertyPath], property.displayName);
            EditorGUILayout.EndHorizontal();

            return showPropertiesOfObject[property.propertyPath];
        }

        void DrawObject<T>(
            SerializedProperty property,
            Type objectType,
            T objectForDraw,
            Action<SerializedProperty, T> drawFunction)
        {
            if (DrawObjectWithDetph(property))
            {
                int numberOfFields = objectType.GetFields().Where(x => !x.IsStatic).Count();
                int index = 0;
                bool enterChildren = true;
                while (property.NextVisible(enterChildren))
                {
                    enterChildren = false;
                    using (new EditorGUI.DisabledScope("m_Script" == property.propertyPath))
                    {
                        drawFunction(property, objectForDraw);
                    }
                    ++index;
                    if (index >= numberOfFields)
                        break;
                }
            }
        }

        void DrawHyperparameters(SerializedProperty property, MLAgentsConfigFile configFile)
        {
            bool isPpoAndPocaSpecific = Hyperparameters.OnlyPpoAndPocaFields.Contains(property.name);
            bool isSacSpecific = Hyperparameters.OnlySacFields.Contains(property.name);

            //PPO OR POCA
            if (((configFile.trainerType == TrainerType.ppo || configFile.trainerType == TrainerType.poca)
                && isPpoAndPocaSpecific)
                // SAC
                || (configFile.trainerType == TrainerType.sac && isSacSpecific)
                //Not specific field, so it should be render always
                || (!isPpoAndPocaSpecific && !isSacSpecific))
            {
                DrawPropertyWithDetph(property);
            }
        }

        void DrawNetworkSettings(SerializedProperty property, NetworkSettings networkSettings)
        {
            if (property.name != nameof(networkSettings.memory)
                            || networkSettings.isUseMemory)
            {
                DrawPropertyWithDetph(property);
            }
        }

        void DrawRewardSignals(SerializedProperty property, MLAgentsConfigFile configFile)
        {
            //Extrinsic
            if(property.name == nameof(configFile.rewardSignals.extrinsic))
            {
                if (configFile.rewardSignals.isUseExtrinsic)
                    DrawObject(property, typeof(ExtrinsicReward), (NetworkSettings)null,  DrawReward);
            }
            //Curiosity
            else if (property.name == nameof(configFile.rewardSignals.curiosity))
            {
                if(configFile.rewardSignals.isUseCuriosity)
                    DrawObject(
                        property,
                        typeof(CuriosityIntrinsicReward),
                        configFile.rewardSignals.curiosity.networkSettings,
                        DrawReward);
            }
            //Gail
            else if (property.name == nameof(configFile.rewardSignals.gail))
            {
                if(configFile.rewardSignals.isUseGail)
                    DrawObject(
                        property, 
                        typeof(GailIntrinsicReward),
                        configFile.rewardSignals.gail.networkSettings,
                        DrawReward);
            }
            //Rnd
            else if (property.name == nameof(configFile.rewardSignals.rnd))
            {
                if(configFile.rewardSignals.isUseRnd)
                    DrawObject(
                        property, 
                        typeof(RndIntrinsicReward),
                        configFile.rewardSignals.rnd.networkSettings,
                        DrawReward);
            }
            else
            {
                DrawPropertyWithDetph(property);
            }
        }

        void DrawReward(SerializedProperty property, NetworkSettings networkSettings)
        {
            if (property.type == typeof(NetworkSettings).Name)
                DrawObject(property, typeof(NetworkSettings), networkSettings, DrawNetworkSettings);
            else
                DrawPropertyWithDetph(property);
        }
    }
}
#endif