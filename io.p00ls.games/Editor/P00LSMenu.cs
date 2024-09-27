using System.IO;
using UnityEditor;
using UnityEngine;

namespace P00LS.Games.Editor
{
    public class P00LSMenu
    {
        [MenuItem("Services/Setup p00ls template")]
        static void ImportWebGLTemplate()
        {
            PlayerSettings.SetTemplateCustomValue("P00LS_ENV", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_API_KEY", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_AUTH_DOMAIN", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_PROJECT_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_STORAGE_BUCKET", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_MESSAGING_SENDER_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_APP_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_GAME_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_SDK_VERSION", "v1.1");

            var logo = (Sprite)AssetDatabase.LoadAssetAtPath("Packages/io.p00ls.games/Assets/Materials/Logo.png", typeof(Sprite));
            PlayerSettings.SplashScreen.logos = new[] { PlayerSettings.SplashScreenLogo.Create(2f, logo) };
            if (!AssetDatabase.IsValidFolder("Assets/WebGLTemplates"))
            {
                AssetDatabase.CreateFolder("Assets", "WebGLTemplates");
            }

            if (!AssetDatabase.IsValidFolder("Assets/WebGLTemplates/P00LS"))
            {
                AssetDatabase.CreateFolder("Assets/WebGLTemplates", "P00LS");
            }

            var source = Path.GetFullPath("Packages/io.p00ls.games/Assets/WebGLTemplates/P00LS/index.html");
            var destination = Path.Combine("Assets/WebGLTemplates/P00LS", "index.html");
            if (File.Exists(destination))
            {
                Debug.Log("Replacing template");
                FileUtil.ReplaceFile(source, destination);
            }
            else
            {
                Debug.Log("Importing WebGL Template");
                FileUtil.CopyFileOrDirectory(source, destination);
            }
            PlayerSettings.WebGL.template = "PROJECT:P00LS";
        }
    }
}