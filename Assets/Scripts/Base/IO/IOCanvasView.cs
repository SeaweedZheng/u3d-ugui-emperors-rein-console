using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class SectionData
{
    public bool selected;
    public IOBaseSection baseSection;
    public IOSectionState state;
}

public partial class IOCanvasView : MonoSingleton<IOCanvasView>
{
    private int curSelect;
    private bool isGetJackpotData;
    private bool betDataCoinValueShow;

    /// <summary>
    /// 当前选择设置的对象
    /// </summary>
    [HideInInspector]
    public SectionData selectSection = new SectionData();  
    private IOState State
    {
        set { IOCanvasModel.Instance.state = value; }
        get { return IOCanvasModel.Instance.state; }
    }

    private Text title;
    private Text subtitle;

    private List<IOBaseShow> titleLeftList = new List<IOBaseShow>();
    private List<IOBaseShow> titleRightList = new List<IOBaseShow>();

    #region prefabs
    private GameObject baseShowPrefab;
    private GameObject baseShowPrefab1;
    private GameObject baseShowPrefab2;
    private GameObject baseShowPrefab3;
    private GameObject baseShowPrefab4;
    private GameObject baseShowPrefab5;
    private GameObject baseShowPrefab6;
    private GameObject baseBtnPrefab;
    private GameObject baseBtnPrefab1;
    private GameObject baseBtnPrefab2;
    private GameObject baseTogglePrefab;
    private GameObject baseSectionPrefab;
    private GameObject baseSectionPrefab1;
    private GameObject ticketRatioPrefab;
    private GameObject switchSectionsPrefab;
    private GameObject dateTimeSectionsPrefab;
    private GameObject basePassword;
    private GameObject modDate;

    private RectTransform IOArrowRect;
    private MyButton arrowUp;
    private Image arrowUpImg;
    private MyButton arrowDown;
    private Image arrowDownImg;
    private GameObject modDatePanel;
    private IOBaseShow dateShow;
    private IOBaseShow diffcultyShow;

    private IOBaseBtn scoreShowBtn;
    private List<IOBaseShow> betDataShowList = new List<IOBaseShow>();

    #endregion

    [HideInInspector]
    public IOPasswordPanel passwordPanel;
    private Transform menuPanel;
    private GridLayoutGroup gridLayout;
    private RectTransform gridLayoutRect;
    private List<IOBaseSelection> selectionList = new List<IOBaseSelection>();
    private List<IOBaseSelection> onlyShowList = new List<IOBaseSelection>();
    private List<GameObject> tempObjList = new List<GameObject>();

    private Coroutine loadingCoroutine;

    private void Awake()
    {
        ResMgr.Instance.LoadAssetBundle("io", "baseShow0", (obj) => baseShowPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow1", (obj) => baseShowPrefab1 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow2", (obj) => baseShowPrefab2 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow3", (obj) => baseShowPrefab3 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow4", (obj) => baseShowPrefab4 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow5", (obj) => baseShowPrefab5 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow6", (obj) => baseShowPrefab6 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseButton0", (obj) => baseBtnPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseButton1", (obj) => baseBtnPrefab1 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseButton2", (obj) => baseBtnPrefab2 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseToggle", (obj) => baseTogglePrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseSection0", (obj) => baseSectionPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseSection1", (obj) => baseSectionPrefab1 = (GameObject)obj);

        ResMgr.Instance.LoadAssetBundle("io", "ticketRatio", (obj) => ticketRatioPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "switchSection", (obj) => switchSectionsPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "dateTimeSection", (obj) => dateTimeSectionsPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "basePassword", (obj) => basePassword = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "modDate", (obj) => modDate = (GameObject)obj);

        IOCanvasManager.Instance.Init(this);
        title = transform.Find("title/center/title").GetComponent<Text>();
        subtitle = transform.Find("title/center/subtitle").GetComponent<Text>();
        IOArrowRect = transform.Find("IOArrow").GetComponent<RectTransform>();
        arrowUp = IOArrowRect.Find("up").GetComponent<MyButton>();
        arrowUpImg = arrowUp.transform.Find("Image").GetComponent<Image>();
        arrowDown = IOArrowRect.Find("down").GetComponent<MyButton>();
        arrowDownImg = arrowDown.transform.Find("Image").GetComponent<Image>();
        var titleLeft = transform.Find("title/left");
        for (int i = 0; i < titleLeft.childCount; i++)
        {
            var baseShow = titleLeft.GetChild(i).GetComponent<IOBaseShow>();
            titleLeftList.Add(baseShow);
            baseShow.gameObject.SetActive(false);
        }
        var titleRight = transform.Find("title/right");
        for (int i = 0; i < titleRight.childCount; i++)
        {
            var baseShow = titleRight.GetChild(i).GetComponent<IOBaseShow>();
            titleRightList.Add(baseShow);
            baseShow.gameObject.SetActive(false);
        }

        arrowUp.OnPointerDownEvent.AddListener(OnArrowUpPointerDown);
        arrowUp.OnPointerUpEvent.AddListener(OnArrowUpPointerUp);

        arrowDown.OnPointerDownEvent.AddListener(OnArrowDownPointerDown);
        arrowDown.OnPointerUpEvent.AddListener(OnArrowDownPointerUp);
    }

