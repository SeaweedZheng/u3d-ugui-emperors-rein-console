using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LoopScrollRect))]
[DisallowMultipleComponent]
public class NumView : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public int jackpotIdx;
    private int numIdx;
    public int idxRecord;
    private int idxFrame;

    private const float scrollSpeed = 400f;

    public GameObject item;
    private LoopScrollRect loopScrollRect;
    Stack<Transform> pool = new Stack<Transform>();

    public GameObject GetObject(int index)
    {
        if (pool.Count == 0)
            return Instantiate(item);
        Transform candidate = pool.Pop();
        candidate.gameObject.SetActive(true);
        return candidate.gameObject;
    }

    public void ReturnObject(Transform trans)
    {
        trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        pool.Push(trans);
    }

    public void ProvideData(Transform transform, int idx)
    {
        transform.SendMessage("ScrollCellIndex", idx);
    }

    private void Awake()
    {
        loopScrollRect = GetComponent<LoopScrollRect>();
        loopScrollRect.prefabSource = this;
        loopScrollRect.dataSource = this;
        loopScrollRect.totalCount = -1;
    }

    private void Start()
    {
        EventCenter.Instance.AddEventListener<JackpotChangeData>(EventHandle.CHANGE_JACKPOT, OnChangeJackpot);
    }

    private void OnChangeJackpot(JackpotChangeData jackpotChangeData)
    {
        if (jackpotChangeData.jackpotIdx != jackpotIdx || numIdx != jackpotChangeData.numIdx) return;
        idxRecord += 1;
        loopScrollRect.ScrollToCell(idxRecord, scrollSpeed);
        if (idxRecord % 10 == 0)
        {
            JackpotChangeData data = new JackpotChangeData
            {
                jackpotIdx = jackpotIdx,
                numIdx = numIdx + 1,
            };
            if (Model.scrollNumAniType == 0)
                EventCenter.Instance.EventTrigger(EventHandle.CHANGE_JACKPOT, data);
        }
    }

    public void ScrollByTimes(int times)
    {
        loopScrollRect.ScrollToCell(idxRecord + times, scrollSpeed);
    }

    public void InitList(int numIdx, int num = 0)
    {
        //Debug.LogError($"InitList numIdx = {numIdx}, num = {num}");
        this.numIdx = numIdx;
        idxFrame = num;
        loopScrollRect.RefillCells(idxFrame);
        idxRecord = loopScrollRect.GetLastItem(out _);
    }

    private void Update()
    {
        if (numIdx != 0)
            return;
        int tempIdx = loopScrollRect.GetLastItem(out _);
        if (tempIdx != idxRecord && tempIdx % 10 == 0)
        {
            JackpotChangeData data = new JackpotChangeData
            {
                jackpotIdx = jackpotIdx,
                numIdx = numIdx + 1,
            };
            if (Model.scrollNumAniType == 0)
                EventCenter.Instance.EventTrigger(EventHandle.CHANGE_JACKPOT, data);
        }
        if (tempIdx != idxRecord)
            idxRecord = tempIdx;
    }

    public int GetLastItemIdx()
    {
        return loopScrollRect.GetLastItem(out _);
    }
}
