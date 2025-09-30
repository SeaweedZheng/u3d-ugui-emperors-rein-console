using GameUtil;
using UnityEngine;
using UnityEngine.UI;

public class PopTips : MonoSingleton<PopTips>
{
    private Text text;
    private float showTime;
    private bool isShow = true;
    private DelayTimer showTimer;
    private RectTransform rect;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
        rect = transform.GetComponent<RectTransform>();
    }

    public void ChangeTips(string str)
    {
        text.text = str;
    }

    public void ShowTips(string str, float time = 3)
    {
        if (text.text == str && isShow) return;
        rect.anchoredPosition = Vector3.zero;
        Show(str, time, false);
    }

    public void ShowSystemTips(string str, float time = 3)
    {
        if (text.text == str && isShow) return;
        rect.anchoredPosition = Vector3.zero;
        Show(str, time, true);
    }

    private void Show(string str, float time = 3, bool isSystem = false)
    {
        text.text = str;
        showTime = time;
        if (showTime > -1)
        {
            isShow = true;
            ShowTimer(time);
        }
        else
            isShow = true;
        transform.localScale = isSystem ? Vector3.one * 0.75f : Vector3.one;
    }

    private void ShowTimer(float time)
    {
        if (showTimer == null)
            showTimer = Timer.DelayAction(time, HideTips);
        else
            showTimer.Restart();
    }

    public void HideTips()
    {
        if (showTimer != null)
            showTimer.Cancel();
        text.text = "";
        transform.localScale = Vector3.zero;
        isShow = false;
    }

}
