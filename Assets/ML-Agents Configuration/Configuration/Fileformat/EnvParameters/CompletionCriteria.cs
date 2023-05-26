using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public enum CompletionCriteriaMeasure { progress, reward, Elo}


    [Serializable]
    public class CompletionCriteria
    {
        //This is temporary only
        public bool isUse = false;

        public CompletionCriteriaMeasure measure;
        public string behavior;
        public bool signalSmoothing;
        public uint minLessonLength;
        public float threshold;

        public CompletionCriteria() { }

        public CompletionCriteria(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.completionCriteriaText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.completionCriteriaText} is not right.");

            isUse = true;

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string lowerValue = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.measureText:
                            if (lowerValue == "progress")
                                measure = CompletionCriteriaMeasure.progress;
                            else if (lowerValue == "reward")
                                measure = CompletionCriteriaMeasure.reward;
                            else if (lowerValue == "Elo")
                                measure = CompletionCriteriaMeasure.Elo;
                            break;
                        case ConfigText.behaviorText:
                            behavior = yamlValue.value;
                            break;
                        case ConfigText.signalSmoothingText:
                            if (lowerValue == "true")
                                signalSmoothing = true;
                            break;
                        case ConfigText.minLessonLengthText:
                            UInt32.TryParse(lowerValue, out minLessonLength);
                            break;
                        case ConfigText.thresholdText:
                            float.TryParse(lowerValue, NumberStyles.Any, CultureInfo.InvariantCulture, out threshold);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.completionCriteriaText;

            yaml.elements.Add(new YamlValue(ConfigText.measureText, measure));
            yaml.elements.Add(new YamlValue(ConfigText.behaviorText, behavior));
            yaml.elements.Add(new YamlValue(ConfigText.signalSmoothingText, signalSmoothing));
            yaml.elements.Add(new YamlValue(ConfigText.minLessonLengthText, minLessonLength));
            yaml.elements.Add(new YamlValue(ConfigText.thresholdText, threshold));

            return yaml;
        }
    }
}