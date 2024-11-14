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
	private Button _loadPartDataButton;
    private Button _savePartDataButton;

    public delegate void LoadUserData();
    
    public delegate void SaveUserData(UserData userData);

	public delegate void LoadPartData(string docKey);
    
    public delegate void SavePartData(string docKey, UserData userData);
    
    public event LoadUserData OnLoadUserData;
    
    public event SaveUserData OnSaveUserData;

	public event LoadPartData OnLoadPartData;
    
    public event SavePartData OnSavePartData;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _loadDataButton = uiDocument.rootVisualElement.Q<Button>("LoadUserData");
        _saveDataButton = uiDocument.rootVisualElement.Q<Button>("SaveUserData");
        _saveDataButton.clicked += Save;
        _loadDataButton.clicked += Load;

		_loadPartDataButton = uiDocument.rootVisualElement.Q<Button>("ReadPartData");
        _savePartDataButton = uiDocument.rootVisualElement.Q<Button>("SavePartData");
        _savePartDataButton.clicked += SavePart;
        _loadPartDataButton.clicked += LoadPart;
    }

    public void OnDisable()
    {
        _loadDataButton.clicked -= Load;
        _saveDataButton.clicked -= Save;
        _loadPartDataButton.clicked -= LoadPart;
        _savePartDataButton.clicked -= SavePart;
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

	private void LoadPart()
    {
        OnLoadPartData?.Invoke("testKey");
    }

    private void SavePart()
    {
        OnSavePartData?.Invoke("testKey", new UserData
        {
            userName = "A User fancy name",
            score = (int)(Random.value * 10),
        });
    }


}