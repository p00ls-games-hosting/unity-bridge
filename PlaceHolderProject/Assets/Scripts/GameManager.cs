using System;
using P00LS.Games;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private P00LSGamesSdk sdk;
    [SerializeField] private KitchenSinkController ui;

    private void Awake()
    {
        ui.OnUserInfoAsked += OnUserInfoAsked;
        ui.OnPurchaseItem += OnPurchaseItem;
        sdk.OnPurchase += OnPurchaseDone;
    }

    private void OnPurchaseDone(PurchaseResult obj)
    {
        var item = obj.purchaseParams = (PurchaseItemParams) obj.purchaseParams;
        ui.Log(JsonUtility.ToJson(obj) + "\n" + JsonUtility.ToJson(item));
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

    private void OnDestroy()
    {
        ui.OnUserInfoAsked -= OnUserInfoAsked;
        ui.OnPurchaseItem -= OnPurchaseItem;
    }

    private void OnUserInfoAsked()
    {
        var user = sdk.GetUserProfile();
        ui.Log(JsonUtility.ToJson(user));
    }
}