    private void OnEnable()
    {
        isGetJackpotData = false;
        InitManagerPasswordPanel();
        AddEventListener();
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener<KeyCode>(EventHandle.KEY_DOWN, OnKeyDown);
        EventCenter.Instance.AddEventListener<KeyCode>(EventHandle.KEY_UP, OnKeyUp);
        EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_UP, OnHardwareKeyUp);
        EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_DOWN, OnHardwareKeyDown);
        EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_CLICK, OnHardwareClick);

        EventCenter.Instance.AddEventListener<int>(EventHandle.PASSWORD_CUR_SELECT, OnPasswordCurSelect);
        EventCenter.Instance.AddEventListener<int>(EventHandle.PASSWORD_INPUT, OnPasswordInput);
        EventCenter.Instance.AddEventListener(EventHandle.PASSWORD_DELETE, OnPasswordDelete);
        EventCenter.Instance.AddEventListener(EventHandle.PASSWORD_CONFIRM, OnPasswordConfirm);
        EventCenter.Instance.AddEventListener(EventHandle.REFRESH_IOCANVAS, OnRefreshIOCanvas);
        EventCenter.Instance.AddEventListener(EventHandle.INIT_BILL_VIEW, OnInitBillView);
        EventCenter.Instance.AddEventListener(EventHandle.INIT_CODER_VIEW, OnInitCoderView);
        EventCenter.Instance.AddEventListener(EventHandle.GET_SANDBOX_DATE, OnGetSandboxDate);
        //EventCenter.Instance.AddEventListener<Dictionary<int, List<BetData>>>(EventHandle.GET_NET_JACKPOT_DATA, OnGetNetJackpotData);
    }

    private void RemoveEventListener()
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>(EventHandle.KEY_DOWN, OnKeyDown);
        EventCenter.Instance.RemoveEventListener<KeyCode>(EventHandle.KEY_UP, OnKeyUp);
        EventCenter.Instance.RemoveEventListener<ulong>(EventHandle.HARDWARE_KEY_UP, OnHardwareKeyUp);
        EventCenter.Instance.RemoveEventListener<ulong>(EventHandle.HARDWARE_KEY_DOWN, OnHardwareKeyDown);
        EventCenter.Instance.RemoveEventListener<ulong>(EventHandle.HARDWARE_KEY_CLICK, OnHardwareClick);

        EventCenter.Instance.RemoveEventListener<int>(EventHandle.PASSWORD_CUR_SELECT, OnPasswordCurSelect);
        EventCenter.Instance.RemoveEventListener<int>(EventHandle.PASSWORD_INPUT, OnPasswordInput);
        EventCenter.Instance.RemoveEventListener(EventHandle.PASSWORD_DELETE, OnPasswordDelete);
        EventCenter.Instance.RemoveEventListener(EventHandle.REFRESH_IOCANVAS, OnRefreshIOCanvas);
        EventCenter.Instance.RemoveEventListener(EventHandle.INIT_BILL_VIEW, OnInitBillView);
        EventCenter.Instance.RemoveEventListener(EventHandle.INIT_CODER_VIEW, OnInitCoderView);
        EventCenter.Instance.RemoveEventListener(EventHandle.GET_SANDBOX_DATE, OnGetSandboxDate);
    }

    private void InitManagerPasswordPanel()
    {
        if (passwordPanel != null)
            return;
        IOCanvasModel.Instance.state = IOState.CheckPermissions;
        passwordPanel = Instantiate(basePassword, transform).GetComponent<IOPasswordPanel>();
        passwordPanel.SetParams(placeholderStr: "Please enter password");
        passwordPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(30, 450);
        passwordPanel.transform.localScale = Vector3.one;
    }

    public void DestroyManagerPasswordPanel()
    {
        Destroy(passwordPanel.gameObject);
        passwordPanel = null;
    }

    public void InstantiateMenuPanel()
    {
        var trans = new GameObject("menuPanel", typeof(GridLayoutGroup)).transform;
        trans.SetParent(transform);
        trans.localScale = Vector3.one;
        trans.localPosition = Vector3.zero;
        menuPanel = trans;

        InitFunctionPanel();
    }

    private void SetMenuGridLayout()
    {
        gridLayout = menuPanel.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(490f, 150f);
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        gridLayoutRect = menuPanel.GetComponent<RectTransform>();
        gridLayoutRect.anchorMin = new Vector2(0.5f, 0);
        gridLayoutRect.anchorMax = new Vector2(0.5f, 0);
        gridLayoutRect.sizeDelta = new Vector2(1920f, 950);
        gridLayoutRect.anchoredPosition = new Vector2(0, 470f);
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.spacing = new Vector2(180f, 30f);
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
    }

    public void ClearMenuBtn()
    {
        selectionList.ForEach(baseBtn =>
        {
            if (baseBtn != null)
                Destroy(baseBtn.gameObject);
        });
        selectionList.Clear();
        onlyShowList.ForEach(baseShow => {
            if (baseShow != null)
                Destroy(baseShow.gameObject);
        });
        dateShow = null;
        diffcultyShow = null;
        onlyShowList.Clear();
        tempObjList.ForEach(temp => {
            if (temp != null)
                Destroy(temp);
        });
        tempObjList.Clear();
    }

    public void InitFunctionPanel()
    {
        SetMenuGridLayout();
        title.text = Utils.GetLanguage("Menu");
        subtitle.text = "";
        int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 54 : 66;
        InstantiateBaseBtn(IOFunction.Params.ToString(), InitParamsPanel, fontSize: fontSize);
        //#seaweed# InstantiateBaseBtn(IOFunction.Bill.ToString(), InitBillPanel, fontSize: fontSize);
        InstantiateBaseBtn(IOFunction.Bill.ToString(), () => IOPopTips.Instance.ShowTips(Utils.GetLanguage("Comming soon")), fontSize: fontSize);

        InstantiateBaseBtn(IOFunction.Code.ToString(), InitCodePanel, fontSize: fontSize); // 激活报表
        //InstantiateBaseBtn(IOFunction.Code.ToString(), () => { InitCodePanel); }, fontSize: fontSize); 


        InstantiateBaseBtn(IOCanvasModel.Instance.curlanguage.ToString(), ChangeCurLanguage, fontSize: fontSize);
        InstantiateBaseBtn(IOFunction.EditPassword.ToString(), InitEditPasswordPanel, fontSize: fontSize);
        InstantiateBaseBtn(IOFunction.JackpotSetting.ToString(), () => IOPopTips.Instance.ShowTips(Utils.GetLanguage("Comming soon")), fontSize: fontSize);
        //InstantiateBaseBtn(IOFunction.JackpotBet.ToString(), () => IOPopTips.Instance.ShowTips(Utils.GetLanguage("Comming soon")), fontSize: fontSize);
        //InstantiateBaseBtn(IOFunction.JackpotWins.ToString(), () => IOPopTips.Instance.ShowTips(Utils.GetLanguage("Comming soon")), fontSize: fontSize);
        
        
        //#seaweed# InstantiateBaseBtn(IOFunction.CheckCoinPushHardware.ToString(), () => { InitSelectVisibleCoinPushMachinePanel(InitCheckCoinPushHardwarePanel); }, fontSize: fontSize);

        if (IOCanvasModel.Instance.CfgData.MachineIdLock == 0 || IOCanvasModel.Instance.permissions == 3) InstantiateBaseBtn(IOParams.ProSetting.ToString(), InitProSettingPanel, fontSize: fontSize);

       /* if (IOCanvasModel.Instance.CfgData.MachineIdLock == 0 || IOCanvasModel.Instance.permissions == 3)
            InstantiateBaseBtn(IOParams.ProSetting.ToString(), 
                () => { InitSelectVisibleCoinPushMachinePanel(InitProSettingPanel); },
                fontSize: fontSize);*/


        ReturnToSelectVisibleCoinPushMachinePanel(InitProSettingPanel);


        InstantiateBaseBtn(IOFunction.Exit.ToString(), ExitIOCanvas, fontSize: fontSize);
        curSelect = 0;
        SetCurSelect();
    }

    /// <summary>
    /// 参数设置界面
    /// </summary>
    public void InitParamsPanel()
    {
        ClearMenuBtn();
        SetParamsGridLayout();
        IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);

        title.text = Utils.GetLanguage(IOFunction.Params.ToString());
        subtitle.text = "";
        int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 38 : 50;

        // 场地
        //#seaweed# InstantiateEasyShow(IOParams.PlaceMode.ToString(), (IOCanvasModel.Instance.tempCfgData.PlaceType.ToString()), fontSize: fontSize);

        //IOCanvasModel.Instance.tempBallValue = IOCanvasModel.Instance.BallValue;
        IOCanvasModel.Instance.tempGroupId = IOCanvasModel.Instance.groupId;
        InstantiateBaseSection(IOParams.GroupId.ToString(), IOCanvasModel.Instance.groupId, onClick: () => { OnSectionClick((int)IOParams.GroupId, IOSectionState.GroupId);  });

        //#seaweed# InstantiateBaseSection(IOParams.CountDown.ToString(),IOCanvasModel.Instance.tempCfgData.CountDown, onClick: () => { OnSectionClick((int)IOParams.CountDown, IOSectionState.CountDown); }, ioParams: IOParams.CountDown);
        //#seaweed# InstantiateBaseSection(IOParams.MinBet.ToString(), IOCanvasModel.Instance.tempCfgData.MinBet, onClick: () => {  OnSectionClick((int)IOParams.MinBet, IOSectionState.MinBet); });
        InstantiateBaseSection(IOParams.CoinRatio.ToString(), IOCanvasModel.Instance.tempCfgData.CoinValue, onClick: () => {   OnSectionClick((int)IOParams.CoinRatio, IOSectionState.CoinRatio); }, ioParams: IOParams.CoinRatio);
        InstantiateTicketRatioSection(IOParams.TicketRatio.ToString(), onClick: () => {   OnSectionClick((int)IOParams.TicketRatio, IOSectionState.TicketRatio); });
        //#seaweed# InstantiateBaseSection(IOParams.RefundMode.ToString(), Utils.GetEnumNames(typeof(IORefundMode)), IOCanvasModel.Instance.tempCfgData.TicketMode, onClick: () => {   OnSectionClick((int)IOParams.RefundMode, IOSectionState.RefundMode); });
        //#seaweed# InistantiateSwitchSection(IOParams.SkillMode.ToString(), IOCanvasModel.Instance.switchList, onClick: SwitchSectionClick);
        //#seaweed# InstantiateBaseSection(IOParams.ClientWinLock.ToString(), IOCanvasModel.Instance.tempCfgData.PlayerWinLock, onClick: () => {   OnSectionClick((int)IOParams.ClientWinLock, IOSectionState.ClientWinLock); }, ioParams: IOParams.ClientWinLock);
        //#seaweed# InstantiateBaseSection(IOParams.OffsetRatio.ToString(), IOCanvasModel.Instance.tempCfgData.PulseValue, onClick: () => {   OnSectionClick((int)IOParams.OffsetRatio, IOSectionState.OffsetRatio); });
        //#seaweed# InstantiateBaseSection(IOParams.LEDBrightness.ToString(), IOCanvasModel.Instance.LEDBrightness, onClick: () => {   OnSectionClick((int)IOParams.LEDBrightness, IOSectionState.LEDBrightness); });
        //#seaweed# InstantiateBaseSection(IOParams.SoundVolumScale.ToString(), IOCanvasModel.Instance.SoundVolumScale, onClick: () => {   OnSectionClick((int)IOParams.SoundVolumScale, IOSectionState.SoundVolumScale); });
        //#seaweed# InstantiateBaseSection(IOParams.JackpotLimit.ToString(), IOCanvasModel.Instance.tempCfgData.BetsMinOfJackpot, onClick: () => { OnSectionClick((int)IOParams.JackpotLimit, IOSectionState.JackpotLimit); });


        InstantiateBaseSection(IOParams.BallValue.ToString(), IOCanvasModel.Instance.tempCfgData.BallValue, onClick: () => { OnSectionClick((int)IOParams.BallValue, IOSectionState.BallValue); }, ioParams: IOParams.BallValue);



        InstantiateBlank();
        InstantiateBlank();
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = transform;
        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.anchoredPosition = new Vector2(0, -430);
        parentRect.sizeDelta = new Vector2(975, 100);
        parentRect.localScale = Vector3.one;
        InstantiateBaseBtn("Save", SaveConfig, parent: parent.transform, style: 2);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2);
        curSelect = 0;
        SetCurSelect();

        GameObject paramsDescr = new GameObject();
        tempObjList.Add(paramsDescr);
        var paramsDescrText = paramsDescr.AddComponent<Text>();
        paramsDescr.transform.parent = transform;
        var paramsDescrRect = paramsDescr.GetComponent<RectTransform>();
        paramsDescrRect.anchoredPosition = new Vector2(707, -480);
        paramsDescrRect.sizeDelta = new Vector2(458, 200);
        paramsDescrRect.localScale = Vector3.one;
        paramsDescrText.text = Utils.GetLanguage("ParamsDescr");
        paramsDescrText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        paramsDescrText.fontStyle = FontStyle.Bold;
        paramsDescrText.fontSize = 30;
        paramsDescrText.color = IOCanvasModel.Instance.textNormalColor;

        State = IOState.Params;
    }

    private void SetParamsGridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperLeft;
        gridLayout.spacing = new Vector2(295.5f, 55f);
        gridLayout.cellSize = new Vector2(350, 67.5f);
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        gridLayoutRect.sizeDelta = new Vector2(845, 680);
        gridLayoutRect.anchoredPosition = new Vector2(-355, 560);

    }

    private void InitBillPanel()
    {
        ClearMenuBtn();
        SetBillPanelGridLayout();
        State = IOState.Bill;
        title.text = Utils.GetLanguage("BillShow");
        subtitle.text = $"({Utils.GetLanguage("TotalBill")})";
        SBoxIdea.GetAccount();
    }

    private void OnInitBillView()
    {
        ClearMenuBtn();
        IOCanvasModel.Instance.curBillPage = 0;

        int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 48 : 54;

        var parent0 = InstantiateBillParent();
        InstantiateEasyShow(Utils.GetLanguage("AllScoreUp"), IOCanvasModel.Instance.billData.scoreUp.ToString(), parent: parent0, scale: 1, fontSize: fontSize);
        InstantiateEasyShow(Utils.GetLanguage("AllScoreDown"), IOCanvasModel.Instance.billData.scoreDown.ToString(), parent: parent0, scale: 1, fontSize: fontSize);
        InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent0, scale: 1, fontSize: fontSize);
        int profit0 = IOCanvasModel.Instance.billData.scoreUp - IOCanvasModel.Instance.billData.scoreDown;
        InstantiateEasyShow(Utils.GetLanguage("AllProfit"), profit0.ToString(), parent: parent0, scale: 1, fontSize: fontSize);

        var parent2 = InstantiateBillParent();
        InstantiateEasyShow(Utils.GetLanguage("AllCoinIn"), IOCanvasModel.Instance.billData.coinIn.ToString(), parent: parent2, scale: 1f, fontSize: fontSize);
        InstantiateEasyShow(Utils.GetLanguage("AllCollectTicket"), IOCanvasModel.Instance.billData.ticketOut.ToString(), parent: parent2, scale: 1f, fontSize: fontSize);
        InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent2, scale: 1f, fontSize: fontSize);
        int profit2 = IOCanvasModel.Instance.billData.coinIn - IOCanvasModel.Instance.billData.ticketOut;
        InstantiateEasyShow(Utils.GetLanguage("AllProfit"), profit2.ToString(), parent: parent2, scale: 1f, fontSize: fontSize);

        var parent1 = InstantiateBillParent();
        InstantiateEasyShow(Utils.GetLanguage("AllBet"), IOCanvasModel.Instance.billData.bets.ToString(), parent: parent1, scale: 1, fontSize: fontSize);
        InstantiateEasyShow(Utils.GetLanguage("AllWin"), IOCanvasModel.Instance.billData.wins.ToString(), parent: parent1, scale: 1, fontSize: fontSize);
        InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent1, scale: 1, fontSize: fontSize);
        int profit1 = IOCanvasModel.Instance.billData.bets - IOCanvasModel.Instance.billData.wins;
        InstantiateEasyShow(Utils.GetLanguage("AllProfit"), profit1.ToString(), parent: parent1, scale: 1, fontSize: fontSize);


        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = transform;
        parent.transform.localScale = Vector3.one;
        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.anchoredPosition = new Vector2(0, -370);
        parentRect.sizeDelta = new Vector2(875, 100);
        parentRect.localScale = Vector3.one;
        InstantiateBaseBtn("Clients", InitClientBillPanelNo0, parent: parent.transform, style: 2, scale: 1);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1);

        InstantiateBillDetal();

        curSelect = 0;
        SetCurSelect();
    }


