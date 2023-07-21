using UnityEngine;
using Xardas.MLAgents.Yaml;
using EnvironmentSetting = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.EnvironmentSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [CreateAssetMenu(fileName = "EnvironmentSettings", menuName = "ML-Agents Config files/Environment Settings")]
    public class EnvironmentSettings : Settings<EnvironmentSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new EnvironmentSetting(yaml);
        }

        public override bool IsValid()
        {
            if(settings == null)
                return false;

            bool isValid = true;

            if (settings.isUseEnvPath && string.IsNullOrEmpty(settings.envPath))
            {
                Debug.LogError("The env path can't be empty.");
                isValid = false;
            }

            if (settings.isUseEnvArgs && string.IsNullOrEmpty(settings.envArgs))
            {
                Debug.LogError("The env args can't be empty.");
                isValid = false;
            }

            if (isValid)
                Debug.Log("It looks a valid environment settings file.");

            return isValid;
        }
    }
}