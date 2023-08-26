using System;
using UnityEngine;
using Xardas.MLAgents.Yaml;

namespace Xardas.MLAgents.Configuration.Fileformat.SettingParameter
{
    [Serializable]
    public class CliSettings : ISettings
    {

        public bool isUseDeterministic;
        [Tooltip(ConfigTooltip.deterministic)]
        public bool deterministic = false;

        public bool isUseNumAreas;
        [Tooltip(ConfigTooltip.numAreas)]
        [Min(1)]
        public uint numAreas = 1;

        public bool isUseDebug;
        [Tooltip(ConfigTooltip.debug)]
        public bool debug = false;


        public bool isUseResultsDir;
        [Tooltip(ConfigTooltip.resultsDir)]
        public string resultsDir = "results";

        public bool IsUse =>
                isUseDeterministic || isUseNumAreas
            || isUseDebug || isUseResultsDir;

        public CliSettings() { }

        public YamlObject ToYaml()
        {
            return null;
        }
    }
}