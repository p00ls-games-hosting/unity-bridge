using P00LS.Games;
using UnityEngine;
using UnityEngine.UIElements;

public class StatisticsUIController : MonoBehaviour
{
    private Button _saveButton;
    private Button _loadButton;

    public delegate void StatisticsAsked();

    public delegate void StatisticsUpdateAsked(StatisticUpdate[] updates);


    public event StatisticsAsked OnStatisticsAsked;
    public event StatisticsUpdateAsked OnStatisticsUpdateAsked;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _saveButton = uiDocument.rootVisualElement.Q<Button>("Save");
        _saveButton.clicked += UpdateStatistics;

        _loadButton = uiDocument.rootVisualElement.Q<Button>("Load");
        _loadButton.clicked += GetStatistics;
    }

    public void OnDisable()
    {
        _saveButton.clicked -= UpdateStatistics;
        _loadButton.clicked -= GetStatistics;
    }

    private void GetStatistics()
    {
        OnStatisticsAsked?.Invoke();
    }

    private void UpdateStatistics()
    {
        OnStatisticsUpdateAsked?.Invoke(new StatisticUpdate[]
        {
            new()
            {
                name = "test_var_1",
                value = 100
                
            },
            new()
            {
                name = "test_var_2",
                value = 300
                
            }
        });
    }
}