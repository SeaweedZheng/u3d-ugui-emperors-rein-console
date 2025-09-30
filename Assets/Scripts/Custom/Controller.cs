using GameUtil;
using Newtonsoft.Json;
using SBoxApi;
using System.Collections.Generic;
using UnityEngine;

public class Controller : BaseManager<Controller>
{
    public View view;

    public void Init()
    {
        InputMgr.Instance.StartOrEndCheck(true);
        AddEventListener();
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener(EventHandle.INIT_VIEW_FINISH, OnInitViewFinish);
        EventCenter.Instance.AddEventListener(EventHandle.CLEAR_ALL_DATA, OnClearAllData);
        EventCenter.Instance.AddEventListener<SBoxJackpotBetCoinPush>(EventHandle.JACKPOT_BET, OnJackpotBet);
        EventCenter.Instance.AddEventListener<SBoxJackpotData>(SBoxEventHandle.SBOX_JACKPOT_HOST_INIT, OnJackpotHostInit);
        EventCenter.Instance.AddEventListener<SBoxJackpotData>(SBoxEventHandle.SBOX_JACKPOT_BET_HOST, OnJackpotBetHost);
        EventCenter.Instance.AddEventListener<SBoxJackpotConfigData>(SBoxEventHandle.SBOX_JACKPOT_READ_CONFIG, OnJackpotReadConfig);
        EventCenter.Instance.AddEventListener<SBoxJackpotInfoData>(SBoxEventHandle.SBOX_JACKPOT_GET_WITHDRAWAL_POINTS, OnJackpotGetWithdrawalPoints);
    }

    private void OnInitViewFinish()
    {
        Debug.Log("Call OnInitViewFinish");
        SBoxIdea.JackpotReadConfig();
        SBoxIdea.JackpotHostInit();
    }

