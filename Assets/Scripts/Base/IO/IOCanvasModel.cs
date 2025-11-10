
using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    chs,
    cht,
    en
}

public enum Month
{
    Jan,
    Feb,
    Mar,
    Apr,
    May,
    Jun,
    Jul,
    Aug,
    Sep,
    Oct,
    Nov,
    Dec,
}

public enum IOState
{
    CheckPermissions,
    SelectFunction,
    TestScreen,
    TestTouch,

    /// <summary> 参数设置界面 </summary>
    Params,
    Bill,
    Clients,
    Code,
    EditPassword,
    DateTime,
    JackpotSetting,
    JackpotBet,
    JaclpotWins,
    ProSetting,


    //子页-推币机硬件测试
    SelectCoinPushMachine,
    CheckCoinPushHardware,
}

public enum IOSectionState
{
    GroupId,
    BallValue,

    NewGameMode,
    SkillMode,
    CountDown,
    MinBet,
    LimitBetsWins,
    CoinRatio,
    TicketRatio,
    ScoreUpRatio,
    Language,
    RefundMode,
    LineId,
    MacId,
    EditPassword,
    NewPassword,
    //PrinterMode,
    RecordMode,
    //PrintTime,
    //PrintDensity,
    ClientWinLock,
    //RoundWinLock,
    //NetSwitch,
    OffsetRatio,
    LEDBrightness,
    SoundVolumScale,
    DateTime,
    EditLineId,
    EditMacId,
    JackpotSwitch,
    JackpotPercent,
    JackpotLimit,
    //JackpotLevel,
    WaveGameCount,
}

/// <summary>
/// 最外层菜单
/// </summary>
public enum IOFunction
{
    Params,
    Bill,
    Code,
    Language,
    EditPassword,
    ModifiedDate,
    JackpotSetting,
    JackpotBet,
    JackpotWins,
    ClientData,
    ProSetting,
    Exit,


    //推币机
    SelectCoinPushMachine,

    /// <summary> 后台菜单页子项-硬件测试 </summary>
    CheckCoinPushHardware,
}

/// <summary>
/// 参数设置(与排序相关)
/// </summary>
public enum IOParams
{
    //NewGameMode,

    /// <summary> 组id </summary>
    GroupId,



    /// <summary> 倒计时 </summary>
    //#seaweed# CountDown,

    /// <summary> 最小鼓励 </summary>
    //#seaweed# MinBet,
    //LimitBetsWins,
    //ScoreUpRatio,

    /// <summary> 投币比例 </summary>
    CoinRatio,

    /// <summary> 彩票比例 </summary>
    TicketRatio,


    /// <summary> 1球多少分 </summary>
    BallValue,

    /// <summary> 上下分（新增） </summary>
    ScoreUpRatio,

    /// <summary> 退票模式 </summary>
    //#seaweed# RefundMode,
    //PrintDensity,
    //PrinterMode,
    //RecordMode,
    //PrintTime,

    /// <summary> 切换单位 </summary>
    //#seaweed# SkillMode,

    /// <summary> 分机暴击 </summary>
    //#seaweed# ClientWinLock,
    //RoundWinLock,
    //NetSwitch,

    /// <summary> 脉冲比例 </summary>
    //#seaweed# OffsetRatio,

    /// <summary> LED亮度 </summary>
    //#seaweed# LEDBrightness,

    /// <summary> 音量设置 </summary>
    //#seaweed# SoundVolumScale,

    /// <summary> 彩金门槛 </summary>
    //#seaweed# JackpotLimit,
    //JackpotLevel,
    Save,
    Return,
    WaveGameCount,
    OpenBox,
    ModifiedDate,
    ProSetting,

}

public enum IOBill
{
    Clients,
    Return
}

public enum IOClients
{
    Server,
    PrePage,
    NextPage,
    Return
}

public enum IOPlaceMode
{
    Normal,
    Skill,
    Pro,
}

public enum IOProSetting
{
    LineId,
    MacId,
    WinLock,
    EditPassword,
}

public enum IOMagnificationMode
{
    Low,
    Hight,
    Random
}

public enum IORefundMode
{
    NormalMode,
    Immediately
}

public enum IOPrinterMode
{
    NoPrint,
    NormallyPrint,
    StretchPrint
}

public enum IORecordMode
{
    Word,
    Icon
}

public enum IOLanguage
{
    ch,
    en
}

