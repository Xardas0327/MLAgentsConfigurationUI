#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine;
using Xardas.MLAgents.Configuration.Fileformat;
using Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter;
using Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter.Reward;

namespace Xardas.MLAgents.Configuration.Inspector
{
    [CustomEditor(typeof(Behavior))]
    public class BehaviorInspector : Editor
    {
        private static Dictionary<string, bool> showPropertiesOfObject = new();
        private const float depthSize = 15f;

        public override void OnInspectorGUI()
        {
            var behavior = (Behavior)target;
            DrawInspector(behavior);

            GUILayout.Space(20);
            if (GUILayout.Button("Validation"))
            {
                behavior.IsValid();
            }
        }

        void DrawInspector(Behavior behavior)
        {
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
                            DrawObject(iterator, typeof(Hyperparameters), behavior, DrawHyperparametersProperties);
                        }
                        else if (iterator.type == typeof(NetworkSettings).Name)
                        {
                            DrawObject(iterator, typeof(NetworkSettings), behavior.networkSettings, DrawNetworkSettingsProperties);
                        }
                        else if (iterator.type == typeof(RewardSignals).Name)
                        {
                            DrawObject(iterator, typeof(RewardSignals), behavior, DrawRewardSignalsProperties);
                        }
                        else if (iterator.type == typeof(BehavioralCloning).Name)
                        {
                            if (behavior.isUseBehavioralCloning)
                                DrawObject(iterator, typeof(BehavioralCloning), behavior.behavioralCloning, DrawBehavioralCloningProperties);
                        }
                        else if (iterator.type == typeof(SelfPlay).Name)
                        {
                            if (behavior.isUseSelfPlay)
                                DrawObject(iterator, typeof(SelfPlay), behavior.selfPlay, DrawSelfPlayProperties);
                        }
                        else if (iterator.name == nameof(behavior.initPath))
                        {
                            DrawInitPathProperty(iterator, behavior);
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

        void DrawDemoPathProperty(SerializedProperty property, IDemoPathObject demoPathObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * property.depth));
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFilePanel("Select demo file", Application.dataPath, "demo");

                    if (newPath != demoPathObject.DemoPath && !string.IsNullOrEmpty(newPath))
                        demoPathObject.DemoPath = newPath;
                };
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawInitPathProperty(SerializedProperty property, IInitPathObject initPathObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(depthSize * property.depth));
            EditorGUILayout.PropertyField(property, true);
            if (GUILayout.Button("Browse", GUILayout.MaxWidth(100)))
            {
                EditorApplication.delayCall += () =>
                {
                    string newPath = EditorUtility.OpenFilePanel("Select init file", Application.dataPath, "pt");

                    if (newPath != initPathObject.InitPath && !string.IsNullOrEmpty(newPath))
                        initPathObject.InitPath = newPath;
                };
            }
            EditorGUILayout.EndHorizontal();
        }

        bool DrawFoldout(SerializedProperty property)
        {
            if (!showPropertiesOfObject.ContainsKey(property.propertyPath))
            {
                showPropertiesOfObject.Add(property.propertyPath, false);
            }

            if (property.depth == 0)
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

        void DrawHyperparametersProperties(SerializedProperty property, Behavior behavior)
        {
            bool isPpoAndPocaSpecific = Hyperparameters.OnlyPpoAndPocaFields.Contains(property.name);
            bool isSacSpecific = Hyperparameters.OnlySacFields.Contains(property.name);

            //Not specific field, so it should be render always
            if (!isPpoAndPocaSpecific && !isSacSpecific)
            {
                if (property.name == nameof(behavior.hyperparameters.bufferSize))
                {
                    if (behavior.hyperparameters.overwriteBufferSize)
                        DrawProperty(property);
                }
                else if(property.name == nameof(behavior.hyperparameters.learningRateSchedule))
                {
                    if (behavior.hyperparameters.overwriteLearningRateSchedule)
                        DrawProperty(property);
                }
                else
                    DrawProperty(property);
            }
            //PPO OR POCA
            else if ((behavior.trainerType == TrainerType.ppo || behavior.trainerType == TrainerType.poca) && isPpoAndPocaSpecific)
            {
                if (property.name == nameof(behavior.hyperparameters.betaSchedule))
                {
                    if (behavior.hyperparameters.overwriteBetaSchedule)
                        DrawProperty(property);
                }
                else if (property.name == nameof(behavior.hyperparameters.epsilonSchedule))
                {
                    if (behavior.hyperparameters.overwriteEpsilonSchedule)
                        DrawProperty(property);
                }
                else
                    DrawProperty(property);
            }
            // SAC
            else if (behavior.trainerType == TrainerType.sac && isSacSpecific)
            {
                if (property.name == nameof(behavior.hyperparameters.rewardSignalNumUpdate))
                {
                    if (behavior.hyperparameters.overwriteRewardSignalNumUpdate)
                        DrawProperty(property);
                }
                else
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
            if (property.name == nameof(behavior.rewardSignals.extrinsic))
            {
                if (behavior.rewardSignals.isUseExtrinsic)
                    DrawObject(property, typeof(ExtrinsicReward), (NetworkSettings)null, DrawRewardProperties);
            }
            //Curiosity
            else if (property.name == nameof(behavior.rewardSignals.curiosity))
            {
                if (behavior.rewardSignals.isUseCuriosity)
                    DrawObject(
                        property,
                        typeof(CuriosityIntrinsicReward),
                        behavior.rewardSignals.curiosity.networkSettings,
                        DrawRewardProperties);
            }
            //Gail
            else if (property.name == nameof(behavior.rewardSignals.gail))
            {
                if (behavior.rewardSignals.isUseGail)
                    DrawObject(
                        property,
                        typeof(GailIntrinsicReward),
                        behavior.rewardSignals.gail,
                        DrawRewardGailProperties);
            }
            //Rnd
            else if (property.name == nameof(behavior.rewardSignals.rnd))
            {
                if (behavior.rewardSignals.isUseRnd)
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

        void DrawRewardGailProperties(SerializedProperty property, GailIntrinsicReward gail)
        {
            if (property.type == typeof(NetworkSettings).Name)
            {
                DrawObject(property, typeof(NetworkSettings), gail.networkSettings, DrawNetworkSettingsProperties);
            }
            else if (property.name == nameof(gail.demoPath))
            {
                DrawDemoPathProperty(property, gail);
            }
            else
                DrawProperty(property);
        }

        void DrawBehavioralCloningProperties(SerializedProperty property, BehavioralCloning behavioralCloning)
        {
            if (property.name == nameof(behavioralCloning.batchSize))
            {
                if (behavioralCloning.overwriteBatchSize)
                    DrawProperty(property);
            }
            else if (property.name == nameof(behavioralCloning.numEpoch))
            {
                if (behavioralCloning.overwriteNumEpoch)
                    DrawProperty(property);
            }
            else if (property.name == nameof(behavioralCloning.demoPath))
            {
                DrawDemoPathProperty(property, behavioralCloning);
            }
            else
                DrawProperty(property);
        }

        void DrawSelfPlayProperties(SerializedProperty property, SelfPlay selfPlay)
        {
            if (property.name != nameof(selfPlay.teamChange)
                            || selfPlay.overwriteTeamChange)
            {
                DrawProperty(property);
            }
        }
    }
}
#endif