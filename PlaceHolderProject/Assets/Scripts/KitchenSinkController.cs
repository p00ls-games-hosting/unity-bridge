using System;
using UnityEngine;
using UnityEngine.UIElements;

public class KitchenSinkController : MonoBehaviour
{
    private Button _getUserInfoButton;
    private Button _purchaseItemButton;
    private Label _log;

    public delegate void UserInfoAsked();
    
    public delegate void PurchaseItemAsked();
    
    public UserInfoAsked OnUserInfoAsked;
    public PurchaseItemAsked OnPurchaseItem;
    
    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _getUserInfoButton = uiDocument.rootVisualElement.Q<Button>("GetUserInfo");
        _purchaseItemButton = uiDocument.rootVisualElement.Q<Button>("PurchaseItem");
        _log = uiDocument.rootVisualElement.Q<Label>("Log");
        _getUserInfoButton.clicked += GetUserInfo;
        _purchaseItemButton.clicked += PurchaseItem;
    }

    private void PurchaseItem()
    {
        OnPurchaseItem?.Invoke();
    }

    public void Log(string message)
    {
        _log.text = message;
    }
    public void OnDisable()
    {
        _getUserInfoButton.clicked -= GetUserInfo;
        _purchaseItemButton.clicked -= PurchaseItem;
    }

    private void GetUserInfo()
    {
        OnUserInfoAsked?.Invoke();
    }
}