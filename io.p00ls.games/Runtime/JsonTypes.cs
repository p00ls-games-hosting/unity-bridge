using System;
using System.Collections.Generic;

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

    [Serializable]
    public class PurchaseResult
    {
        public string paymentId;
        public PurchaseParams purchaseParams;
        public string status;
    }

    [Serializable]
    public class UserProfile
    {
        public string userId;
        public string firstName;
        public string username;
        public bool isPremium;
        public string game;
        public string referrer;
    }

    [Serializable]
    public class Referrer
    {
        public string firstName;
    }

    [Serializable]
    public class Referee
    {
        public string firstName;
        public DateTime createdAt;
    }

    [Serializable]
    public class GetRefereesResult
    {
        public List<Referee> page;
        public int total;
        public string next;
    }

    [Serializable]
    public class GetRefereesRequest
    {
        public string next;
        public int pageSize;
    }
}