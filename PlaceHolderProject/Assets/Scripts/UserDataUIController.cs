using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


[Serializable]
public class UserData
{
    public string userName;
    public int score;
}
public class UserDataUIController: MonoBehaviour
{
    private Button _loadDataButton;
    private Button _saveDataButton;

    public delegate void LoadUserData();
    
    public delegate void SaveUserData(UserData userData);
    
    public event LoadUserData OnLoadUserData;
    
    public event SaveUserData OnSaveUserData;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _loadDataButton = uiDocument.rootVisualElement.Q<Button>("LoadUserData");
        _saveDataButton = uiDocument.rootVisualElement.Q<Button>("SaveUserData");
        _saveDataButton.clicked += Save;
        _loadDataButton.clicked += Load;
    }

    public void OnDisable()
    {
        _loadDataButton.clicked -= Load;
        _saveDataButton.clicked -= Save;
    }

    private void Load()
    {
        OnLoadUserData?.Invoke();
    }

    private void Save()
    {
        OnSaveUserData?.Invoke(new UserData()
        {
            userName = "A User fancy name",
            score = (int)(Random.value * 10),
        });
    }


}