using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    private Label _log;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        _log = uiDocument.rootVisualElement.Q<Label>("Log");
    }

    public void Log(string message)
    {
        _log.text = message;
    }
    
    public void AppendLog(string message)
    {
        _log.text = $"{_log.text}\n{message}";
    }
}