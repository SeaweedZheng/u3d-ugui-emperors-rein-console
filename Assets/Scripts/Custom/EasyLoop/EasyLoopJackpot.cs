using System;
using System.Collections.Generic;
using UnityEngine;

public class EasyLoopJackpot : MonoBehaviour
{
    private int test = 99;
    private int curJackpot;
    private List<EasyLoopNum> nums = new List<EasyLoopNum>();

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            nums.Add(transform.GetChild(i).GetComponent<EasyLoopNum>());
        SetJackpot(test);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    test += 12;
        //    ScrollToJackpot(test);
        //    Debug.Log("target = " + test);
        //}

        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    SetJackpot(100);
        //}
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
    }

    public void ScrollToJackpot(int targetJackpot)
    {
        int offset = 1;
        curJackpot = 0;
        for (int i = 0; i < nums.Count; i++)
        {
            curJackpot += nums[i].value * offset;
            offset *= 10;
        }
        int times = targetJackpot - curJackpot;
        nums[0].ScrollByTimes(times);
    }
}
