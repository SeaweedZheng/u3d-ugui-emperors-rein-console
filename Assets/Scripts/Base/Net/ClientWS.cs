using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityWebSocket;
using MyTimer = GameUtil.Timer;

public class ClientWS : MonoSingleton<ClientWS>
{
    WebSocket mSocket;

    //Udp相关
    private int mBroadcastPort;
    private UdpClient mUdpclient = null; //主机和分机的udpclient
    private IPEndPoint endpoint;
    // ServerInfo strSrvInfo = JsonConvert.SerializeObject(new ServerInfo());

    private bool IsStop = false;

    public bool GetHost = false;

    public bool IsConnected { get; set; }

    private Thread RcvThread = null;
    ServerInfo serverinfo;
    // 是否开始心跳
    public bool canHeart = false;
    public string mAddress;
    public float LastHeartHeatTime = 0.0f;
    public int HeartHeatDelta = 10; //心跳间隔
    private MyTimer heartHeatTimer;
    private MyTimer checkSrvTimer;

    private void Start()
    {

        IsConnected = false;
    }

    void SetServerInfo(ServerInfo si)
    {
        serverinfo = si;
    }

    public void StartUdp(int broadcastPort)
    {

        Debug.Log($"【UDP-WS】StartClinet;  broadcastPort:{broadcastPort}");

        mBroadcastPort = broadcastPort;
        mUdpclient = new UdpClient(new IPEndPoint(IPAddress.Parse(Utils.LocalIP()), 0));
        endpoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
        IsStop = false;

        RcvThread = new Thread(new ThreadStart(ReciveUdpMsg))
        {
            IsBackground = true
        };
        RcvThread.Start();

        if (checkSrvTimer != null)
        {
            checkSrvTimer.Resume();
        }
        else
        {
            checkSrvTimer = MyTimer.LoopAction(3.0f, CheckHostServerInfo);
        }

    }

    public void Reconnect()
    {
        //mUdpclient.Close();
        //mUdpclient = new UdpClient(new IPEndPoint(IPAddress.Parse(Utils.LocalIP()), 0));
        //endpoint = new IPEndPoint(IPAddress.Broadcast, mBroadcastPort);
#pragma warning disable CS0618 // Type or member is obsolete
        RcvThread?.Resume();
#pragma warning restore CS0618 // Type or member is obsolete
        checkSrvTimer?.Resume();
    }

