using System;
using JetBrains.Annotations;

namespace P00LS.Games
{
    [Serializable]
    public abstract class PurchaseParams
    {
        public int price;
        public int quantity;
    }

    [Serializable]
    public class PurchaseItemParams : PurchaseParams
    {
        public string itemId;
        public string title;
        public string description;
    }

    [Serializable]
    public class PurchaseCurrencyParams : PurchaseParams
    {
        public string symbol;
        public string name;
    }

    public struct PurchaseResult
    {
        public string paymentId;
        public PurchaseParams purchaseParams;
        public string status;
    }

    [Serializable]
    public struct UserProfile
    {
        public string userId;
        public string firstName;
        public string username;
        public bool isPremium;
        public string game;
        public string referrer;
    }

    [Serializable]
    public struct Referrer
    {
        public string firstName;
    }

    [Serializable]
    public struct Referee
    {
        public string firstName;
        public DateTime createdAt;
    }

    public struct GetRefereesResult
    {
        public Referee[] page;
        public int total;
        public string next;
    }

    [Serializable]
    public struct Statistic
    {
        public long value;
        public int version;
        public long? resetIn;
    }

    public struct UserLeaderboardPosition
    {
        public int position;
        public long value;
        public int version;
        public long? resetIn;
    }

    public struct LeaderboardEntry
    {
        public long userId;
        public int position;
        public long value;
        public string displayName;
    }

    public struct Leaderboard
    {
        public LeaderboardEntry[] entries;
        public int version;
        public long? resetIn;
        [CanBeNull] public string next;
    }
}