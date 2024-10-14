using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace P00LS.Games
{
    internal class BrowserBridge : IInternalBridge
    {
        private readonly string _objectName;
        private Action<string> _getUserDataHandler;
        private Action<string> _getIdTokenHandler;
        private Action<bool> _showAdHandler;
        private Action<string> _getReferralLinkHandler;
        private Action<Referrer> _getReferrerHandler;
        private Action<GetRefereesResult> _getRefereesHandler;

        public BrowserBridge(string objectName)
        {
            _objectName = objectName;
        }

        public event Action<PurchaseResult> OnPurchase;

        public void SaveUserData(object data)
        {
            var payload = JsonUtility.ToJson(data);
            JsFunctions.p00ls_SaveUserData(payload);
        }

        public void GetUserData<T>(Action<T> callback)
        {
            _getUserDataHandler = value => { callback.Invoke(FromJson<T>(value)); };
            JsFunctions.p00ls_GetUserData(_objectName, "GetUserDataCallback", "GetUserDataFallback");
        }

        public void GetIdToken(Action<string> callback)
        {
            _getIdTokenHandler = callback;
            JsFunctions.p00ls_GetIdToken(_objectName, "GetIdTokenCallback");
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
            _showAdHandler = callback;
            JsFunctions.p00ls_ShowAd(_objectName, "ShowAdCallback");
        }

        public void GetReferralLink(Action<string> callback)
        {
            _getReferralLinkHandler = callback;
            JsFunctions.p00ls_GetReferralLink(_objectName, "GetReferralLinkCallback");
        }

        public void GetReferrer(Action<Referrer> callback)
        {
            _getReferrerHandler = callback;
            JsFunctions.p00ls_GetReferrer(_objectName, "GetReferrerCallback");
        }

        public void GetReferees(GetRefereesRequest request, Action<GetRefereesResult> callback)
        {
            _getRefereesHandler = callback;
            var p = request != null ? JsonUtility.ToJson(request) : "{}";
            JsFunctions.p00ls_GetReferees(p, _objectName, "GetRefereesCallback");
        }

        public void OnPurchaseCallback(string value)
        {
            var rawPurchaseResult = FromJson<RawPurchaseResult>(value);
            if (rawPurchaseResult.purchaseParams.itemId != null)
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
            _showAdHandler?.Invoke(value);
            _showAdHandler = null;
        }

        public void GetReferralLinkCallback(string value)
        {
            _getReferralLinkHandler?.Invoke(value);
            _getReferralLinkHandler = null;
        }

        public void GetReferrerCallback(string value)
        {
            _getReferrerHandler?.Invoke(FromJson<Referrer>(value));
            _getReferrerHandler = null;
        }

        public void GetRefereesCallback(string value)
        {
            var raw = FromJson<RawGetRefereesResult>(value);
            _getRefereesHandler?.Invoke(new GetRefereesResult
            {
                total = raw.total,
                next = raw.next,
                page = raw.page.ConvertAll(r => new Referee {firstName = r.firstName, createdAt = DateTime.Parse(r.createdAt)})
                    
            });
            _getRefereesHandler = null;
        }

        public void GetUserDataCallback(string value)
        {
            _getUserDataHandler?.Invoke(value);
            _getUserDataHandler = null;
        }

        public void GetUserDataFallback(string error)
        {
            Debug.Log(error);
        }

        public void GetIdTokenCallback(string value)
        {
            _getIdTokenHandler?.Invoke(value);
            _getIdTokenHandler = null;
        }

        private static T FromJson<T>(string value)
        {
            return value == "null" ? default : JsonUtility.FromJson<T>(value);
        }
    }


    [Serializable]
    internal class RawPurchaseResult
    {
        public string paymentId;
        public RawPurchaseParams purchaseParams;
        public string status;
    }

    [Serializable]
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
    
    [Serializable]
    internal class RawReferee
    {
        public string firstName;
        public string createdAt;
    }

    [Serializable]
    internal class RawGetRefereesResult
    {
        public List<RawReferee> page;
        public int total;
        public string next;
    }
}