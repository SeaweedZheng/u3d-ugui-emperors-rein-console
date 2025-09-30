using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityWebSocket;
using System.Text;
using System;

public class InternetServerMgr : MonoSingleton<InternetServerMgr>
{
    public bool isConnected;
    public bool canHeart;
    public bool isReconnecting;

    private float lastHeartHeatTime;

    private InternetNetInfo internetNetInfo;
    private WebSocket mSocket;
    public const int heartHeatDelta = 15;
    private GameUtil.Timer heartHeatTimer;
    private GameUtil.Timer reconnectTimer;

    // Start is called before the first frame update
    private void Start()
    {
        isConnected = false;
    }

    public void DeviceConnectServer()
    {
        var dic = new Dictionary<string, string>
        {
            ["name"] = "device1",
            ["pwd"] = "123456"
        };

        Utils.Post("http://8.138.117.128:11000/device_login", JsonConvert.SerializeObject(dic), OnSucceed, OnError);
    }

    public void ClientConnectServer(string account, string password)
    {
        var dic = new Dictionary<string, string>
        {
            ["name"] = account,
            ["pwd"] = password
        };

        Utils.Post("http://8.138.117.128:11000/user_login", JsonConvert.SerializeObject(dic), OnSucceed, OnError);
    }

    private void OnSucceed(string result)
    {
        if (string.IsNullOrEmpty(result)) return;
        string content = AESManager.Instance.TryDecrypt(result);
        internetNetInfo = JsonConvert.DeserializeObject<InternetNetInfo>(content);
        internetNetInfo.device_ip = string.Format("ws://{0}", internetNetInfo.device_ip);
        InitSocket(0);
    }

    private void OnError(string result)
    {
        if (string.IsNullOrEmpty(result)) return;
        string content = AESManager.Instance.TryDecrypt(result);
        Debug.Log("OnError: " + content);
    }

    private void InitSocket(int _)
    {
        if (mSocket != null)
        {
            mSocket.OnOpen -= SocketOnOpen;
            mSocket.OnMessage -= SocketOnMessage;
            mSocket.OnClose -= SocketOnClose;
            mSocket.OnError -= SocketOnError;
            mSocket.CloseAsync();
            mSocket = null;
        }
        try
        {
            mSocket = new WebSocket(internetNetInfo.device_ip, internetNetInfo.token_id);
            mSocket.OnOpen += SocketOnOpen;
            mSocket.OnMessage += SocketOnMessage;
            mSocket.OnClose += SocketOnClose;
            mSocket.OnError += SocketOnError;
            mSocket.ConnectAsync();

            lastHeartHeatTime = Time.time;
            heartHeatTimer ??= GameUtil.Timer.LoopAction(3.0f, ClientHeartHeat);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void SocketOnOpen(object sender, OpenEventArgs e)
    {
        reconnectTimer?.Cancel();
        Debug.Log(string.Format("Connected: {0}", internetNetInfo.device_ip));
        isConnected = true;
        canHeart = true;
        SendHeartHeat();
        LoginVideoServer();
        //PopTips.Instance.ShowSystemTips("Connected");
    }

    private void SocketOnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            Debug.Log(string.Format("Receive Bytes ({1}): {0}", e.Data, e.RawData.Length));
        }
        else if (e.IsText)
        {
            string jsonData = AESManager.Instance.TryDecrypt(e.Data);

            var mes = JsonConvert.DeserializeObject<InternetMes>(jsonData);
            if (mes.protocol_key == InterNetHandle.PING)
                lastHeartHeatTime = Time.time;
            else
                Loom.QueueOnMainThread((_) =>
                {
                    EventCenter.Instance.EventTrigger(EventHandle.INTERNET_SERVER_INFO, mes);
                }, mes);
        }
    }

    private void SocketOnClose(object sender, CloseEventArgs e)
    {
        Debug.Log(string.Format("Closed: StatusCode: {0}, Reason: {1}", e.StatusCode, e.Reason));
        isConnected = false;
        Reconnect();
        //PopTips.Instance.ShowSystemTips(Utils.GetLanguage("Disconnected"), -1);
    }

    private void SocketOnError(object sender, ErrorEventArgs e)
    {
        Debug.Log(string.Format("Error: {0}", e.Message));
        isConnected = false;
        Reconnect();
    }

    private void Reconnect()
    {
        if (reconnectTimer != null)
            reconnectTimer.Resume();
        else
            reconnectTimer = GameUtil.Timer.LoopAction(3.0f, InitSocket);
    }

    private void LoginVideoServer()
    {
        InternetMes mes = new InternetMes()
        {
            protocol_key = InterNetHandle.DEVICE_LOGIN,
        };

        mes.data = new Dictionary<string, string>
        {
            ["access_token"] = internetNetInfo.token_id,
        };

        SendMes(JsonConvert.SerializeObject(mes));
    }

    public void SendHeartHeat()
    {
        InternetMes mes = new InternetMes()
        {
            protocol_key = InterNetHandle.PING,
        };
        SendMes(JsonConvert.SerializeObject(mes));
    }

    private void ClientHeartHeat(int _)
    {
        if (canHeart && isConnected)
        {
            if (Time.time - lastHeartHeatTime > heartHeatDelta)
            {
                Debug.LogError("Heart heat overtime!");
                isConnected = false;
                Reconnect();
            }
            else
                SendHeartHeat();
        }
    }

    public void SendTransformMes(MsgInfo msgInfo)
    {
        InternetMes mes = new InternetMes()
        {
            protocol_key = InterNetHandle.TRANSFORM_MESSAGE,
            data = msgInfo
        };
        SendMes(JsonConvert.SerializeObject(mes));
    }

    public void SendMes(string mes)
    {
        try
        {
            if (mSocket != null && mSocket.ReadyState != WebSocketState.Closed)
            {
                mes = AESManager.Instance.TryEncrypt(mes);
                byte[] buffer = Encoding.UTF8.GetBytes(mes);
                mSocket.SendAsync(buffer);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("∑¢ÀÕ ß∞‹! " + e.Message);
        }
    }

    public void Close()
    {
        reconnectTimer?.Cancel();
        heartHeatTimer?.Cancel();
        if (mSocket != null)
        {
            mSocket.CloseAsync();
            mSocket = null;
        }
        isConnected = false;
        canHeart = false;
    }
}

public class InternetMes
{
    public string protocol_key;
    public object data;
}

public class InternetNetInfo
{
    public int err;
    public string token_id;
    public string device_ip;
}

public class VideoForwardMes
{
    public string handle;
    public string data;
}

public class InterNetHandle
{
    public const string PING = "ping";
    public const string LOGIN = "login";
    public const string DEVICE_LOGIN = "device_login";
    public const string TRANSFORM_MESSAGE = "transform_message";
}
