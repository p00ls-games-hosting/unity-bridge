using System;
using UnityEngine;

namespace P00LS.Games
{
    internal class BrowserBridge : IInternalBridge
    {
        private readonly string objectName;

        private Action<string> getUserDataHandler;

        private Action<string> getIdTokenHandler;
        private Action<bool> showAdHandler;

        public BrowserBridge(string objectName)
        {
            this.objectName = objectName;
        }

        public event Action<PurchaseResult> OnPurchase;

        public void SaveUserData(object data)
        {
            var payload = JsonUtility.ToJson(data);
            JsFunctions.p00ls_SaveUserData(payload);
        }

        public void GetUserData<T>(Action<T> callback)
        {
            getUserDataHandler = value => { callback.Invoke(FromJson<T>(value)); };
            JsFunctions.p00ls_GetUserData(objectName, "GetUserDataCallback", "GetUserDataFallback");
        }

        public void GetIdToken(Action<string> callback)
        {
            getIdTokenHandler = callback;
            JsFunctions.p00ls_GetIdToken(objectName, "GetIdTokenCallback");
        }

        public void LogEvent(string eventName, object eventParams)
        {
            var payload = JsonUtility.ToJson(eventParams);
            JsFunctions.p00ls_LogEvent(eventName, payload);
        }

        public void LogEvent(string eventName)
        {
            JsFunctions.p00ls_LogEvent(eventName, "{}");
        }

        public UserProfile GetUserProfile()
        {
            return JsonUtility.FromJson<UserProfile>(JsFunctions.p00ls_GetUserProfile());
        }

        public void ImpactHapticFeedback(ImpactFeedBackForce force)
        {
            JsFunctions.p00ls_HapticFeedback("impact", force.ToString().ToLower());
        }

        public void NotificationHapticFeedback(NotificationFeedBackForce force)
        {
            JsFunctions.p00ls_HapticFeedback("notification", force.ToString().ToLower());
        }

        public void SelectHapticFeedback()
        {
            JsFunctions.p00ls_HapticFeedback("select", "");
        }

        public void InitPurchase(PurchaseParams purchaseParams)
        {
            JsFunctions.p00ls_InitPurchase(JsonUtility.ToJson(purchaseParams));
        }

        public void ShowAd(Action<bool> callback)
        {
            showAdHandler = callback;
            JsFunctions.p00ls_ShowAd(objectName, "ShowAdCallback");
        }

        public void OnPurchaseCallback(string value)
        {
            var rawPurchaseResult = FromJson<RawPurchaseResult>(value);
            if (rawPurchaseResult.purchaseParams != null)
            {
                OnPurchase?.Invoke(new PurchaseResult
                {
                    paymentId = rawPurchaseResult.paymentId,
                    purchaseParams = new PurchaseItemParams
                    {
                        itemId = rawPurchaseResult.purchaseParams.itemId,
                        title = rawPurchaseResult.purchaseParams.title,
                        description = rawPurchaseResult.purchaseParams.description,
                        quantity = rawPurchaseResult.purchaseParams.quantity,
                        price = rawPurchaseResult.purchaseParams.price
                    },
                    status = rawPurchaseResult.status
                });
            }
            else
            {
                OnPurchase?.Invoke(new PurchaseResult
                {
                    paymentId = rawPurchaseResult.paymentId,
                    purchaseParams = new PurchaseCurrencyParams
                    {
                        symbol = rawPurchaseResult.purchaseParams.symbol,
                        name = rawPurchaseResult.purchaseParams.name,
                        quantity = rawPurchaseResult.purchaseParams.quantity,
                        price = rawPurchaseResult.purchaseParams.price
                    },
                    status = rawPurchaseResult.status
                });
            }
        }

        public void ShowAdCallback(bool value)
        {
            showAdHandler?.Invoke(value);
        }

        public void GetUserDataCallback(string value)
        {
            getUserDataHandler?.Invoke(value);
            getUserDataHandler = null;
        }

        public void GetUserDataFallback(string error)
        {
            Debug.Log(error);
        }

        public void GetIdTokenCallback(string value)
        {
            getIdTokenHandler?.Invoke(value);
            getIdTokenHandler = null;
        }

        private static T FromJson<T>(string value)
        {
            return value == "null" ? default : JsonUtility.FromJson<T>(value);
        }
    }


    internal class RawPurchaseResult
    {
        public string paymentId;
        public RawPurchaseParams purchaseParams;
        public string status;
    }

    internal class RawPurchaseParams
    {
        public string itemId;
        public string title;
        public string description;
        public string symbol;
        public string name;
        public string currency;
        public int quantity;
        public int price;
    }
}