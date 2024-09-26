using System;

namespace P00LS.Games.Editor
{
    [Serializable]
    class EnvConfig
    {
        public EnvConfig()
        {
            appId = "";
            apiKey = "";
        }

        public string apiKey;
        public string appId;
    }
}