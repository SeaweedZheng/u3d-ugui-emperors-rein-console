

public class Player
{
    //唯一Id
    private long _clientId;
    public long ClientId
    {
        set 
        { 
            _clientId = value;
            if (SQLiteModel.Instance.PlayerIdDataClientIdDic.ContainsKey(value))
                _logicId = SQLiteModel.Instance.PlayerIdDataClientIdDic[value].logicId;
            else
            {
                int ran = Utils.GetRandom(10000000, 99999999);
                while (SQLite.Instance.GetLogicIdCount(ran) != 0)
                    ran = Utils.GetRandom(10000000, 99999999);
                _logicId = ran;
                PlayerIdData playerIdData = new PlayerIdData
                {
                    clientId = value,
                    logicId = _logicId,
                    gameType = _gameType,
                    macId = _macId,
                };
                SQLiteModel.Instance.PlayerIdDataClientIdDic[value] = playerIdData;
                SQLiteModel.Instance.PlayerIdDataLogicIdDic[playerIdData.logicId] = playerIdData;
                SQLite.Instance.InsertPlayerData(playerIdData);
            }
        }
        get { return _clientId; }
    }

    private int _logicId;
    public int LogicId
    {
        get { return _logicId; }
    }

    //游戏类型
    private int _gameType;
    public int gameType
    {
        set { _gameType = value; }
        get { return _gameType; }
    }


    //机台号
    private int _macId;
    public int macId
    {
        set { _macId = value; }
        get { return _macId; }
    }
    private WebSockets.ClientConnection _client;

    public WebSockets.ClientConnection client
    {
        set { _client = value; }
        get { return _client; }
    }

    private bool isOnline;
    public bool IsOnline
    {
        set
        {
            isOnline = value;
        }
        get { return isOnline; }
    }

    public long lastReceMesTime;

    public void SendToClient(string data)
    {
        NetMgr.Instance.SendToClient(client, data);
    }
}
