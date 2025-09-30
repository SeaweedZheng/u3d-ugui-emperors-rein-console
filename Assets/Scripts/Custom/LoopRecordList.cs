using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LoopScrollRect))]
[DisallowMultipleComponent]
public class LoopRecordList : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject item;
    private GameObject mask;
    private LoopScrollRect loopScrollRect;
    Stack<Transform> pool = new Stack<Transform>();
    private float scrollSpeed = 10;
    private bool initLoopScrollRect;

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
        mask = transform.Find("Mask").gameObject;
        loopScrollRect.prefabSource = this;
        loopScrollRect.dataSource = this;
    }

    private void Start()
    {
        loopScrollRect.totalCount = -1;
        if (Model.Instance.highestWinsOrderData.Count > 0)
        {
            mask.SetActive(false);
            loopScrollRect.RefillCells(0);
            loopScrollRect.ScrollNeverStop(scrollSpeed);
            initLoopScrollRect = true;
        }
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(EventHandle.REFRESH_JACKPOT_RECORD, OnRefreshRecordList);
        EventCenter.Instance.AddEventListener<bool>(EventHandle.HIDE_RECORD, OnHideRecord);
    }

    private void OnRefreshRecordList()
    {
        if (Model.Instance.highestWinsOrderData.Count > 0)
        {
            mask.SetActive(false);
            if (!initLoopScrollRect)
            {
                loopScrollRect.RefillCells(0);
                loopScrollRect.ScrollNeverStop(scrollSpeed);
                initLoopScrollRect = true;
            }
        }
    }

    private void OnHideRecord(bool hide)
    {
        if (hide)
            loopScrollRect.StopNeverStopScroll();
        else if (Model.Instance.highestWinsOrderData.Count > 0)
            loopScrollRect.ScrollNeverStop(scrollSpeed);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(EventHandle.REFRESH_JACKPOT_RECORD, OnRefreshRecordList);
        EventCenter.Instance.RemoveEventListener<bool>(EventHandle.HIDE_RECORD, OnHideRecord);
    }
}
