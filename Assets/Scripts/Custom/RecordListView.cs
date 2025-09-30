using GameUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordListView : MonoBehaviour
{
    private float moveTime = 30f;
    private float space = 90;
    private float speed;
    private Transform content;
    private int curOrderDataMode = 0;
    private CanvasGroup canvasGroup;
    private List<RecordItem> itemList = new List<RecordItem>();

    private void Start()
    {
        speed = -space * 10 / moveTime;
        InitView();
    }

    private void InitView()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        content = transform.Find("content");
        for (int i = 0; i < content.childCount; i++)
            itemList.Add(content.GetChild(i).GetComponent<RecordItem>());
        ShowList();
        Timer.LoopAction(12, Tween);
    }

    private void ShowList()
    {
        for (int i = 0; i < itemList.Count; i++)
            itemList[i].gameObject.SetActive(false);


        int count = Model.Instance.highestWinsOrderData[curOrderDataMode].Count > 8 ? 8 : Model.Instance.highestWinsOrderData[curOrderDataMode].Count;
        for (int i = 0; i < count; i++)
        {
            itemList[i].ScrollCellIndex(curOrderDataMode, i);
            itemList[i].gameObject.SetActive(true);
        }
    }

    private void Tween(int _)
    {
        StartCoroutine(Tween());
    }

    private IEnumerator Tween()
    {
        float t = 0;
        float from = 1;
        float to = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t * 2);
            yield return null;
        }
        canvasGroup.alpha = to;
        curOrderDataMode++;
        curOrderDataMode = curOrderDataMode > 4 ? 0 : curOrderDataMode;
        ShowList();
        t = 0;
        float from1 = 0;
        float to1 = 1;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from1, to1, t * 2);
            yield return null;
        }
    }
}
