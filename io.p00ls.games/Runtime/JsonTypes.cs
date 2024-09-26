using System;

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
    }
}