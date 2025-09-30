using SBoxApi;
using System.Collections.Generic;
using UnityEngine;

public class Model : BaseManager<Model>
{

    public SBoxJackpotData sBoxJackpotData;

    public bool showingWin;
    public int curOrderIdx;
    public const int scrollNumAniType = 1;
    public Dictionary<int, List<OrderData>> highestWinsOrderData;
    public List<string> broadCastNames = new List<string> {"多机台组合式联机奖励系统" };
    public Dictionary<int, List<OrderData>> unfinishOrderDatas = new Dictionary<int, List<OrderData>>();
    public Queue<ShowWinInfo> showWinInfos = new Queue<ShowWinInfo>();

    public void DealWithUnFinishOrder(List<OrderData> orderDatas)
    {
        foreach (var orderData in orderDatas)
        {
            if (unfinishOrderDatas.ContainsKey(orderData.logicId) && unfinishOrderDatas[orderData.logicId] != null)
                unfinishOrderDatas[orderData.logicId].Add(orderData);
            else
            {
                unfinishOrderDatas[orderData.logicId] = new List<OrderData>
                {
                    orderData
                };
            }
        }
    }

    //public string GetGameName(int gameType)
    //{
    //    string gameName;
    //    if (IOCanvasModel.Instance.gameDataDic != null)
    //        gameName = IOCanvasModel.Instance.gameDataDic.TryGetValue(gameType, out string name) ? name : "未知";
    //    else
    //    {
    //        gameName = gameType switch
    //        {
    //            101 => "飞龙在天",
    //            102 => "PK至尊",
    //            103 => "海底狂鲨",
    //            105 => "欢乐水果",
    //            106 => "1001夜",
    //            107 => "海底狂鲨",
    //            108 => "海底狂鲨",
    //            109 => "欢乐水果",

    //            110 => "骰宝",
    //            111 => "钱坤运转",
    //            112 => "水果总动员",
    //            113 => "鳄鱼大亨",
    //            114 => "鳄鱼大亨",
    //            115 => "水果总动员",
    //            116 => "金船出海",
    //            117 => "PK至尊2",
    //            118 => "喜从天降2",
    //            119 => "十二生肖",
    //            _ => "未知",
    //        };
    //    }
    //    if (!broadCastNames.Contains(gameName))
    //        broadCastNames.Add(gameName);
    //    return gameName;
    //}

    public void AddFakeData()
    {
        if (highestWinsOrderData[(int)OrderDataMode.Fix].Count >= 6) return;
        Debug.LogError("call AddFakeData");
        var orderData0 = new OrderData
        {
            id = curOrderIdx,
            gameType = 101,
            logicId = 10100006,
            seatId = 8,
            jackpotId = 0,
            wins = 632600,
            orderId = 1734339268701,
            time = 1734339268701,
            finish = 1
        };
        highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData0);
        SQLite.Instance.InsertOrderData(orderData0);
        PlayerIdData playerIdData = new PlayerIdData
        {
            clientId = 10100006,
            logicId = 10100006,
            macId = 10100006,
            gameType = 101
        };
        SQLite.Instance.InsertPlayerData(playerIdData);
        curOrderIdx++;

        var orderData1 = new OrderData
        {
            id = curOrderIdx,
            gameType = 114,
            logicId = 11400002,
            seatId = 3,
            jackpotId = 0,
            wins = 352000,
            orderId = 1734339298228,
            time = 1734339298228,
            finish = 1
        };
        highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData1);
        SQLite.Instance.InsertOrderData(orderData1);
        PlayerIdData playerIdData1 = new PlayerIdData
        {
            clientId = 11400002,
            logicId = 11400002,
            macId = 11400002,
            gameType = 114
        };
        SQLite.Instance.InsertPlayerData(playerIdData1);
        curOrderIdx++;

        var orderData2 = new OrderData
        {
            id = curOrderIdx,
            gameType = 105,
            logicId = 10500003,
            seatId = 6,
            jackpotId = 0,
            wins = 412800,
            orderId = 1734339325919,
            time = 1734339325919,
            finish = 1
        };
        highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData2);
        SQLite.Instance.InsertOrderData(orderData2);
        PlayerIdData playerIdData2 = new PlayerIdData
        {
            clientId = 10500003,
            logicId = 10500003,
            macId = 10500003,
            gameType = 105
        };
        SQLite.Instance.InsertPlayerData(playerIdData2);
        curOrderIdx++;

        var orderData3 = new OrderData
        {
            id = curOrderIdx,
            gameType = 106,
            logicId = 10600005,
            seatId = 1,
            jackpotId = 1,
            wins = 32400,
            orderId = 1734337538235,
            time = 1734337538235,
            finish = 1
        };
        highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData3);
        SQLite.Instance.InsertOrderData(orderData3);
        PlayerIdData playerIdData3 = new PlayerIdData
        {
            clientId = 10600005,
            logicId = 10600005,
            macId = 10600005,
            gameType = 106
        };
        SQLite.Instance.InsertPlayerData(playerIdData3);
        curOrderIdx++;

        var orderData4 = new OrderData
        {
            id = curOrderIdx,
            gameType = 113,
            logicId = 11300003,
            seatId = 5,
            jackpotId = 2,
            wins = 20000,
            orderId = 1734337606816,
            time = 1734337606816,
            finish = 1
        };
        highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData4);
        SQLite.Instance.InsertOrderData(orderData4);
        PlayerIdData playerIdData4 = new PlayerIdData
        {
            clientId = 11300003,
            logicId = 11300003,
            macId = 11300003,
            gameType = 113
        };
        SQLite.Instance.InsertPlayerData(playerIdData4);
        curOrderIdx++;

        var orderData5 = new OrderData
        {
            id = curOrderIdx,
            gameType = 112,
            logicId = 11200001,
            seatId = 1,
            jackpotId = 3,
            wins = 7900,
            orderId = 1734337623033,
            time = 1734337623033,
            finish = 1
        };
        highestWinsOrderData[(int)OrderDataMode.Fix].Add(orderData5);
        SQLite.Instance.InsertOrderData(orderData5);
        PlayerIdData playerIdData5 = new PlayerIdData
        {
            clientId = 11200001,
            logicId = 11200001,
            macId = 11200001,
            gameType = 112
        };
        SQLite.Instance.InsertPlayerData(playerIdData5);
        curOrderIdx++;
    }
}

public class ShowWinInfo
{
    public int winId;
    public string winNumStr;
}
