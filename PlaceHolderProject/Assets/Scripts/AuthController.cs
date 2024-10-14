using UnityEngine;
using UnityEngine.UIElements;

public class AuthController : MonoBehaviour
{
    private Button _getUserInfoButton;
    private Button _purchaseItemButton;

    public delegate void UserInfoAsked();

    public delegate void PurchaseItemAsked();

    public UserInfoAsked OnUserInfoAsked;
    public PurchaseItemAsked OnPurchaseItem;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _getUserInfoButton = uiDocument.rootVisualElement.Q<Button>("GetUserInfo");
        _purchaseItemButton = uiDocument.rootVisualElement.Q<Button>("PurchaseItem");
        _getUserInfoButton.clicked += GetUserInfo;
        _purchaseItemButton.clicked += PurchaseItem;
    }

    private void PurchaseItem()
    {
        OnPurchaseItem?.Invoke();
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