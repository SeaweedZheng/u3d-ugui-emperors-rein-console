using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyLoopNum : MonoBehaviour
{
    public int jackpotIdx;
    public int numIdx;
    public int times;
    public int value;
    private bool isInit;
    private bool moving;
    private List<EasyLoopNumFrame> easyLoopNumFrames = new List<EasyLoopNumFrame>();

    private void Start()
    {
        InitView();
    }

    private void InitView()
    {
        if (isInit) return;
        for (int i = 0; i < transform.childCount; i++)
            easyLoopNumFrames.Add(transform.GetChild(i).GetComponent<EasyLoopNumFrame>());
        isInit = true;
        AddEventListener();
    }

    private void AddEventListener()
    {
        //EventCenter.Instance.AddEventListener<JackpotChangeData>(EventHandle.CHANGE_JACKPOT, OnChangeJackpotNum);
    }

    private void OnChangeJackpotNum(JackpotChangeData jackpotChangeData)
    {
        if (jackpotChangeData.jackpotIdx != jackpotIdx || numIdx != jackpotChangeData.numIdx || Model.scrollNumAniType != 0) return;
        ScrollByTimes(1);
    }

    public void InitNum(int idx, int num)
    {
        StopAllCoroutines();
        moving = false;
        InitView();
        numIdx = idx;
        SetNum(num);
    }

    public void InitNum(int idx)
    {
        StopAllCoroutines();
        numIdx = idx;
        SetNum(0, false);
    }

    public void SetNum(int num, bool showNum = true)
    {
        value = num;
        foreach (var easyLoopNumFrame in easyLoopNumFrames)
            easyLoopNumFrame.SetNum(value, showNum);
        easyLoopNumFrames[0].gameObject.SetActive(true);
        for (int i = 1; i < easyLoopNumFrames.Count; i++)
            easyLoopNumFrames [i].gameObject.SetActive(false);
    }

    public void ScrollByTimes(int times)
    {
        if (!moving)
        {
            this.times = times;
            StartCoroutine(Scroll());
        }
        else
            this.times = times + 1;
    }

    private IEnumerator Scroll()
    {
        moving = true;
        while (times > 0)
        {
            value++;
            if (value > 9)
            {
                value = 0;
                JackpotChangeData data = new JackpotChangeData
                {
                    jackpotIdx = jackpotIdx,
                    numIdx = numIdx + 1,
                };
                //if (Model.scrollNumAniType == 0)
                //    EventCenter.Instance.EventTrigger(EventHandle.CHANGE_JACKPOT, data);
            }
            int frameCurIdx = 0;
            for (int i = 0; i < easyLoopNumFrames.Count - 1; i++)
            {
                easyLoopNumFrames[frameCurIdx].gameObject.SetActive(false);
                frameCurIdx++;
                easyLoopNumFrames[frameCurIdx].gameObject.SetActive(true);
                
                yield return new WaitForSeconds(0.02f);
            }

            foreach (var easyLoopNumFrame in easyLoopNumFrames)
                easyLoopNumFrame.SetNum(value);
            easyLoopNumFrames[easyLoopNumFrames.Count - 1].gameObject.SetActive(false);
            easyLoopNumFrames[0].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.02f);
            times--;
        }
        moving = false;
    }
}
