using System;

namespace P00LS.Games
{
    public interface IBridge
    {
        public event Action<PurchaseResult> OnPurchase;

        public void SaveUserData(object data);
        
        public void SavePartData(string docKey, object data);

        public void GetUserData<T>(Action<T> callback);
        
        public void ReadPartData<T>(string docKey, Action<T> callback);

        public void GetIdToken(Action<string> callback);

        public void LogEvent(string eventName, object eventParams);

        public void LogEvent(string eventName);

        public UserProfile GetUserProfile();

        public void ImpactHapticFeedback(ImpactFeedBackForce force);

        public void NotificationHapticFeedback(NotificationFeedBackForce force);

        public void SelectHapticFeedback();

        public void InitPurchase(PurchaseParams purchaseParams);

        public void ShowAd(Action<bool> callback);
        
        public void ShowAd(AdType type, Action<bool> callback);

        public void GetReferralLink(Action<string> callback);

        public void GetReferrer(Action<Referrer> callback);
        
        public void GetReferees(GetRefereesRequest request, Action<GetRefereesResult> callback);
    }

    public enum AdType
    {
        Interstitial,
        Rewarded,
    }
    
    public enum ImpactFeedBackForce
    {
        Light,
        Medium,
        Heavy,
        Rigid,
        Soft
    }

    public enum NotificationFeedBackForce
    {
        Error,
        Success,
        Warning,
    }
}