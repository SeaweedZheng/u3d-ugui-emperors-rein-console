using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumItem : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    public void ScrollCellIndex(int idx)
    {
        text.text = idx == 0 ? " " : (idx % 10).ToString();
    }
}
