using SBoxApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BillCanvasView : MonoSingleton<BillCanvasView>
{
    private int curSelect;

    private Text title;
    private Text subtitle;

    private GameObject baseShowPrefab;
    private GameObject baseShowPrefab1;
    private GameObject baseShowPrefab2;
    private GameObject baseShowPrefab3;
    private GameObject baseBtnPrefab;
    private GameObject baseBtnPrefab1;
    private GameObject baseBtnPrefab2;
    private GameObject baseTogglePrefab;

    private IOState State
    {
        set { IOCanvasModel.Instance.state = value; }
        get { return IOCanvasModel.Instance.state; }
    }

    private Transform menuPanel;
    private GridLayoutGroup gridLayout;
    private RectTransform gridLayoutRect;
    private List<IOBaseSelection> selectionList = new List<IOBaseSelection>();
    private List<IOBaseBtnSelection> onlyShowList = new List<IOBaseBtnSelection>();
    private List<GameObject> tempObjList = new List<GameObject>();

    private void Awake()
    {
        ResMgr.Instance.LoadAssetBundle("io", "baseShow0", (obj) => baseShowPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow1", (obj) => baseShowPrefab1 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow2", (obj) => baseShowPrefab2 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseShow3", (obj) => baseShowPrefab3 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseButton0", (obj) => baseBtnPrefab = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseButton1", (obj) => baseBtnPrefab1 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseButton2", (obj) => baseBtnPrefab2 = (GameObject)obj);
        ResMgr.Instance.LoadAssetBundle("io", "baseToggle", (obj) => baseTogglePrefab = (GameObject)obj);

        title = transform.Find("title/center/title").GetComponent<Text>();
        subtitle = transform.Find("title/center/subtitle").GetComponent<Text>();
        InstantiateMenuPanel();
        SetBillPanelGridLayout();
    }

    private void OnEnable()
    {
        InitBillPanel();
        AddEventListener();
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener(EventHandle.INIT_BILL_VIEW, OnInitBillView);
    }

    private void OnDisable()
    {
        RemoveEventListener();
    }

    private void RemoveEventListener()
    {
        EventCenter.Instance.RemoveEventListener(EventHandle.INIT_BILL_VIEW, OnInitBillView);
    }

    public void InstantiateMenuPanel()
    {
        var trans = new GameObject("menuPanel", typeof(GridLayoutGroup)).transform;
        trans.SetParent(transform);
        trans.localScale = Vector3.one;
        trans.localPosition = Vector3.zero;
        menuPanel = trans;
    }

    private void SetBillPanelGridLayout()
    {
        gridLayout = menuPanel.GetComponent<GridLayoutGroup>();
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.padding = new RectOffset(0, 0, 50, 0);
        gridLayout.spacing = new Vector2(220, 37);
        gridLayout.cellSize = new Vector2(350, 300);
        gridLayoutRect = menuPanel.GetComponent<RectTransform>();
        gridLayoutRect.sizeDelta = new Vector2(1780, 800);
        gridLayoutRect.anchoredPosition = Vector2.zero;
    }

    public void ClearMenuBtn()
    {
        selectionList.ForEach(baseBtn => Destroy(baseBtn.gameObject));
        selectionList.Clear();
        onlyShowList.ForEach(baseShow => Destroy(baseShow.gameObject));
        onlyShowList.Clear();
        tempObjList.ForEach(temp => Destroy(temp));
        tempObjList.Clear();
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


        //if (IOCanvasModel.Instance.billData.scoreUp > 0 || IOCanvasModel.Instance.billData.scoreDown > 0)
        {
            var parent0 = InstantiateBillParent();
            InstantiateEasyShow(Utils.GetLanguage("AllScoreUp"), IOCanvasModel.Instance.billData.scoreUp.ToString(), parent: parent0);
            InstantiateEasyShow(Utils.GetLanguage("AllScoreDown"), IOCanvasModel.Instance.billData.scoreDown.ToString(), parent: parent0);
            InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent0);
            int profit0 = IOCanvasModel.Instance.billData.scoreDown == 0 ? 0 : IOCanvasModel.Instance.billData.scoreUp - IOCanvasModel.Instance.billData.scoreDown;
            InstantiateEasyShow(Utils.GetLanguage("AllProfit"), profit0.ToString(), parent: parent0);
        }

        //if (IOCanvasModel.Instance.billData.coinIn > 0 || IOCanvasModel.Instance.billData.ticketOut > 0)
        {
            var parent1 = InstantiateBillParent();
            InstantiateEasyShow(Utils.GetLanguage("AllCoinIn"), IOCanvasModel.Instance.billData.coinIn.ToString(), parent: parent1);
            InstantiateEasyShow(Utils.GetLanguage("AllCollectTicket"), IOCanvasModel.Instance.billData.ticketOut.ToString(), parent: parent1);
            InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent1);
            int profit1 = IOCanvasModel.Instance.billData.ticketOut == 0 ? 0 : IOCanvasModel.Instance.billData.coinIn - IOCanvasModel.Instance.billData.ticketOut;
            InstantiateEasyShow(Utils.GetLanguage("AllProfit"), profit1.ToString(), parent: parent1);
        }

        var parent2 = InstantiateBillParent();
        InstantiateEasyShow(Utils.GetLanguage("A"), IOCanvasModel.Instance.billData.bets.ToString(), parent: parent2);
        InstantiateEasyShow(Utils.GetLanguage("B"), IOCanvasModel.Instance.billData.wins.ToString(), parent: parent2);
        InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent2);
        int profit2 = IOCanvasModel.Instance.billData.bets - IOCanvasModel.Instance.billData.wins;
        InstantiateEasyShow(Utils.GetLanguage("C"), profit2.ToString(), parent: parent2);

        var parent3 = InstantiateBillParent();
        InstantiateEasyShow(Utils.GetLanguage("DelayA"), IOCanvasModel.Instance.billData.delayBet.ToString(), parent: parent3);
        InstantiateEasyShow(Utils.GetLanguage("DelayB"), IOCanvasModel.Instance.billData.delayWin.ToString(), parent: parent3);
        InstantiateEasyShow(Utils.GetLanguage("AllScore"), IOCanvasModel.Instance.billData.credit.ToString(), parent: parent3);
        int profit3 = IOCanvasModel.Instance.billData.delayBet - IOCanvasModel.Instance.billData.delayWin;
        InstantiateEasyShow(Utils.GetLanguage("DelayC"), profit3.ToString(), parent: parent3);



        InstantiateBillDetal();

        var closeBtn = InstantiateBaseBtn("CloseBtn", Close, parent: transform, fontSize: 50);
        var closeBtnRect = closeBtn.transform.GetComponent<RectTransform>();
        closeBtnRect.sizeDelta = new Vector2(300, 90);
        closeBtnRect.anchoredPosition = new Vector2(0, -350);

        curSelect = 0;
        SetCurSelect();
    }

    private void InstantiateBillDetal()
    {
        DateTime dt = DateTime.Now;
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

        var baseShow0 = InstantiateEasyShow(str, "", parent: transform, fontSize: 30);
        float x = 680;
        baseShow0.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -325f);

        string str1 = $"ID:{IOCanvasModel.Instance.CfgData.LineId}/{IOCanvasModel.Instance.CfgData.MachineId}:{IOCanvasModel.Instance.CfgData.CoinValue}";
        var baseShow1 = InstantiateEasyShow(str1, "", parent: transform, fontSize: 30);
        baseShow1.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -375f);

        string tempStr = IOCanvasModel.Instance.winLockBalance / 10000 > 0 ? $"<color=#F8DF1B>{IOCanvasModel.Instance.winLockBalance / 10000}</color>" : $"<color=#FF0000>{IOCanvasModel.Instance.winLockBalance / 10000}</color>";

        string str2 = $"{PlayerPrefs.GetString("CurVersion", "1.0.0")}/{IOCanvasModel.Instance.sBoxVersion}/{IOCanvasModel.Instance.CfgData.difficulty}/{tempStr}";

        var baseShow2 = InstantiateEasyShow(str2, "", parent: transform, fontSize: 30);
        baseShow2.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, -425f);
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

    private void InitClientBillPanelNo1()
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
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1);

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

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1);
        }
        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.scoreUp.ToString(),
            IOCanvasModel.Instance.billData.scoreDown.ToString(),
            (IOCanvasModel.Instance.billData.scoreUp - IOCanvasModel.Instance.billData.scoreDown).ToString(),
            IOCanvasModel.Instance.billData.credit.ToString(),
        };
        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo3, parent: parent);
        InstantiateBaseBtn("Return", RetrunToFunction, parent: parent.transform, style: 2);

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
        subtitle.text = $"({Utils.GetLanguage("ScoreUp")}/{Utils.GetLanguage("ScoreDown")})";
        List<string> titleList = new List<string>
        {
            Utils.GetLanguage("MacId"),
            Utils.GetLanguage("ScoreUp"),
            Utils.GetLanguage("ScoreDown"),
            Utils.GetLanguage("Profit"),
            Utils.GetLanguage("AllScore"),
        };
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1);

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

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1);
        }
        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.scoreUp.ToString(),
            IOCanvasModel.Instance.billData.scoreDown.ToString(),
            (IOCanvasModel.Instance.billData.scoreUp - IOCanvasModel.Instance.billData.scoreDown).ToString(),
            IOCanvasModel.Instance.billData.credit.ToString(),
        };
        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo3, parent: parent);
        InstantiateBaseBtn("Return", RetrunToFunction, parent: parent, style: 2);

        InstantiateBillDetal();

        curSelect = 0;
        SetCurSelect(setColor: true);
        State = IOState.Clients;
    }

    private Transform InstantiateBillBtnParent()
    {
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.AddComponent<ToggleGroup>();
        parent.transform.parent = transform;
        var rect = parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(726, 100);
        rect.anchoredPosition = new Vector2(-131, -276);
        return parent.transform;
    }

    private void SetBillPanelNo1GridLayout()
    {
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.padding = new RectOffset(0, 0, 0, 0);
        gridLayout.cellSize = new Vector2(900, 5);
        gridLayoutRect.sizeDelta = new Vector2(1000, 636);
        gridLayoutRect.anchoredPosition = new Vector2(0, 345.5f);
    }

    private void InitClientBillPanelNo2(bool isShow)
    {
        if (!isShow) return;
        ClearMenuBtn();
        SetBillPanelNo1GridLayout();

        IOCanvasModel.Instance.curBillPage = 1;
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
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1);
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
            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1);
        }
        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.coinIn.ToString(),
            IOCanvasModel.Instance.billData.ticketOut.ToString(),
            (IOCanvasModel.Instance.billData.coinIn - IOCanvasModel.Instance.billData.ticketOut).ToString(),
            IOCanvasModel.Instance.billData.credit.ToString(),
        };
        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo3, parent: parent);
        InstantiateBaseBtn("Return", RetrunToFunction, parent: parent, style: 2);

        InstantiateBillDetal();

        curSelect = 1;
        SetCurSelect(curSelect, true);
        State = IOState.Clients;
    }

    private void InitClientBillPanelNo3(bool isShow)
    {
        if (!isShow) return;
        ClearMenuBtn();
        SetBillPanelNo1GridLayout();

        IOCanvasModel.Instance.curBillPage = 2;
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
        InstantiateBaseShow("", titleList, contentAlignment: TextAnchor.MiddleCenter, style: 1);


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
                contentList.Add(((playerAccountData.Bets - playerAccountData.Wins) / playerAccountData.Bets).ToString());
            else
                contentList.Add("0");

            InstantiateBaseShow("", contentList, contentAlignment: TextAnchor.MiddleCenter, style: 1);
        }

        List<string> totalList = new List<string>
        {
            Utils.GetLanguage("Total"),
            IOCanvasModel.Instance.billData.bets.ToString(),
            IOCanvasModel.Instance.billData.wins.ToString(),
            (IOCanvasModel.Instance.billData.bets - IOCanvasModel.Instance.billData.wins).ToString(),
        };

        int profit = IOCanvasModel.Instance.billData.bets == 0 ? 0 : (IOCanvasModel.Instance.billData.bets - IOCanvasModel.Instance.billData.wins) / IOCanvasModel.Instance.billData.bets;
        totalList.Add(profit.ToString());

        InstantiateBaseShow("", totalList, contentAlignment: TextAnchor.MiddleCenter, style: 1);

        var parent = InstantiateBillBtnParent();

        InstantiateBaseToggle($"{Utils.GetLanguage("ScoreUp")} / {Utils.GetLanguage("ScoreDown")}", InitClientBillPanelNo1, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("CoinIn")} / {Utils.GetLanguage("TicketOut")}", InitClientBillPanelNo2, parent: parent);
        InstantiateBaseToggle($"{Utils.GetLanguage("ProfitPage")}", InitClientBillPanelNo3, parent: parent);
        InstantiateBaseBtn("Return", RetrunToFunction, parent: parent, style: 2);

        InstantiateBillDetal();

        curSelect = 2;
        SetCurSelect(curSelect, true);
        State = IOState.Clients;
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private IOBaseToggle InstantiateBaseToggle(string str, UnityAction<bool> unityAction, bool showBg = true, TextAnchor textAnchor = TextAnchor.MiddleCenter, Transform parent = null, ToggleGroup toggleGroup = null)
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
        baseToggle.titleText.fontSize = IOCanvasModel.Instance.curlanguage == Language.cht ? 25 : 14;
        baseToggle.titleText.text = str;
        baseToggle.titleText.alignment = textAnchor;
        baseToggle.AddListener(unityAction);
        selectionList.Add(baseToggle);
        return baseToggle;
    }

    private void InstantiateBlank()
    {
        InstantiateEasyShow("", "");
    }

    private IOBaseShow InstantiateEasyShow(string title, string content, bool showBg = false, TextAnchor contentAlignment = TextAnchor.MiddleLeft, int fontSize = 50, Transform parent = null)
    {
        fontSize = fontSize == 30 ? IOCanvasModel.Instance.defaultFontSize : fontSize;
        return InstantiateBaseShow(title, new List<string>() { content }, showBg, contentAlignment: contentAlignment, fontSize: fontSize, parent: parent, style: 3);
    }

    private IOBaseShow InstantiateBaseShow(string title, List<string> contentList, bool showBg = false, Transform parent = null, TextAnchor contentAlignment = TextAnchor.MiddleLeft, int fontSize = 30, int style = 0)
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
            default:
                baseShow = Instantiate(baseShowPrefab3).GetComponent<IOBaseShow>();
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
        return baseShow;
    }

    private IOBaseBtn InstantiateBaseBtn(string str, UnityAction unityAction, bool showBg = true, TextAnchor textAnchor = TextAnchor.MiddleCenter, Transform parent = null, int style = 0, int fontSize = 20)
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
        baseBtn.titleText.fontSize = fontSize == 20 ? (style == 2 ? 30 : fontSize) : fontSize;
        baseBtn.titleText.text = Utils.GetLanguage(str);
        baseBtn.titleText.alignment = textAnchor;
        baseBtn.AddListener(unityAction);
        selectionList.Add(baseBtn);
        return baseBtn;
    }
    public void SetCurSelect(int index = 0, bool setColor = false)
    {
        if (setColor)
        {
            for (int i = 0; i < selectionList.Count; i++)
                selectionList[i].Color = IOCanvasModel.Instance.textNormalColor;

            selectionList[index].Color = IOCanvasModel.Instance.textSelectedColor;
        }
        curSelect = index;
    }

    private void RetrunToFunction()
    {
        gameObject.SetActive(false);
        ClearMenuBtn();
    }
}
