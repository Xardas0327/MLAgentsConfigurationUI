using System.IO;

namespace Xardas.MLAgents.Configuration
{
    public static class Paths
    {
        public readonly static string SettingsPath = Path.Combine("Assets", "ML-Agents Configuration");
        public readonly static string FilesPath = Path.Combine(SettingsPath, "Files");
    }
}