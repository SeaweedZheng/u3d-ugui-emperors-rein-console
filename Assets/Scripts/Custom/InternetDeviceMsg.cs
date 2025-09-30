using Newtonsoft.Json;
using SBoxApi;
using System.Collections.Generic;
using UnityEngine;

public class InternetDeviceMsg : BaseManager<InternetDeviceMsg>
{
    public void Init()
    {
        EventCenter.Instance.AddEventListener<InternetMes>(EventHandle.INTERNET_SERVER_INFO, OnInternetServerData);
    }

    private void OnInternetServerData(InternetMes internetMes)
    {

        switch (internetMes.protocol_key)
        {
            case InterNetHandle.DEVICE_LOGIN:
                OnDeviceLogin(JsonConvert.DeserializeObject<InternetLoginMes>(internetMes.data.ToString()));
                break;
            case InterNetHandle.TRANSFORM_MESSAGE:
                OnTransformMessage(JsonConvert.DeserializeObject<InternetTransformMes>(internetMes.data.ToString()));
                break;
            default:
                break;
        }
    }

    private void OnDeviceLogin(InternetLoginMes internetLoginMes)
    {
        AESManager.Instance.RefreshKey(internetLoginMes.aes_key, internetLoginMes.aes_iv);
    }

    private void OnTransformMessage(InternetTransformMes internetTransformMes)
    {
        switch (internetTransformMes.cmd)
        {
            case (int)C2S_CMD.C2S_InitJackpotInfo:
                OnInitJackpotInfo(internetTransformMes);
                break;
            case (int)C2S_CMD.C2S_JackpotBet:
                OnJackpotBet(internetTransformMes);
                break;
            default:
                break;
        }
    }

    private void OnInitJackpotInfo(InternetTransformMes internetTransformMes)
    {
        InitJackpotInfo initJackpotInfo = new InitJackpotInfo
        {
            sBoxJackpotData = Model.Instance.sBoxJackpotData,
            highestWinsOrderData = Model.Instance.highestWinsOrderData,
        };
        MsgInfo msgInfo = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_InitJackpotInfo,
            id = 1,
            jsonData = JsonConvert.SerializeObject(initJackpotInfo)
        };
        InternetServerMgr.Instance.SendTransformMes(msgInfo);
    }

    private void OnJackpotBet(InternetTransformMes internetTransformMes)
    {
        SBoxJackpotBet sBoxJackpotBet = JsonConvert.DeserializeObject<SBoxJackpotBet>(internetTransformMes.jsonData);
        EventCenter.Instance.EventTrigger(EventHandle.JACKPOT_BET, sBoxJackpotBet);
        //OrderData orderData = JsonConvert.DeserializeObject<OrderData>(internetTransformMes.jsonData);
        //Controller.Instance.WinJackpot(new WinJackpotInfo
        //{
        //    macId = orderData.macId,
        //    seat = orderData.seatId,
        //    jackpotId = orderData.jackpotId,
        //    win = orderData.wins
        //});
    }
}
