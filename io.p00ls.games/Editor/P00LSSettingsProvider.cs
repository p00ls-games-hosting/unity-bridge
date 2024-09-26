using UnityEditor;
using UnityEditor.SettingsManagement;

namespace P00LS.Games.Editor
{
    static class P00LSSettingsProvider
    {
        [SettingsProvider]
        static SettingsProvider CreateSettingsProvider()
        {
            var provider = new UserSettingsProvider("Preferences/P00LS Games",
                P00LSGamesSettings.Instance,
                new[] { typeof(P00LSSettingsProvider).Assembly });

            return provider;
        }
    }
}