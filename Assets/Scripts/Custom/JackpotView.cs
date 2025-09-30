using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackpotView : MonoBehaviour
{
    private int jackpotIdx;
    public int JackpotIdx {
        set { jackpotIdx = value; SetNumsJackpotIdx(); }
        get { return jackpotIdx; } 
    }

    private int curJackpot;
    private bool showComma;
    private Animator titleAni;
    private GameObject lightAni;

    private Vector2 commOriPos;
    private Vector2 pointOriPos;
    private Vector2 numsOriPos;

    private RectTransform commaTrans;
    private RectTransform pointTrans;
    private RectTransform numsTrans;

    private RectTransform commaImgTrans;
    private List<EasyLoopNum> nums = new List<EasyLoopNum>();

    private float numPosOffset = 15;

    private void Awake()
    {
        commaTrans = transform.Find("Anchor/comma").GetComponent<RectTransform>();
        commOriPos = commaTrans.anchoredPosition;

        commaImgTrans = transform.Find("Anchor/comma/Image").GetComponent<RectTransform>();

        pointTrans = transform.Find("Anchor/point").GetComponent<RectTransform>();
        pointOriPos = pointTrans.anchoredPosition;

        titleAni = transform.Find("Anchor/Title/Image").GetComponent<Animator>();

        lightAni = transform.Find("Anchor/light").gameObject;
        numsTrans = transform.Find("Anchor/easyLoopNumList").GetComponent<RectTransform>();
        numsOriPos = numsTrans.anchoredPosition;
        for (int i = 0; i < numsTrans.childCount; i++)
            nums.Add(numsTrans.GetChild(i).GetComponent<EasyLoopNum>());
    }

    private void SetNumsJackpotIdx()
    {
        for (int i = 0; i < nums.Count; i++)
            nums[i].jackpotIdx = jackpotIdx;
    }

    private void Start()
    {
        AddEventListener();
    }

    private void AddEventListener()
    {
        //ÒÆ¶¯¶ººÅ
        EventCenter.Instance.AddEventListener<JackpotChangeData>(EventHandle.CHANGE_JACKPOT, OnChangeJackpot);
    }

    private void OnChangeJackpot(JackpotChangeData jackpotChangeData)
    {
        //ÒÆ¶¯¶ººÅ
        if (jackpotChangeData.jackpotIdx != jackpotIdx || jackpotChangeData.numIdx != 5 || showComma) return;
        AsyncActionUtils.ApplyAnchoredMovement(this, commaImgTrans.transform, new Vector2(0, -35), new Vector2(0, 0), 0.1f, TweenUtils.VectorTweenLinear);
        showComma = true;

        commaTrans.anchoredPosition = commOriPos + new Vector2(numPosOffset, 0);
        pointTrans.anchoredPosition = pointOriPos + new Vector2(numPosOffset, 0);
        numsTrans.anchoredPosition = numsOriPos + new Vector2(numPosOffset, 0);
    }

    public void EnableAni()
    {
        StartCoroutine(ShowTitleLight());
    }

    private IEnumerator ShowTitleLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Utils.GetRandom(10, 15));
            lightAni.gameObject.SetActive(true);
            titleAni.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            lightAni.gameObject.SetActive(false);
            titleAni.gameObject.SetActive(false);
        }
    }


    public void SetJackpot(int jackpot)
    {
        curJackpot = jackpot;
        var chars = jackpot.ToString().ToCharArray();
        Array.Reverse(chars);

        for (int i = 0; i < chars.Length; i++)
        {
            int temp = int.Parse(chars[i].ToString());
            nums[i].InitNum(i, temp);
        }

        for (int i = chars.Length; i < nums.Count; i++)
            nums[i].InitNum(i);

        if (curJackpot >= 100000)
        {
            showComma = true;
            commaImgTrans.anchoredPosition = new Vector2(0, 0);

            commaTrans.anchoredPosition = commOriPos + new Vector2(numPosOffset, 0);
            pointTrans.anchoredPosition = pointOriPos + new Vector2(numPosOffset, 0);
            numsTrans.anchoredPosition =  numsOriPos + new Vector2(numPosOffset, 0);
        }
        else
        {
            showComma = false;
            commaImgTrans.anchoredPosition = new Vector2(0, -35);
            commaTrans.anchoredPosition = commOriPos;
            pointTrans.anchoredPosition = pointOriPos;
            numsTrans.anchoredPosition = numsOriPos;
        }
    }

    public void ScrollToJackpot(int targetJackpot)
    {
        //switch (Model.scrollNumAniType)
        //{
        //    case 1:
        //        ScrollAllNums(targetJackpot);
        //        break;
        //    default:
        //        ScrollSingledigits(targetJackpot);
        //        break;
        //}
        ScrollAllNums(targetJackpot);
    }

    private void ScrollSingledigits(int targetJackpot)
    {
        int offset = 1;
        curJackpot = 0;
        for (int i = 0; i < nums.Count; i++)
        {
            curJackpot += nums[i].value * offset;
            offset *= 10;
        }
        if (targetJackpot < curJackpot)
            SetJackpot(targetJackpot);
        else if (targetJackpot > curJackpot)
        {
            int times = targetJackpot - curJackpot;
            nums[0].ScrollByTimes(times);
        }
    }

    private void ScrollAllNums(int targetJackpot)
    {
        int offset = 1;
        curJackpot = 0;
        for (int i = 0; i < nums.Count; i++)
        {
            curJackpot += nums[i].value * offset;
            offset *= 10;
        }
        if (targetJackpot < curJackpot)
            SetJackpot(targetJackpot);
        else if (targetJackpot > curJackpot)
        {
            List<int> targetDigits = new List<int>();
            foreach (char c in targetJackpot.ToString())
                targetDigits.Add(int.Parse(c.ToString()));
            targetDigits.Reverse();

            List<int> curDigits = new List<int>();
            foreach (char c in curJackpot.ToString())
                curDigits.Add(int.Parse(c.ToString()));
            curDigits.Reverse();

            for (int i = 0; i < targetDigits.Count; i++)
            {
                if (curDigits.Count < i + 1)
                {
                    curDigits.Add(0);
                    if (i == 5)
                    {
                        EventCenter.Instance.EventTrigger(EventHandle.CHANGE_JACKPOT, new JackpotChangeData
                        {
                            jackpotIdx = jackpotIdx,
                            numIdx = i
                        });
                    }
                }
                if (targetDigits[i] < curDigits[i])
                    targetDigits[i] += 10;
                nums[i].ScrollByTimes(targetDigits[i] - curDigits[i]);
            }
        }
    }
}
  