using UnityEditor;
using UnityEditor.SettingsManagement;

namespace P00LS.Games.Editor
{
    public class SettingWrapper<T> : UserSetting<T>
    {
         public SettingWrapper(string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(P00LSGamesSettings.Instance, key, value, scope)
        {
        }
        
        public SettingWrapper(Settings settings, string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(settings, key, value, scope) { }
    }
}
