using System;
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


    private void Awake()
    {
        _mainUIController = FindFirstObjectByType<MainUIController>();
        _auth = FindFirstObjectByType<AuthController>();
        _referralUI = FindFirstObjectByType<ReferralUIController>();
        _purchaseUI = FindFirstObjectByType<PurchaseUIController>();
        _userDataUI = FindFirstObjectByType<UserDataUIController>();

        _auth.OnUserInfoAsked += OnUserInfoAsked;
        _purchaseUI.OnPurchaseItem += OnPurchaseItem;

        _referralUI.OnGetReferrer += OnGetReferrer;
        _referralUI.OnGetReferralLink += OnGetReferralLink;
        _referralUI.OnGetReferees += OnGetReferees;

        _userDataUI.OnLoadUserData += LoadUserData;
        _userDataUI.OnSaveUserData += SaveUserData;
        
        _userDataUI.OnLoadPartData += LoadPartData;
        _userDataUI.OnSavePartData += SavePartData;

        sdk.OnPurchase += OnPurchaseDone;
    }

    private void OnGetReferees()
    {
        sdk.GetReferees(null, referees =>
        {
            _mainUIController.Log($"Total: {referees.total}");
            _mainUIController.AppendLog($"Next: {referees.next}");
            foreach (var referee in referees.page)
            {
                _mainUIController.AppendLog(
                    $"Referee: {referee.firstName}, {referee.createdAt.ToString(CultureInfo.InvariantCulture)}");
            }
        });
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