public enum IODifficult
{
    Visitor,
    Primary,
    Middle,
    Professional,
    Master,
    Supreme
}

public enum IONewGameMode
{
    Auto,
    Manual,
}

public enum IONetSwitch
{
    NetSwitchClose,
    NetSwitchOpen
}   

public enum IOJackpotSetting
{
    JackpotSwitch,
    JpPercent,
    Save,
    Return
}

public enum IOJackpotSwitch
{
    JackpotSwitchOff,
    JackpotSwitchOn,
}


public enum IOCheckCoinPushHardware
{
    CheckLaunchCoin,
    CheckLaunchBall,
    CheckPushPlate,
    CheckWiper,
    CheckBell,
}


public class BillData
{
    public int coinIn;
    public int ticketOut;
    public int scoreUp;
    public int scoreDown;
    public int bets;
    public int wins;
    public int credit;
    public int delayBet;
    public int delayWin;
    public List<SBoxPlayerAccount> playerAccountList = new List<SBoxPlayerAccount>();
}

public class JackpotConfig
{
    public int jackpotSwitch;
    public int betPercent;
    public int jpPercent;
}

/*
public class BetData
{
    public int logicId;
    public int seatId;
    public long bet;
    public int betPercent;
    public int scoreRate;
    public int jpPercent;
    public long win;
    public long grandWin;
    public int grandTimes;
    public long majorWin;
    public int majorTimes;
    public long minorWin;
    public int minorTimes;
    public long miniWin;
    public int miniTimes;
}*/


public class IOCanvasModel : BaseManager<IOCanvasModel>
{
    private Dictionary<int, Dictionary<int, List<BetData>>> _betDataDic;

    public Dictionary<int, Dictionary<int, List<BetData>>> BetDataDic
    {
        set
        {
            _betDataDic = value;
            DealWithBetData();
        }
        get
        {
            return _betDataDic;
        }
    }

    public List<BetData> betDatas;

    public bool mouseClick = true;

    public Language curlanguage = Language.chs;
    public Language CurLanguage
    {
        set
        {
            curlanguage = value;
            defaultFontSize = curlanguage == Language.en ? 38 : 50;
        }
    }
    public int defaultFontSize = 30;

    public int tempNetSwitch;
    public int netSwitch;
    public int winLockBalance;

    public string sBoxVersion;

    public int totalCoinOutCont;
    public bool coinOuting;

    public IOState state;
    public int permissions;
    public bool horizontal;
    public bool isInitMenu;
    public int curBillPage = -1;
    public const int COLUMN_COUNT = 4;
    public Color selectedColor = Color.red;
    public Color disableColor = Color.gray;

    public Color imageNormalColor = Color.white;
    public Color textNormalColor = new Color(248 / 255f, 223 / 255f, 27 / 255f);
    public Color textSelectedColor = Color.green;

    public bool coinIning;
    public bool isGetTrigger;

    public SBoxConfData CfgData
    {
        set
        {
            cfgData = value;
            SetTempCfgData(value);
            switchList = new List<int> { cfgData.SwitchBetsUnitMin, cfgData.SwitchBetsUnitMid, cfgData.SwitchBetsUnitMax };
        }
        get { return cfgData; }
    }

    private SBoxConfData cfgData;
    public SBoxConfData tempCfgData;


    /// <summary>
    /// 这个是联网彩金服的配置（待去掉）
    /// </summary>
    public SBoxJackpotConfigData JackCfgData
    {
        set
        {
            jackCfgData = value;
            SetTempJackCfgData(jackCfgData);
            List<int> minBets = new List<int>();
            for (int i = 0; i < jackCfgData.sBoxJackpotConfigDataItem.Length; i++)
                minBets.Add(jackCfgData.sBoxJackpotConfigDataItem[i].MinBet);
            minBets.Reverse();
            MsgInfo mes = new MsgInfo
            {
                cmd = (int)S2C_CMD.S2C_JackpotMinBet,
                id = -1,
                jsonData = JsonConvert.SerializeObject(minBets)
            };

            NetMgr.Instance.SendToAllClient(JsonConvert.SerializeObject(mes));
        }
        get { return jackCfgData; }
    }
    private SBoxJackpotConfigData jackCfgData;
    public SBoxJackpotConfigData tempJackpotCfgData;

