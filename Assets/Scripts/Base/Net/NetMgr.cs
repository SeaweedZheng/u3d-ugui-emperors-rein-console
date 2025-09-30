using Newtonsoft.Json;
using System;
using UnityEngine;


public class NetMgr : MonoSingleton<NetMgr>
{
    private readonly int port = 6222;
    public int broadcastPort = 10999;
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
        Debug.LogError($"call SetNetAutoConnect host={Host}");
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

        OnDebug(strMsg, true);
    }

    //服务器发送数据给客户端
    public void SendToClient(WebSockets.ClientConnection client,string strMsg)
    {
        serverWS?.SendToClient(client, strMsg);


        OnDebug(strMsg,false);
    }

    //服务器给所有客户端发送消息
    public void SendToAllClient(string strMsg)
    {
        serverWS?.SendToAllClient(strMsg);

        OnDebug(strMsg, false);
    }

    //处理WS服务器收到的消息
    void OnWSServerData(WSSrvMsgData data)
    {
        if (data.Data.Length == 0)
            return;
        string singlePacket = data.Data;
        MsgInfo info = null;

        OnDebug(singlePacket, true);
        try
        {
            info = JsonConvert.DeserializeObject<MsgInfo>(singlePacket);
        }
        catch (System.Exception ex)
        {
            Debug.Log("JSON : " + ex.Message);
        }
        if (info != null)
        {
            switch ((C2S_CMD)info.cmd)
            {
                case C2S_CMD.C2S_HeartHeat:
                    info.cmd = (int)S2C_CMD.S2C_HeartHeat;
                    info.id = info.id;
                    SendToClient(data.Client, JsonConvert.SerializeObject(info));
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

    public void OnDebug(string strMsg, bool C2S)
    {
        try
        {
            string cmdValue = strMsg.Split(new[] { "\"cmd\":" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
            string rpcName = C2S ?
               $"SendToClinet : {Enum.GetName(typeof(C2S_CMD), (C2S_CMD)(int.Parse(cmdValue)))} -" :
                $"SendToServer : {Enum.GetName(typeof(S2C_CMD), (S2C_CMD)(int.Parse(cmdValue)))} -";

            Debug.LogWarning($"{rpcName} -  {strMsg}");
        }
        catch (Exception ex) { }
        /*
        string rpcName = "";
        Regex regex = new Regex("\"cmd\":\\s*(\\d+)");
        Match match = regex.Match(strMsg);
        if (match.Success)
        {
            string cmdValue = match.Groups[1].Value;
            rpcName = Enum.GetName(typeof(S2C_CMD), (S2C_CMD)(int.Parse(cmdValue)));
        }
        Debug.LogWarning($"SendToClient: {rpcName} -  {strMsg}");*/
    }
}