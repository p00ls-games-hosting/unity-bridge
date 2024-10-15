using UnityEngine;
using UnityEngine.UIElements;

public class AuthController : MonoBehaviour
{
    private Button _getUserInfoButton;

    public delegate void UserInfoAsked();


    public UserInfoAsked OnUserInfoAsked;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _getUserInfoButton = uiDocument.rootVisualElement.Q<Button>("GetUserInfo");
        _getUserInfoButton.clicked += GetUserInfo;
    }

    public void OnDisable()
    {
        _getUserInfoButton.clicked -= GetUserInfo;
    }

    private void GetUserInfo()
    {
        OnUserInfoAsked?.Invoke();
    }
}