using System;
using UnityEngine;

namespace P00LS.Games
{
    public class P00LSGamesSdk : MonoBehaviour, IBridge
    {
        public event Action<PurchaseResult> OnPurchase;
        private IInternalBridge _bridge;

        private void Awake()
        {
            if (_bridge != null)
            {
                return;
            }

            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Debug.Log("Creating fake bridge");
                _bridge = new FakeBridge();
            }
            else
            {
                Debug.Log("Creating actual bridge");
                _bridge = new BrowserBridge(gameObject.name);
            }

            _bridge.OnPurchase += InternalOnPurchase;
        }

        private void OnDestroy()
        {
            _bridge.OnPurchase -= InternalOnPurchase;
        }

        private void InternalOnPurchase(PurchaseResult result)
        {
            OnPurchase?.Invoke(result);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SaveUserData(object data)
        {
            _bridge.SaveUserData(data);
        }

        public void SavePartData(string docKey, object data)
        {
            _bridge.SavePartData(docKey, data); 
        }

        public void GetUserData<T>(Action<T> callback)
        {
            _bridge.GetUserData(callback);
        }

        public void ReadPartData<T>(string docKey, Action<T> callback)
        {
            _bridge.ReadPartData(docKey, callback);
        }

        public void GetIdToken(Action<string> callback)
        {
            _bridge.GetIdToken(callback);
        }

        public void LogEvent(string eventName, object eventParams)
        {
            _bridge.LogEvent(eventName, eventParams);
        }

        public void LogEvent(string eventName)
        {
            _bridge.LogEvent(eventName);
        }

        public UserProfile GetUserProfile()
        {
            return _bridge.GetUserProfile();
        }

        public void ImpactHapticFeedback(ImpactFeedBackForce force)
        {
            _bridge.ImpactHapticFeedback(force);
        }

        public void NotificationHapticFeedback(NotificationFeedBackForce force)
        {
            _bridge.NotificationHapticFeedback(force);
        }

        public void SelectHapticFeedback()
        {
            _bridge.SelectHapticFeedback();
        }

        public void InitPurchase(PurchaseParams purchaseParams)
        {
            _bridge.InitPurchase(purchaseParams);
        }

        public void ShowAd(Action<bool> callback)
        {
            _bridge.ShowAd(callback);
        }

        public void ShowAd(AdType type, Action<bool> callback)
        {
            _bridge.ShowAd(type, callback);
        }

        public void GetReferralLink(Action<string> callback)
        {
            _bridge.GetReferralLink(callback);
        }

        public void GetReferrer(Action<Referrer> callback)
        {
            _bridge.GetReferrer(callback);
        }

        public void GetReferees(GetRefereesRequest request, Action<GetRefereesResult> callback)
        {
            _bridge.GetReferees(request, callback);
        }

        private void GetUserDataCallback(string value)
        {
            _bridge.GetUserDataCallback(value);
        }

        private void GetUserDataFallback(string error)
        {
            _bridge.GetUserDataFallback(error);
        }

        private void GetIdTokenCallback(string value)
        {
            _bridge.GetIdTokenCallback(value);
        }

        private void OnPurchaseCallback(string value)
        {
            _bridge.OnPurchaseCallback(value);
        }

        private void ShowAdCallback(int value)
        {
            _bridge.ShowAdCallback(Convert.ToBoolean(value));
        }

        private void GetReferralLinkCallback(string payload)
        {
            _bridge.GetReferralLinkCallback(payload);
        }

        private void GetReferrerCallback(string payload)
        {
            _bridge.GetReferrerCallback(payload);
        }

        private void GetRefereesCallback(string payload)
        {
            _bridge.GetRefereesCallback(payload);
        }
    }
}