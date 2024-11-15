namespace P00LS.Games
{
    internal interface IReceiver
    {
        void GetUserDataCallback(string value);

        void GetUserDataFallback(string error);

        void GetIdTokenCallback(string value);

        void OnPurchaseCallback(string value);

        void ShowAdCallback(bool value);

        void GetReferralLinkCallback(string value);

        void GetReferrerCallback(string value);

        void GetRefereesCallback(string value);

        void GetStatisticsCallback(string value);
        
        void GetUserPositionCallback(string value);
        
        void GetLeaderboardCallback(string value);
    }

    internal interface IInternalBridge : IBridge, IReceiver
    {
    }
}