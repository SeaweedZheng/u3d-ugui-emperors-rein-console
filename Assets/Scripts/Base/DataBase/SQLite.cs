using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Text;
using SBoxApi;
//各平台下数据库存储的绝对路径(通用)
//PC：sql = new SQLiteHelper("data source=" + Application.dataPath + "/Fire_Line.db");
//Mac：sql = new SQLiteHelper("data source=" + Application.dataPath + "/Fire_Line.db");
//Android：sql = new SQLiteHelper("URI=file:" + Application.persistentDataPath + "/Fire_Line.db");
//iOS：sql = new SQLiteHelper("data source=" + Application.persistentDataPath + "/Fire_Line.db");

//PC平台下的相对路径
//sql = new SQLiteHelper("data source="sqlite4unity.db");
//编辑器：Assets/sqlite4unity.db
//编译后：和AppName.exe同级的文件夹下，这里比較奇葩
//当然能够用更任意的方式sql = new SQLiteHelper("data source="D://SQLite//Fire_Line.db");
//确保路径存在就可以否则会错误发生

//假设是事先创建了一份数据库
//能够将这个数据库放置在StreamingAssets文件夹下然后再复制到
//Application.persistentDataPath + "/Fire_Line.db"路径就可以

public class SQLite : BaseManager<SQLite>
{
    SQLiteHelper sql;
    string dbPath;
    private Thread WriteBetDataThread = null;
    public Queue<SBoxJackpotBetCoinPush> sBoxJackpotBets = new Queue<SBoxJackpotBetCoinPush>();

    public void Init()
    {
#if UNITY_EDITOR//在unity编辑模式下
        dbPath = Application.dataPath + "/game_db.db";
#elif UNITY_ANDROID//ANDROID平台
        dbPath = Application.persistentDataPath + "/game_db.db";
#endif
        InitSqlConnection();
        CreateDefaultTable();
    }

    private void CreateDefaultTable()
    {
        CreatRecordTable();
        CreatBetTable();
        CreatWinRecordTable();
        CreatPlayerData();

        WriteBetDataThread = new Thread(new ThreadStart(WriteBetData))
        {
            IsBackground = true
        };
        WriteBetDataThread.Start();
    }

    private void WriteBetData()
    {
        while (true)
        {
            BetData betData = null;
            bool isInsert = false;
            if (sBoxJackpotBets.Count > 0)
            {
                var sBoxJackpotBet = sBoxJackpotBets.Dequeue();
                if (IOCanvasModel.Instance.BetDataDic.TryGetValue(sBoxJackpotBet.MachineId, out Dictionary<int, List<BetData>> seatBetDatas))
                {
                    if (seatBetDatas.ContainsKey(sBoxJackpotBet.SeatId))
                    {
                        var betDatas = seatBetDatas[sBoxJackpotBet.SeatId];

                        foreach (var item in betDatas)
                        {
                            if (item.scoreRate == sBoxJackpotBet.ScoreRate && item.jpPercent == sBoxJackpotBet.JpPercent)
                            {
                                betData = item;
                                break;
                            }
                        }
                        if (betData == null)
                        {
                            betData = new BetData();
                            seatBetDatas[sBoxJackpotBet.SeatId].Add(betData);
                            isInsert = true;
                        }

                    }
                    else
                    {
                        isInsert = true;
                        betData = new BetData();
                        seatBetDatas[sBoxJackpotBet.SeatId] = new List<BetData> { betData };
                    }
                }
                else
                {
                    isInsert = true;
                    seatBetDatas = new Dictionary<int, List<BetData>>();
                    betData = new BetData();
                    seatBetDatas[sBoxJackpotBet.SeatId] = new List<BetData> { betData };
                    IOCanvasModel.Instance.BetDataDic[sBoxJackpotBet.MachineId] = seatBetDatas;
                }
                betData.logicId = sBoxJackpotBet.MachineId;
                betData.seatId = sBoxJackpotBet.SeatId;
                betData.bet += sBoxJackpotBet.majorBet;
                betData.betPercent = sBoxJackpotBet.BetPercent;
                betData.scoreRate = sBoxJackpotBet.ScoreRate;
                betData.jpPercent = sBoxJackpotBet.JpPercent;
                if (isInsert)
                {
                    IOCanvasModel.Instance.DealWithBetData();
                    int count = GetOrderDataCount();
                    if (count == 100)
                        DeleteOneData();
                    InsertBetData(betData);
                }
                else
                    UpdateBetData(betData);
            }
            Thread.Sleep(50);
        }
    }

