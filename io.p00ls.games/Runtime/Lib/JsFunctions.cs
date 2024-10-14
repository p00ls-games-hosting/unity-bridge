using System.Runtime.InteropServices;

namespace P00LS.Games
{
    internal static class JsFunctions
    {
        [DllImport("__Internal")]
        public static extern void p00ls_SaveUserData(string data);

        [DllImport("__Internal")]
        public static extern void p00ls_GetUserData(string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void p00ls_GetIdToken(string objectName, string callback);

        [DllImport("__Internal")]
        public static extern void p00ls_LogEvent(string eventName, string eventParams);

        [DllImport("__Internal")]
        public static extern string p00ls_GetUserProfile();

        [DllImport("__Internal")]
        public static extern void p00ls_HapticFeedback(string type, string style);

        [DllImport("__Internal")]
        public static extern void p00ls_InitPurchase(string purchaseParams);

        [DllImport("__Internal")]
        public static extern void p00ls_ShowAd(string objectName, string callback);
        
        [DllImport("__Internal")]
        public static extern void p00ls_GetReferralLink(string objectName, string callback);
        
        [DllImport("__Internal")]
        public static extern void p00ls_GetReferrer(string objectName, string callback);
        
        [DllImport("__Internal")]
        public static extern void p00ls_GetReferees(string parameters, string objectName, string callback);
    }
}