using GameUtil;
using SBoxApi;
using UnityEngine;

public class SandboxController : BaseManager<SandboxController>
{
    private DelayTimer coinInTimer;


    public void Init()
    {
        AddEventListener();
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener(EventHandle.PLAYER_COIN_IN, OnPlayerCoinIn);
    }

    private void OnPlayerCoinIn()
    {
        IOCanvasModel.Instance.coinIning = true;
        if (coinInTimer == null)
            coinInTimer = Timer.DelayAction(2, () => {
                IOCanvasModel.Instance.coinIning = false;
            });
        else
            coinInTimer.Restart();
    }
}
