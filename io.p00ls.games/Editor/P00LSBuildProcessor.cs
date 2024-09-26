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
        private static SharedEnvParams _devel = new(
            authDomain: "prj-p00ls-games-devel-b18a.firebaseapp.com",
            projectId: "prj-p00ls-games-devel-b18a",
            storageBucket: "prj-p00ls-games-devel-b18a.appspot.com",
            messagingSenderId: "710487297939"
        );

        private static SharedEnvParams _prod = new(
            authDomain: "prj-p00ls-games-prod-9e17.firebaseapp.com",
            projectId: "prj-p00ls-games-prod-9e17",
            storageBucket: "prj-p00ls-games-prod-9e17.appspot.com",
            messagingSenderId: "534422411536"
        );

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

            var sharedEnvParams = env == "prod" ? _prod : _devel;

            PlayerSettings.SetTemplateCustomValue("P00LS_AUTH_DOMAIN", sharedEnvParams.AuthDomain);
            PlayerSettings.SetTemplateCustomValue("P00LS_PROJECT_ID", sharedEnvParams.ProjectId);
            PlayerSettings.SetTemplateCustomValue("P00LS_STORAGE_BUCKET", sharedEnvParams.StorageBucket);
            PlayerSettings.SetTemplateCustomValue("P00LS_MESSAGING_SENDER_ID", sharedEnvParams.MessagingSenderId);
            Debug.Log("P00LS Environment configured for " + env);
        }
    }
}