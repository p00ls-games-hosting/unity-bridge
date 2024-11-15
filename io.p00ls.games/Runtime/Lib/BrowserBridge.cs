using System;
using System.Collections.Generic;
using Unity.Serialization.Json;
using UnityEngine;
using Object = System.Object;

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
        private Action<Dictionary<string, Statistic>> _getStatisticsCallback;
        private Action<UserLeaderboardPosition?> _getUserPositionCallback;
        private Action<Leaderboard> _getLeaderboardCallback;

        public BrowserBridge(string objectName)
        {
            _objectName = objectName;
        }

        public event Action<PurchaseResult> OnPurchase;

        public void SaveUserData(object data)
        {
            var payload = JsonSerialization.ToJson(data);
            JsFunctions.p00ls_SaveUserData(payload);
        }

        public void SavePartData(string docKey, object data)
        {
            var payload = JsonSerialization.ToJson(data);
            JsFunctions.p00ls_SavePartData(docKey, payload);
        }

        public void GetUserData<T>(Action<T> callback)
        {
            _getUserDataHandler = value => { callback.Invoke(FromJson<T>(value)); };
            JsFunctions.p00ls_GetUserData(_objectName, "GetUserDataCallback", "GetUserDataFallback");
        }

        public void ReadPartData<T>(string docKey, Action<T> callback)
        {
            _getUserDataHandler = value => { callback.Invoke(FromJson<T>(value)); };
            JsFunctions.p00ls_ReadPartData(docKey, _objectName, "GetUserDataCallback", "GetUserDataFallback");
        }

        public void GetIdToken(Action<string> callback)
        {
            _getIdTokenHandler = callback;
            JsFunctions.p00ls_GetIdToken(_objectName, "GetIdTokenCallback");
        }

        public void LogEvent(string eventName, object eventParams)
        {
            var payload = JsonSerialization.ToJson(eventParams);
            JsFunctions.p00ls_LogEvent(eventName, payload);
        }

        public void LogEvent(string eventName)
        {
            JsFunctions.p00ls_LogEvent(eventName, "{}");
        }

        public UserProfile GetUserProfile()
        {
            var p00LsGetUserProfile = JsFunctions.p00ls_GetUserProfile();
            return JsonSerialization.FromJson<UserProfile>(p00LsGetUserProfile);
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
            JsFunctions.p00ls_InitPurchase(JsonSerialization.ToJson(purchaseParams, new JsonSerializationParameters()
            {
                SerializedType = typeof(RawPurchaseParams)
            }));
        }

        public void ShowAd(Action<bool> callback)
        {
            ShowAd(AdType.Rewarded, callback);
        }

        public void ShowAd(AdType type, Action<bool> callback)
        {
            _showAdHandler = callback;
            JsFunctions.p00ls_ShowAd(type == AdType.Interstitial ? "interstitial" : "rewarded", _objectName,
                "ShowAdCallback");
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

        public void GetReferees(Action<GetRefereesResult> callback, int pageSize = 50, string next = null)
        {
            _getRefereesHandler = callback;
            var pa = new Dictionary<string, Object> { { "pageSize", pageSize } };
            if (next != null)
            {
                pa.Add("next", next);
            }

            JsFunctions.p00ls_GetReferees(JsonSerialization.ToJson(pa), _objectName, "GetRefereesCallback");
        }

        public void GetStatistics(Action<Dictionary<string, Statistic>> callback)
        {
            _getStatisticsCallback = callback;
            JsFunctions.p00ls_GetStatistics(_objectName, "GetStatisticsCallback");
        }

        public void UpdateStatistic(StatisticUpdate[] statisticUpdate)
        {
            JsFunctions.p00ls_UpdateStatistics(JsonSerialization.ToJson(statisticUpdate));
        }

        public void GetUserPosition(string statisticName, Action<UserLeaderboardPosition?> callback)
        {
            _getUserPositionCallback = callback;
            JsFunctions.p00ls_GetUserPosition(statisticName, _objectName, "GetUserPositionCallback");
        }

        public void GetLeaderboard(string statisticName, Action<Leaderboard> callback, int pageSize = 50, string next = null)
        {
            _getLeaderboardCallback = callback;
            var pa = new Dictionary<string, Object> { { "limit", pageSize }, {"statistic", statisticName } };
            if (next != null)
            {
                pa.Add("next", next);
            }

            JsFunctions.p00ls_GetLeaderboard(JsonSerialization.ToJson(pa), _objectName, "GetLeaderboardCallback");
        }

        public void OnPurchaseCallback(string value)
        {
            var rawPurchaseResult = JsonSerialization.FromJson<RawPurchaseResult>(value);
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
            var rawGetRefereesResult = JsonSerialization.FromJson<RawGetRefereesResult>(value);
            _getRefereesHandler?.Invoke(new GetRefereesResult
            {
                total = rawGetRefereesResult.total,
                next = rawGetRefereesResult.next,
                page = Array.ConvertAll(rawGetRefereesResult.page, r => new Referee
                    { firstName = r.firstName, createdAt = DateTime.Parse(r.createdAt) })
            });
            _getRefereesHandler = null;
        }

        public void GetStatisticsCallback(string value)
        {
            _getStatisticsCallback?.Invoke(JsonSerialization.FromJson<Dictionary<string, Statistic>>(value));
            _getStatisticsCallback = null;
        }

        public void GetUserPositionCallback(string value)
        {
            var result = JsonHelper.FromJson<UserLeaderboardPosition>(value);
            _getUserPositionCallback?.Invoke(result);
            _getUserPositionCallback = null;
        }

        public void GetLeaderboardCallback(string value)
        {
            var result = JsonHelper.FromJson<Leaderboard>(value);
            _getLeaderboardCallback?.Invoke(result);
            _getLeaderboardCallback = null;
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
            return JsonHelper.FromJson<T>(value);
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
        public RawReferee[] page;
        public int total;
        public string next;
    }

    [Serializable]
    internal struct GetRefereesRequest
    {
        public string next;
        public int pageSize;
    }
}