using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.EnvParameters
{
    public enum CompletionCriteriaMeasure { progress, reward, Elo}


    [Serializable]
    public class CompletionCriteria
    {
        public CompletionCriteriaMeasure measure;
        public string behavior;
        public bool signalSmoothing;
        public uint minLessonLength;
        public float threshold;

        public CompletionCriteria() { }

        public CompletionCriteria(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.completionCriteria || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.completionCriteria} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string lowerValue = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.measure:
                            if (lowerValue == "progress")
                                measure = CompletionCriteriaMeasure.progress;
                            else if (lowerValue == "reward")
                                measure = CompletionCriteriaMeasure.reward;
                            else if (lowerValue == "Elo")
                                measure = CompletionCriteriaMeasure.Elo;
                            break;
                        case ConfigText.behavior:
                            behavior = yamlValue.value;
                            break;
                        case ConfigText.signalSmoothing:
                            if (lowerValue == "true")
                                signalSmoothing = true;
                            break;
                        case ConfigText.minLessonLength:
                            UInt32.TryParse(lowerValue, out minLessonLength);
                            break;
                        case ConfigText.threshold:
                            float.TryParse(lowerValue, NumberStyles.Any, CultureInfo.InvariantCulture, out threshold);
                            break;
                    }
                }
            }
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.completionCriteria;

            yaml.elements.Add(new YamlValue(ConfigText.measure, measure));
            yaml.elements.Add(new YamlValue(ConfigText.behavior, behavior));
            yaml.elements.Add(new YamlValue(ConfigText.signalSmoothing, signalSmoothing));
            yaml.elements.Add(new YamlValue(ConfigText.minLessonLength, minLessonLength));
            yaml.elements.Add(new YamlValue(ConfigText.threshold, threshold));

            return yaml;
        }
    }
}