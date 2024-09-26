using System;
using UnityEngine;

namespace P00LS.Games
{
    internal class FakeBridge : IInternalBridge
    {
        private object userData;
        private IInternalBridge internalBridgeImplementation;

        public event Action<PurchaseResult> OnPurchase;

        public void SaveUserData(object data)
        {
            userData = data;
        }

        public void GetUserData<T>(Action<T> callback)
        {
            callback.Invoke((T)userData);
        }

        public void GetIdToken(Action<string> callback)
        {
            Debug.Log("GetIdToken");
            callback.Invoke("");
        }

        public void LogEvent(string eventName, object eventParams)
        {
            Debug.Log("LogEvent: " + eventName);
        }

        public void LogEvent(string eventName)
        {
            Debug.Log("LogEvent: " + eventName);
        }

        public UserProfile GetUserProfile()
        {
            return new UserProfile
                { firstName = "anonymous", username = "anoynmous", userId = "12345", isPremium = false };
        }

        public void ImpactHapticFeedback(ImpactFeedBackForce force)
        {
        }

        public void NotificationHapticFeedback(NotificationFeedBackForce force)
        {
        }

        public void SelectHapticFeedback()
        {
        }


        public void InitPurchase(PurchaseParams purchaseParams)
        {
            OnPurchase?.Invoke(new PurchaseResult
            {
                purchaseParams = purchaseParams,
                status = "paid",
                paymentId = "fakePaymentId",
            });
        }

        public void ShowAd(Action<bool> callback)
        {
            Debug.Log("Showing ad");
            callback.Invoke(true);
        }

        public void GetUserDataCallback(string value)
        {
        }

        public void GetUserDataFallback(string error)
        {
        }

        public void GetIdTokenCallback(string value)
        {
        }

        public void OnPurchaseCallback(string value)
        {
            Debug.Log("OnPurchaseCallback: " + value);
        }

        public void ShowAdCallback(bool value)
        {
            Debug.Log("ShowAdCallback: " + value);
        }
    }
}