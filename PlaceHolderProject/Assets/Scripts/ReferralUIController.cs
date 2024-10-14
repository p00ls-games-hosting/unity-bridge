using UnityEngine;
using UnityEngine.UIElements;

public class ReferralUIController : MonoBehaviour
{
    private Button _getReferralLinkButton;
    private Button _getReferrerButton;
    private Button _getRefereesButton;

    public delegate void ActionAsked();
    
    public ActionAsked OnGetReferralLink;
    public ActionAsked OnGetReferrer;
    public ActionAsked OnGetReferees;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _getReferralLinkButton = uiDocument.rootVisualElement.Q<Button>("GetReferralLink");
        _getReferrerButton = uiDocument.rootVisualElement.Q<Button>("GetReferrer");
        _getRefereesButton = uiDocument.rootVisualElement.Q<Button>("GetReferees");
        _getReferralLinkButton.clicked += GetReferralLink;
        _getReferrerButton.clicked += GetReferrer;
        _getRefereesButton.clicked += GetReferees;
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