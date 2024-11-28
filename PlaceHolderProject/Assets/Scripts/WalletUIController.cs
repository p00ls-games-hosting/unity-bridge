using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WalletUIController: MonoBehaviour
{
    private Button _fetchWalletButton;
    private Button _changeWalletButton;
    
    public delegate void WalletFetchAsked();
    public delegate void WalletChangeAsked();
    
    public WalletFetchAsked OnWalletFetchAsked;
    public WalletChangeAsked OnWalletChangeAsked;

    private void OnEnable()
    {
        var tab = GetComponent<UIDocument>().rootVisualElement.Q<Tab>("Wallet");
        _fetchWalletButton = tab.Q<Button>("FetchWallet");
        _changeWalletButton = tab.Q<Button>("ChangeWallet");
        _fetchWalletButton.clicked += FetchWallet;
        _changeWalletButton.clicked += ChangeWallet;
    }

    private void ChangeWallet()
    {
        OnWalletChangeAsked?.Invoke();
    }

    private void FetchWallet()
    {
        OnWalletFetchAsked?.Invoke();
    }
}