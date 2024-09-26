using UnityEditor;
using UnityEditor.SettingsManagement;

namespace P00LS.Games.Editor
{
    static class P00LSGamesSettings
    {
        private const string PackageName = "io.p00ls.games";

        private static Settings _instance;

        internal static Settings Instance => _instance ??= new Settings(PackageName);
    }
}