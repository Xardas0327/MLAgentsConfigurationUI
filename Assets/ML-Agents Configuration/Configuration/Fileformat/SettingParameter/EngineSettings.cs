using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.SettingParameter
{
    [Serializable]
    public class EngineSettings : ISettings
    {
        public bool isUseWidth;
        [Tooltip(ConfigTooltip.width)]
        [Min(1)]
        public uint width = 84;

        public bool isUseHeight;
        [Tooltip(ConfigTooltip.height)]
        [Min(1)]
        public uint height = 84;

        public bool isUseQualityLevel;
        [Tooltip(ConfigTooltip.qualityLevel)]
        [Min(1)]
        public uint qualityLevel = 5;

        public bool isUseTimeScale;
        [Tooltip(ConfigTooltip.timeScale)]
        public uint timeScale = 20;

        public bool isUseTargetFrameRate;
        [Tooltip(ConfigTooltip.targetFrameRate)]
        [Min(-1)]
        public int targetFrameRate = -1;

        public bool isUseCaptureFrameRate;
        [Tooltip(ConfigTooltip.captureFrameRate)]
        [Min(1)]
        public uint captureFrameRate = 60;

        public bool isUseNoGraphics;
        [Tooltip(ConfigTooltip.noGraphics)]
        public bool noGraphics = false;

        public bool IsUse => 
                isUseWidth || isUseHeight
            || isUseQualityLevel || isUseTimeScale
            || isUseTargetFrameRate || isUseCaptureFrameRate
            || isUseNoGraphics;

        public EngineSettings() { }

        public EngineSettings(YamlObject yaml) 
        {
            if (yaml == null || yaml.name != ConfigText.engineSettings || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.engineSettings} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.width:
                            UInt32.TryParse(value, out width);
                            isUseWidth = true;
                            break;
                        case ConfigText.height:
                            UInt32.TryParse(value, out height);
                            isUseHeight = true;
                            break;
                        case ConfigText.qualityLevel:
                            UInt32.TryParse(value, out qualityLevel);
                            isUseQualityLevel = true;
                            break;
                        case ConfigText.timeScale:
                            UInt32.TryParse(value, out timeScale);
                            isUseTimeScale = true;
                            break;
                        case ConfigText.targetFrameRate:
                            Int32.TryParse(value, out targetFrameRate);
                            isUseTargetFrameRate = true;
                            break;
                        case ConfigText.captureFrameRate:
                            UInt32.TryParse(value, out captureFrameRate);
                            isUseCaptureFrameRate = true;
                            break;
                        case ConfigText.noGraphics:
                            if (value == "true")
                                noGraphics = true;

                            isUseNoGraphics = true;
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.engineSettings;

            if (isUseWidth)
                yaml.elements.Add(new YamlValue(ConfigText.width, width));

            if (isUseHeight)
                yaml.elements.Add(new YamlValue(ConfigText.height, height));

            if (isUseQualityLevel)
                yaml.elements.Add(new YamlValue(ConfigText.qualityLevel, qualityLevel));

            if (isUseTimeScale)
                yaml.elements.Add(new YamlValue(ConfigText.timeScale, timeScale));

            if (isUseTargetFrameRate)
                yaml.elements.Add(new YamlValue(ConfigText.targetFrameRate, targetFrameRate));

            if (isUseCaptureFrameRate)
                yaml.elements.Add(new YamlValue(ConfigText.captureFrameRate, captureFrameRate));

            if (isUseNoGraphics)
                yaml.elements.Add(new YamlValue(ConfigText.noGraphics, noGraphics));

            return yaml;
        }
    }
}