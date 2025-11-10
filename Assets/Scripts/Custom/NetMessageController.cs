
#define NEW_NET_01

#if NEW_NET_01

using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using WebSockets;

public class NetMessageController : BaseManager<NetMessageController>
{

    public void Init()
    {

        RemoveEventListener();

        Messenger.AddListener<MsgInfo, ClientConnection>(MessageName.Event_ServerNetworkRecv, OnServerNetworkRecv);
        EventCenter.Instance.AddEventListener<ClientConnection>(EventHandle.PLAYER_DISCONNECT, OnPlayerDisconnect);
        EventCenter.Instance.AddEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnClinetReadConfR);
        EventCenter.Instance.AddEventListener<SBoxApi.SBoxJackpotData>(SBoxEventHandle.SBOX_JACKPOT_BET_HOST, OnSBoxJackpotData);

        SBoxIdea.ReadConf();
    }


    void RemoveEventListener()
    {
        Messenger.RemoveListener<MsgInfo, ClientConnection>(MessageName.Event_ServerNetworkRecv, OnServerNetworkRecv);
        EventCenter.Instance.RemoveEventListener<ClientConnection>(EventHandle.PLAYER_DISCONNECT, OnPlayerDisconnect);
        EventCenter.Instance.RemoveEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnClinetReadConfR);
        EventCenter.Instance.RemoveEventListener<SBoxApi.SBoxJackpotData>(SBoxEventHandle.SBOX_JACKPOT_BET_HOST, OnSBoxJackpotData); //彩金押注结果返回
    }


    /// <summary> 彩金押注结果-响应队列 </summary>
    Dictionary<long, Action<SBoxApi.SBoxJackpotData>> funcJackpotDataDic = new Dictionary<long, Action<SBoxJackpotData>>();




    void OnResponseDebug(S2C_CMD cmd, MsgInfo res)
    {
        string nam = Enum.GetName(typeof(S2C_CMD), cmd);
        Debug.LogWarning($"<color=yellow>server response</color>: {nam} = {JsonConvert.SerializeObject(res)}");
    }
    void OnRequestDebug(C2S_CMD cmd, MsgInfo req)
    {
        string nam = Enum.GetName(typeof(C2S_CMD), cmd);
        Debug.LogWarning($"<color=green>clinet request</color>: {nam} = {JsonConvert.SerializeObject(req)}");
    }



    /// <summary>
    /// 彩金押注结果返回
    /// </summary>
    /// <param name="data"></param>
    void OnSBoxJackpotData(SBoxApi.SBoxJackpotData data)
    {
        Debug.LogWarning($"当前值：curSBoxJackpotData = {JsonConvert.SerializeObject(SBoxIdea.curSBoxJackpotData)}");

        foreach (KeyValuePair<long, Action<SBoxApi.SBoxJackpotData>> item in funcJackpotDataDic)
        {
            item.Value?.Invoke(data);
        }
    }


    SBoxConfData curSBoxConfData;
    void OnClinetReadConfR(SBoxConfData res)
    {
        curSBoxConfData = res;
        Debug.LogError($"读到配置数据 {JsonConvert.SerializeObject(curSBoxConfData)}");
    }




    /// <summary>
    /// 监听到机台给过来的请求数据
    /// </summary>
    /// <param name="info"></param>
    /// <param name="client"></param>
    private void OnServerNetworkRecv(MsgInfo info, ClientConnection client)
    {

        //Debug.LogWarning($"接收到机台发来的数据： {JsonConvert.SerializeObject(info)}");


        OnRequestDebug((C2S_CMD)info.cmd, info);

        if (info.id == 0)
        {
            return;
        }

        switch ((C2S_CMD)info.cmd)
        {
            case C2S_CMD.C2S_Login:
                { 
                    OnLogin(info, client);                
                }
                break;
            case C2S_CMD.C2S_ReadConf:
                {
                    OnReadConf(info, client);
                }
                break;
            case C2S_CMD.C2S_JackBet: 
                { 
                    OnJackBet(info, client);                
                }
                break;
            case C2S_CMD.C2S_GetJackpotShowValue:
                {
                    // OnPlayerJackBet(info);
                    OnGetJackpotShowValue(info, client);
                }
                break;
            case C2S_CMD.C2S_ReceiveJackpot:  // 领奖
                {
                    OnReceiveJackpot(info);
                }
                break;
           
            case C2S_CMD.C2S_GetJackpotData: // 彩金查账
                {
                    OnGetJackpotData(info);
                }
                break;
            default:
                Debug.LogError($"未设置 {info.cmd} 响应");
                break;
        }
    }

    /// <summary>
    /// 机台登录
    /// </summary>
    /// <param name="info"></param>
    /// <param name="client"></param>
    private void OnLogin(MsgInfo info, ClientConnection client)
    {

        LoginInfo loginInfo;
        LoginInfoR loginInfoR = new LoginInfoR();

        MsgInfo res;

        try
        {
            loginInfo = JsonConvert.DeserializeObject<LoginInfo>(info.jsonData); //#seaweed# 这里改了
            loginInfoR.seqId = loginInfo.seqId;
        }
        catch (Exception e)
        {
            loginInfoR.code = 1;
            loginInfoR.msg = "输入数据格式有误";
            res = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_LoginR,
                id = info.id, // 机器id
                jsonData = JsonConvert.SerializeObject(loginInfoR)
            };
            NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
            OnResponseDebug((S2C_CMD)res.cmd, res);

            return;
        }


        Debug.LogError($"收到玩家登录信息：macId: {loginInfo.macId} ; LoginInfo: {info.jsonData}");



        if (IOCanvasModel.Instance.groupId != loginInfo.groudId)
        {
            // 组号不对
            Debug.LogError($"组号不对，拒绝登录：macId: {loginInfo.macId} ; LoginInfo: {info.jsonData}");

            loginInfoR.code = 1;
            loginInfoR.msg = "组号不对，拒绝登录";
            res = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_LoginR,
                id = info.id, // 机器id
                jsonData = JsonConvert.SerializeObject(loginInfoR)
            };
            NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
            OnResponseDebug((S2C_CMD)res.cmd, res);
            return;
        }


        if (IOCanvasModel.Instance.seatIdMacIdMap.ContainsKey(loginInfo.seatId)
            && IOCanvasModel.Instance.seatIdMacIdMap[loginInfo.seatId] != loginInfo.macId)
        {
            // 座位号重复
            Debug.LogError($"座位号重复，拒绝登录：macId: {loginInfo.macId} ; LoginInfo: {info.jsonData}");

            loginInfoR.code = 1;
            loginInfoR.msg = "座位号重复，拒绝登录";
            res = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_LoginR,
                id = info.id, // 机器id
                jsonData = JsonConvert.SerializeObject(loginInfoR)
            };
            NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
            OnResponseDebug((S2C_CMD)res.cmd, res);

            return;
        }

        IOCanvasModel.Instance.seatIdMacIdMap[loginInfo.seatId] = loginInfo.macId;


        Player player = PlayerMgr.Instance.PlayerInsert(loginInfo, client);


        /*
        List<int> minBets = new List<int>();

        try
        {
            //#seaweed# 【待解决】这里会爆错
            for (int i = 0; i < IOCanvasModel.Instance.JackCfgData.sBoxJackpotConfigDataItem.Length; i++)  
                minBets.Add(IOCanvasModel.Instance.JackCfgData.sBoxJackpotConfigDataItem[i].MinBet);
            minBets.Reverse();
        }
        catch (Exception e)
        {
        }
        */


        //#seaweed#  这里改了
        loginInfoR.code = 0;
        loginInfoR.msg = "登录成功";


        res = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_LoginR,
            id = player.macId, // 机器id
            jsonData = JsonConvert.SerializeObject(loginInfoR)
        };
        NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
        OnResponseDebug((S2C_CMD)res.cmd, res);

    }

    /// <summary>
    /// 读取彩金后台配置信息
    /// </summary>
    /// <param name="info"></param>
    /// <param name="client"></param>
    private void OnReadConf(MsgInfo info, ClientConnection client)
    {
        MsgInfo res;
        //JObject jsonData = new JObject();

        RequestBase requestInfo;
        ReadConfR readConfR = new ReadConfR();

        try
        {
            requestInfo = JsonConvert.DeserializeObject<RequestBase>(info.jsonData);
            readConfR.seqId = requestInfo.seqId;
        }
        catch (Exception e)
        {
            readConfR.code = 1;
            readConfR.msg =  "输入数据格式有误";
            res = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_ReadConfR,
                id = info.id, // 机器id
                jsonData = JsonConvert.SerializeObject(readConfR)
            };
            NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
            OnResponseDebug((S2C_CMD)res.cmd, res);

            return;
        }



        long clientId = long.Parse(requestInfo.gameType.ToString() + info.id.ToString());

        Player player = PlayerMgr.Instance.GetPlayer(clientId);


        readConfR.code = 0;
        readConfR.msg = "";
        readConfR.sboxConfData = curSBoxConfData;

        res = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_ReadConfR,
            id = player.macId,
            jsonData = JsonConvert.SerializeObject(readConfR)
        };
        NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
        OnResponseDebug((S2C_CMD)res.cmd, res);


    }





    /// <summary>
    /// 机台下注后，彩金的数据
    /// </summary>
    /// <param name="player"></param>
    /// <param name="seatId"></param>
    /// <param name="res1"></param>
    private void ReSendWinJackpot(Player player, int seatId, JackBetInfoCoinPushR res1)
    {

        MsgInfo msgInfo = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_JackBetR,
            id = player.macId,
            jsonData = JsonConvert.SerializeObject(res1)
        };
        player.SendToClient(JsonConvert.SerializeObject(msgInfo));

        // 
        /*
        if (Model.Instance.unfinishOrderDatas.TryGetValue(player.LogicId, out List<OrderData> orderDatas))
        {
            if (orderDatas.Count > 0)
            {
                WinJackpotInfo winJackpotInfo = new WinJackpotInfo
                {
                    macId = orderDatas[0].logicId,
                    seat = orderDatas[0].seatId,
                    jackpotId = orderDatas[0].jackpotId,
                    win = orderDatas[0].wins,
                    orderId = orderDatas[0].orderId,
                    time = orderDatas[0].time,
                };
                MsgInfo msgInfo = new MsgInfo
                {
                    cmd = (int)S2C_CMD.S2C_WinJackpot,
                    id = player.macId,
                    jsonData = JsonConvert.SerializeObject(winJackpotInfo)
                };
                player.SendToClient(JsonConvert.SerializeObject(msgInfo));
            }
        }*/
    }



    /// <summary>
    /// 彩金押注，获取押注结果
    /// </summary>
    /// <param name="info"></param>
    /// <param name="client"></param>
    private void OnJackBet(MsgInfo info, ClientConnection client)
    {
        if (true)
        {

            MsgInfo res;


            JackBetInfoCoinPush jpBet;
            JackBetInfoCoinPushR jackBetR = new JackBetInfoCoinPushR();
            try
            {
                jpBet = JsonConvert.DeserializeObject<JackBetInfoCoinPush>(info.jsonData);
                jackBetR.seqId = jpBet.seqId;
            }
            catch (Exception e)
            {
                jackBetR.code = 1;
                jackBetR.msg = "输入数据格式有误";
                res = new MsgInfo
                {
                    cmd = (int)S2C_CMD.S2C_JackBetR,
                    id = info.id, // 机器id
                    jsonData = JsonConvert.SerializeObject(jackBetR)
                };
                NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
                OnResponseDebug((S2C_CMD)res.cmd, res);

                return;
            }
        


            long clientId = long.Parse(jpBet.gameType.ToString() + info.id.ToString());
            Player player = PlayerMgr.Instance.GetPlayer(clientId);
            if (player == null)
            {
                Debug.LogError($"OnPlayerJackBet: Player not found for clientId: {clientId}");


                jackBetR.code = 1;
                jackBetR.msg = " Player not found for clientId";
                res = new MsgInfo
                {
                    cmd = (int)S2C_CMD.S2C_JackBetR,
                    id = info.id, // 机器id
                    jsonData = JsonConvert.SerializeObject(jackBetR)
                };
                NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
                OnResponseDebug((S2C_CMD)res.cmd, res);
                return;
            }




            funcJackpotDataDic.Add(clientId, (SBoxJackpotData rs) =>
            {
                // 保存当前的彩金值-其他机台查询用
                IOCanvasModel.Instance.CurJackpotOut = rs.JackpotOut;

                jackBetR.code = 0;
                jackBetR.sboxJackpotData = rs;

                ReSendWinJackpot(player, jpBet.seat, jackBetR);
                funcJackpotDataDic.Remove(clientId);
            });

            SBoxJackpotBetCoinPush sBoxJackpot = new SBoxJackpotBetCoinPush
            {
                MachineId = player.LogicId,
                SeatId = jpBet.seat,
                majorBet = jpBet.grandBet,
                grandBet = jpBet.majorBet,
                BetPercent = jpBet.betPercent, //100
                ScoreRate = jpBet.scoreRate, //1000
                JpPercent = jpBet.JPPercent, //1000

            };
            EventCenter.Instance.EventTrigger(EventHandle.JACKPOT_BET, sBoxJackpot);

        }
        else
        {
            var jackBetInfoList = JsonConvert.DeserializeObject<List<JackBetInfo>>(info.jsonData);
            for (int i = 0; i < jackBetInfoList.Count; i++)
            {
                var jackBetInfo = jackBetInfoList[i];
                long clientId = long.Parse(jackBetInfo.gameType.ToString() + info.id.ToString());
                Player player = PlayerMgr.Instance.GetPlayer(clientId);
                if (player == null)
                {
                    Debug.LogError($"OnPlayerJackBet: Player not found for clientId: {clientId}");
                    return;
                }
                SBoxJackpotBet sBoxJackpot = new SBoxJackpotBet
                {
                    MachineId = player.LogicId,
                    SeatId = jackBetInfo.seat,
                    Bet = jackBetInfo.bet,
                    BetPercent = jackBetInfo.betPercent,
                    ScoreRate = jackBetInfo.scoreRate,
                    JpPercent = 1000,
                };
                EventCenter.Instance.EventTrigger(EventHandle.JACKPOT_BET, sBoxJackpot);
                //ReSendWinJackpot(player, jackBetInfo.seat);
            }
        }

    }







    /// <summary>
    /// 获取当前彩金显示值
    /// </summary>
    /// <param name="info"></param>
    private void OnGetJackpotShowValue(MsgInfo info, ClientConnection client)
    {
        MsgInfo res;

        RequestBase requestInfo;
        JackpotGameShowInfoR jpGameShowR = new JackpotGameShowInfoR();

        try
        {
            requestInfo = JsonConvert.DeserializeObject<RequestBase>(info.jsonData);
            jpGameShowR.seqId = requestInfo.seqId;
        }
        catch (Exception e)
        {
            jpGameShowR.code = 1;
            jpGameShowR.msg = "输入数据格式有误";
            res = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_GetJackpotShowValueR,
                id = info.id,  
                jsonData = JsonConvert.SerializeObject(jpGameShowR)
            };
            NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
            OnResponseDebug((S2C_CMD)res.cmd, res);

            return;
        }


        jpGameShowR.code = 0;
        jpGameShowR.curJackpotOut = IOCanvasModel.Instance.CurJackpotOut;

        res = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_GetJackpotShowValueR,
            id = info.id,
            jsonData = JsonConvert.SerializeObject(jpGameShowR)
        };
        NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(res));
        OnResponseDebug((S2C_CMD)res.cmd, res);

        
    }


    /// <summary>
    /// 机台彩金领奖
    /// </summary>
    /// <param name="info"></param>
    private void OnReceiveJackpot(MsgInfo info)
    {
        ReceiveJackpotInfo receiveJackpotInfo = JsonConvert.DeserializeObject<ReceiveJackpotInfo>(info.jsonData);
        long clientId = long.Parse(receiveJackpotInfo.gameType.ToString() + info.id.ToString());
        Player player = PlayerMgr.Instance.GetPlayer(clientId);
        if (player == null)
        {
            Debug.LogError($"OnPlayerJackBet: Player not found for clientId: {clientId}");
            return;
        }
        if (Model.Instance.unfinishOrderDatas.TryGetValue(player.LogicId, out List<OrderData> orderDatas))
        {
            for (int i = 0; i < orderDatas.Count; i++)
            {
                if (orderDatas[i].orderId == receiveJackpotInfo.orderId)
                {
                    orderDatas[i].finish = 1;
                    SQLite.Instance.UpdateOrderData(orderDatas[i]);
                    orderDatas.RemoveAt(i);
                    break;
                }
            }
        }
    }


    /// <summary>
    /// 获取彩金记录
    /// </summary>
    /// <param name="info"></param>
    private void OnGetJackpotData(MsgInfo info)
    {
        Debug.LogError($"OnGetJackpotData: {JsonConvert.SerializeObject(IOCanvasModel.Instance.BetDataDic)}");
        if (IOCanvasModel.Instance.BetDataDic.TryGetValue(info.id, out Dictionary<int, List<BetData>> betDataDic))
        {
            MsgInfo mes = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_GetJackpotDataR,
                id = info.id,
                jsonData = JsonConvert.SerializeObject(betDataDic),
            };
            PlayerMgr.Instance.GetPlayer(info.id).SendToClient(JsonConvert.SerializeObject(mes));
        }
        else
        {
            betDataDic = new Dictionary<int, List<BetData>>();
            MsgInfo mes = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_GetJackpotDataR,
                id = info.id,
                jsonData = JsonConvert.SerializeObject(betDataDic),
            };
            PlayerMgr.Instance.GetPlayer(info.id).SendToClient(JsonConvert.SerializeObject(mes));
        }
    }

    private void OnPlayerDisconnect(ClientConnection client)
    {
        PlayerMgr.Instance.PlayerDisconnect(client);
    }
}