    public void SetTempJackCfgData(SBoxJackpotConfigData data)
    {
        tempJackpotCfgData = Utils.DeepCopy(data);
        tempJackpotCfgData.sBoxJackpotConfigDataItem = new SBoxJackpotConfigDataItem[data.sBoxJackpotConfigDataItem.Length];
        for (int i = 0; i < data.sBoxJackpotConfigDataItem.Length; i++)
            tempJackpotCfgData.sBoxJackpotConfigDataItem[i] = Utils.DeepCopy(data.sBoxJackpotConfigDataItem[i]);
    }

    public int SoundVolumScale
    {
        set
        {
            soundVolumScale = value;
            PlayerPrefs.SetInt("soundVolumScale", soundVolumScale);
            PlayerPrefs.GetInt("soundVolumScale");
            AudioManager.Instance.SoundVolumScale = float.Parse(soundVolumScale.ToString()) / 100f;
        }
        get { return soundVolumScale; }
    }
    private int soundVolumScale;

    public int LEDBrightness
    {
        set
        {
            ledBrightness = value;
            PlayerPrefs.SetInt("ledBrightness", ledBrightness);
            PlayerPrefs.GetInt("ledBrightness", 5);
        }
        get { return ledBrightness; }
    }
    private int ledBrightness;


    public JackpotConfig jackpotCfg;
    public JackpotConfig tempJackpotCfg;

    public Dictionary<int, List<BetData>> betDataDic;
    public List<BetData> betDataList;

    public SBoxCoderData coderData;

    public BillData billData = new BillData();

    public List<int> switchList = new List<int> { 10, 50, 100 };

    public readonly List<int> SWITCH_LIMIT_MIN = new List<int> { 10, 50, 100 };
    public readonly List<int> SWITCH_LIMIT_MAX = new List<int> { 50, 200, 500 };




    public const int MIN_COUNT_DOWN = 10;
    public const int MAX_COUNT_DOWN = 30;

    public const int MIN_MIN_BET = 10;
    public const int MAX_MIN_BET = 1000;

    public const int MIN_MAX_BET = 100;
    public const int MAX_MAX_BET = 9999;

    public const int MIN_LIMIT_BETS_WINS = 1000;
    public const int MAX_LIMIT_BETS_WINS = 100000;

    //#seaweed# public const int MIN_COIN_RATIO = 5;
    public const int MIN_COIN_RATIO = 1;
    public const int MAX_COIN_RATIO = 200;

    public const int MAX_TICKET_VALUE = 200;
    public const int MAX_VALUE_TICKET = 50;

    public const int MIN_SCORE_UP_RATIO = 100;
    public const int MAX_SCORE_UP_RATIO = 10000;

    public const int MIN_PRINT_TIME = 10;
    public const int MAX_PRINT_TIME = 120;

    public const int MIN_PRINT_DENSITY = 0;
    public const int MAX_PRINT_DENSITY = 2;

    public const int MIN_CLIENT_WIN_LOCK = 0;
    public const int MAX_CLIENT_WIN_LOCK = 300000;

    public const int MIN_ROUND_WIN_LOCK = 100000;
    public const int MAX_ROUND_WIN_LOCK = 500000;

    public const int MIN_DOOR_OPEN_TIME = 20;
    public const int MAX_DOOR_OPEN_TIME = 120;

    public const int MIN_OFFSET_RATIO = 1;
    public const int MAX_OFFSET_RATIO = 100;

    public const int MIN_JACKPOT_PERCENT = 1;
    public const int MAX_JACKPOT_PERCENT = 100;

    public const int MIN_LED_BRIGHTNESS = 0;
    public const int MAX_LED_BRIGHTNESS = 15;

    public const int MIN_SOUND_VOLUM_SCALE = 0;
    public const int MAX_SOUND_VOLUM_SCALE = 100;

    public const int MIN_BETS_MIN_OF_JACKPOT = 0;
    public const int MAX_BETS_MIN_OF_JACKPOT = 1000;

    public const int MIN_JACKPOT_LEVEL = 0;
    public const int MAX_JACKPOT_LEVEL = 6;

    public const int MIN_WAVE_GAME_COUNT = 3;
    public const int MAX_WAVE_GAME_COUNT = 10;

    public ulong password;
    public ulong tempPassword;
    public DateTime IODateTime;

    public int waveGameCount = 0;
    public int tempWaveGamecout = 0;

    public Dictionary<Language, Dictionary<string, string>> languageDic;

