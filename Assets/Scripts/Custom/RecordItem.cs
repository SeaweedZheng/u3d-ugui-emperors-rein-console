using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordItem : MonoBehaviour
{
    private bool hasInit = false;

    private Image bg;
    private Text gameType;
    private Text gameName;
    private Text macId;
    private Text seatId;
    private Image signal;
    private Text time;
    private Text winText;

    private List<Sprite> bgImags = new List<Sprite>();
    private List<Sprite> signals = new List<Sprite>();
    private List<Sprite> wins = new List<Sprite>();

    private void Awake()
    {
        InitView();
    }

    private void InitView()
    {
        if (hasInit) return;

        bg = transform.Find("bg").GetComponent<Image>();
        gameType = transform.Find("gameType").GetComponent<Text>();
        gameName = transform.Find("gameName").GetComponent<Text>();
        macId = transform.Find("macId").GetComponent<Text>();
        seatId = transform.Find("seatId").GetComponent<Text>();
        signal = transform.Find("signal").GetComponent<Image>();
        time = transform.Find("time").GetComponent<Text>();
        winText = transform.Find("win").GetComponent<Text>();
        for (int i = 0; i < 4; i++)
        {
            Texture2D texture2D = ResMgr.Instance.LoadObjectFromAssetBundle("ui", $"recordItem{i}") as Texture2D;
            bgImags.Add(Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero));
            texture2D = ResMgr.Instance.LoadObjectFromAssetBundle("ui", $"recordSignal{i}") as Texture2D;
            signals.Add(Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero));
        }
    }

    public void ScrollCellIndex(int dataMode, int idx)
    {
        InitView();
        var orderData = Model.Instance.highestWinsOrderData[dataMode][idx];
        
        bg.sprite = bgImags[orderData.jackpotId];
        signal.sprite = signals[orderData.jackpotId];
        signal.SetNativeSize();
        //string gameNameStr = Model.Instance.GetGameName(orderData.gameType);

        //gameName.fontSize = gameNameStr.Length switch
        //{
        //    5 => 33,
        //    _ => 38,
        //};
        //gameName.text = gameNameStr;
        this.gameType.text = orderData.gameType.ToString();
        macId.text = SQLiteModel.Instance.PlayerIdDataLogicIdDic[orderData.logicId].macId.ToString();
        seatId.text = (orderData.seatId % 1000).ToString();
        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        dt = dt.AddMilliseconds(orderData.time);
        time.text = dt.ToString("MM/dd HH:mm");
        winText.text = orderData.wins == 0 ? "0" : (orderData.wins / 100).ToString();
    }
}
