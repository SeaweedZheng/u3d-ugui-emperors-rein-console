using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numListTest : MonoBehaviour
{
    private int defaultJackpot;
    private int curentJackpot;
    private List<NumView> nums = new List<NumView>();

    // Start is called before the first frame update
    void Start()
    {
        defaultJackpot = 99;
        for (int i = 0; i < transform.childCount; i++)
            nums.Add(transform.GetChild(i).GetComponent<NumView>());
        SetJackpot(defaultJackpot);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            defaultJackpot += 2;
            Debug.Log("targetJackpot = " + defaultJackpot);
            ScrollToJackpot(defaultJackpot);
        }
    }

    public void SetJackpot(int jackpot)
    {
        curentJackpot = jackpot;
        var chars = jackpot.ToString().ToCharArray();
        Array.Reverse(chars);

        for (int i = 0; i < chars.Length; i++)
        {
            int temp = int.Parse(chars[i].ToString());
            if (temp == 0)
                temp = 10;
            nums[i].InitList(i, temp);
        }

        for (int i = chars.Length; i < nums.Count; i++)
            nums[i].InitList(i);
    }

    public void ScrollToJackpot(int targetJackpot)
    {
        curentJackpot = 0;
        for (int i = 0; i < nums.Count; i++)
        {
            nums[i].GetLastItemIdx();
            curentJackpot += (nums[i].GetLastItemIdx() % 10) * (int)Math.Pow(10, i);
        }
        if (targetJackpot == curentJackpot)
            return;

        if (targetJackpot < curentJackpot)
        {
            SetJackpot(targetJackpot);
            return;
        }

        int temp = targetJackpot - curentJackpot;
        nums[0].ScrollByTimes(temp);
    }

}
