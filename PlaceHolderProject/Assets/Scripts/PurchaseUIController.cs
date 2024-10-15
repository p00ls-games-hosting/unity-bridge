using UnityEngine;
using UnityEngine.UIElements;

public class PurchaseUIController : MonoBehaviour
{
    private Button _purchaseItemButton;


    public delegate void PurchaseItemAsked();

    public PurchaseItemAsked OnPurchaseItem;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _purchaseItemButton = uiDocument.rootVisualElement.Q<Button>("PurchaseItem");
        _purchaseItemButton.clicked += PurchaseItem;
    }

    private void PurchaseItem()
    {
        OnPurchaseItem?.Invoke();
    }

    public void OnDisable()
    {
        _purchaseItemButton.clicked -= PurchaseItem;
    }

}