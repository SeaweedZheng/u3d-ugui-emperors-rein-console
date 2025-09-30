using System.Net.Sockets;
//服务器信息
public class ServerInfo
{
    public string IP { get; set; }
    public int port { get; set; }
}

//服务器收到的数据结构(TOOD 此处可能需要优化)
public class SrvMsgData
{
    public Socket mSocket { get; set; }
    public string mData { get; set; }
}

//服务器收到的websocket数据结构
public class WSSrvMsgData
{
    public WebSockets.ClientConnection Client { get; set; }
    public string Data { get; set; }
}

//消息体
public class MsgInfo
{
    public int cmd { get; set; }        //协议
    public int id { get; set; }         //这里一般都是机台ID
    public string jsonData; //
}

//C2S_JackBet
public class JackBetInfo
{
    public int gameType;                       // 游戏类型
    public int seat;                           // 分机号/座位号                   
    public int bet;                            // 当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用
    public int betPercent;                     // 100 - 押分比例，目前拉霸(推币机)默认值传1，同样需要乘以100          
    public int scoreRate;                      // 1000 - 分值比，1分多少钱，需要乘以1000再往下传
    public int JPPercent;                      // 1000 - 分机彩金百分比，每次押分贡献给彩金的比例。需要乘以1000再往下传
}


public class JackBetInfoCoinPush
{
    public int gameType;                       // 游戏类型
    public int seat;                           // 分机号/座位号                   
    public int betPercent;                     // 100 - 押分比例，目前拉霸(推币机)默认值传1，同样需要乘以100          
    public int scoreRate;                      // 1000 - 分值比，1分多少钱，需要乘以1000再往下传
    public int JPPercent;                      // 1000 - 分机彩金百分比，每次押分贡献给彩金的比例。需要乘以1000再往下传

    public int majorBet;  // major贡献值  (当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用)
    public int grandBet;  // grand贡献值  (当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用)
}



//S2C_WinJackpot
public class WinJackpotInfo
{
    public int macId;
    public int seat;
    public int win;
    public int jackpotId;
    public long orderId;
    public long time;
}




//S2C_Error
public class ErrorInfo
{
    public int errCode;
    public string errString;
}


public enum GameType
{
    None = 0,
    CoinPusher = 1,
    Slot = 2,
}
//C2S_login
public class LoginInfo
{
    /// <summary> 游戏类型 </summary>
    public int gameType;
    /// <summary> 机台id </summary>
    public int macId;

    // 组号
    public int groudId;
    // 座位号
    public int seatId;

}

//C2S_ReceiveJackpot
public class ReceiveJackpotInfo
{
    public int gameType;
    public long orderId;
}

public enum OrderDataMode
{
    Grand = 0,
    Major = 1,
    Minor = 2,
    Mini = 3,
    Fix = 4
}


public class RequestBaseInfo
{
    public int gameType;
}