    public string GetDate(int remainMinute)
    {
        int temp = remainMinute;
        int day = temp / 1440;
        temp %= 1440;
        int hour = temp / 60;
        temp %= 60;
        return day + Utils.GetLanguage("Day") + hour + Utils.GetLanguage("Hour") + temp + Utils.GetLanguage("Minute");
    }

    public void SetTempCfgData(SBoxConfData data)
    {
        tempCfgData = new SBoxConfData
        {
            result = data.result,
            PwdType = data.PwdType,
            PlaceType = data.PlaceType,
            difficulty = data.difficulty,
            odds = data.odds,
            WinLock = data.WinLock,
            MachineId = data.MachineId,
            LineId = data.LineId,
            TicketMode = data.TicketMode,
            TicketValue = data.TicketValue,
            scoreTicket = data.scoreTicket,
            CoinValue = data.CoinValue,
            MaxBet = data.MaxBet,
            MinBet = data.MinBet,
            CountDown = data.CountDown,
            MachineIdLock = data.MachineIdLock,
            BetsMinOfJackpot = data.BetsMinOfJackpot,
            JackpotStartValue = data.JackpotStartValue,
            LimitBetsWins = data.LimitBetsWins,
            ReturnScore = data.ReturnScore,
            SwitchBetsUnitMin = data.SwitchBetsUnitMin,
            SwitchBetsUnitMid = data.SwitchBetsUnitMid,
            SwitchBetsUnitMax = data.SwitchBetsUnitMax,
            ScoreUpUnit = data.ScoreUpUnit,
            PrintMode = data.PrintMode,
            ShowMode = data.ShowMode,
            CheckTime = data.CheckTime,
            OpenBoxTime = data.OpenBoxTime,
            PrintLevel = data.PrintLevel,
            PlayerWinLock = data.PlayerWinLock,
            LostLock = data.LostLock,
            LostLockCustom = data.LostLockCustom,
            PulseValue = data.PulseValue,
            NewGameMode = data.NewGameMode,

            BallValue = data.BallValue,
        };
    }

    public void DealWithBetData()
    {
        List<BetData> list = new List<BetData>();
        foreach (Dictionary<int, List<BetData>> seatBetDic in BetDataDic.Values)
        {
            foreach (var item in seatBetDic.Values)
            {
                foreach (var item1 in item)
                {
                    list.Add(item1);
                }
            }
        }

        list.Sort(
            (x, y) =>
            {
                if (x.logicId > y.logicId)
                    return 1;
                else if (x.logicId < y.logicId)
                    return -1;
                else
                {
                    if (x.seatId > y.seatId)
                        return 1;
                    else
                        return -1;
                }

            });
        betDatas = list;
    }

    public void DealWithBetDataDic()
    {
        betDataList = new List<BetData>();
        foreach (var clientBetDataList in betDataDic.Values)
            foreach (var item in clientBetDataList)
                if (item.jpPercent == jackpotCfg.jpPercent && int.Parse(((1f / cfgData.CoinValue) * 1000).ToString()) == item.scoreRate)
                    betDataList.Add(item);
        betDataList.Sort((a, b) => b.seatId.CompareTo(a.seatId));
    }


    /// <summary>
    /// 后台修改彩金组号id 临时保存
    /// </summary>
    public int tempGroupId = 0;

    /// <summary> 彩金后台 组id </summary>
    public int groupId
    {
        get
        {
            if (_groupId == null) {
                //PlayerPrefs.DeleteKey(PARAM_GROUP_ID);_groupId = 0;
                _groupId = PlayerPrefs.GetInt(PARAM_GROUP_ID, 0);
            }
            return (int)_groupId;
        }
        set
        {
            _groupId = value;
            PlayerPrefs.SetInt(PARAM_GROUP_ID, value);
            PlayerPrefs.Save();
        }
    }
    int? _groupId = null;
    const string PARAM_GROUP_ID = "PARAM_GROUP_ID";


    /// <summary> 座位号机台号绑定 </summary>
    public Dictionary<int,int> seatIdMacIdMap = new Dictionary<int,int>();


    /// <summary> 一球几分 </summary>
    public int BallValue
    {
        get => CfgData.BallValue;
        set => CfgData.BallValue = value;
    }

    //public int tempBallValue = 0;
    public  int[] BallValueLst = new int[] { 1, 2, 5, 10, 20, 100 };



    // 当前显示的彩金值
    public int[] CurJackpotOut = new int[] { 0, 0 ,0 ,0};
}