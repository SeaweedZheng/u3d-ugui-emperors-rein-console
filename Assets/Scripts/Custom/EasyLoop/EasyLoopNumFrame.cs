using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyLoopNumFrame : MonoBehaviour
{
    private bool isInit;
    private List<Text> texts = new List<Text>();

    private void Start()
    {
        InitView();
    }

    private void InitView()
    {
        if (isInit) return;
        for (int i = 0; i < transform.childCount; i++)
            texts.Add(transform.GetChild(i).GetComponent<Text>());
        isInit = true;
    }

    public void SetNum(int num, bool showNum = true)
    {
        InitView();
        for (int i = 0; i < texts.Count; i++)
        {
            string str = num == 0 && !showNum ? "" : num.ToString();
            texts[i].text = str;
            num++;
            num = num > 9 ? 0 : num;
        }
    }
}
