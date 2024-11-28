using System;
using System.Collections.Generic;
using System.Globalization;
using P00LS.Games;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private P00LSGamesSdk sdk;
    private MainUIController _mainUIController;
    private AuthController _auth;
    private PurchaseUIController _purchaseUI;
    private ReferralUIController _referralUI;
    private UserDataUIController _userDataUI;
    private StatisticsUIController _statisticsUI;
    private LeaderboardUIController _leaderboardUI;
    private MiniAppUIController _miniAppUI;
    private WalletUIController _walletUI;


    private void Awake()
    {
        _mainUIController = FindFirstObjectByType<MainUIController>();
        _auth = FindFirstObjectByType<AuthController>();
        _referralUI = FindFirstObjectByType<ReferralUIController>();
        _purchaseUI = FindFirstObjectByType<PurchaseUIController>();
        _userDataUI = FindFirstObjectByType<UserDataUIController>();
        _statisticsUI = FindFirstObjectByType<StatisticsUIController>();
        _leaderboardUI = FindFirstObjectByType<LeaderboardUIController>();
        _miniAppUI = FindFirstObjectByType<MiniAppUIController>();
        _walletUI = FindFirstObjectByType<WalletUIController>();

        _auth.OnUserInfoAsked += OnUserInfoAsked;
        _purchaseUI.OnPurchaseItem += OnPurchaseItem;

        _referralUI.OnGetReferrer += OnGetReferrer;
        _referralUI.OnGetReferralLink += OnGetReferralLink;
        _referralUI.OnGetReferees += OnGetReferees;
        _referralUI.OnShareLink += OnShareLink;

        _userDataUI.OnLoadUserData += LoadUserData;
        _userDataUI.OnSaveUserData += SaveUserData;

        _userDataUI.OnLoadPartData += LoadPartData;
        _userDataUI.OnSavePartData += SavePartData;

        _statisticsUI.OnStatisticsAsked += LoadStatistics;
        _statisticsUI.OnStatisticsUpdateAsked += UpdateStatistics;

        _leaderboardUI.OnPositionAsked += GetUserPosition;
        _leaderboardUI.OnGlobalLeaderboardAsked += GetGlobalLeaderboard;
        _leaderboardUI.OnLeaderboardAroundAsked += GetLeaderboardAround;

        _miniAppUI.OnOpenURL += OpenURL;
        _miniAppUI.OnShareURL += ShareURL;

        _walletUI.OnWalletFetchAsked += FetchWallet;
        _walletUI.OnWalletChangeAsked += ChangeWallet;

        sdk.OnPurchase += OnPurchaseDone;
    }

    private void ChangeWallet()
    {
        sdk.InitiateWalletChange();
    }

    private void FetchWallet()
    {
        sdk.GetUserWalletAddress(address => _mainUIController.Log($"Wallet address: {address}"));
    }

    private void ShareURL(string url)
    {
        sdk.ShareURL(url);
    }

    private void OpenURL(string url)
    {
        sdk.OpenURL(url);
    }

    private void Start()
    {
        sdk.GetServerTime(date => _mainUIController.Log($"Server time: {date.ToString(CultureInfo.InvariantCulture)}"));
    }

    private void OnShareLink()
    {
        sdk.ShareReferralLink();
    }

    private void GetLeaderboardAround(string statistic)
    {
        sdk.GetLeaderboardAround(statistic, leaderboard =>
        {
            _mainUIController.Log("Leaderboard around");
            LogLeaderboard(leaderboard);
        });
    }

    private void GetGlobalLeaderboard(string statistic)
    {
        sdk.GetLeaderboard(statistic, leaderboard =>
        {
            _mainUIController.Log("Global leaderboard");
            LogLeaderboard(leaderboard);
        });
    }

    private void LogLeaderboard(Leaderboard leaderboard)
    {
        _mainUIController.AppendLog($"Version: {leaderboard.version}");
        _mainUIController.AppendLog($"Reset: {leaderboard.resetIn.HasValue}:{leaderboard.resetIn}");
        _mainUIController.AppendLog($"Next: {leaderboard.next != null}{leaderboard.next}");
        foreach (var leaderboardEntry in leaderboard.entries)
        {
            _mainUIController.AppendLog($"Entry: {leaderboardEntry.displayName}:{leaderboardEntry.position}");
        }
    }

    private void GetUserPosition(string statistic)
    {
        sdk.GetUserPosition(statistic, (result) =>
        {
            _mainUIController.Log("Position:");
            _mainUIController.AppendLog($"Position: {result?.position}, Value: {result?.value}");
        });
    }

    private void UpdateStatistics(Dictionary<string, long> updates)
    {
        sdk.UpdateStatistic(updates);
    }

    private void LoadStatistics()
    {
        sdk.GetStatistics(values =>
            {
                _mainUIController.Log("Statistics");
                foreach (var (statName, stat) in values)
                {
                    _mainUIController.AppendLog(
                        $"{statName}: {stat.value} // {stat.version}  Reset : {stat.resetIn.HasValue} {stat.resetIn}");
                }
            }
        );
    }

    private void OnGetReferees()
    {
        sdk.GetReferees(referees =>
        {
            _mainUIController.Log($"Total: {referees.total}");
            _mainUIController.AppendLog($"Next: {referees.next}");
            foreach (var referee in referees.page)
            {
                _mainUIController.AppendLog(
                    $"Referee: {referee.firstName}, {referee.createdAt.ToString(CultureInfo.InvariantCulture)}");
            }
        }, since: DateTime.Now);
    }

    private void OnGetReferralLink()
    {
        sdk.GetReferralLink(link => { _mainUIController.Log(link); });
    }

    private void OnGetReferrer()
    {
        sdk.GetReferrer(referrer => { _mainUIController.Log(JsonUtility.ToJson(referrer, true)); });
    }

    private void OnPurchaseDone(PurchaseResult obj)
    {
        var item = obj.purchaseParams = (PurchaseItemParams)obj.purchaseParams;
        _mainUIController.Log(JsonUtility.ToJson(obj) + "\n" + JsonUtility.ToJson(item));
    }

    private void OnPurchaseItem()
    {
        sdk.InitPurchase(new PurchaseItemParams
        {
            itemId = "itemId",
            description = "Test purchase",
            price = 1,
            quantity = 2,
            title = "Purchase"
        });
    }

    private void OnUserInfoAsked()
    {
        var user = sdk.GetUserProfile();
        _mainUIController.Log(JsonUtility.ToJson(user));
    }

    private void LoadUserData()
    {
        sdk.GetUserData<UserData>(data => _mainUIController.Log(JsonUtility.ToJson(data, true)));
    }

    private void SaveUserData(UserData userData)
    {
        sdk.SaveUserData(userData);
    }

    private void LoadPartData(string docKey)
    {
        sdk.ReadPartData<UserData>(docKey, data => _mainUIController.Log(JsonUtility.ToJson(data, true)));
    }

    private void SavePartData(string docKey, UserData userData)
    {
        sdk.SavePartData(docKey, userData);
    }
}