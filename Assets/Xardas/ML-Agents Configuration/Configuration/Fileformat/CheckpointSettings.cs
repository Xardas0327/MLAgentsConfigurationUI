using UnityEngine;
using Xardas.MLAgents.Yaml;
using CheckpointSetting = Xardas.MLAgents.Configuration.Fileformat.SettingParameter.CheckpointSettings;

namespace Xardas.MLAgents.Configuration.Fileformat
{
    [CreateAssetMenu(fileName = "CheckpointSettings", menuName = "ML-Agents Config files/Checkpoint Settings")]
    public class CheckpointSettings : Settings<CheckpointSetting>
    {
        public override void LoadData(YamlObject yaml)
        {
            settings = new CheckpointSetting(yaml);
        }

        public override bool IsValid()
        {
            if (settings == null)
                return false;

            bool isValid = true;

            if (settings.isUseRunId && string.IsNullOrEmpty(settings.runId))
            {
                Debug.LogError("The run id can't be empty.");
                isValid = false;
            }

            if (settings.isUseInitializeFrom && string.IsNullOrEmpty(settings.initializeFrom))
            {
                Debug.LogError("The initialize from can't be empty.");
                isValid = false;
            }

            if (isValid)
                Debug.Log("It looks a valid environment settings file.");

            return isValid;
        }
    }
}