    void InitSqlConnection()
    {
        //创建数据库连接
#if UNITY_EDITOR//在unity编辑模式下
        sql = new SQLiteHelper("data source=" + dbPath);
#elif UNITY_ANDROID//ANDROID平台
        sql = new SQLiteHelper("URI=file:" + dbPath);
#endif
    }
    //-------------------------------------------------------------------------------------------------游戏数据表
    private void CreatRecordTable()
    {
        sql.CreateTable("order_data", new string[] { "id", "gameType", "macId", "seatId", "jackpotId", "wins", "orderId", "time", "finish" },
            new string[] { "INTEGER", "INTEGER", "INTEGER", "INTEGER", "INTEGER", "INTEGER", "INTEGER", "INTEGER", "INTEGER" });
    }

    private void CreatBetTable()
    {
        sql.CreateTable("bet_data", new string[] { "logicId", "seatId", "bet", "betPercent", "scoreRate", "jpPercent", "win", "grandWin", "grandTimes", "majorWin", "majorTimes", "minorWin", "minorTimes", "miniWin", "miniTimes" },
            new string[15] { "INTEGER", "INTEGER", "TEXT", "INTEGER", "INTEGER", "INTEGER", "INTERGER", "INTERGER", "INTERGER", "INTERGER", "INTERGER", "INTERGER", "INTERGER", "INTERGER", "INTERGER" });
    }

    private void CreatWinRecordTable()
    {

    }

    private void CreatPlayerData()
    {
        sql.CreateTable("player_data", new string[] { "clientId", "logicId", "gameType", "macId" },
            new string[] { "INTEGER", "INTEGER", "INTEGER", "INTEGER" });
    }

    public Dictionary<long, PlayerIdData> LoadPlayerData()
    {
        SqliteDataReader reader = sql.ExecuteQuery("SELECT * FROM player_data");
        Dictionary<long, PlayerIdData> clientLogicDic = new Dictionary<long, PlayerIdData>();
        while (reader.Read())
        {
            long clientId = reader.GetInt64(reader.GetOrdinal("clientId"));
            int logicId = reader.GetInt32(reader.GetOrdinal("logicId"));
            int gameType = reader.GetInt32(reader.GetOrdinal("gameType"));
            int macId = reader.GetInt32(reader.GetOrdinal("macId"));
            PlayerIdData playerIdData = new PlayerIdData
            {
                clientId = clientId,
                logicId = logicId,
                gameType = gameType,
                macId = macId
            };
            clientLogicDic[clientId] = playerIdData;
        }
        return clientLogicDic;
    }

    public Dictionary<int, PlayerIdData> LoadPlayerData1()
    {
        SqliteDataReader reader = sql.ExecuteQuery("SELECT * FROM player_data");
        Dictionary<int, PlayerIdData> clientLogicDic = new Dictionary<int, PlayerIdData>();
        while (reader.Read())
        {
            long clientId = reader.GetInt64(reader.GetOrdinal("clientId"));
            int logicId = reader.GetInt32(reader.GetOrdinal("logicId"));
            int gameType = reader.GetInt32(reader.GetOrdinal("gameType"));
            int macId = reader.GetInt32(reader.GetOrdinal("macId"));
            PlayerIdData playerIdData = new PlayerIdData
            {
                clientId = clientId,
                logicId = logicId,
                gameType = gameType,
                macId = macId
            };
            clientLogicDic[logicId] = playerIdData;
        }
        return clientLogicDic;
    }

