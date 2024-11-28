using UnityEngine;
using UnityEngine.UIElements;


public class MiniAppUIController : MonoBehaviour
{
    private Button _openURLButton;
    private Button _shareURLButton;

    public delegate void OpenURLAsked(string url);

    public delegate void ShareURLAsked(string url);

    public OpenURLAsked OnOpenURL;
    public ShareURLAsked OnShareURL;

    public void OnEnable()
    {
        var tab = GetComponent<UIDocument>().rootVisualElement.Q<Tab>("MiniApp");
        _openURLButton = tab.Q<Button>("OpenLink");
        _shareURLButton = tab.Q<Button>("ShareLink");
        _openURLButton.clicked += OpenURL;
        _shareURLButton.clicked += ShareURL;
    }

    private void ShareURL()
    {
        OnShareURL?.Invoke("https://p00ls.games");
    }

    private void OpenURL()
    {
        OnOpenURL?.Invoke("https://p00ls.games");
    }
}