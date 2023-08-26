#if UNITY_EDITOR
using System.IO;

namespace Xardas.MLAgents
{
    public static class Paths
    {
        public readonly static string SettingsPath = Path.Combine("Assets", "Xardas", "ML-Agents Configuration");
        public readonly static string FilesPath = Path.Combine(SettingsPath, "Files");
    }
}
#endif