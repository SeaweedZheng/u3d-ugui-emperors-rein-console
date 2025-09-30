using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkDeviceView : MonoBehaviour
{
    private List<LinkDeviceItemView> linkGameItemViews = new List<LinkDeviceItemView>();
    private List<Player> players = new List<Player>();

    private bool isInit;
    private int pageIdx;
    private int pageItemCount;

    private void Start()
    {
        InitView();
    }

    private void OnEnable()
    {
        AddEventListener();
        ShowLinkGame();
    }

    private void OnDisable()
    {
        RemoveEventListener();
        foreach (var item in linkGameItemViews)
            item.gameObject.SetActive(false);
    }

    private void InitView()
    {
        if (isInit) return;
        for (int i = 0; i < transform.childCount; i++)
            linkGameItemViews.Add(transform.GetChild(i).GetComponent<LinkDeviceItemView>());
        pageItemCount = transform.childCount;

        isInit = true;
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener(EventHandle.DEVICE_PANEL_PRE_PAGE, OnDevicePanelPrePage);
        EventCenter.Instance.AddEventListener(EventHandle.DEVICE_PANEL_NEXT_PAGE, OnDevicePanelNextPage);
    }

    private void RemoveEventListener()
    {
        EventCenter.Instance.RemoveEventListener(EventHandle.DEVICE_PANEL_PRE_PAGE, OnDevicePanelPrePage);
        EventCenter.Instance.RemoveEventListener(EventHandle.DEVICE_PANEL_NEXT_PAGE, OnDevicePanelNextPage);
    }

    private void OnDevicePanelNextPage()
    {
        int curIdx = (pageIdx + 1) * pageItemCount;
        if (curIdx >= players.Count) return;
        pageIdx++;
        SetItemViewData();
    }

    private void OnDevicePanelPrePage()
    {
        if (pageIdx == 0) return;
        pageIdx--;
        SetItemViewData();
    }

    private void ShowLinkGame()
    {
        InitView();
        pageIdx = 0;
        players.Clear();
        foreach (var player in PlayerMgr.Instance.playerClientIdDic.Values)
            players.Add(player);

        SetItemViewData();
    }

    public void SetItemViewData()
    {
        int curIdx = pageIdx * pageItemCount;
        for (int i = 0; i < pageItemCount; i++)
        {
            if (curIdx >= players.Count)
                linkGameItemViews[i].gameObject.SetActive(false);
            else
                linkGameItemViews[i].SetViewData(i, players[curIdx]);
            curIdx++;
        }
    }
}
