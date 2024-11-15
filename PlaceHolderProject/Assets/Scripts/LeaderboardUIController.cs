using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;


public class LeaderboardDataSource
{
    public string StatisticName = null;
}

public class LeaderboardUIController : MonoBehaviour
{
    private TextField _statisticName;
    private Button _getPosition;
    private readonly LeaderboardDataSource _source = new LeaderboardDataSource();

    public delegate void PositionAsked(string statistic);

    public event PositionAsked OnPositionAsked;

    private void OnEnable()
    {
        var tab = GetComponent<UIDocument>().rootVisualElement.Q<Tab>("Leaderboard");
        tab.dataSource = _source;
        _statisticName = tab.Q<TextField>("Statistic");
        _statisticName.SetBinding("value", new DataBinding
        {
            dataSourcePath = PropertyPath.FromName(nameof(LeaderboardDataSource.StatisticName)),
            bindingMode = BindingMode.TwoWay,
            updateTrigger = BindingUpdateTrigger.EveryUpdate
        });
        _getPosition = tab.Q<Button>("Position");
        _getPosition.clicked += AskPosition;
    }

    private void AskPosition()
    {
        OnPositionAsked?.Invoke(_source.StatisticName);
    }
}