#if false
    private void InstantiateBillDetal()
    {
        //DateTime dt = IOCanvasModel.Instance.IODateTime;
        //var str = GetDateString(dt);

        //dateShow = InstantiateEasyShow(str, "", parent: transform, fontSize: 35);
        float x = 950;
        //dateShow.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -390);

        if (IOCanvasModel.Instance.curlanguage == Language.en)
        {
            string str3 = $"ACI:Attendant credit in";
            var baseShow2 = InstantiateEasyShow(str3, "", parent: transform, fontSize: 29);
            baseShow2.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -305);
            string str4 = $"ACO:Attendant credit out";
            var baseShow3 = InstantiateEasyShow(str4, "", parent: transform, fontSize: 29);
            baseShow3.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -345);
        }

        string str1 = $"ID:{IOCanvasModel.Instance.CfgData.LineId}/{IOCanvasModel.Instance.CfgData.MachineId}:{IOCanvasModel.Instance.CfgData.CoinValue}";
        var baseShow1 = InstantiateEasyShow(str1, "", parent: transform, fontSize: 35);
        baseShow1.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -440);

        string tempStr = IOCanvasModel.Instance.winLockBalance / 10000 > 0 ? $"<color=#F8DF1B>{IOCanvasModel.Instance.winLockBalance / 10000}</color>" : $"<color=#FF0000>{IOCanvasModel.Instance.winLockBalance / 10000}</color>";
        string str2 = $"{PlayerPrefs.GetString("CurVersion", "1.0.0")}/{IOCanvasModel.Instance.sBoxVersion}/{IOCanvasModel.Instance.CfgData.difficulty}/{tempStr}";
        diffcultyShow = InstantiateEasyShow(str2, "", parent: transform, fontSize: 35);
        diffcultyShow.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -490);
    }

