using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace P00LS.Games.Editor
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once ClassNeverInstantiated.Global
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


        [MenuItem("Services/P00LS/Build And Deploy")]
        public static void BuildAndDeploy()
        {
            var deployApiKey = P00LSGamesSettings.Instance.Get<string>("general.deployApiKey");
            if (deployApiKey == "")
            {
                MissingApiKeyWindow.CreateApikeySaveWindow();
                return;
            }
            Debug.Log(deployApiKey);
            var buildRootPath = "build";
            var buildDestPath = $"{buildRootPath}/build";
            var buildZip = "build.zip";
            Debug.Log($"Build And Deploy: {buildDestPath}");
            var buildUrl = $"http://localhost:3000/api/build/{deployApiKey}";
            CleanOldBuild(buildRootPath, buildZip);
            Build(buildDestPath);
            PostBuildToGamesPortal(buildUrl, buildZip, buildRootPath);
            Debug.Log("Build file upload and successfully deployed");
        }

        private static void PostBuildToGamesPortal(string buildUrl,string buildZip, string buildRootPath)
        {
            ZipBuild(buildRootPath, buildZip);
            WebClient myWebClient = new WebClient();
            var response = myWebClient.UploadFile(buildUrl, buildZip);
            Debug.Log(Encoding.UTF8.GetString(response));
        }

        private static void ZipBuild(string buildRootPath, string buildZip)
        {
            ZipFile.CreateFromDirectory(buildRootPath, buildZip);
            Debug.Log($"Zip file created ${buildZip}");
        }

        private static void Build(string buildDestPath)
        {
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = buildDestPath,
                target = BuildTarget.WebGL
            };
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = buildReport.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + $" bytes under {buildDestPath}");
            }


            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        private static void CleanOldBuild(string buildRootPath, string buildZip)
        {
            try
            {
                Directory.Delete(buildRootPath, true);
            }
            // ReSharper disable once RedundantEmptyFinallyBlock
            finally
            {
                // nothing
            }

            try
            {
                File.Delete(buildZip);
            }
            // ReSharper disable once RedundantEmptyFinallyBlock
            finally
            {
                // nothing
            }
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
            PlayerSettings.SetTemplateCustomValue("P00LS_AUTH_DOMAIN", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_PROJECT_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_STORAGE_BUCKET", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_MESSAGING_SENDER_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_APP_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_GAME_ID", "devel");
            PlayerSettings.SetTemplateCustomValue("P00LS_SDK_VERSION", "v2.0");
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
                Debug.Log($"copy {file.Source} to {file.Destination}");
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
    public class MissingApiKeyWindow : EditorWindow
    {
        
        void OnGUI()
        {
            EditorGUILayout.HelpBox("Missing Deploy API Key\nUnity Settings -> POOLS Games -> Deploy API Key", MessageType.Info);
            EditorGUILayout.HelpBox("Restart the build when api key is set", MessageType.Error);
        }

        public static void CreateApikeySaveWindow()
        {
            MissingApiKeyWindow window = ScriptableObject.CreateInstance<MissingApiKeyWindow>();
            window.ShowUtility();
        }
    }
}