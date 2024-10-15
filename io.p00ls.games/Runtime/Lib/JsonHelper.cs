using UnityEngine;

namespace P00LS.Games
{
    internal static class JsonHelper
    {
        internal static T FromJson<T>(string value)
        {
            return value == "null" ? default : JsonUtility.FromJson<T>(value);
        }
        
    }
}