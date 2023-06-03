using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [Serializable]
    public class SelfPlay
    {
        public int saveSteps = 20000;
        public int teamChange;
        public int swapSteps = 10000;
        public float playAgainstLatestModelRatio = 0.5f;
        public int window = 10;
        public float initialElo = 1200f;

        int DefaultTeamChange { get { return 5 * saveSteps; } }

        public SelfPlay() 
        {
            teamChange = DefaultTeamChange;
        }

        public SelfPlay(YamlObject yaml)
        {
            if (yaml == null || yaml.name != ConfigText.selfPlay || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.selfPlay} is not right.");

            bool wasTeamChange = false;
            foreach (var element in yaml.elements)
            {
                if (element is YamlValue yamlValue)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.saveSteps:
                            Int32.TryParse(value, out saveSteps);
                            break;
                        case ConfigText.teamChange:
                            Int32.TryParse(value, out teamChange);
                            wasTeamChange = true;
                            break;
                        case ConfigText.swapSteps:
                            Int32.TryParse(value, out swapSteps);
                            break;
                        case ConfigText.playAgainstLatestModelRatio:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out playAgainstLatestModelRatio);
                            break;
                        case ConfigText.window:
                            Int32.TryParse(value, out window);
                            break;
                        case ConfigText.initialElo:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out initialElo);
                            break;
                    }
                }
            }

            if (!wasTeamChange)
                teamChange = DefaultTeamChange;
        }

        public YamlObject ToYaml()
        {
            var yaml = new YamlObject();
            yaml.name = ConfigText.selfPlay;

            yaml.elements.Add(new YamlValue(ConfigText.saveSteps, saveSteps));
            yaml.elements.Add(new YamlValue(ConfigText.teamChange, teamChange));
            yaml.elements.Add(new YamlValue(ConfigText.swapSteps, swapSteps));
            yaml.elements.Add(new YamlValue(ConfigText.playAgainstLatestModelRatio, playAgainstLatestModelRatio));
            yaml.elements.Add(new YamlValue(ConfigText.window, window));
            yaml.elements.Add(new YamlValue(ConfigText.initialElo, initialElo));

            return yaml;
        }
    }
}