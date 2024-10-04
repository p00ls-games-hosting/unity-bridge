using UnityEditor;
using UnityEditor.SettingsManagement;

namespace P00LS.Games.Editor
{
    class P00LSSettingsMenu : EditorWindow
    {
        [UserSetting("General", "Game Id")] static SettingWrapper<string> _gameId =
            new("general.gameId", "");
        
        [UserSetting("General", "Block Id")] static SettingWrapper<string> _blockId =
            new("general.blockId", "");
        
        [UserSetting("General", "Deploy API Key")] static SettingWrapper<string> _deployApiKey =
            new("general.deployApiKey", "");

        [UserSetting] static SettingWrapper<EnvConfig> _devel =
            new("devel.conf", new EnvConfig());
        
        [UserSetting] static SettingWrapper<EnvConfig> _prod =
            new("prod.conf", new EnvConfig());
        
        [UserSettingBlock("Devel")]
        static void DevelConfig(string searchContext)
        {
            ConfigBlock(searchContext, _devel);
        }
        
        [UserSettingBlock("Production")]
        static void ProdConfig(string searchContext)
        {
            ConfigBlock(searchContext, _prod);
        }

        private static void ConfigBlock(string searchContext, SettingWrapper<EnvConfig> element)
        {
            EditorGUI.BeginChangeCheck();
            var conf = element.value;

            conf.apiKey = SettingsGUILayout.SearchableTextField("API Key", conf.apiKey, searchContext);
            conf.appId = SettingsGUILayout.SearchableTextField("App Id", conf.appId, searchContext);

            // Because EnvConf is a reference type, we need to apply the changes to the backing repository (SetValue
            // would also work here).
            if (EditorGUI.EndChangeCheck())
                element.ApplyModifiedProperties();


            if (EditorGUI.EndChangeCheck())
                P00LSGamesSettings.Instance.Save();
        }
        
    }
}