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
    private Button _getLeaderboard;
    private Button _getLeaderboardAround;
    private readonly LeaderboardDataSource _source = new();

    public delegate void StatisticEventHandler(string statistic);
    

    public event StatisticEventHandler OnPositionAsked;
    
    public event StatisticEventHandler OnGlobalLeaderboardAsked;
    
    public event StatisticEventHandler OnLeaderboardAroundAsked;

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
        
        _getLeaderboard = tab.Q<Button>("GlobalLeaderboard");
        _getLeaderboard.clicked += AskGlobalLeaderboard;
        
        _getLeaderboardAround = tab.Q<Button>("LeaderboardAround");
        _getLeaderboardAround.clicked += AskLeaderboardAround;
    }

    private void AskLeaderboardAround()
    {
        OnLeaderboardAroundAsked?.Invoke(_source.StatisticName);
    }

    private void AskGlobalLeaderboard()
    {
        OnGlobalLeaderboardAsked?.Invoke(_source.StatisticName);
    }

    private void AskPosition()
    {
        OnPositionAsked?.Invoke(_source.StatisticName);
    }
}