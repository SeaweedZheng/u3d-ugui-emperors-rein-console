//服务器发送给客户端的消息
public enum S2C_CMD
{
    S2C_HeartHeat = 1000,                      //心跳
    S2C_WinJackpot,                            //获得彩金
    S2C_Error,                                 //错误
    S2C_GetJackpotData,                        //获取彩金数据
    S2C_JackpotMinBet,                         //彩金最小押分
    S2C_ChangeLanguage,                        //多语言切换


    S2C_InitJackpotInfo = 1500,                //初始化彩金信息
    S2C_JackpotBet,                            //彩金下注
    S2C_KickOut,                               //踢出
    S2C_ConnectFail,                           //连接失败

    //【推币机】新加的协议
    S2C_ReadConfR,  // 返回配置
}
//客户端发送给服务器的消息
public enum C2S_CMD
{
    C2S_HeartHeat = 2000,                      //心跳
    C2S_Login,                                 //登录
    C2S_JackBet,                               //下注
    C2S_ReceiveJackpot,                        //领取彩金
    C2S_GetJackpotData,                        //获取彩金数据

    C2S_InitJackpotInfo = 2500,                //初始化彩金信息
    C2S_JackpotBet,                            //彩金下注

    //【推币机】新加的协议
    C2S_ReadConf,                            //读取配置

}