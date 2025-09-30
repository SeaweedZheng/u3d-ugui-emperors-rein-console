using SBoxApi;
using System.Collections.Generic;

public class InternetLoginMes
{
    public int err;
    public string device_id;
    public string name;
    public string aes_key;
    public string aes_iv;
}

public class InternetTransformMes
{
    public string user_id;
    public string device_id;
    public int err;
    public int id;
    public string jsonData;
    public int cmd;
}

public class InitJackpotInfo
{
    public SBoxJackpotData sBoxJackpotData;
    public Dictionary<int, List<OrderData>> highestWinsOrderData;
}