#endif


    private void InstantiateBillDetal()
    {
        string machineIdStr = $"{curSelectMachineId}";
        string lineIdStr = machineIdStr.Substring(0, machineIdStr.Length - 4);

        float x = 950;


        if (IOCanvasModel.Instance.curlanguage == Language.en)
        {
            string str3 = $"ACI:Attendant credit in";
            var baseShow2 = InstantiateEasyShow(str3, "", parent: transform, fontSize: 29);
            baseShow2.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -305);
            string str4 = $"ACO:Attendant credit out";
            var baseShow3 = InstantiateEasyShow(str4, "", parent: transform, fontSize: 29);
            baseShow3.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -345);
        }

        string str1 = $"ID:{lineIdStr}/{machineIdStr}:{IOCanvasModel.Instance.CfgData.CoinValue}";
        var baseShow1 = InstantiateEasyShow(str1, "", parent: transform, fontSize: 35);
        baseShow1.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -440);

        string tempStr = IOCanvasModel.Instance.winLockBalance / 10000 > 0 ? $"<color=#F8DF1B>{IOCanvasModel.Instance.winLockBalance / 10000}</color>" : $"<color=#FF0000>{IOCanvasModel.Instance.winLockBalance / 10000}</color>";
        string str2 = $"{PlayerPrefs.GetString("CurVersion", "1.0.0")}/{IOCanvasModel.Instance.sBoxVersion}/{IOCanvasModel.Instance.CfgData.difficulty}/{tempStr}";
        diffcultyShow = InstantiateEasyShow(str2, "", parent: transform, fontSize: 35);
        diffcultyShow.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -490);
    }



    private string GetDateString(DateTime dt)
    {
        string str = dt.Year.ToString();
        if (IOCanvasModel.Instance.curlanguage == Language.cht)
        {
            str += Utils.GetLanguage("Year");
            str += dt.Month.ToString();
            str += Utils.GetLanguage("Month");
            str += dt.Day.ToString();
            str += Utils.GetLanguage("Day1");
        }
        else
        {
            str += ", ";
            str += (Month)(dt.Month - 1) + ". ";
            str += dt.Day.ToString() + " ";
        }
        str += dt.Hour.ToString();
        str += ":";
        str += dt.Minute < 10 ? "0" + dt.Minute : dt.Minute.ToString();
        return str;
    }

    private void RefreshDiffcultyShow()
    {
        string tempStr = IOCanvasModel.Instance.winLockBalance / 10000 > 0 ? $"<color=#F8DF1B>{IOCanvasModel.Instance.winLockBalance / 10000}</color>" : $"<color=#FF0000>{IOCanvasModel.Instance.winLockBalance / 10000}</color>";
        string str2 = $"{PlayerPrefs.GetString("CurVersion", "1.0.0")}/{IOCanvasModel.Instance.sBoxVersion}/{IOCanvasModel.Instance.CfgData.difficulty}/{tempStr}";
        diffcultyShow.SetContent(str2, new List<string> { });
    }

    private void RefreshDateShow()
    {
        if (dateShow == null) return;
        DateTime dt = IOCanvasModel.Instance.IODateTime;
        var str = GetDateString(dt);
        dateShow.SetContent(str, new List<string> { });
    }

    private Transform InstantiateBillParent()
    {
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<VerticalLayoutGroup>();
        parent.transform.parent = menuPanel;
        parent.transform.localScale = Vector3.one;
        return parent.transform;
    }

    private void SetBillPanelGridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        gridLayout.spacing = new Vector2(160, 37);
        gridLayout.cellSize = new Vector2(350, 600);
        gridLayoutRect.sizeDelta = new Vector2(1885, 280);
        gridLayoutRect.anchoredPosition = new Vector2(0, 608);
    }

    private void InitClientBillPanelNo0()
    {
        ClearMenuBtn();
        SetBillPanelNo1GridLayout();

        IOCanvasModel.Instance.curBillPage = 0;
        title.text = Utils.GetLanguage("BillShow");
        subtitle.text = $"({Utils.GetLanguage("ScoreUp")}/{Utils.GetLanguage("ScoreDown")})";
        List<string> titleList = new List<string>
        {
            Utils.GetLanguage("MacId"),
            Utils.GetLanguage("ScoreUp"),
            Utils.GetLanguage("ScoreDown"),
            Utils.GetLanguage("Profit"),
            Utils.GetLanguage("AllScore"),
        };
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        for (int i = 0; i < IOCanvasModel.Instance.billData.playerAccountList.Count; i++)
        {
            var playerAccountData = IOCanvasModel.Instance.billData.playerAccountList[i];
            List<string> contentList = new List<string>
            {
                playerAccountData.PlayerId.ToString(),
                playerAccountData.ScoreIn.ToString(),
                playerAccountData.ScoreOut.ToString(),
                (playerAccountData.ScoreIn - playerAccountData.ScoreOut).ToString(),
                playerAccountData.Credit.ToString(),
            };

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);
        }
        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.scoreUp.ToString(),
            IOCanvasModel.Instance.billData.scoreDown.ToString(),
            (IOCanvasModel.Instance.billData.scoreUp - IOCanvasModel.Instance.billData.scoreDown).ToString(),
            IOCanvasModel.Instance.billData.credit.ToString(),
        };
        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo0, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1);

        InstantiateBillDetal();

        curSelect = 0;
        SetCurSelect(setColor: true);
        State = IOState.Clients;
    }

    private void InitClientBillPanelNo0(bool isShow)
    {
        if (!isShow) return;

        ClearMenuBtn();
        SetBillPanelNo1GridLayout();

        IOCanvasModel.Instance.curBillPage = 0;
        title.text = Utils.GetLanguage("BillShow");
        subtitle.text = $"({Utils.GetLanguage("ScoreUp")}/{Utils.GetLanguage("ScoreDown")})";
        List<string> titleList = new List<string>
        {
            Utils.GetLanguage("MacId"),
            Utils.GetLanguage("ScoreUp"),
            Utils.GetLanguage("ScoreDown"),
            Utils.GetLanguage("Profit"),
            Utils.GetLanguage("AllScore"),
        };
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        for (int i = 0; i < IOCanvasModel.Instance.billData.playerAccountList.Count; i++)
        {
            var playerAccountData = IOCanvasModel.Instance.billData.playerAccountList[i];
            List<string> contentList = new List<string>
            {
                playerAccountData.PlayerId.ToString(),
                playerAccountData.ScoreIn.ToString(),
                playerAccountData.ScoreOut.ToString(),
                (playerAccountData.ScoreIn - playerAccountData.ScoreOut).ToString(),
                playerAccountData.Credit.ToString(),
            };

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);
        }
        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.scoreUp.ToString(),
            IOCanvasModel.Instance.billData.scoreDown.ToString(),
            (IOCanvasModel.Instance.billData.scoreUp - IOCanvasModel.Instance.billData.scoreDown).ToString(),
            IOCanvasModel.Instance.billData.credit.ToString(),
        };
        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo0, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent, style: 2, scale: 1);

        InstantiateBillDetal();

        curSelect = 0;
        SetCurSelect(setColor: true);
        State = IOState.Clients;
    }

    private void InitClientBillPanelNo1(bool isShow)
    {
        if (!isShow) return;

        ClearMenuBtn();
        SetBillPanelNo1GridLayout();

        IOCanvasModel.Instance.curBillPage = 0;
        title.text = Utils.GetLanguage("BillShow");
        subtitle.text = $"({Utils.GetLanguage("CoinIn")}/{Utils.GetLanguage("TicketOut")})";
        List<string> titleList = new List<string>
        {
            Utils.GetLanguage("MacId"),
            Utils.GetLanguage("CoinIn"),
            Utils.GetLanguage("TicketOut"),
            Utils.GetLanguage("Profit"),
            Utils.GetLanguage("AllScore"),
        };
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        for (int i = 0; i < IOCanvasModel.Instance.billData.playerAccountList.Count; i++)
        {
            var playerAccountData = IOCanvasModel.Instance.billData.playerAccountList[i];
            List<string> contentList = new List<string>
            {
                playerAccountData.PlayerId.ToString(),
                playerAccountData.CoinIn.ToString(),
                playerAccountData.CoinOut.ToString(),
                (playerAccountData.CoinIn - playerAccountData.CoinOut).ToString(),
                playerAccountData.Credit.ToString(),
            };

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);
        }
        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.coinIn.ToString(),
            IOCanvasModel.Instance.billData.ticketOut.ToString(),
            (IOCanvasModel.Instance.billData.coinIn - IOCanvasModel.Instance.billData.ticketOut).ToString(),
            IOCanvasModel.Instance.billData.credit.ToString(),
        };
        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo0, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent, style: 2, scale: 1);

        InstantiateBillDetal();

        curSelect = 1;
        SetCurSelect(curSelect, setColor: true);
        State = IOState.Clients;
    }

    private void SetBillPanelNo1GridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        gridLayout.cellSize = new Vector2(1450, 13);
        gridLayout.spacing = new Vector2(40, 47);
        gridLayoutRect.sizeDelta = new Vector2(1600, 780f);
        gridLayoutRect.anchoredPosition = new Vector2(0, 576);
    }

    private void InitClientBillPanelNo2(bool isShow)
    {
        if (!isShow) return;
        ClearMenuBtn();
        SetBillPanelNo1GridLayout();

        IOCanvasModel.Instance.curBillPage = 1;
        title.text = Utils.GetLanguage("BillShow");
        subtitle.text = $"({Utils.GetLanguage("ProfitPage")})";
        List<string> titleList = new List<string>
        {
            Utils.GetLanguage("MacId"),
            Utils.GetLanguage("AllBet"),
            Utils.GetLanguage("AllWin"),
            Utils.GetLanguage("AllProfit"),
            Utils.GetLanguage("ProfitMargin"),
        };
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);


        for (int i = 0; i < IOCanvasModel.Instance.billData.playerAccountList.Count; i++)
        {
            var playerAccountData = IOCanvasModel.Instance.billData.playerAccountList[i];
            List<string> contentList = new List<string>
            {
                playerAccountData.PlayerId.ToString(),
                playerAccountData.Bets.ToString(),
                playerAccountData.Wins.ToString(),
                (playerAccountData.Bets - playerAccountData.Wins).ToString(),
            };
            if (playerAccountData.Bets != 0)
                contentList.Add(string.Format("{0:F}", (float)(playerAccountData.Bets - playerAccountData.Wins) / (float)playerAccountData.Bets));
            else
                contentList.Add("0");

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);
        }

        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.bets.ToString(),
            IOCanvasModel.Instance.billData.wins.ToString(),
            (IOCanvasModel.Instance.billData.bets - IOCanvasModel.Instance.billData.wins).ToString(),
        };
        if (IOCanvasModel.Instance.billData.bets != 0)
            totalList.Add(string.Format("{0:F}", (float)(IOCanvasModel.Instance.billData.bets - IOCanvasModel.Instance.billData.wins) / (float)IOCanvasModel.Instance.billData.bets));
        else
            totalList.Add("0");

        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1, scale: 1, fontSize: 50);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo0, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent, style: 2, scale: 1);

        InstantiateBillDetal();

        curSelect = 2;
        SetCurSelect(curSelect, true);
        State = IOState.Clients;
    }

    private Transform InstantiateBillBtnParent()
    {
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        var horizontalLayout = parent.AddComponent<HorizontalLayoutGroup>();
        horizontalLayout.spacing = 258;
        parent.AddComponent<ToggleGroup>();
        parent.transform.parent = transform;
        var rect = parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(960, 100);
        rect.anchoredPosition = new Vector2(-80, -430);
        rect.localScale = Vector3.one;
        return parent.transform;
    }


    int curSelectMachineId = -1;
    private void InitCodePanel()
    {
        ClearMenuBtn();
        SetCodePanelGridLayout();

        title.text = Utils.GetLanguage(IOFunction.Code.ToString());
        subtitle.text = "";
        State = IOState.Code;

        SBoxIdea.RequestCoder(0);
    }

    private void OnInitCoderView()
    {
        ClearMenuBtn();
        if (passwordPanel != null)
        {
            Destroy(passwordPanel.gameObject);
            passwordPanel = null;
        }

        InstantiateBaseShow($"A.{Utils.GetLanguage("AllBet")}", new List<string> { IOCanvasModel.Instance.coderData.Bets.ToString() }, style: 4, scale: 1, fontSize: 40);
        InstantiateBaseShow($"B.{Utils.GetLanguage("AllWin")}", new List<string> { IOCanvasModel.Instance.coderData.Wins.ToString() }, style: 4, scale: 1, fontSize: 40);
        InstantiateBaseShow($"C.{Utils.GetLanguage("MachineId")}", new List<string> { IOCanvasModel.Instance.coderData.MachineId.ToString() }, style: 4, scale: 1, fontSize: 40);
        InstantiateBaseShow($"D.{Utils.GetLanguage("Frequency")}", new List<string> { IOCanvasModel.Instance.coderData.CoderCount.ToString() }, style: 4, scale: 1, fontSize: 40);
        InstantiateBaseShow($"E.{Utils.GetLanguage("Code1")}", new List<string> { IOCanvasModel.Instance.coderData.CheckValue.ToString() }, style: 4, scale: 1, fontSize: 40);
        InstantiateBaseShow(Utils.GetLanguage("TimeLeft"), new List<string> { IOCanvasModel.Instance.GetDate(IOCanvasModel.Instance.coderData.RemainMinute) }, style: 4, scale: 1, fontSize: 40);

        passwordPanel = Instantiate(basePassword, transform).GetComponent<IOPasswordPanel>();
        passwordPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(468, 555);
        passwordPanel.SetParams(InputField.ContentType.Standard, "Please enter code", localScal: 0.7f);
        selectionList.AddRange(passwordPanel.GetSelectionList());

        InstantiateBlank();
        InstantiateBlank();
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = menuPanel;
        parent.transform.localScale = Vector3.one;
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1f);
        //InstantiateBaseBtn("Return", ReturnToSelectVisibleCoinPushMachinePanel(InitCodePanel), parent: parent.transform, style: 2, scale: 1f);


        //#seaweed# var baseShow2 = InstantiateEasyShow(Utils.GetLanguage("ClickCancleTips"), "", parent: transform, fontSize: 40);
        //#seaweed# baseShow2.GetComponent<RectTransform>().anchoredPosition = IOCanvasModel.Instance.curlanguage == Language.en ? new Vector2(-50, -340) : new Vector2(205, -340);

        State = IOState.Code;
        curSelect = 0;
        SetCurSelect();
        InstantiateBillDetal();
        IOPopTips.Instance.transform.SetAsLastSibling();
    }

    private void InitJackpotSettingPanel()
    {
        ClearMenuBtn();
        IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
        title.text = Utils.GetLanguage(IOFunction.JackpotSetting.ToString());

        InstantiateBaseSection(IOJackpotSetting.JackpotSwitch.ToString(), Utils.GetEnumNames(typeof(IOJackpotSwitch)), IOCanvasModel.Instance.tempJackpotCfg.jackpotSwitch, onClick: () => { OnSectionClick((int)IOJackpotSetting.JackpotSwitch, IOSectionState.JackpotSwitch); }, fontSize: 55);
        InstantiateBaseSection(IOJackpotSetting.JpPercent.ToString(), IOCanvasModel.Instance.tempJackpotCfg.jpPercent, onClick: () => { OnSectionClick((int)IOJackpotSetting.JpPercent, IOSectionState.JackpotPercent); }, fontSize: 55);
        InstantiateEasyShow("ScoreRate", (1f / IOCanvasModel.Instance.CfgData.CoinValue).ToString(), fontSize: 55);

        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = transform;
        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.anchoredPosition = new Vector2(0, -430);
        parentRect.sizeDelta = new Vector2(975, 100);
        parentRect.localScale = Vector3.one;
        InstantiateBaseBtn("Save", SaveJackpotCfg, parent: parent.transform, style: 2, scale: 1);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1);

        State = IOState.JackpotSetting;

        curSelect = 0;
        SetCurSelect();
        IOPopTips.Instance.transform.SetAsLastSibling();
    }

    //private void InitJackpotBetPanel()
    //{
    //    title.text = Utils.GetLanguage(IOFunction.JackpotBet.ToString());
    //    ClearMenuBtn();
    //    SetJackpotBetPanelGridLayout();
    //    State = IOState.JackpotBet;
    //    if (!isGetJackpotData)
    //    {
    //        IOPopTips.Instance.ShowTips(Utils.GetLanguage("Loading"), -1);
    //        MsgInfo msgInfo = new MsgInfo
    //        {
    //            cmd = (int)C2S_CMD.C2S_GetJackpotData,
    //            id = IOCanvasModel.Instance.CfgData.MachineId,
    //        };
    //        NetMgr.Instance.SendToServer(JsonConvert.SerializeObject(msgInfo));
    //        loadingCoroutine = StartCoroutine(LoadingCoroutine());
    //    }
    //    else
    //        InitJackpotBetPanelView();
    //}

    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(5);
        IOPopTips.Instance.ShowTips(Utils.GetLanguage("LoadFault"), 3);
        yield return new WaitForSeconds(3);
        loadingCoroutine = null;
        ReturnToFunction();
    }

    //private void InitJackpotBetPanelView()
    //{
    //    betDataCoinValueShow = false;
    //    GameObject parent = new GameObject();
    //    tempObjList.Add(parent);
    //    parent.AddComponent<HorizontalLayoutGroup>();
    //    parent.transform.parent = transform;
    //    var parentRect = parent.GetComponent<RectTransform>();
    //    parentRect.anchoredPosition = new Vector2(0, 370);
    //    parentRect.sizeDelta = new Vector2(1455, 100);
    //    parentRect.localScale = Vector3.one;
    //    InstantiateBaseShow("ScoreRate", new List<string> { (int.Parse(((1f / IOCanvasModel.Instance.CfgData.CoinValue) * 1000).ToString()) / 1000f).ToString() }, parent: parent.transform, style: 5, scale: 1, fontSize: 50);
    //    InstantiateBaseShow("JpPercent", new List<string> { IOCanvasModel.Instance.JackpotCfg.jpPercent + "��" }, parent: parent.transform, style: 5, scale: 1, fontSize: 50);

    //    List<string> titleList = new List<string>
    //    {
    //        Utils.GetLanguage("AllBet"),
    //        Utils.GetLanguage("TurnInScore"),
    //        Utils.GetLanguage("AllWin"),
    //    };
    //    InstantiateBaseShow(Utils.GetLanguage("MacId"), titleList, style: 5, scale: 1, fontSize: 50);
    //    for (int i = 0; i < IOCanvasModel.Instance.betDataList.Count; i++)
    //    {
    //        var betData = IOCanvasModel.Instance.betDataList[i];

    //        var baseShow = InstantiateBaseShow(betData.seatId.ToString(), new List<string>
    //        {
    //            (betData.bet / 100).ToString(),
    //            betData.bet * betData.jpPercent / 100000f + "",
    //            (betData.win / 100 * IOCanvasModel.Instance.CfgData.CoinValue).ToString()
    //        }, style: 5, scale: 1, fontSize: 50);
    //        betDataShowList.Add(baseShow);
    //    }

    //    GameObject btnParent = new GameObject();
    //    tempObjList.Add(btnParent);
    //    btnParent.AddComponent<HorizontalLayoutGroup>();
    //    btnParent.transform.parent = transform;
    //    var btnParentRect = btnParent.GetComponent<RectTransform>();
    //    btnParentRect.anchoredPosition = new Vector2(0, -430);
    //    btnParentRect.sizeDelta = new Vector2(975, 100);
    //    scoreShowBtn = InstantiateBaseBtn("CoinValueShow", ChangeBetDataShow, parent: btnParent.transform, style: 2);
    //    InstantiateBaseBtn("Return", ReturnToFunction, parent: btnParent.transform, style: 2);
    //}

    private void InitJackpotWinPanel()
    {
        title.text = Utils.GetLanguage(IOFunction.JackpotBet.ToString());
        ClearMenuBtn();
        SetJackpotWinPanelGridLayout();
        State = IOState.JaclpotWins;
        if (!isGetJackpotData)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("Loading"), -1);
            MsgInfo msgInfo = new MsgInfo
            {
                cmd = (int)C2S_CMD.C2S_GetJackpotData,
                id = IOCanvasModel.Instance.CfgData.MachineId,
            };
            NetMgr.Instance.SendToServer(JsonConvert.SerializeObject(msgInfo));
            loadingCoroutine = StartCoroutine(LoadingCoroutine());
        }
        else
            InitJackpotWinPanelView();
    }

    private void SetJackpotWinPanelGridLayout()
    {
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        gridLayoutRect = menuPanel.GetComponent<RectTransform>();
        gridLayoutRect.anchorMin = new Vector2(0.5f, 0);
        gridLayoutRect.anchorMax = new Vector2(0.5f, 0);
        gridLayoutRect.sizeDelta = new Vector2(1870f, 890f);
        gridLayoutRect.anchoredPosition = new Vector2(0, 470f);
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.spacing = new Vector2(0f, 0);
        gridLayout.cellSize = new Vector2(1600, 83);
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
    }

    private void InitJackpotWinPanelView()
    {
        List<string> titleList = new List<string>
        {
            Utils.GetLanguage("MacId"),
            Utils.GetLanguage("Grand"),
            Utils.GetLanguage("Times"),
            Utils.GetLanguage("Major"),
            Utils.GetLanguage("Times"),
            Utils.GetLanguage("Minor"),
            Utils.GetLanguage("Times"),
            Utils.GetLanguage("Mini"),
            Utils.GetLanguage("Times"),
        };
        InstantiateBaseShow("", titleList, style: 6, fontSize: 36, scale: 1);
        for (int i = 0; i < IOCanvasModel.Instance.betDataList.Count; i++)
        {
            var betData = IOCanvasModel.Instance.betDataList[i];
            List<string> contentList = new List<string>
            {
                betData.seatId.ToString(),
                betData.grandWin / 100f + "",
                betData.grandTimes.ToString(),
                betData.majorWin / 100f + "",
                betData.majorTimes.ToString(),
                betData.minorWin / 100f + "",
                betData.minorTimes.ToString(),
                betData.miniWin / 100f + "",
                betData.miniTimes.ToString(),
            };
            InstantiateBaseShow("", contentList, style: 6, fontSize: 36, scale: 1);
            GameObject btnParent = new GameObject();
            tempObjList.Add(btnParent);
            btnParent.AddComponent<HorizontalLayoutGroup>();
            btnParent.transform.parent = transform;
            var btnParentRect = btnParent.GetComponent<RectTransform>();
            btnParentRect.anchoredPosition = new Vector2(0, -430);
            btnParentRect.sizeDelta = new Vector2(975, 100);
            InstantiateBaseBtn("Return", ReturnToFunction, parent: btnParent.transform, style: 2);
        }
    }

    private void ChangeBetDataShow()
    {
        if (!isGetJackpotData) return;
        betDataCoinValueShow = !betDataCoinValueShow;
        scoreShowBtn.titleText.text = betDataCoinValueShow ? Utils.GetLanguage("ScoreShow") : Utils.GetLanguage("CoinValueShow");
        for (int i = 0; i < IOCanvasModel.Instance.betDataList.Count; i++)
        {
            var betData = IOCanvasModel.Instance.betDataList[i];
            betDataShowList[i].SetContent(betData.seatId.ToString(), new List<string>
            {
                (betData.bet / 100).ToString(),
                betDataCoinValueShow ? betData.bet * betData.jpPercent / 100000f / IOCanvasModel.Instance.CfgData.CoinValue + "" : betData.bet * betData.jpPercent / 100000f + "",
                betDataCoinValueShow ? (betData.win / 100).ToString() : (betData.win / 100 * IOCanvasModel.Instance.CfgData.CoinValue).ToString()
            });
        }
    }

    private void SetJackpotBetPanelGridLayout()
    {
        gridLayout.padding = new RectOffset(200, 0, 0, 0);
        gridLayoutRect = menuPanel.GetComponent<RectTransform>();
        gridLayoutRect.anchorMin = new Vector2(0.5f, 0);
        gridLayoutRect.anchorMax = new Vector2(0.5f, 0);
        gridLayoutRect.sizeDelta = new Vector2(1870f, 890f);
        gridLayoutRect.anchoredPosition = new Vector2(0, 400f);
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.spacing = new Vector2(0f, 0);
        gridLayout.cellSize = new Vector2(950, 83);
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
    }

    //private void OnGetNetJackpotData(Dictionary<int, List<BetData>> betDataDic)
    //{
    //    isGetJackpotData = true;
    //    if (loadingCoroutine != null)
    //    {
    //        StopCoroutine(loadingCoroutine);
    //        loadingCoroutine = null;
    //    }
    //    IOCanvasModel.Instance.betDataDic = betDataDic;
    //    IOPopTips.Instance.HideTips();
    //    IOCanvasModel.Instance.DealWithBetDataDic();
    //    if (State == IOState.JackpotBet)
    //        InitJackpotBetPanelView();
    //    else if (State == IOState.JaclpotWins)
    //        InitJackpotWinPanelView();

    //}

    private void OnGetSandboxDate()
    {
        switch (State)
        {
            case IOState.DateTime:
                PopModifiedDatePanel();
                break;
            case IOState.Bill:
            case IOState.Code:
                RefreshDateShow();
                break;
            default:
                break;
        }
    }

    private void SetCodePanelGridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.padding = new RectOffset(0, 0, 15, 0);
        gridLayout.spacing = new Vector2(40, 20);
        gridLayout.cellSize = new Vector2(780, 70);
        gridLayoutRect.sizeDelta = new Vector2(1000, 820);
        gridLayoutRect.anchoredPosition = new Vector2(0, 460);


    }

    private void ChangeCurLanguage()
    {
        var temp = (int)IOCanvasModel.Instance.curlanguage;
        temp = temp + 1 > Enum.GetValues(typeof(Language)).Cast<int>().Max() ? 0 : temp + 1;
        IOCanvasModel.Instance.CurLanguage = (Language)(Enum.Parse(typeof(Language), temp.ToString()));
        selectionList[3].title = IOCanvasModel.Instance.curlanguage.ToString();
        RefreshFunctionText();
        PlayerPrefs.SetInt("CurLanguage", (int)IOCanvasModel.Instance.curlanguage);
        PlayerPrefs.GetInt("CurLanguage");
        EventCenter.Instance.EventTrigger(EventHandle.CHANGE_LANGUAGE);
        MsgInfo msgInfo = new MsgInfo
        {
            cmd = (int)S2C_CMD.S2C_ChangeLanguage,
            id = -1,
            jsonData = ((int)IOCanvasModel.Instance.curlanguage).ToString()
        };
        NetMgr.Instance.SendToAllClient(JsonConvert.SerializeObject(msgInfo));
    }

    private void InitEditPasswordPanel()
    {
        ClearMenuBtn();
        if (passwordPanel != null)
        {
            Destroy(passwordPanel.gameObject);
            passwordPanel = null;
        }
        SetEditPasswordGridLayout();
        title.text = Utils.GetLanguage(IOFunction.EditPassword.ToString());

        var baseShow1 = InstantiateEasyShow(Utils.GetLanguage("EditPasswordShow"), "", parent: transform, scale: 1);
        baseShow1.GetComponent<RectTransform>().anchoredPosition = new Vector3(55, 375);
        for (int i = 0; i < 9; i++)
            InstantiateBlank();
        passwordPanel = Instantiate(basePassword, transform).GetComponent<IOPasswordPanel>();
        passwordPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 515);
        passwordPanel.SetParams(InputField.ContentType.Password, "Please enter new password", localScal: 0.7f);
        selectionList.AddRange(passwordPanel.GetSelectionList());
        InstantiateBaseBtn("Return", ReturnToFunction, style: 2, scale: 1);
        //#seaweed# var baseShow2 = InstantiateEasyShow(Utils.GetLanguage("ClickCancleTips"), "", parent: transform, fontSize: 40);
        //#seaweed# baseShow2.GetComponent<RectTransform>().anchoredPosition = IOCanvasModel.Instance.curlanguage == Language.en ? new Vector2(-50, -340) : new Vector2(205, -340);
        State = IOState.EditPassword;
        curSelect = 0;
        SetCurSelect();
        selectSection.state = IOSectionState.EditPassword;
        IOPopTips.Instance.transform.SetAsLastSibling();
        //OnSectionClick(curSelect, IOSectionState.EditPassword);
        //passwordPanel.Focuse(true);
    }



    /// <summary>
    /// 修改线号机台号
    /// </summary>
    private void InitProSettingPanel()
    {
        ClearMenuBtn();
        if (passwordPanel != null)
        {
            Destroy(passwordPanel.gameObject);
            passwordPanel = null;
        }
        SetProSettingGridLayout();
        title.text = Utils.GetLanguage(IOFunction.ProSetting.ToString());
        InstantiateEasySection(Utils.GetLanguage("LineId"), IOCanvasModel.Instance.tempCfgData.LineId.ToString(),
                onClick: () => OnEditIdSectionClick(0, IOSectionState.EditLineId));
        InstantiateEasySection(Utils.GetLanguage("MachineId"), IOCanvasModel.Instance.tempCfgData.MachineId.ToString(),
            onClick: () => OnEditIdSectionClick(1, IOSectionState.EditMacId));

        InstantiateKeyBoard();

        GameObject parent = new GameObject();

        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = transform;
        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.anchoredPosition = new Vector2(0, -430);
        parentRect.sizeDelta = new Vector2(975, 100);
        InstantiateBaseBtn("Save", SaveConfig, parent: parent.transform, style: 2, scale: 1);
        InstantiateBaseBtn("Return", () =>
        {
            if (selectSection.selected)
            {
                IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
                int data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
                selectSection.baseSection.Content = data.ToString();
                selectSection.selected = false;
                if (IOCanvasModel.Instance.permissions == 3)
                    SetCurSelect(selectSection.state == IOSectionState.EditLineId ? 0 : 1);
                else
                    SetCurSelect();
            }
            else
            {
                ReturnToFunction();
            }

        }, parent: parent.transform, style: 2, scale: 1);
        parent.transform.localScale = Vector3.one;

        //InstantiateBaseBtn("Save", () => {
        //    SaveConfig();
        //}, style: 2);
        //InstantiateBaseBtn("Return", () => {
        //    if (selectSection.selected)
        //    {
        //        IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
        //        int data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
        //        selectSection.baseSection.Content = data.ToString();
        //        selectSection.selected = false;
        //        if (IOCanvasModel.Instance.permissions == 3)
        //            SetCurSelect(selectSection.state == IOSectionState.EditLineId ? 0 : 1);
        //        else
        //            SetCurSelect();
        //    }
        //    else
        //        ReturnToFunction();
        //}, style: 2);
        State = IOState.ProSetting;
        curSelect = 0;
        SetCurSelect();
        IOPopTips.Instance.transform.SetAsLastSibling();
    }

    private void SetProSettingGridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        if (IOCanvasModel.Instance.permissions == 3)
            gridLayout.spacing = new Vector2(40, 19);
        else
            gridLayout.spacing = new Vector2(40, 29.5f);
        gridLayout.cellSize = new Vector2(600, 76.5f);
        gridLayoutRect.sizeDelta = new Vector2(845, 954);
        //#seaweed# gridLayoutRect.anchoredPosition = new Vector2(0, 502);

        gridLayoutRect.anchorMin = new Vector2(0.5f, 1);
        gridLayoutRect.anchorMax = new Vector2(0.5f, 1); //-600   520
        gridLayoutRect.anchoredPosition = new Vector2(0, -600);  //#seaweed#

    }

    private void InitModeifedDatePanel()
    {
        State = IOState.DateTime;
        SBoxSandbox.GetDateTime();
    }

    private void ExitIOCanvas()
    {
        curSelect = 0;
        gameObject.SetActive(false);
        ClearMenuBtn();
        EventCenter.Instance.EventTrigger(EventHandle.LEAVE_SETTING);
    }

    public void PopModifiedDatePanel()
    {
        modDatePanel = Instantiate(modDate, transform);
        var rect = modDatePanel.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;
        var parent = modDatePanel.transform;
        var dateSection = InistantiateDateTimeSection("", onClick: DateTimeSectionClick, parent: parent);
        var dateTimeRect = dateSection.GetComponent<RectTransform>();
        dateTimeRect.anchoredPosition = new Vector2(-18f, 27f);
        GameObject parent1 = new GameObject();
        parent1.AddComponent<HorizontalLayoutGroup>();
        parent1.transform.parent = parent;
        parent1.transform.localScale = Vector3.one;
        var parentRect1 = parent1.GetComponent<RectTransform>();
        parentRect1.anchoredPosition = new Vector2(0, -120);
        parentRect1.sizeDelta = new Vector2(705, 85);
        InstantiateBaseBtn("Save", SaveDateTime, parent: parent1.transform, style: 2, scale: 1);
        InstantiateBaseBtn("Return", CloseModifiedDatePanel, parent: parent1.transform, style: 2, scale: 1);

        State = IOState.DateTime;
        curSelect = selectionList.Count - 3;
        SetCurSelect(curSelect);

        dateSection.Selected = true;
        selectSection.selected = dateSection.Selected;
        selectSection.baseSection = dateSection;
        selectSection.state = IOSectionState.DateTime;

        selectSection.baseSection.CurIndex = 0;


        //DefaultDateTimeSectionClick();
    }

    private void SaveDateTime()
    {
        if (!Application.isEditor)
            SBoxSandbox.SetDateTime(IOCanvasModel.Instance.IODateTime);
    }

    public void CloseModifiedDatePanel()
    {
        Destroy(modDatePanel);
        IOArrowRect.gameObject.SetActive(false);
        State = IOState.SelectFunction;
        int removeIdx = selectionList.Count - 3;
        for (int i = 0; i < 3; i++)
            selectionList.RemoveAt(removeIdx);
        selectSection.selected = false;
        SetCurSelect();
    }

    private void SetEditPasswordGridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.padding = new RectOffset(0, 0, 20, 0);
        gridLayout.spacing = new Vector2(40, 16);
        gridLayout.cellSize = new Vector2(600, 75);
        gridLayoutRect.sizeDelta = new Vector2(845, 960);
        gridLayoutRect.anchoredPosition = new Vector2(0, 505);
    }

    /// <summary>
    /// 返回后台菜单页面
    /// </summary>
    private void ReturnToFunction()
    {
        if (passwordPanel != null)
        {
            Destroy(passwordPanel.gameObject);
            passwordPanel = null;
        }
        scoreShowBtn = null;
        betDataShowList.Clear();
        for (int i = 0; i < titleLeftList.Count; i++)
            titleLeftList[i].gameObject.SetActive(false);
        for (int i = 0; i < titleRightList.Count; i++)
            titleRightList[i].gameObject.SetActive(false);
        IOArrowRect.gameObject.SetActive(false);
        ClearMenuBtn();
        InitFunctionPanel();
        State = IOState.SelectFunction;
    }

    private void OnSectionClick(int index, IOSectionState sectionState)
    {
        curSelect = index;
        var section = selectionList[curSelect] as IOBaseSection;

        for (int i = 0; i < selectionList.Count; i++)
            selectionList[i].Color = IOCanvasModel.Instance.textNormalColor;

        section.Selected = true;
        selectSection.selected = section.Selected;
        selectSection.baseSection = section;
        selectSection.state = sectionState;
        if (!selectSection.selected)
            SetCurSelect(curSelect, false);

        if (State == IOState.Params || State == IOState.DateTime || State == IOState.JackpotSetting)
        {
            IOArrowRect.gameObject.SetActive(true);
            IOArrowRect.anchoredPosition = section.transform.localPosition;

            switch (State)
            {
                case IOState.DateTime:
                    IOArrowRect.anchoredPosition += new Vector2(410, 0);
                    break;
                case IOState.Params:
                    if (curSelect > 10)
                        IOArrowRect.anchoredPosition += new Vector2(-160, 20);
                    else if (curSelect > 4)
                        IOArrowRect.anchoredPosition += new Vector2(50, 20);
                    else
                        IOArrowRect.anchoredPosition += new Vector2(0, 20);
                    break;
                case IOState.JackpotSetting:
                    IOArrowRect.anchoredPosition += new Vector2(180, -70);
                    break;
                default:
                    break;
            }

            IOArrowRect.SetAsLastSibling();
        }
    }

    private void OnEditIdSectionClick(int index, IOSectionState sectionState)
    {
        IOBaseSection section = selectionList[index] as IOBaseSection;
        section.Selected = true;
        selectSection.selected = true;
        selectSection.baseSection = section;
        selectSection.state = sectionState;
        int setSelectIdx = IOCanvasModel.Instance.permissions == 3 ? 2 : 1;
        SetCurSelect(setSelectIdx, false);
    }

    private void EditIdNumBtnClick(int num)
    {
        if (!selectSection.selected) return;
        int data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
        string str = data != 0 ? data.ToString() + num.ToString() : num.ToString();
        if (str.Length > 9) return;
        if (IOCanvasModel.Instance.permissions == 3)
        {
            if (num == 0)
                SetCurSelect(12);
            else
                SetCurSelect(num + 1);
        }
        else
            if (num == 0)
            SetCurSelect(11);
        else
            SetCurSelect(num);
        selectSection.baseSection.Content = str;
        if (selectSection.state == IOSectionState.EditLineId)
            IOCanvasModel.Instance.tempCfgData.LineId = int.Parse(str);
        else
            IOCanvasModel.Instance.tempCfgData.MachineId = int.Parse(str);
    }

    private void EditIdDeleteBtnClick()
    {
        if (!selectSection.selected) return;
        if (IOCanvasModel.Instance.permissions == 3)
            SetCurSelect(11);
        else
            SetCurSelect(10);
        int data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
        string str = data.ToString().Substring(0, data.ToString().Length - 1);
        selectSection.baseSection.Content = str;
        if (selectSection.state == IOSectionState.EditLineId)
            IOCanvasModel.Instance.tempCfgData.LineId = string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
        else
            IOCanvasModel.Instance.tempCfgData.MachineId = string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
    }

    private void InstantiateBlank()
    {
        InstantiateEasyShow("", "");
    }

    private IOBaseShow InstantiateEasyShow(string title, string content, bool showBg = false, TextAnchor contentAlignment = TextAnchor.MiddleLeft, int fontSize = 30, Transform parent = null, float scale = 1)
    {
        return InstantiateBaseShow(title, new List<string>() { content }, showBg, contentAlignment: contentAlignment, fontSize: fontSize, parent: parent, scale: scale);
    }

    private IOBaseShow InstantiateBaseShow(string title, List<string> contentList, bool showBg = false, Transform parent = null, TextAnchor contentAlignment = TextAnchor.MiddleLeft, int fontSize = 30, int style = 0, float scale = 1f)
    {
        IOBaseShow baseShow;
        switch (style)
        {
            case 0:
                baseShow = Instantiate(baseShowPrefab).GetComponent<IOBaseShow>();
                break;
            case 1:
                baseShow = Instantiate(baseShowPrefab1).GetComponent<IOBaseShow>();
                break;
            case 2:
                baseShow = Instantiate(baseShowPrefab2).GetComponent<IOBaseShow>();
                break;
            case 3:
                baseShow = Instantiate(baseShowPrefab3).GetComponent<IOBaseShow>();
                break;
            case 4:
                baseShow = Instantiate(baseShowPrefab4).GetComponent<IOBaseShow>();
                break;
            case 5:
                baseShow = Instantiate(baseShowPrefab5).GetComponent<IOBaseShow>();
                break;
            case 6:
                baseShow = Instantiate(baseShowPrefab6).GetComponent<IOBaseShow>();
                break;
            default:
                baseShow = Instantiate(baseShowPrefab).GetComponent<IOBaseShow>();
                break;
        }
        if (parent)
            baseShow.SetParentAndReset(parent);
        else
            baseShow.SetParentAndReset(menuPanel);
        baseShow.ShowBg = showBg;
        baseShow.title = title;
        if (baseShow.titleText != null)
        {
            baseShow.titleText.fontSize = fontSize;
            baseShow.titleText.text = Utils.GetLanguage(title);
        }
        for (int i = 0; i < contentList.Count; i++)
        {
            baseShow.contentList[i].text = contentList[i];
            baseShow.contentList[i].alignment = contentAlignment;
            baseShow.contentList[i].gameObject.SetActive(true);
            baseShow.contentList[i].fontSize = fontSize;
        }
        onlyShowList.Add(baseShow);
        baseShow.transform.localScale = Vector3.one * scale;
        return baseShow;
    }

    private IOBaseBtn InstantiateBaseBtn(string str, UnityAction unityAction, bool showBg = true, TextAnchor textAnchor = TextAnchor.MiddleCenter, Transform parent = null, int style = 1, int fontSize = 20, float scale = 1)
    {
        IOBaseBtn baseBtn;
        switch (style)
        {
            case 0:
                baseBtn = Instantiate(baseBtnPrefab).GetComponent<IOBaseBtn>();
                break;
            case 1:
                baseBtn = Instantiate(baseBtnPrefab1).GetComponent<IOBaseBtn>();
                break;
            case 2:
                baseBtn = Instantiate(baseBtnPrefab2).GetComponent<IOBaseBtn>();
                break;
            default:
                baseBtn = Instantiate(baseBtnPrefab).GetComponent<IOBaseBtn>();
                break;
        }
        if (parent)
            baseBtn.SetParentAndReset(parent);
        else
            baseBtn.SetParentAndReset(menuPanel);
        baseBtn.ShowBg = showBg;
        baseBtn.style = style;
        baseBtn.title = str;
        baseBtn.titleText.fontSize = fontSize == 20 ? (style == 2 ? 52 : fontSize) : fontSize;
        baseBtn.titleText.text = Utils.GetLanguage(str);
        baseBtn.titleText.alignment = textAnchor;
        baseBtn.transform.localScale = scale * Vector3.one;
        baseBtn.AddListener(unityAction);
        selectionList.Add(baseBtn);
        return baseBtn;
    }

    private IOBaseToggle InstantiateBaseToggle(string str, UnityAction<bool> unityAction, bool showBg = true, TextAnchor textAnchor = TextAnchor.MiddleCenter, Transform parent = null, ToggleGroup toggleGroup = null, float scale = 1)
    {
        IOBaseToggle baseToggle = Instantiate(baseTogglePrefab).GetComponent<IOBaseToggle>();
        if (parent)
            baseToggle.SetParentAndReset(parent);
        else
            baseToggle.SetParentAndReset(menuPanel);
        if (toggleGroup != null)
            baseToggle.toggle.group = toggleGroup;
        baseToggle.ShowBg = showBg;
        baseToggle.style = 1;
        baseToggle.title = str;
        baseToggle.titleText.fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 30 : 45;
        baseToggle.titleText.text = str;
        baseToggle.titleText.alignment = textAnchor;
        baseToggle.AddListener(unityAction);
        baseToggle.transform.localScale = Vector3.one * scale;
        selectionList.Add(baseToggle);
        return baseToggle;
    }

    private void InstantiateEasySection(string title, string content, UnityAction onClick = null, bool showBg = false, int style = 0, Transform parent = null)
    {
        string[] strings = new string[] { content };
        InstantiateBaseSection(title, strings, onClick: onClick, showBg: showBg, style: style, parent: parent);
    }

    private void InstantiateBaseSection(string title, string[] contents, int index = 0, UnityAction onClick = null, bool showBg = false, int style = 0, Transform parent = null, int fontSize = -1)
    {
        IOBaseSection baseSelection = style == 0
            ? Instantiate(baseSectionPrefab).GetComponent<IOBaseSection>()
            : Instantiate(baseSectionPrefab1).GetComponent<IOBaseSection>();
        if (parent)
            baseSelection.SetParentAndReset(parent);
        else
            baseSelection.SetParentAndReset(menuPanel);
        baseSelection.ShowBg = showBg;
        baseSelection.title = title;
        baseSelection.titleText.text = Utils.GetLanguage(title);
        baseSelection.titleText.fontSize = fontSize == -1 ? IOCanvasModel.Instance.defaultFontSize : fontSize;
        baseSelection.contentText.fontSize = fontSize == -1 ? IOCanvasModel.Instance.defaultFontSize : fontSize;
        baseSelection.contents = contents;
        baseSelection.ioParams = IOParams.Return;
        baseSelection.CurIndex = index;
        baseSelection.AddListener(onClick);
        baseSelection.transform.localScale = Vector3.one;
        selectionList.Add(baseSelection);
    }

    private void InstantiateBaseSection(string title, int defaultValue = 0, UnityAction onClick = null, bool showBg = false, int style = 0, Transform trans = null, IOParams ioParams = IOParams.Return, int fontSize = -1)
    {
        IOBaseSection baseSelection = style == 0
            ? Instantiate(baseSectionPrefab).GetComponent<IOBaseSection>()
            : Instantiate(baseSectionPrefab1).GetComponent<IOBaseSection>();
        if (trans)
            baseSelection.SetParentAndReset(trans);
        else
            baseSelection.SetParentAndReset(menuPanel);
        baseSelection.ShowBg = showBg;
        baseSelection.title = title;
        baseSelection.titleText.text = Utils.GetLanguage(title);
        baseSelection.titleText.fontSize = fontSize == -1 ? IOCanvasModel.Instance.defaultFontSize : fontSize;
        baseSelection.contentText.fontSize = fontSize == -1 ? IOCanvasModel.Instance.defaultFontSize : fontSize;
        baseSelection.ioParams = ioParams;
        baseSelection.CurIndex = defaultValue;
        baseSelection.AddListener(onClick);
        baseSelection.transform.localScale = Vector3.one;
        selectionList.Add(baseSelection);
    }

    private void InstantiateTicketRatioSection(string title, UnityAction onClick = null, bool showBg = false)
    {
        IOBaseSection baseSelection = Instantiate(ticketRatioPrefab).GetComponent<IOBaseSection>();
        baseSelection.SetParentAndReset(menuPanel);
        baseSelection.ShowBg = showBg;
        baseSelection.title = title;
        baseSelection.titleText.text = Utils.GetLanguage(title);
        baseSelection.titleText.fontSize = IOCanvasModel.Instance.defaultFontSize;
        baseSelection.contentText.fontSize = IOCanvasModel.Instance.defaultFontSize;
        (baseSelection as IOTicketRatioSection).SetCurIndex();
        baseSelection.AddListener(onClick);
        baseSelection.transform.localScale = Vector3.one;
        selectionList.Add(baseSelection);
    }




    private void InistantiateSwitchSection(string title, List<int> orignalList, UnityAction onClick = null, bool showBg = false)
    {
        IOSwitchSection switchSection = Instantiate(switchSectionsPrefab).GetComponent<IOSwitchSection>();
        switchSection.SetParentAndReset(menuPanel);
        switchSection.ShowBg = showBg;
        switchSection.title = title;
        switchSection.titleText.text = Utils.GetLanguage(title);
        switchSection.titleText.fontSize = IOCanvasModel.Instance.defaultFontSize;
        switchSection.OriganlList = orignalList;
        //switchSection.DataList = list;
        switchSection.AddListener(onClick);
        switchSection.transform.localScale = Vector3.one;
        selectionList.Add(switchSection);
    }

    private void InstantiateKeyBoard()
    {
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        var gridLayoutGroup = parent.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(217.5f, 106.5f);
        gridLayoutGroup.spacing = new Vector2(121f, 45f);
        gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        parent.transform.parent = transform;
        for (int i = 0; i < 12; i++)
        {
            if (i < 9)
            {
                int num = i + 1;
                InstantiateBaseBtn(num.ToString(), () => EditIdNumBtnClick(num), true, TextAnchor.MiddleCenter, fontSize: 72, parent: parent.transform, style: 0, scale: 1f);
            }
            else
            {
                int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 40 : 80;
                switch (i)
                {
                    case 9:
                        InstantiateBaseBtn(Utils.GetLanguage("DELETE"), EditIdDeleteBtnClick, true, TextAnchor.MiddleCenter, fontSize: fontSize, parent: parent.transform, style: 0, scale: 1f);
                        break;
                    case 10:
                        InstantiateBaseBtn("0", () => EditIdNumBtnClick(0), true, TextAnchor.MiddleCenter, fontSize: 72, parent: parent.transform, style: 0, scale: 1f);
                        break;
                    case 11:
                        InstantiateBaseBtn(Utils.GetLanguage("CONFIRM"), () =>
                        {
                            if (!selectSection.selected) return;
                            SetCurSelect(0);
                            selectSection.selected = false;
                            //SaveConfig();
                        }, true, TextAnchor.MiddleCenter, fontSize: fontSize, parent: parent.transform, style: 0, scale: 1f);
                        break;
                }
            }
        }
        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.sizeDelta = new Vector2(1000, 535);
        parentRect.anchoredPosition = new Vector2(0, 24f);
        parentRect.localScale = Vector3.one;
    }

    private IODateTimeSection InistantiateDateTimeSection(string title, UnityAction onClick = null, bool showBg = false, Transform parent = null, float scale = 1f)
    {
        IODateTimeSection dateTimeSection = Instantiate(dateTimeSectionsPrefab).GetComponent<IODateTimeSection>();
        if (parent)
            dateTimeSection.SetParentAndReset(parent);
        else
            dateTimeSection.SetParentAndReset(menuPanel);
        dateTimeSection.ShowBg = showBg;
        dateTimeSection.title = title;
        dateTimeSection.titleText.text = Utils.GetLanguage(title);
        dateTimeSection.AddListener(onClick);
        dateTimeSection.InitDateTimeView();
        dateTimeSection.transform.localScale = Vector3.one * scale;
        selectionList.Add(dateTimeSection);
        return dateTimeSection;
    }

    /*#seaweed#
    private void SwitchSectionClick()
    {
        OnSectionClick((int)IOParams.SkillMode, IOSectionState.SkillMode);
        Debug.LogError(Input.mousePosition.x);
        if (Input.mousePosition.x <= 770)
            selectSection.baseSection.CurIndex = 0;
        else if (Input.mousePosition.x > 770 && Input.mousePosition.x < 835)
            selectSection.baseSection.CurIndex = 1;
        else
            selectSection.baseSection.CurIndex = 2;
    }
    */
    private void DateTimeSectionClick()
    {
        int idx = selectionList.Count - 3;
        OnSectionClick(idx, IOSectionState.DateTime);
        if (Input.mousePosition.x <= 530)
            selectSection.baseSection.CurIndex = 0;
        else if (Input.mousePosition.x > 530 && Input.mousePosition.x <= 640)
            selectSection.baseSection.CurIndex = 1;
        else if (Input.mousePosition.x > 640 && Input.mousePosition.x <= 740)
            selectSection.baseSection.CurIndex = 2;
        else if (Input.mousePosition.x > 740 && Input.mousePosition.x <= 800)
            selectSection.baseSection.CurIndex = 3;
        else
            selectSection.baseSection.CurIndex = 4;
    }


    /// <summary>
    /// 当前选择项
    /// </summary>
    /// <param name="index"></param>
    /// <param name="setColor"></param>
    public void SetCurSelect(int index = 0, bool setColor = true)
    {
        if (setColor)
        {
            for (int i = 0; i < selectionList.Count; i++)
                selectionList[i].Color = IOCanvasModel.Instance.textNormalColor;

            selectionList[index].Color = IOCanvasModel.Instance.textSelectedColor;
        }
        curSelect = index;
        if ((State == IOState.Code && index == 12)
            || ((State == IOState.EditPassword && index == 12)))
            passwordPanel.SetCurSelect(index, false);
        if (State == IOState.ProSetting)
        {
            int offset = IOCanvasModel.Instance.permissions == 3 ? 1 : 0;
            for (int i = offset + 1; i < selectionList.Count - 1; i++)
                selectionList[i].transform.Find("Select").gameObject.SetActive(false);
            for (int i = 0; i < selectionList.Count; i++)
            {
                if (selectionList[i] != selectSection.baseSection)
                    selectionList[i].Color = IOCanvasModel.Instance.textNormalColor;
            }
            if (curSelect > offset && curSelect < selectionList.Count - 1)
                selectionList[curSelect].transform.Find("Select").gameObject.SetActive(true);
            else
                selectionList[index].Color = IOCanvasModel.Instance.textSelectedColor;
        }
    }

    private void RefreshFunctionText()
    {
        title.text = Utils.GetLanguage("Menu");
        subtitle.text = "";
        int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 54 : 66;
        for (int i = 0; i < selectionList.Count; i++)
        {
            if (selectionList[i] is IOBaseSection)
            {
                IOBaseSection baseSection = selectionList[i] as IOBaseSection;
                baseSection.titleText.text = Utils.GetLanguage(baseSection.title);
                baseSection.titleText.fontSize = fontSize;
                for (int j = 0; j < baseSection.contents.Length; j++)
                    baseSection.contents[j] = Utils.GetLanguage(baseSection.contents[j]);
            }
            else if (selectionList[i] is IOBaseBtn)
            {
                IOBaseBtn baseBtn = selectionList[i] as IOBaseBtn;
                baseBtn.titleText.fontSize = fontSize;
                baseBtn.titleText.text = Utils.GetLanguage(baseBtn.title);
            }
        }
    }

    private void OnDisable()
    {
        RemoveEventListener();
    }







    #region 选择推币机机台

    /// <summary>
    /// 通用选择推币机机台界面
    /// </summary>
    /// <param name="onSelectMachineCallback"></param>
    private void InitSelectVisibleCoinPushMachinePanel(Action onSelectMachineCallback)
    {
        curSelectMachineId = -1;

        ClearMenuBtn();
        //SetCodePanelGridLayout();
        SetCheckCoinPushHardwarePanelGridLayout();

        title.text = Utils.GetLanguage(IOFunction.SelectCoinPushMachine.ToString()); //  Utils.GetLanguage(IOFunction.Code.ToString());
        subtitle.text = "";
        State = IOState.SelectCoinPushMachine;

        //SBoxIdea.RequestCoder(0); //异步请求数据
        OnInitSelectVisibleCoinPushMachinePanel(onSelectMachineCallback);
    }

    void OnInitSelectVisibleCoinPushMachinePanel(Action onClickCallback)
    {
        int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 54 : 60;

        //Dictionary<long, Player> playerClientIdDic = PlayerMgr.Instance.playerClientIdDic;

        Dictionary<long, Player> playerClientIdDic = new Dictionary<long, Player>()
        {
            [1] = new Player()
            {
                IsOnline = true,
                macId = 11109001,
                gameType = (int)GameType.CoinPusher,
            },
            [2] = new Player()
            {
                IsOnline = true,
                macId = 11109002,
                gameType = (int)GameType.CoinPusher,
            },
            [3] = new Player()
            {
                IsOnline = true,
                macId = 11109003,
                gameType = (int)GameType.CoinPusher,
            },
        };

        foreach (KeyValuePair<long, Player> info in playerClientIdDic)
        {
            Player player = info.Value;
            if (player.IsOnline && player.gameType == (int)GameType.CoinPusher)
            {
                // Coin Push Mahcine {0}
                int machineId = player.macId;
                InstantiateBaseBtn($"{Utils.GetLanguage("CoinPushMachine")} {machineId}", () => {
                    curSelectMachineId = machineId;
                    onClickCallback();
                }, fontSize: fontSize, style: 0);
            }
        }

        // 返回按钮
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = menuPanel;
        parent.transform.localScale = Vector3.one;
        LayoutElement le = parent.AddComponent<LayoutElement>();
        le.ignoreLayout = true;
        parent.transform.localPosition = new Vector3(0, -450, 0);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1f);
    }


    private void InstantiateMachineInfo(int machineId)
    {
        float x = 950;

        string machineIdStr = $"{machineId}";
        string lineIdStr = machineIdStr.Substring(0, machineIdStr.Length - 4);

        string str1 = $"ID:{lineIdStr}/{machineId}";
        var baseShow1 = InstantiateEasyShow(str1, "", parent: transform, fontSize: 35);
        baseShow1.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -440);

    }

    private UnityAction ReturnToSelectVisibleCoinPushMachinePanel(Action onSelectMachineCallback)
    {
       return () => InitSelectVisibleCoinPushMachinePanel(onSelectMachineCallback);
    }
    

    #endregion



    #region |推币机硬件测试

    private void SetCheckCoinPushHardwarePanelGridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperLeft;
        gridLayout.spacing = new Vector2(295.5f, 55f);
        gridLayout.cellSize = new Vector2(500, 80f);
        gridLayout.padding = new RectOffset(0, 0, 0, 0);

        gridLayoutRect.anchorMin = Vector2.zero;       // 左下锚点 (0, 0)
        gridLayoutRect.anchorMax = Vector2.one;        // 右上锚点 (1, 1)
                                                       // 设置 Offsets 为 0（与边缘对齐）
        gridLayoutRect.offsetMin = new Vector2(60, 200); // 左60px，下边距200px（注意y轴方向）
        gridLayoutRect.offsetMax = new Vector2(-60, -150); // 右60px，上边距150px

    }

    private void InitCheckCoinPushHardwarePanel()
    {
        ClearMenuBtn();
        //SetCodePanelGridLayout();
        SetCheckCoinPushHardwarePanelGridLayout();

        title.text =Utils.GetLanguage(IOFunction.CheckCoinPushHardware.ToString()); //  Utils.GetLanguage(IOFunction.Code.ToString());
        subtitle.text = "";
        State = IOState.CheckCoinPushHardware;


        //SBoxIdea.RequestCoder(0); //异步请求数据
        OnInitCheckCoinPushHardwarePanel();
    }
    void OnInitCheckCoinPushHardwarePanel()
    {

        int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 54 : 66;
        InstantiateBaseBtn(IOCheckCoinPushHardware.CheckLaunchCoin.ToString(), () => { Debug.Log("i am here"); }, fontSize: fontSize, style:0);
        InstantiateBaseBtn(IOCheckCoinPushHardware.CheckLaunchBall.ToString(), () => { Debug.Log("i am here"); }, fontSize: fontSize, style: 0);
        InstantiateBaseBtn(IOCheckCoinPushHardware.CheckPushPlate.ToString(), () => { Debug.Log("i am here"); }, fontSize: fontSize, style: 0);
        InstantiateBaseBtn(IOCheckCoinPushHardware.CheckWiper.ToString(), () => { Debug.Log("i am here"); }, fontSize: fontSize, style: 0);
        InstantiateBaseBtn(IOCheckCoinPushHardware.CheckBell.ToString(), () => { Debug.Log("i am here"); }, fontSize: fontSize, style: 0);

        // 返回按钮
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = menuPanel;
        parent.transform.localScale = Vector3.one;
        LayoutElement le = parent.AddComponent<LayoutElement>();
        le.ignoreLayout = true;
        parent.transform.localPosition = new Vector3(0,-450,0);
        //InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1f);
        InstantiateBaseBtn("Return", ReturnToSelectVisibleCoinPushMachinePanel(InitCheckCoinPushHardwarePanel), parent: parent.transform, style: 2, scale: 1f);


        InstantiateMachineInfo(curSelectMachineId);
        /* 返回按钮提示
        var baseShow2 = InstantiateEasyShow(Utils.GetLanguage("ClickCancleTips"), "", parent: transform, fontSize: 40);
        baseShow2.GetComponent<RectTransform>().anchoredPosition = IOCanvasModel.Instance.curlanguage == Language.en ? new Vector2(-50, -340) : new Vector2(205, -340);
        */
    }






    #endregion
}