#else



using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SBoxApi;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using WebSockets;
using static UnityEditor.Progress;

public class NetMessageController : BaseManager<NetMessageController>
{

    public void Init()
    {

        RemoveEventListener();

        Messenger.AddListener<MsgInfo, ClientConnection>(MessageName.Event_ServerNetworkRecv, OnServerNetworkRecv);
        EventCenter.Instance.AddEventListener<ClientConnection>(EventHandle.PLAYER_DISCONNECT, OnPlayerDisconnect);
        EventCenter.Instance.AddEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnClinetReadConfR);
        EventCenter.Instance.AddEventListener<SBoxApi.SBoxJackpotData>(SBoxEventHandle.SBOX_JACKPOT_BET_HOST, OnSBoxJackpotData);

        SBoxIdea.ReadConf();
    }


    void RemoveEventListener()
    {
        Messenger.RemoveListener<MsgInfo, ClientConnection>(MessageName.Event_ServerNetworkRecv, OnServerNetworkRecv);
        EventCenter.Instance.RemoveEventListener<ClientConnection>(EventHandle.PLAYER_DISCONNECT, OnPlayerDisconnect);
        EventCenter.Instance.RemoveEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnClinetReadConfR);
        EventCenter.Instance.RemoveEventListener<SBoxApi.SBoxJackpotData>(SBoxEventHandle.SBOX_JACKPOT_BET_HOST, OnSBoxJackpotData);
    }

    Dictionary<long,Action<SBoxApi.SBoxJackpotData>> funcJackpotDataDic = new Dictionary<long, Action<SBoxJackpotData>>();
    void OnSBoxJackpotData(SBoxApi.SBoxJackpotData data)
    {
        Debug.LogWarning($"当前值：curSBoxJackpotData = {JsonConvert.SerializeObject(SBoxIdea.curSBoxJackpotData)}");

        foreach (KeyValuePair<long, Action<SBoxApi.SBoxJackpotData>> item in funcJackpotDataDic)
        {
            item.Value?.Invoke(data);
        }
    }


    SBoxConfData curSBoxConfData;
    void OnClinetReadConfR(SBoxConfData res)
    {
        curSBoxConfData = res;
        Debug.LogError($"读到配置数据 {JsonConvert.SerializeObject(curSBoxConfData)}");
    }




    /// <summary>
    /// 监听到机台给过来的请求数据
    /// </summary>
    /// <param name="info"></param>
    /// <param name="client"></param>
    private void OnServerNetworkRecv(MsgInfo info, ClientConnection client)
    {

        //Debug.LogWarning($"接收到机台发来的数据： {JsonConvert.SerializeObject(info)}");


        OnRequestDebug((C2S_CMD)info.cmd, info);

        if (info.id == 0)
        {
            return;
        }

        switch ((C2S_CMD)info.cmd)
        {
            case C2S_CMD.C2S_Login:
                OnPlayerLogin(info, client);
                //Debug.Log($"Login: {info.id}");
                break;
            case C2S_CMD.C2S_ReadConf:
                {
                    OnClinetReadConf(info, client);
                   // Debug.Log($"ReadConf: {info.id}");
                }
                break;
            case C2S_CMD.C2S_JackBet:
                //Debug.Log($"C2S_JackBet:{info.jsonData}");
                OnPlayerJackBet(info);
                break;
            case C2S_CMD.C2S_ReceiveJackpot:
                OnReceiveJackpot(info);
                break;
            case C2S_CMD.C2S_GetJackpotData:
                OnGetJackpotData(info);
                break;
            default:
                Debug.LogError($"未设置 {info.cmd} 响应");
                break;
        }
    }

    private void OnPlayerLogin(MsgInfo info, ClientConnection client)
    {
        if (string.IsNullOrEmpty(info.jsonData)) return;

        LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(info.jsonData);


        Debug.LogError($"收到玩家登录信息：macId: {loginInfo.macId} ; LoginInfo: {info.jsonData}");



        if (IOCanvasModel.Instance.groupId != loginInfo.groudId)
        {
            // 组号不对
            Debug.LogError($"组号不对，拒绝登录：macId: {loginInfo.macId} ; LoginInfo: {info.jsonData}");

  /*
            JObject req = new JObject();
            req.Add("code", 0);
            req.Add("msg", "");
            req.Add("data", "");
          
        MsgInfo mes = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_JackpotMinBet,
            id = player.macId,
            jsonData = JsonConvert.SerializeObject(minBets)
        };
        NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(mes));
        OnResponseDebug((S2C_CMD)mes.cmd, mes);
             * 
             */


            return;
        }
        if (IOCanvasModel.Instance.seatIdMacIdMap.ContainsKey(loginInfo.seatId) 
            && IOCanvasModel.Instance.seatIdMacIdMap[loginInfo.seatId] != loginInfo.macId)
        {
            // 座位号重复
            Debug.LogError($"座位号重复，拒绝登录：macId: {loginInfo.macId} ; LoginInfo: {info.jsonData}");
            return;
        }

        IOCanvasModel.Instance.seatIdMacIdMap[loginInfo.seatId] = loginInfo.macId;




        Player player = PlayerMgr.Instance.PlayerInsert(loginInfo, client);

        List<int> minBets = new List<int>();


        /*
        try
        {
            //#seaweed# 【待解决】这里会爆错
            for (int i = 0; i < IOCanvasModel.Instance.JackCfgData.sBoxJackpotConfigDataItem.Length; i++)  
                minBets.Add(IOCanvasModel.Instance.JackCfgData.sBoxJackpotConfigDataItem[i].MinBet);
            minBets.Reverse();
        }
        catch (Exception e)
        {
        }
        */

        MsgInfo mes = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_JackpotMinBet,
            id = player.macId,
            jsonData = JsonConvert.SerializeObject(minBets)
        };
        NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(mes));
        OnResponseDebug((S2C_CMD)mes.cmd, mes);
    }

    /// <summary>
    /// 读取配置信息
    /// </summary>
    /// <param name="info"></param>
    /// <param name="client"></param>
    private void OnClinetReadConf(MsgInfo info, ClientConnection client)
    {

        RequestBaseInfo receiveBaseInfo = JsonConvert.DeserializeObject<RequestBaseInfo>(info.jsonData);

        long clientId = long.Parse(receiveBaseInfo.gameType.ToString() + info.id.ToString());
       
        Player player = PlayerMgr.Instance.GetPlayer(clientId);

        MsgInfo mes = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_ReadConfR,
            id = player.macId,
            jsonData = JsonConvert.SerializeObject(curSBoxConfData)
        };
        NetMgr.Instance.SendToClient(client, JsonConvert.SerializeObject(mes));
        OnResponseDebug((S2C_CMD)mes.cmd, mes);
    }


    void OnResponseDebug(S2C_CMD cmd, MsgInfo res)
    {
        string nam = Enum.GetName(typeof(S2C_CMD), cmd);
        Debug.LogWarning($"<color=yellow>server response</color>: {nam} = {JsonConvert.SerializeObject(res)}");
    }
    void OnRequestDebug(C2S_CMD cmd, MsgInfo req)
    {
        string nam = Enum.GetName(typeof(C2S_CMD), cmd);
        Debug.LogWarning($"<color=green>clinet request</color>: {nam} = {JsonConvert.SerializeObject(req)}");
    }


    private void ReSendWinJackpot(Player player, int seatId, SBoxJackpotData res1)
    {

        MsgInfo msgInfo = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_WinJackpot,
            id = player.macId,
            jsonData = JsonConvert.SerializeObject(res1)
        };
        player.SendToClient(JsonConvert.SerializeObject(msgInfo));

        // 
        /*
        if (Model.Instance.unfinishOrderDatas.TryGetValue(player.LogicId, out List<OrderData> orderDatas))
        {
            if (orderDatas.Count > 0)
            {
                WinJackpotInfo winJackpotInfo = new WinJackpotInfo
                {
                    macId = orderDatas[0].logicId,
                    seat = orderDatas[0].seatId,
                    jackpotId = orderDatas[0].jackpotId,
                    win = orderDatas[0].wins,
                    orderId = orderDatas[0].orderId,
                    time = orderDatas[0].time,
                };
                MsgInfo msgInfo = new MsgInfo
                {
                    cmd = (int)S2C_CMD.S2C_WinJackpot,
                    id = player.macId,
                    jsonData = JsonConvert.SerializeObject(winJackpotInfo)
                };
                player.SendToClient(JsonConvert.SerializeObject(msgInfo));
            }
        }*/
    }

    private void OnPlayerJackBet(MsgInfo info)
    {
        if (true)
        {
            JackBetInfoCoinPush jpBetInfo = JsonConvert.DeserializeObject<JackBetInfoCoinPush>(info.jsonData);

            long clientId = long.Parse(jpBetInfo.gameType.ToString() + info.id.ToString());
            Player player = PlayerMgr.Instance.GetPlayer(clientId);
            if (player == null)
            {
                Debug.LogError($"OnPlayerJackBet: Player not found for clientId: {clientId}");
                return;
            }

            funcJackpotDataDic.Add(clientId, (SBoxJackpotData rs) =>
            {
                ReSendWinJackpot(player, jpBetInfo.seat, rs);
                funcJackpotDataDic.Remove(clientId);
            });

            SBoxJackpotBetCoinPush sBoxJackpot = new SBoxJackpotBetCoinPush
            {
                MachineId = player.LogicId,
                SeatId = jpBetInfo.seat,
                majorBet = jpBetInfo.grandBet,
                grandBet = jpBetInfo.majorBet,
                BetPercent = jpBetInfo.betPercent, //100
                ScoreRate = jpBetInfo.scoreRate, //1000
                JpPercent = jpBetInfo.JPPercent, //1000

            };
            EventCenter.Instance.EventTrigger(EventHandle.JACKPOT_BET, sBoxJackpot);
    
        }
        else
        {
            var jackBetInfoList = JsonConvert.DeserializeObject<List<JackBetInfo>>(info.jsonData);
            for (int i = 0; i < jackBetInfoList.Count; i++)
            {
                var jackBetInfo = jackBetInfoList[i];
                long clientId = long.Parse(jackBetInfo.gameType.ToString() + info.id.ToString());
                Player player = PlayerMgr.Instance.GetPlayer(clientId);
                if (player == null)
                {
                    Debug.LogError($"OnPlayerJackBet: Player not found for clientId: {clientId}");
                    return;
                }
                SBoxJackpotBet sBoxJackpot = new SBoxJackpotBet
                {
                    MachineId = player.LogicId,
                    SeatId = jackBetInfo.seat,
                    Bet = jackBetInfo.bet,
                    BetPercent = jackBetInfo.betPercent,
                    ScoreRate = jackBetInfo.scoreRate,
                    JpPercent = 1000,
                };
                EventCenter.Instance.EventTrigger(EventHandle.JACKPOT_BET, sBoxJackpot);
                //ReSendWinJackpot(player, jackBetInfo.seat);
            }
        }

    }

    private void OnReceiveJackpot(MsgInfo info)
    {
        ReceiveJackpotInfo receiveJackpotInfo = JsonConvert.DeserializeObject<ReceiveJackpotInfo>(info.jsonData);
        long clientId = long.Parse(receiveJackpotInfo.gameType.ToString() + info.id.ToString());
        Player player = PlayerMgr.Instance.GetPlayer(clientId);
        if (player == null)
        {
            Debug.LogError($"OnPlayerJackBet: Player not found for clientId: {clientId}");
            return;
        }
        if (Model.Instance.unfinishOrderDatas.TryGetValue(player.LogicId, out List<OrderData> orderDatas))
        {
            for (int i = 0; i < orderDatas.Count; i++)
            {
                if (orderDatas[i].orderId == receiveJackpotInfo.orderId)
                {
                    orderDatas[i].finish = 1;
                    SQLite.Instance.UpdateOrderData(orderDatas[i]);
                    orderDatas.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void OnGetJackpotData(MsgInfo info)
    {
        Debug.LogError($"OnGetJackpotData: {JsonConvert.SerializeObject(IOCanvasModel.Instance.BetDataDic)}");
        if (IOCanvasModel.Instance.BetDataDic.TryGetValue(info.id, out Dictionary<int, List<BetData>> betDataDic))
        {
            MsgInfo mes = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_GetJackpotData,
                id = info.id,
                jsonData = JsonConvert.SerializeObject(betDataDic),
            };
            PlayerMgr.Instance.GetPlayer(info.id).SendToClient(JsonConvert.SerializeObject(mes));
        }
        else
        {
            betDataDic = new Dictionary<int, List<BetData>>();
            MsgInfo mes = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_GetJackpotData,
                id = info.id,
                jsonData = JsonConvert.SerializeObject(betDataDic),
            };
            PlayerMgr.Instance.GetPlayer(info.id).SendToClient(JsonConvert.SerializeObject(mes));
        }
    }

    private void OnPlayerDisconnect(ClientConnection client)
    {
        PlayerMgr.Instance.PlayerDisconnect(client);
    }
}


#endif




