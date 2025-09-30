using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : BaseManager<PlayerMgr>
{
    public Dictionary<long, Player> playerClientIdDic = new Dictionary<long, Player>();
    public Dictionary<int, Player> playerLogicIdDic = new Dictionary<int, Player>();
    public Dictionary<int, List<long>> gameDic = new Dictionary<int, List<long>>();

    public Player GetPlayer(long clientId)
    {
        if (playerClientIdDic.TryGetValue(clientId, out Player player))
            return player;
        return null;
    }

    public Player GetPlayer(int logicId)
    {
        if (playerLogicIdDic.TryGetValue(logicId, out Player player))
            return player;
        return null;
    }

    private Player CreatePlayer(long clientId, LoginInfo loginInfo, WebSockets.ClientConnection client)
    {
        Debug.LogWarning($"创建玩家：call CreatePlayer {clientId}");
        if (playerClientIdDic.TryGetValue(clientId, out Player player))
            return player;
        else
        {
            //必须保证赋值顺序!
            player = new Player
            {
                gameType = loginInfo.gameType,
                macId = loginInfo.macId,
                IsOnline = true,
                client = client,
            };
            player.ClientId = clientId;
            playerClientIdDic[clientId] = player;
            playerLogicIdDic[playerClientIdDic[clientId].LogicId] = player;
            return player;
        }
    }

    public Player PlayerInsert(LoginInfo loginInfo, WebSockets.ClientConnection client)
    {
        long clientId = long.Parse(loginInfo.gameType.ToString() + loginInfo.macId.ToString());
        if (gameDic.ContainsKey(loginInfo.gameType))
            gameDic[loginInfo.gameType].Add(clientId);
        else
            gameDic[loginInfo.gameType] = new List<long> { clientId };

        //Model.Instance.GetGameName(loginInfo.gameType);

        if (playerClientIdDic.TryGetValue(clientId, out Player player))
        {
           
            if (!player.IsOnline)  // 断线，则重连
            {
                Debug.LogWarning($"玩家断线重连：  {clientId}");
                player.client = client;
                player.IsOnline = true;
            }
            else
            {
                //3秒内未接到消息则直接踢掉
                if (Utils.GetTimeStamp() - player.lastReceMesTime > 3000)
                {
                    MsgInfo msgInfo = new MsgInfo
                    {
                        cmd = (int)S2C_CMD.S2C_KickOut,
                        id = player.macId,
                    };
                    player.SendToClient(JsonConvert.SerializeObject(msgInfo));
                    player.client.Disconnect();
                    Debug.LogWarning($"旧玩家超时处理：  {clientId}");

                    player.client = client; // 新玩家
                }
                else
                {
                    MsgInfo msgInfo = new MsgInfo
                    {
                        cmd = (int)S2C_CMD.S2C_ConnectFail,
                        id = -1,
                    };
                    NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(msgInfo));

                    Debug.LogWarning($"玩家两家失败：  {clientId}");
                }


            }
           
            return player;
        }
        else
            return CreatePlayer(clientId, loginInfo, client);
    }

    public void PlayerDisconnect(WebSockets.ClientConnection client)
    {
        foreach (var player in playerClientIdDic.Values)
        {
            if (player.client == client)
            {
                player.IsOnline = false;
                int macId = player.macId;
                break;
            }
        }
    }
}
