using System.Collections.Generic;
using P00LS.Games;
using UnityEngine;
using UnityEngine.UIElements;

public class StatisticsUIController : MonoBehaviour
{
    private Button _saveButton;
    private Button _loadButton;

    public delegate void StatisticsAsked();

    public delegate void StatisticsUpdateAsked(Dictionary<string, long> updates);


    public event StatisticsAsked OnStatisticsAsked;
    public event StatisticsUpdateAsked OnStatisticsUpdateAsked;

    public void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _saveButton = uiDocument.rootVisualElement.Q<Button>("SaveStatistics");
        _saveButton.clicked += UpdateStatistics;

        _loadButton = uiDocument.rootVisualElement.Q<Button>("LoadStatistics");
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
        OnStatisticsUpdateAsked?.Invoke(new Dictionary<string, long>
        {
            { "test_var_1", 100 },
            { "test_var_2", 300 },
        });
    }
}