namespace P00LS.Games
{
    internal interface IReceiver
    {
        void GetUserDataCallback(string value);

        void GetUserDataFallback(string error);

        void GetIdTokenCallback(string value);

        void OnPurchaseCallback(string value);

        void ShowAdCallback(bool value);
    }

    internal interface IInternalBridge : IBridge, IReceiver
    {
    }
}