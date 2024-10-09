using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace P00LS.Games.Editor
{
    internal readonly struct SharedEnvParams
    {
        public SharedEnvParams(string authDomain, string projectId, string storageBucket, string messagingSenderId)
        {
            AuthDomain = authDomain;
            ProjectId = projectId;
            StorageBucket = storageBucket;
            MessagingSenderId = messagingSenderId;
        }

        public string AuthDomain { get; }
        public string ProjectId { get; }
        public string StorageBucket { get; }
        public string MessagingSenderId { get; }
    }


    public class P00LSBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;


        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("Configuring P00LS Environment");
            var env = PlayerSettings.GetTemplateCustomValue("P00LS_ENV");
            var envConfig = P00LSGamesSettings.Instance.Get<EnvConfig>(env + ".conf");
            var gameId = P00LSGamesSettings.Instance.Get<string>("general.gameId");
            var blockId = P00LSGamesSettings.Instance.Get<string>("general.blockId");
            PlayerSettings.SetTemplateCustomValue("P00LS_GAME_ID", gameId);
            PlayerSettings.SetTemplateCustomValue("P00LS_BLOCK_ID", blockId);
            PlayerSettings.SetTemplateCustomValue("P00LS_API_KEY", envConfig.apiKey);
            PlayerSettings.SetTemplateCustomValue("P00LS_APP_ID", envConfig.appId);
            Debug.Log("P00LS Environment configured for " + env);
        }
    }
}