using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace P00LS.Games.Editor
{
    internal class P00LSMenu
    {
        [MenuItem("Services/P00LS/Import WebGL Template")]
        public static void ImportWebGLTemplate()
        {
            CopyWebGLTemplate();
            SetProjectWebGLTemplate();
            SetSplashScreenLogo();
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
            PlayerSettings.SplashScreen.unityLogoStyle = PlayerSettings.SplashScreen.UnityLogoStyle.LightOnDark;
        }

        private static void SetProjectWebGLTemplate()
        {
            PlayerSettings.WebGL.template = "PROJECT:P00LS";
        }

        private static void ResetToDefault()
        {
            PlayerSettings.SetTemplateCustomValue("P00LS_ENV", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_API_KEY", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_APP_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_GAME_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_SDK_VERSION", "v2.3");
        }

        private static void CopyWebGLTemplate()
        {
            if (!AssetDatabase.IsValidFolder("Assets/WebGLTemplates"))
            {
                AssetDatabase.CreateFolder("Assets", "WebGLTemplates");
            }

            if (AssetDatabase.IsValidFolder("Assets/WebGLTemplates/P00LS"))
            {
                AssetDatabase.DeleteAsset("Assets/WebGLTemplates/P00LS");
            }

            Directory.CreateDirectory(Path.GetFullPath("Assets/WebGLTemplates/P00LS"));
            Copy(Path.GetFullPath("Packages/io.p00ls.games/Assets/WebGLTemplates/P00LS"),
                Path.GetFullPath("Assets/WebGLTemplates/P00LS"));
            AssetDatabase.ImportAsset("Assets/WebGLTemplates/P00LS");
            AssetDatabase.Refresh();
        }

        private static void Copy(string source, string destination)
        {
            var files = from file in Directory.EnumerateFiles(source)
                where Path.GetExtension(file) != ".meta"
                select new
                {
                    Source = file,
                    Destination = Path.Combine(destination, Path.GetFileName(file))
                };

            foreach (var file in files)
            {
                Debug.Log($"copy ${file.Source} to ${file.Destination}");
                File.Copy(file.Source, file.Destination);
            }

            foreach (var directory in Directory.EnumerateDirectories(source))
            {
                Directory.CreateDirectory(Path.Combine(destination, Path.GetFileName(directory)));
                Debug.Log($"copy ${directory} to ${destination}");
                Copy(directory, Path.Combine(destination, Path.GetFileName(directory)));
            }
        }
    }
}