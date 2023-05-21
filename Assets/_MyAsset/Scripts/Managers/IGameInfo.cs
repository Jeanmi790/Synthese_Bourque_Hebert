using UnityEngine;

public interface IGameInfo
{
    float InGameTime { get; set; }
    float Score { get; set; }
    bool IsPlayerDead { get; set; }

}