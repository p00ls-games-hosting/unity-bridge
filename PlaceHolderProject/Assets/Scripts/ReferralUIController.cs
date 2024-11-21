using UnityEngine;
using UnityEngine.UIElements;

public class ReferralUIController : MonoBehaviour
{
    private Button _getReferralLinkButton;
    private Button _getReferrerButton;
    private Button _getRefereesButton;
    private Button _shareLinkButton;

    public delegate void ActionAsked();
    
    public ActionAsked OnGetReferralLink;
    public ActionAsked OnGetReferrer;
    public ActionAsked OnGetReferees;
    public ActionAsked OnShareLink;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var tab = GetComponent<UIDocument>().rootVisualElement.Q<Tab>("Referral");
        _getReferralLinkButton = tab.Q<Button>("GetReferralLink");
        _getReferrerButton = tab.Q<Button>("GetReferrer");
        _getRefereesButton = tab.Q<Button>("GetReferees");
        _shareLinkButton = tab.Q<Button>("Share");
        _getReferralLinkButton.clicked += GetReferralLink;
        _getReferrerButton.clicked += GetReferrer;
        _getRefereesButton.clicked += GetReferees;
        _shareLinkButton.clicked += ShareLink;
    }

    private void ShareLink()
    {
        OnShareLink?.Invoke();
    }

    private void GetReferrer()
    {
        OnGetReferrer?.Invoke();
    }

    private void GetReferralLink()
    {
        OnGetReferralLink?.Invoke();
    }

    private void GetReferees()
    {
        OnGetReferees?.Invoke();
    }

    public void OnDisable()
    {
        _getReferralLinkButton.clicked -= GetReferralLink;
        _getReferrerButton.clicked -= GetReferrer;
        _getRefereesButton.clicked -= GetReferees;
    }
}