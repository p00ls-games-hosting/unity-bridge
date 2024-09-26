using System;
using UnityEngine;

namespace P00LS.Games
{
    public class P00LSGamesSdk : MonoBehaviour, IBridge
    {
        public event Action<PurchaseResult> OnPurchase;

        private Action<string> getUserDataHandler;

        private Action<string> getIdTokenHandler;

        private IInternalBridge bridge;

        [SerializeField] private bool stub;

        private void Awake()
        {
            if (bridge != null)
            {
                return;
            }

            if (Application.platform != RuntimePlatform.WebGLPlayer || stub)
            {
                Debug.Log("Creating fake bridge");
                bridge = new FakeBridge();
            }
            else
            {
                Debug.Log("Creating actual bridge");
                bridge = new BrowserBridge(gameObject.name);
            }

            bridge.OnPurchase += InternalOnPurchase;
        }

        private void OnDestroy()
        {
            bridge.OnPurchase -= InternalOnPurchase;
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
            bridge.SaveUserData(data);
        }

        public void GetUserData<T>(Action<T> callback)
        {
            bridge.GetUserData(callback);
        }

        public void GetIdToken(Action<string> callback)
        {
            bridge.GetIdToken(callback);
        }

        public void LogEvent(string eventName, object eventParams)
        {
            bridge.LogEvent(eventName, eventParams);
        }

        public void LogEvent(string eventName)
        {
            bridge.LogEvent(eventName);
        }

        public UserProfile GetUserProfile()
        {
            return bridge.GetUserProfile();
        }

        public void ImpactHapticFeedback(ImpactFeedBackForce force)
        {
            bridge.ImpactHapticFeedback(force);
        }

        public void NotificationHapticFeedback(NotificationFeedBackForce force)
        {
            bridge.NotificationHapticFeedback(force);
        }

        public void SelectHapticFeedback()
        {
            bridge.SelectHapticFeedback();
        }

        public void InitPurchase(PurchaseParams purchaseParams)
        {
            bridge.InitPurchase(purchaseParams);
        }

        public void ShowAd(Action<bool> callback)
        {
            bridge.ShowAd(callback);
        }

        private void GetUserDataCallback(string value)
        {
            bridge.GetUserDataCallback(value);
        }

        private void GetUserDataFallback(string error)
        {
            bridge.GetUserDataFallback(error);
        }

        private void GetIdTokenCallback(string value)
        {
            bridge.GetIdTokenCallback(value);
        }

        private void OnPurchaseCallback(string value)
        {
            bridge.OnPurchaseCallback(value);
        }

        private void ShowAdCallback(int value)
        {
            bridge.ShowAdCallback(Convert.ToBoolean(value));
        }
    }
}