    private void OnClearAllData()
    {
        SQLite.Instance.ClearAllData();
        Model.Instance.highestWinsOrderData.Clear();
        Model.Instance.unfinishOrderDatas.Clear();
        Model.Instance.showWinInfos.Clear();
        IOCanvasModel.Instance.betDatas.Clear();
        IOCanvasModel.Instance.BetDataDic.Clear();
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Fix] = SQLite.Instance.LoadDataWithHighestWins();
        Model.Instance.AddFakeData();
        SBoxIdea.JackpotHostInit();
    }

    private void OnJackpotHostInit(SBoxJackpotData sBoxJackpotData)
    {
        Debug.Log(JsonConvert.SerializeObject(sBoxJackpotData));
        Model.Instance.sBoxJackpotData = sBoxJackpotData;
        //view.InitJackpotViews(sBoxJackpotData.JackpotOut);
    }

    private void OnJackpotBetHost(SBoxJackpotData sBoxJackpotData)
    {
        Model.Instance.sBoxJackpotData = sBoxJackpotData;
        //view.UpdateJackpotViews(sBoxJackpotData.JackpotOut);

        MsgInfo msgInfo = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_JackpotBet,
            id = -1,
            jsonData = JsonConvert.SerializeObject(sBoxJackpotData)
        };
        InternetServerMgr.Instance.SendTransformMes(msgInfo);

        for (int i = 0; i < sBoxJackpotData.Lottery.Length; i++)
        {
            if (sBoxJackpotData.Lottery[i] == 1)
            {
                Player player = PlayerMgr.Instance.GetPlayer(sBoxJackpotData.MachineId);
                WinJackpotInfo winJackpotInfo = new WinJackpotInfo
                {
                    macId = player.macId,
                    seat = sBoxJackpotData.SeatId,
                    jackpotId = i,
                    win = sBoxJackpotData.Jackpotlottery[i],
                };
                WinJackpot(winJackpotInfo, player.gameType);
                Debug.LogError($"WinJackpot:{JsonConvert.SerializeObject(sBoxJackpotData)}");

                var betDatas = IOCanvasModel.Instance.BetDataDic[sBoxJackpotData.MachineId][sBoxJackpotData.SeatId];
                BetData betData = null;
                foreach (var item in betDatas)
                {
                    if (item.scoreRate == sBoxJackpotData.ScoreRate && item.jpPercent == sBoxJackpotData.JpPercent)
                    {
                        betData = item;
                        break;
                    }
                }
                if (betData == null) return;
                betData.win += winJackpotInfo.win;
                switch (winJackpotInfo.jackpotId)
                {
                    case 0:
                        betData.grandWin += winJackpotInfo.win;
                        betData.grandTimes++;
                        break;
                    case 1:
                        betData.majorWin += winJackpotInfo.win;
                        betData.majorTimes++;
                        break;
                    case 2:
                        betData.minorWin += winJackpotInfo.win;
                        betData.minorTimes++;
                        break;
                    case 3:
                        betData.miniWin += winJackpotInfo.win;
                        betData.miniTimes++;
                        break;

                }
                SQLite.Instance.UpdateBetData(betData);
                break;
            }
        }
        IOCanvasModel.Instance.isGetTrigger = true;
        SBoxIdea.JackpotGetWithdrawalPoints(0, new int[4]);
    }

    private void OnJackpotReadConfig(SBoxJackpotConfigData sBoxJackpotConfigData)
    {
        Debug.Log(JsonConvert.SerializeObject(sBoxJackpotConfigData));
        IOCanvasModel.Instance.JackCfgData = sBoxJackpotConfigData;
    }

    private void OnJackpotGetWithdrawalPoints(SBoxJackpotInfoData sBoxJackpotInfoData)
    {
        if (!IOCanvasModel.Instance.isGetTrigger)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("SaveSucceed"));
            //return;
        }
        //Debug.Log("SBoxJackpotInfoData:" + JsonConvert.SerializeObject(sBoxJackpotInfoData));
        //IOCanvasModel.Instance.triggers.Clear();
        //for (int i = 0; i < sBoxJackpotInfoData.Triggers.Length; i++)
        //    IOCanvasModel.Instance.triggers.Add(sBoxJackpotInfoData.Triggers[i]);
        //IOCanvasModel.Instance.rewardPoints.Clear();
        //for (int i = 0; i < sBoxJackpotInfoData.RewardPoints.Length; i++)
        //    IOCanvasModel.Instance.rewardPoints.Add(sBoxJackpotInfoData.RewardPoints[i]);
    }

    public void OnJackpotBet(SBoxJackpotBetCoinPush sBoxJackpotBet) //(SBoxJackpotBet sBoxJackpotBet)
    {
        //Dictionary<string, string> postDic = new Dictionary<string, string>
        //{
        //    { "record", JsonConvert.SerializeObject(sBoxJackpotBet) }
        //};
        //Utils.Post("http://192.168.3.152/api/bonus_bet", postDic);
        SQLite.Instance.sBoxJackpotBets.Enqueue(sBoxJackpotBet);

        SBoxIdea.JackpotBetHost(sBoxJackpotBet);
    }

    private void WinJackpot(WinJackpotInfo winJackpotInfo, int gameType)
    {
        var time = Utils.GetTimeStamp();
        var orderId = time;
        long clientId = long.Parse(gameType.ToString() + winJackpotInfo.macId.ToString());
        Player player = PlayerMgr.Instance.GetPlayer(clientId);
        OrderData orderData = new OrderData
        {
            id = Model.Instance.curOrderIdx,
            gameType = gameType,
            logicId = player.LogicId,
            seatId = winJackpotInfo.seat,
            jackpotId = winJackpotInfo.jackpotId,
            wins = winJackpotInfo.win,
            orderId = orderId,
            time = time,
            finish = 0
        };
        string winNumStr = (winJackpotInfo.win / 100).ToString();;
        ShowWinInfo showWinInfo = new ShowWinInfo
        {
            winId = winJackpotInfo.jackpotId,
            winNumStr = winNumStr,
        };
        Model.Instance.showWinInfos.Enqueue(showWinInfo);
        //if (Model.Instance.showWinInfos.Count == 1 && !Model.Instance.showingWin)
        //    view.ShowWin();

        CheckHighestWins(orderData);

        SQLite.Instance.InsertOrderData(orderData);
        Model.Instance.curOrderIdx++;
        List<OrderData> unfinishOrders;

        if (Model.Instance.unfinishOrderDatas.ContainsKey(player.LogicId) && Model.Instance.unfinishOrderDatas[player.LogicId] != null)
            unfinishOrders = Model.Instance.unfinishOrderDatas[player.LogicId];
        else
        {
            unfinishOrders = new List<OrderData>();
            Model.Instance.unfinishOrderDatas.Add(player.LogicId, unfinishOrders);
        }
        if (unfinishOrders.Count > 10)
            unfinishOrders.RemoveAt(0);
        unfinishOrders.Add(orderData);
        Debug.LogError(JsonConvert.SerializeObject(Model.Instance.unfinishOrderDatas));
        Debug.LogError($"WinJackpot 当前{player.LogicId}未完成的订单数量:{unfinishOrders.Count}");
        Debug.LogError($"玩家赢得彩金: {JsonConvert.SerializeObject(winJackpotInfo)}");
        //if (player == null)
        //    Debug.LogError("玩家不存在!");
        //if (!player.IsOnline)
        //    Debug.LogError("玩家不在线!");

        if (player != null && player.IsOnline)
        {
            winJackpotInfo.orderId = orderId;
            winJackpotInfo.time = time;
            winJackpotInfo.macId = player.macId;
            MsgInfo msgInfo = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_WinJackpot,
                id = player.macId,
                jsonData = JsonConvert.SerializeObject(winJackpotInfo)
            };
            Debug.LogError($"发送赢得彩金信息:{JsonConvert.SerializeObject(winJackpotInfo)}");
            player.SendToClient(JsonConvert.SerializeObject(msgInfo));
        }

        //Dictionary<string, string> postDic = new Dictionary<string, string>
        //{
        //    { "record", JsonConvert.SerializeObject(orderData) }
        //};
        //Utils.Post("http://192.168.3.152/api/bonus", postDic);
    }

    public void CheckHighestWins(OrderData orderData)
    {
        Debug.Log($"CheckHighestWinsBefore:{JsonConvert.SerializeObject(Model.Instance.highestWinsOrderData)}");
        if (Model.Instance.highestWinsOrderData.Count > 8)
            Model.Instance.highestWinsOrderData[orderData.jackpotId].RemoveAt(Model.Instance.highestWinsOrderData.Count - 1);
        Model.Instance.highestWinsOrderData[orderData.jackpotId].Add(orderData);
        Model.Instance.highestWinsOrderData[orderData.jackpotId].Sort((a, b) => b.wins.CompareTo(a.wins));
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData);
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Fix].Sort((a, b) => b.wins.CompareTo(a.wins));
        //view.CheckShowRecordTitle();
        Debug.Log($"CheckHighestWinsAfter:{JsonConvert.SerializeObject(Model.Instance.highestWinsOrderData)}");
    }
}

public class JackpotChangeData
{
    public int jackpotIdx;
    public int numIdx;
}
