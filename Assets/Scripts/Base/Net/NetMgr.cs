using Newtonsoft.Json;
using System;
using UnityEngine;


public class NetMgr : MonoSingleton<NetMgr>
{
    private readonly int port = 6222;
    //#seaweed# 改
    public int broadcastPort = 10122; //10999; //  1220 >> 10122
    public bool IsHost = false;

    //WebSocket
    ServerWS serverWS;
    ClientWS clientWS;


    private void Awake()
    {
        serverWS = this.transform.GetComponent<ServerWS>();
        clientWS = this.transform.GetComponent<ClientWS>();



        OnRemoveListener();

        Messenger.AddListener<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, OnWSServerData);
        Messenger.AddListener<byte[]>(MessageName.Event_NetworkClientData, OnClientData);
    }

    void OnRemoveListener()
    {
        Messenger.RemoveListener<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, OnWSServerData);
        Messenger.RemoveListener<byte[]>(MessageName.Event_NetworkClientData, OnClientData);
    }


    public void SetLastHeartHeat()
    {
        if (clientWS != null)
            clientWS.LastHeartHeatTime = Time.time;
    }

    void OnClientData(byte[] data)
    {
        Messenger.Broadcast<byte[]>(MessageName.Event_ClientNetworkRecv, data);
    }

    public void SetNetAutoConnect(bool Host)
    {
        Debug.LogWarning($"call SetNetAutoConnect host={Host}");
        IsHost = Host;

        Debug.Log($"SetNetAutoConnect: broadcastPort = {broadcastPort}");
        if (IsHost)
        {
            if (serverWS == null)
                serverWS = gameObject.AddComponent<ServerWS>();
            serverWS.StartServer(port, broadcastPort);
        }
        else
        {
            if (clientWS == null)
                clientWS = gameObject.AddComponent<ClientWS>();
            clientWS.StartUdp(broadcastPort);
        }
    }

    //客户端发送数据给服务器
    public void SendToServer(string strMsg)
    {
        clientWS?.SendToServer(strMsg);

        //OnDebug(strMsg, true);
    }

    //服务器发送数据给客户端
    public void SendToClient(WebSockets.ClientConnection client,string strMsg)
    {
        serverWS?.SendToClient(client, strMsg);


        //OnDebug(strMsg,false);
    }

    //服务器给所有客户端发送消息
    public void SendToAllClient(string strMsg)
    {
        serverWS?.SendToAllClient(strMsg);

        //OnDebug(strMsg, false);
    }

    //处理WS服务器收到的消息
    void OnWSServerData(WSSrvMsgData data)
    {
        if (data.Data.Length == 0)
            return;
        string singlePacket = data.Data;
        MsgInfo info = null;

        //OnDebug(singlePacket, true);
        try
        {
            info = JsonConvert.DeserializeObject<MsgInfo>(singlePacket);
        }
        catch (System.Exception ex)
        {
            DebugUtils.LogError("【UDP-WS】MsgInfo error : " + ex.Message);
        }
        if (info != null)
        {
            switch ((C2S_CMD)info.cmd)
            {
                case C2S_CMD.C2S_Heartbeat:
                    MsgInfo infoR = new MsgInfo();
                    infoR.cmd = (int)S2C_CMD.S2C_HeartbeatR;
                    infoR.id = info.id;

                    try
                    {
                        HeartbeatInfo res = JsonConvert.DeserializeObject<HeartbeatInfo>(info.jsonData);

                        int code = 0;
                        string msg = "";
                        if (IOCanvasModel.Instance.macIdSeatIdMap.ContainsKey(res.macId))
                        {

                            if (res.groupId != IOCanvasModel.Instance.groupId)
                            {
                                code = 1;
                                msg = "组号错误，请从新登录";
                            }
                            else if (res.seatId != IOCanvasModel.Instance.macIdSeatIdMap[res.macId].seatId)
                            {
                                code = 1;
                                msg = "座位号错误，请从新登录";
                            }
                            else if (Time.unscaledTime - IOCanvasModel.Instance.macIdSeatIdMap[res.macId].lastHeartbeatTimeS  > 15)
                            {
                                code = 1;
                                msg = "心跳超时";
                            }
                            else
                            {
                                IOCanvasModel.Instance.macIdSeatIdMap[res.macId].lastHeartbeatTimeS = Time.unscaledTime;
                            }

                            // 同时进行超时踢出服务器
                            if (Time.unscaledTime - IOCanvasModel.Instance.macIdSeatIdMap[res.macId].lastHeartbeatTimeS > 15)
                            {
                                IOCanvasModel.Instance.macIdSeatIdMap.Remove(res.macId);
                            }
                        }
                        else
                        {
                            code = 1;
                            msg = "请先登录服务器";
                        }
                        
                        infoR.jsonData = JsonConvert.SerializeObject(
                            new HeartbeatInfoR()
                            {
                                macId = res.macId,
                                code = code,
                                msg = msg,
                            }
                        );
                    }
                    catch (Exception ex)
                    {

                    }

                    SendToClient(data.Client, JsonConvert.SerializeObject(infoR));
                    break;
                default:
                    {
                        Messenger.Broadcast(MessageName.Event_ServerNetworkRecv, info, data.Client);
                    }
                    break;
            }
        }
    }



    protected override void OnDestroy()
    {
        base.OnDestroy();
        Messenger.RemoveListener<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, OnWSServerData);
        Messenger.RemoveListener<byte[]>(MessageName.Event_NetworkClientData, OnClientData);
    }
    /*
    public void OnDebug(string strMsg, bool C2S)
    {
        try
        {
            string cmdValue = strMsg.Split(new[] { "\"cmd\":" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
            string rpcName = C2S ?
               $"{Enum.GetName(typeof(C2S_CMD), (C2S_CMD)(int.Parse(cmdValue)))} -" :
                $"{Enum.GetName(typeof(S2C_CMD), (S2C_CMD)(int.Parse(cmdValue)))} -";

            Debug.LogWarning($"{rpcName} -  {strMsg}");
        }
        catch (Exception ex) { }
    }
    */
}