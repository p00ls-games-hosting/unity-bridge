using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Serialization.Json;
using UnityEngine;

namespace P00LS.Games
{
    internal class FakeBridge : IInternalBridge
    {
        public event Action<PurchaseResult> OnPurchase;

        public void SaveUserData(object data)
        {
            try
            {
                var userDataPath = GetUserDataPath();
                Debug.Log($"Saving user data to {userDataPath}");
                File.WriteAllText(userDataPath, JsonSerialization.ToJson(data));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void SavePartData(string docKey, object data)
        {
            try
            {
                var partDataPath = GetPartDataPath(docKey);
                Debug.Log($"Saving part data to {partDataPath}");
                File.WriteAllText(partDataPath, JsonSerialization.ToJson(data));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void GetUserData<T>(Action<T> callback)
        {
            try
            {
                var raw = File.ReadAllText(GetUserDataPath());
                callback.Invoke(JsonHelper.FromJson<T>(raw));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                callback.Invoke(default);
            }
        }

        public void ReadPartData<T>(string docKey, Action<T> callback)
        {
            try
            {
                var raw = File.ReadAllText(GetPartDataPath(docKey));
                callback.Invoke(JsonHelper.FromJson<T>(raw));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                callback.Invoke(default);
            }
        }

        private static string GetUserDataPath()
        {
            var path = Path.Combine(Application.persistentDataPath, "UserData.json");
            return path;
        }

        private static string GetPartDataPath(string docKey)
        {
            var path = Path.Combine(Application.persistentDataPath, docKey + ".json");
            return path;
        }

        private static string GetStatisticsPath()
        {
            var path = Path.Combine(Application.persistentDataPath, "statistics.json");
            return path;
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
            ShowAd(AdType.Rewarded, callback);
        }

        public void ShowAd(AdType type, Action<bool> callback)
        {
            Debug.Log($"Showing ad {type}");
            callback.Invoke(true);
        }

        public void GetReferralLink(Action<string> callback)
        {
            Debug.Log("GetReferralLink");
            callback.Invoke("https://www.google.com");
        }

        public void GetReferrer(Action<Referrer> callback)
        {
            callback.Invoke(new Referrer { firstName = "Fake referrer" });
        }

        public void GetReferees(Action<GetRefereesResult> callback, int pageSize = 50, string next = null)
        {
            callback(new GetRefereesResult
            {
                next = null,
                total = 0,
                page = Array.Empty<Referee>()
            });
        }

        public void GetStatistics(Action<Dictionary<string, Statistic>> callback)
        {
            callback.Invoke(ReadStatistics());
        }

        private static Dictionary<string, Statistic> ReadStatistics()
        {
            try
            {
                var raw = File.ReadAllText(GetStatisticsPath());
                return string.IsNullOrEmpty(raw)
                    ? new Dictionary<string, Statistic>()
                    : JsonHelper.FromJson<Dictionary<string, Statistic>>(raw);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return new Dictionary<string, Statistic>();
            }
        }

        public void UpdateStatistic(StatisticUpdate[] statisticUpdate)
        {
            try
            {
                var dictionary = ReadStatistics();
                foreach (var update in statisticUpdate)
                {
                    var def = new Statistic
                    {
                        version = 1,
                        resetIn = null
                    };
                    var elem = dictionary.GetValueOrDefault(update.name, def);
                    elem.value = update.value;
                    dictionary[update.name] = elem;
                }

                var userDataPath = GetStatisticsPath();
                Debug.Log($"Saving statistics to {userDataPath}");
                File.WriteAllText(userDataPath, JsonSerialization.ToJson(dictionary));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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
            Debug.Log($"OnPurchaseCallback: {value}");
        }

        public void ShowAdCallback(bool value)
        {
            Debug.Log($"ShowAdCallback: {value}");
        }

        public void GetReferralLinkCallback(string value)
        {
            Debug.Log($"GetReferralLinkCallback: {value}");
        }

        public void GetReferrerCallback(string value)
        {
            Debug.Log($"GetReferrerCallback: {value}");
        }

        public void GetRefereesCallback(string value)
        {
            Debug.Log($"GetRefereesCallback: {value}");
        }

        public void GetStatisticsCallback(string value)
        {
            Debug.Log($"GetStatisticsCallback: {value}");
        }
    }
}