    private void ReciveUdpMsg()
    {
        while (!IsStop && mUdpclient != null)
        {
            try
            {
                byte[] buf = mUdpclient.Receive(ref endpoint);
                if (buf != null)
                {
                    string msg = Encoding.UTF8.GetString(buf);

                    Debug.Log($"【UDP-WS】ReciveUdpMsg(S2C): {msg}");

                    if (!string.IsNullOrEmpty(msg) && !GetHost)
                    {
                        serverinfo = JsonConvert.DeserializeObject<ServerInfo>(msg);
                        GetHost = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("【UDP-WS】" + e.Message);
            }
        }
        Debug.LogWarning("【UDP-WS】ReciveUdpMsg OUT");
    }

    //使用udp发送消息
    public void SendUpdMsg(string strMsg)
    {
        try
        {
            if (mUdpclient != null)
            {
                byte[] bf = Encoding.UTF8.GetBytes(strMsg);
                mUdpclient.Send(bf, bf.Length, endpoint);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }


    void CheckHostServerInfo(int loopTimes)
    {
        if (!IsConnected && serverinfo != null)
        {
            Debug.Log("【UDP-WS】StopUdp ");
            InitSocket(serverinfo.IP, serverinfo.port);
            StopUdp();
        }
        else if (serverinfo == null)
        {
            ServerInfo clientInfo = new ServerInfo
            {
                IP = Utils.LocalIP(),
                port = mBroadcastPort
            };
            string msg = JsonConvert.SerializeObject(clientInfo);
            SendUpdMsg(msg);
            Debug.Log($"【UDP-WS】UDP/C2S : {msg} ");
        }
    }

    void StopUdp()
    {
#pragma warning disable CS0618 // 类型或成员已过时
        RcvThread?.Suspend();
#pragma warning restore CS0618 // 类型或成员已过时
    }

    public void InitSocket(string server_ip, int port)
    {        
        Debug.Log("【UDP-WS】InitSocket----> ip = " + server_ip + " and port = " + port);
        if (mSocket != null)
        {
            mSocket.OnOpen -= SocketOnOpen;
            mSocket.OnMessage -= SocketOnMessage;
            mSocket.OnClose -= SocketOnClose;
            mSocket.OnError -= SocketOnError;
            mSocket.CloseAsync();
            mSocket = null;
            //StopCoroutine(ClientHeartHeat());
        }
        try
        {
            mAddress = string.Format("ws://{0}:{1}", server_ip, port);
            mSocket = new WebSocket(mAddress);
            mSocket.OnOpen += SocketOnOpen;
            mSocket.OnMessage += SocketOnMessage;
            mSocket.OnClose += SocketOnClose;
            mSocket.OnError += SocketOnError;
            //mSocket.BinaryType = 
            mSocket.ConnectAsync();
            Messenger.Broadcast<int>(MessageName.Event_NetworkErr, 1);

            LastHeartHeatTime = Time.time;
            if (heartHeatTimer == null)
            {
                heartHeatTimer = MyTimer.LoopAction(3.0f, ClientHeartHeat);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    //给服务器发送心跳
    public void SendHeartHeat()
    {
        MsgInfo cmd = new MsgInfo();
        cmd.cmd = (int)C2S_CMD.C2S_HeartHeat;
        //cmd.id = Model.Instance.macId;
        SendToServer(JsonConvert.SerializeObject(cmd));
    }

    //给服务器发数据
    public void SendToServer(string strData)
    {
        try
        {
            if (mSocket != null && mSocket.ReadyState != WebSocketState.Closed)
            {
                //直接发给服务器了，不需要放进队列里等待发送。
                mSocket.SendAsync(strData);

                OnDebug(strData, true);
            }

        }
        catch (Exception e)
        {
            //mClientSocket.Close();
            Debug.Log("【UDP-WS】发送失败 " + e.Message);
        }
    }

    //每3秒发一次心跳
    void ClientHeartHeat(int ck)
    {
        if (canHeart)
        {
            float delta = Time.time - LastHeartHeatTime;
            if (delta > HeartHeatDelta) //心跳超时了,重新连接服务器
            {
                GetHost = false;
                IsConnected = false;
                serverinfo = null;
                //#seaweed#PopTips.Instance.ShowSystemTips(Utils.GetLanguage("Disconnected"), -1);
                Reconnect();
                //StartUdp(mBroadcastPort);
            }
            SendHeartHeat();
        }
    }

    private void SocketOnOpen(object sender, OpenEventArgs e)
    {
        Debug.Log(string.Format("【UDP-WS】Connected: {0}", mAddress));
        IsConnected = true;
        canHeart = true;
        SendHeartHeat();
        PopTips.Instance.ShowSystemTips("Connected");
    }

    private void SocketOnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            Debug.Log(string.Format("【UDP-WS】Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
        }
        else if (e.IsText)
        {
            //Debug.Log(string.Format("Receive: {0}", e.Data));
            //TODO 添加消息处理
            Messenger.Broadcast<byte[]>(MessageName.Event_NetworkClientData, Encoding.UTF8.GetBytes(e.Data));
        }
    }

    private void SocketOnClose(object sender, CloseEventArgs e)
    {
        Debug.LogError("【UDP-WS】call SocketOnClose");
        Debug.Log(string.Format("【UDP-WS】Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
        PopTips.Instance.ShowSystemTips(Utils.GetLanguage("Disconnected"), -1);
        serverinfo = null;
        IsConnected = false;
        Reconnect();
    }

    private void SocketOnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("【UDP-WS】call SocketOnError");
        Debug.Log(string.Format("【UDP-WS】Error: {0}", e.Message));
        PopTips.Instance.ShowSystemTips(Utils.GetLanguage("Disconnected"), -1);
        serverinfo = null;
        IsConnected = false;
        Reconnect();
    }

    public void CloseSocket()
    {
        serverinfo = null;
        GetHost = false;
        IsConnected = false;
        canHeart = false;
        heartHeatTimer?.Cancel();
        if (mSocket != null)
        {
            mSocket.OnOpen -= SocketOnOpen;
            mSocket.OnMessage -= SocketOnMessage;
            mSocket.OnClose -= SocketOnClose;
            mSocket.OnError -= SocketOnError;
            mSocket.CloseAsync();
            mSocket = null;
        }
        Debug.Log("CloseSocket");
    }

    private new void OnDestroy()
    {
        canHeart = false;
        StopUdp();
        MyTimer.CancelAllRegisteredTimers();
        if (mSocket != null)
        {
            mSocket.CloseAsync();
            mSocket = null;
        }

        if (mUdpclient != null)
        {
            mUdpclient.Close();
            mUdpclient = null;
        }
    }


    public void OnDebug(string strMsg, bool C2S = true)
    {
        try
        {
            string cmdValue = strMsg.Split(new[] { "\"cmd\":" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
            string rpcName = C2S ?
               $"{Enum.GetName(typeof(C2S_CMD), (C2S_CMD)(int.Parse(cmdValue)))} -" :
                $"{Enum.GetName(typeof(S2C_CMD), (S2C_CMD)(int.Parse(cmdValue)))} -";

            Debug.LogWarning($"【UDP-WS】WS/{rpcName} -  {strMsg}");
        }
        catch (Exception ex) { }
    }




}