using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroadcastText : MonoBehaviour
{
    private bool isInit = false;
    private Text text;

    private void Awake()
    {
        InitView();
    }

    private void InitView()
    {
        if (isInit) return;
        text = GetComponent<Text>();
        isInit = true;
    }

    public void SetText(string str)
    {
        InitView();
        text.text = str;
    }
}
