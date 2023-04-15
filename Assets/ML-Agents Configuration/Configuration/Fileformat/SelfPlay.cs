using System;
using System.Globalization;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat
{
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
            if (yaml == null || yaml.name != ConfigText.selfPlayText || yaml.elements.Count < 1)
                throw new System.Exception($"The {ConfigText.selfPlayText} is not right.");

            bool wasTeamChange = false;
            foreach (var element in yaml.elements)
            {
                var yamlValue = element as YamlValue;
                if (yamlValue != null)
                {
                    string value = yamlValue.value.ToLower();
                    switch (yamlValue.name)
                    {
                        case ConfigText.saveStepsText:
                            Int32.TryParse(value, out saveSteps);
                            break;
                        case ConfigText.teamChangeText:
                            Int32.TryParse(value, out teamChange);
                            wasTeamChange = true;
                            break;
                        case ConfigText.swapStepsText:
                            Int32.TryParse(value, out swapSteps);
                            break;
                        case ConfigText.playAgainstLatestModelRatioText:
                            float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out playAgainstLatestModelRatio);
                            break;
                        case ConfigText.windowText:
                            Int32.TryParse(value, out window);
                            break;
                        case ConfigText.initialEloText:
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
            yaml.name = ConfigText.selfPlayText;


            return yaml;
        }
    }
}