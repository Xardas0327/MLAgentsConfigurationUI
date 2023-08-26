using System;
using System.Globalization;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.BehaviorParameter
{
    [Serializable]
    public class SelfPlay
    {
        [Tooltip(ConfigTooltip.saveSteps)]
        [Min(1)]
        public uint saveSteps = 20000;
        [Tooltip(ConfigTooltip.overwriteTeamChange)]
        public bool overwriteTeamChange = false;
        [Tooltip(ConfigTooltip.teamChange)]
        [Min(1)]
        public uint teamChange; //default = 5 * save_steps
        [Tooltip(ConfigTooltip.swapSteps)]
        public uint swapSteps = 10000;
        [Tooltip(ConfigTooltip.playAgainstLatestModelRatio)]
        [Min(0)]
        public float playAgainstLatestModelRatio = 0.5f;
        [Tooltip(ConfigTooltip.window)]
        public uint window = 10;
        public float initialElo = 1200f;

        uint DefaultTeamChange { get { return 5 * saveSteps; } }

        public SelfPlay() 
        {
            teamChange = DefaultTeamChange;
        }

        public SelfPlay(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.selfPlay || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.selfPlay} is not right.");

            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.saveSteps:
                            UInt32.TryParse(value, out saveSteps);
                            break;
                        case ConfigText.teamChange:
                            UInt32.TryParse(value, out teamChange);
                            overwriteTeamChange = true;
                            break;
                        case ConfigText.swapSteps:
                            UInt32.TryParse(value, out swapSteps);
                            break;
                        case ConfigText.playAgainstLatestModelRatio:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out playAgainstLatestModelRatio);
                            break;
                        case ConfigText.window:
                            UInt32.TryParse(value, out window);
                            break;
                        case ConfigText.initialElo:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out initialElo);
                            break;
                    }
                }
            }

            if (!overwriteTeamChange)
                teamChange = DefaultTeamChange;
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.selfPlay;

            yaml.elements.Add(new YamlValue(ConfigText.saveSteps, saveSteps));

            if(overwriteTeamChange)
                yaml.elements.Add(new YamlValue(ConfigText.teamChange, teamChange));
            else
                yaml.elements.Add(new YamlValue(ConfigText.teamChange, DefaultTeamChange));

            yaml.elements.Add(new YamlValue(ConfigText.swapSteps, swapSteps));
            yaml.elements.Add(new YamlValue(ConfigText.playAgainstLatestModelRatio, playAgainstLatestModelRatio));
            yaml.elements.Add(new YamlValue(ConfigText.window, window));
            yaml.elements.Add(new YamlValue(ConfigText.initialElo, initialElo));

            return yaml;
        }

        public bool IsValid()
        {
            var teamChangeNumber = overwriteTeamChange ? teamChange : DefaultTeamChange;

            if (teamChangeNumber % saveSteps != 0)
                Debug.LogWarning($"The save steps should be multiple times than team change.");

            return true;
        }
    }
}