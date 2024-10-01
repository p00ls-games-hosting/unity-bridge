using System.IO;
using UnityEditor;
using UnityEngine;

namespace P00LS.Games.Editor
{
    public class P00LSMenu
    {
        [MenuItem("Services/P00LS/Import WebGL Template")]
        public static void ImportWebGLTemplate()
        {
            CopyWebGLTemplate();
            SetProjectWebGLTemplate();
        }

        [MenuItem("Services/P00LS/Reset Template Variables")]
        public static void SetCustomTemplateVariables()
        {
            ResetToDefault();
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.nameFilesAsHashes = true;
            SetSplashScreenLogo();
        }

        private static void SetSplashScreenLogo()
        {
            var logo = (Sprite)AssetDatabase.LoadAssetAtPath("Packages/io.p00ls.games/Assets/Materials/Logo.png",
                typeof(Sprite));
            PlayerSettings.SplashScreen.show = true;
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SplashScreen.logos = new[] { PlayerSettings.SplashScreenLogo.Create(2f, logo) };
            PlayerSettings.SplashScreen.backgroundColor = Color.black;
            PlayerSettings.SplashScreen.blurBackgroundImage = true;
        }

        private static void SetProjectWebGLTemplate()
        {
            PlayerSettings.WebGL.template = "PROJECT:P00LS";
        }

        private static void ResetToDefault()
        {
            PlayerSettings.SetTemplateCustomValue("P00LS_ENV", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_API_KEY", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_AUTH_DOMAIN", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_PROJECT_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_STORAGE_BUCKET", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_MESSAGING_SENDER_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_APP_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_GAME_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_SDK_VERSION", "v1.2");
        }

        private static void CopyWebGLTemplate()
        {
            if (!AssetDatabase.IsValidFolder("Assets/WebGLTemplates"))
            {
                AssetDatabase.CreateFolder("Assets", "WebGLTemplates");
            }

            if (!AssetDatabase.IsValidFolder("Assets/WebGLTemplates/P00LS"))
            {
                AssetDatabase.CreateFolder("Assets/WebGLTemplates", "P00LS");
            }

            if (!AssetDatabase.IsValidFolder("Assets/WebGLTemplates/P00LS/styles"))
            {
                AssetDatabase.CreateFolder("Assets/WebGLTemplates/P00LS", "styles");
            }

            var indexSource = Path.GetFullPath("Packages/io.p00ls.games/Assets/WebGLTemplates/P00LS/index.html");
            var indexDestination = Path.Combine("Assets/WebGLTemplates/P00LS", "index.html");
            var cssSource = Path.GetFullPath("Packages/io.p00ls.games/Assets/WebGLTemplates/P00LS/styles/global.css");
            var cssDestination = Path.Combine("Assets/WebGLTemplates/P00LS/styles", "global.css");
            Move(indexSource, indexDestination);
            Move(cssSource, cssDestination);
        }

        private static void Move(string source, string destination)
        {
            if (File.Exists(destination))
            {
                Debug.Log("Replacing " + destination);
                FileUtil.ReplaceFile(source, destination);
            }
            else
            {
                Debug.Log("Importing WebGL Template " + destination);
                FileUtil.CopyFileOrDirectory(source, destination);
            }
        }
    }
}