    public int LoadCurIndex()
    {
        int index = 0;
        SqliteDataReader reader = sql.ExecuteQuery("SELECT id FROM order_data order by id desc limit 1");
        while (reader.Read())
            index = reader.GetInt32(reader.GetOrdinal("id"));
        index++;
        return index;
    }

    public List<OrderData> LoadDataWithHighestWins()
    {
        SqliteDataReader reader = sql.ExecuteQuery("SELECT * FROM order_data order by id desc limit 8");
        List<OrderData> orderDatas = new List<OrderData>();
        while (reader.Read())
        {
            OrderData orderData = new OrderData
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                gameType = reader.GetInt32(reader.GetOrdinal("gameType")),
                logicId = reader.GetInt32(reader.GetOrdinal("macId")),
                seatId = reader.GetInt32(reader.GetOrdinal("seatId")),
                jackpotId = reader.GetInt32(reader.GetOrdinal("jackpotId")),
                wins = reader.GetInt32(reader.GetOrdinal("wins")),
                orderId = reader.GetInt64(reader.GetOrdinal("orderId")),
                time = reader.GetInt64(reader.GetOrdinal("time")),
                finish = reader.GetInt32(reader.GetOrdinal("finish"))
            };
            orderDatas.Add(orderData);
        }
        orderDatas.Sort((a, b) => b.wins.CompareTo(a.wins)); // 按照wins降序排序
        return orderDatas;
    }

    public List<OrderData> LoadDataWithHighestWins(int jackpotId)
    {
        SqliteDataReader reader = sql.ExecuteQuery($"SELECT * FROM order_data where jackpotId = {jackpotId} order by wins desc limit 8");
        List<OrderData> orderDatas = new List<OrderData>();
        while (reader.Read())
        {
            OrderData orderData = new OrderData
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                gameType = reader.GetInt32(reader.GetOrdinal("gameType")),
                logicId = reader.GetInt32(reader.GetOrdinal("macId")),
                seatId = reader.GetInt32(reader.GetOrdinal("seatId")),
                jackpotId = reader.GetInt32(reader.GetOrdinal("jackpotId")),
                wins = reader.GetInt32(reader.GetOrdinal("wins")),
                orderId = reader.GetInt64(reader.GetOrdinal("orderId")),
                time = reader.GetInt64(reader.GetOrdinal("time")),
                finish = reader.GetInt32(reader.GetOrdinal("finish"))
            };
            orderDatas.Add(orderData);
        }
        return orderDatas;
    }

    public OrderData LoadDataById(int id)
    {
        SqliteDataReader reader = sql.ExecuteQuery($"SELECT * FROM order_data where id = {id}");
        OrderData orderData = null;
        while (reader.Read())
        {
            orderData = new OrderData();
            orderData.id = reader.GetInt32(reader.GetOrdinal("id"));
            orderData.logicId = reader.GetInt32(reader.GetOrdinal("macId"));
            orderData.seatId = reader.GetInt32(reader.GetOrdinal("seatId"));
            orderData.jackpotId = reader.GetInt32(reader.GetOrdinal("jackpotId"));
            orderData.wins = reader.GetInt32(reader.GetOrdinal("wins"));
            orderData.orderId = reader.GetInt64(reader.GetOrdinal("orderId"));
            orderData.time = reader.GetInt64(reader.GetOrdinal("time"));
            orderData.finish = reader.GetInt32(reader.GetOrdinal("finish"));
        }
        return orderData;
    }

    public void InsertOrderData(OrderData orderData)
    {
        string[] saveValue = new string[] { orderData.id.ToString(), orderData.gameType.ToString(), orderData.logicId.ToString(), orderData.seatId.ToString(), orderData.jackpotId.ToString(),
            orderData.wins.ToString(), orderData.orderId.ToString(), orderData.time.ToString(), orderData.finish.ToString()};
        InsertValues("order_data", saveValue);
    }

    public void InsertPlayerData(PlayerIdData playerIdData)
    {
        string[] saveValue = new string[] { playerIdData.clientId.ToString(), playerIdData.logicId.ToString(), playerIdData.gameType.ToString(), playerIdData.macId.ToString() };
        InsertValues("player_data", saveValue);
    }

    public void UpdateOrderData(OrderData orderData)
    {
        string[] saveName = new string[] { "finish" };
        string[] saveValue = new string[] { orderData.finish.ToString() };
        string[] whereName = new string[] { "orderId" };
        string[] operations = new string[] { "=" };
        string[] whereValue = new string[] { orderData.orderId.ToString() };
        UpdataData("order_data", saveName, saveValue, whereName, operations, whereValue);
    }

    public List<OrderData> ReadUnfinishOrderData()
    {
        SqliteDataReader reader = sql.ExecuteQuery("SELECT * FROM order_data where finish = 0");
        List<OrderData> orderDatas = new List<OrderData>();
        while (reader.Read())
        {
            OrderData orderData = new OrderData();
            orderData.id = reader.GetInt32(reader.GetOrdinal("id"));
            orderData.gameType = reader.GetInt32(reader.GetOrdinal("gameType"));
            orderData.logicId = reader.GetInt32(reader.GetOrdinal("macId"));
            orderData.seatId = reader.GetInt32(reader.GetOrdinal("seatId"));
            orderData.jackpotId = reader.GetInt32(reader.GetOrdinal("jackpotId"));
            orderData.wins = reader.GetInt32(reader.GetOrdinal("wins"));
            orderData.orderId = reader.GetInt64(reader.GetOrdinal("orderId"));
            orderData.time = reader.GetInt64(reader.GetOrdinal("time"));
            orderData.finish = reader.GetInt32(reader.GetOrdinal("finish"));
            orderDatas.Add(orderData);
        }
        return orderDatas;
    }

    public int GetOrderDataCount()
    {
        int count = 0;
        SqliteDataReader reader = sql.ExecuteQuery("SELECT COUNT(*) FROM order_data");
        while (reader.Read())
            count = reader.GetInt32(reader.GetOrdinal("COUNT(*)"));
        return count;
    }

    public int GetLogicIdCount(int logicId)
    {
        int count = 0;
        SqliteDataReader reader = sql.ExecuteQuery($"SELECT COUNT(*) FROM player_data where logicId = {logicId}");
        while (reader.Read())
            count = reader.GetInt32(reader.GetOrdinal("COUNT(*)"));
        return count;
    }

    public void DeleteOneData()
    {
        int id = -1;
        SqliteDataReader reader = sql.ExecuteQuery("SELECT id FROM order_data order by id limit 1");
        while (reader.Read())
            id = reader.GetInt32(reader.GetOrdinal("id"));
        sql.ExecuteQuery($"DELETE FROM order_data where id = {id}");
    }

    public void InsertBetData(BetData betData)
    {
        string[] saveValue = new string[] { betData.logicId.ToString(), betData.seatId.ToString(), betData.bet.ToString(),
            betData.betPercent.ToString(), betData.scoreRate.ToString(), betData.jpPercent.ToString(), betData.win.ToString(),
            betData.grandWin.ToString(), betData.grandTimes.ToString(), betData.majorWin.ToString(), betData.majorTimes.ToString(),
            betData.minorWin.ToString(), betData.minorTimes.ToString(), betData.miniWin.ToString(), betData.miniTimes.ToString()};
        InsertValues("bet_data", saveValue);
    }

    public void UpdateBetData(BetData betData)
    {
        string[] saveName = new string[] { "bet", "betPercent", "scoreRate", "jpPercent", "win", "grandWin", "grandTimes", "majorWin", "majorTimes", "minorWin", "minorTimes", "miniWin", "miniTimes" };
        string[] saveValue = new string[] { betData.bet.ToString(), betData.betPercent.ToString(), betData.scoreRate.ToString(), betData.jpPercent.ToString(), betData.win.ToString(),
            betData.grandWin.ToString(), betData.grandTimes.ToString(), betData.majorWin.ToString(), betData.majorTimes.ToString(), betData.minorWin.ToString(), betData.minorTimes.ToString(), betData.miniWin.ToString(), betData.miniTimes.ToString()};
        string[] whereName = new string[] { "logicId", "seatId", "scoreRate", "jpPercent" };
        string[] operations = new string[] { "=", "=", "=", "=" };
        string[] whereValue = new string[] { betData.logicId.ToString(), betData.seatId.ToString(), betData.scoreRate.ToString(), betData.jpPercent.ToString() };
        UpdataData("bet_data", saveName, saveValue, whereName, operations, whereValue);
    }

    public Dictionary<int, Dictionary<int, List<BetData>>> ReadAllBetData()
    {
        SqliteDataReader reader = sql.ExecuteQuery("SELECT * FROM bet_data");
        Dictionary<int, Dictionary<int, List<BetData>>> betDataDic = new Dictionary<int, Dictionary<int, List<BetData>>>();
        while (reader.Read())
        {
            BetData betData = new BetData
            {
                logicId = reader.GetInt32(reader.GetOrdinal("logicId")),
                seatId = reader.GetInt32(reader.GetOrdinal("seatId")),
                bet = long.Parse(reader.GetString(2)),
                betPercent = reader.GetInt32(reader.GetOrdinal("betPercent")),
                scoreRate = reader.GetInt32(reader.GetOrdinal("scoreRate")),
                jpPercent = reader.GetInt32(reader.GetOrdinal("jpPercent")),
                win = reader.GetInt32(reader.GetOrdinal("win")),
                grandWin = reader.GetInt32(reader.GetOrdinal("grandWin")),
                grandTimes = reader.GetInt32(reader.GetOrdinal("grandTimes")),
                majorWin = reader.GetInt32(reader.GetOrdinal("majorWin")),
                majorTimes = reader.GetInt32(reader.GetOrdinal("majorTimes")),
                minorWin = reader.GetInt32(reader.GetOrdinal("minorWin")),
                minorTimes = reader.GetInt32(reader.GetOrdinal("minorTimes")),
                miniWin = reader.GetInt32(reader.GetOrdinal("miniWin")),
                miniTimes = reader.GetInt32(reader.GetOrdinal("miniTimes")),
            };
            if (betDataDic.TryGetValue(betData.logicId, out Dictionary<int, List<BetData>> seatBetData))
            {
                if (seatBetData.TryGetValue(betData.seatId, out List<BetData> seatBetList))
                    seatBetData[betData.seatId].Add(betData);
                else
                    seatBetData[betData.seatId] = new List<BetData> { betData };
            }
            else
            {
                Dictionary<int, List<BetData>> seatBetDataDic = new Dictionary<int, List<BetData>>
                {
                    [betData.seatId] = new List<BetData> { betData }
                };
                betDataDic[betData.logicId] = seatBetDataDic;
            }
        }
        return betDataDic;
    }

    public void InsertValues(string tableName, string[] saveValue)
    {
        sql.InsertValues(tableName, saveValue);
    }

    public void UpdataData(string tableName, string[] saveName, string[] saveValue, string[] whereName, string[] operations, string[] whereValue)
    {
        sql.UpdateTable(tableName, saveName, saveValue, whereName, operations, whereValue);
    }

    public void CloseConnection()
    {
        sql.CloseConnection();
    }

    public void ClearAllData()
    {
        sql.ClearTable("order_data");
        sql.ClearTable("bet_data");
    }
}

public class BetData
{
    public int logicId;
    public int seatId;
    public long bet;
    public int betPercent;
    public int scoreRate;
    public int jpPercent;
    public long win;
    public long grandWin;
    public int grandTimes;
    public long majorWin;
    public int majorTimes;
    public long minorWin;
    public int minorTimes;
    public long miniWin;
    public int miniTimes;
}

public class OrderData
{
    public int id;
    public int gameType;
    public int logicId;
    public int seatId;
    public int jackpotId;
    public int wins;
    public long orderId;
    public long time;
    public int finish;
}

public class PlayerIdData
{
    public long clientId;
    public int logicId;
    public int gameType;
    public int macId;
}
