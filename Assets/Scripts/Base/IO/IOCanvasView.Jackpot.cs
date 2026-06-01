using Newtonsoft.Json;
using SBoxApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 后台彩金参数设置
/// </summary>
public partial class IOCanvasView
{
    private List<List<IOBaseSection>> paramsSectionList = new List<List<IOBaseSection>>();

    /// <summary>
    /// 彩金参数设置面板初始化
    /// </summary>
    /// <remarks>
    /// * 初始化页面是调用一次
    /// </remarks>
    private void InitJackpotParamsPanel()
    {

       // 设置标题
        title.text = Utils.GetLanguage(IOFunction.JackpotSetting.ToString());//彩金设置
        //title.text = Utils.GetLanguage("JackpotSetting"); //彩金设置

        title.fontStyle = FontStyle.Bold;
        title.fontSize = 78;


        ClearMenuBtn();
        SetJackpotParamsGridLayout();

        // 参数备份
        IOCanvasModel.Instance.SetTempJackCfgData(IOCanvasModel.Instance.JackCfgData); 
        InstantiateEasyShow(Utils.GetLanguage("baseValue") + ":", $"{IOCanvasModel.Instance.JackCfgData.BaseSetValue / 100}", style: 0);


        int offsetIndex = (int)IOParams.miniBaseValue;

        #region MiniJakcpot

        //自定义父节点：
        GameObject miniParent = new GameObject();
        tempObjList.Add(miniParent);
        miniParent.AddComponent<VerticalLayoutGroup>();
        miniParent.transform.parent = transform;
        miniParent.transform.localScale = Vector3.one;
        var miniRect = miniParent.GetComponent<RectTransform>();
        miniRect.anchoredPosition = new Vector2(-450, 200);
        miniRect.sizeDelta = new Vector2(550, 300);

        List<IOBaseSection> miniList = new List<IOBaseSection>();

        miniList.Add(
        InstantiateBaseSection(IOParams.miniBaseValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].BaseValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.miniBaseValue - offsetIndex, IOSectionState.miniBaseValue); },
            ioParams: IOParams.miniBaseValue,
            style: 2,
            parent: miniParent.transform
         ));

        miniList.Add(
        InstantiateBaseSection(IOParams.miniMinTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MinTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.miniMinTriggerValue - offsetIndex, IOSectionState.miniMinTriggerValue); }, ioParams: IOParams.miniMinTriggerValue, style: 2,
            parent: miniParent.transform
            ));

        miniList.Add(
        InstantiateBaseSection(IOParams.miniMaxTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MaxTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.miniMaxTriggerValue - offsetIndex, IOSectionState.miniMaxTriggerValue); }, ioParams: IOParams.miniMaxTriggerValue, style: 2,
            parent: miniParent.transform
            ));

        // 这里不加如数组
        miniList.Add(
        InstantiateBaseSection(IOParams.miniWeight.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].Weight / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.miniWeight - offsetIndex, IOSectionState.miniWeight); }, ioParams: IOParams.miniWeight, style: 2,
            parent: miniParent.transform
            ));

        miniList.Add(
        InstantiateBaseSection(IOParams.miniMinBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MinBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.miniMinBet - offsetIndex, IOSectionState.miniMinBet); }, ioParams: IOParams.miniMinBet, style: 2,
            parent: miniParent.transform
            ));

        miniList.Add(
        InstantiateBaseSection(IOParams.miniMaxBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MaxBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.miniMaxBet - offsetIndex, IOSectionState.miniMaxBet); }, ioParams: IOParams.miniMaxBet, style: 2,
            parent: miniParent.transform
            ));

        #endregion




        #region MinorJackpot

        //自定义父节点：
        GameObject minorParent = new GameObject();
        tempObjList.Add(minorParent);
        minorParent.AddComponent<VerticalLayoutGroup>();
        minorParent.transform.parent = transform;
        minorParent.transform.localScale = Vector3.one;
        var mninorRect = minorParent.GetComponent<RectTransform>();
        mninorRect.anchoredPosition = new Vector2(450, 200);
        mninorRect.sizeDelta = new Vector2(550, 300);

        List<IOBaseSection> minorList = new List<IOBaseSection>();

        minorList.Add(
        InstantiateBaseSection(IOParams.minorBaseValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].BaseValue / 100, fontSize: 30,
        onClick: () => { OnSectionClick((int)IOParams.minorBaseValue - offsetIndex, IOSectionState.minorBaseValue); }, ioParams: IOParams.minorBaseValue, style: 2, parent: minorParent.transform));

        minorList.Add(
        InstantiateBaseSection(IOParams.minorMinTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MinTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.minorMinTriggerValue - offsetIndex, IOSectionState.minorMinTriggerValue); }, ioParams: IOParams.minorMinTriggerValue, style: 2, parent: minorParent.transform));

        minorList.Add(
        InstantiateBaseSection(IOParams.minorMaxTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MaxTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.minorMaxTriggerValue - offsetIndex, IOSectionState.minorMaxTriggerValue); }, ioParams: IOParams.minorMaxTriggerValue, style: 2, parent: minorParent.transform));

        // 这里不加如数组
        minorList.Add(
        InstantiateBaseSection(IOParams.minorWeight.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].Weight / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.minorWeight - offsetIndex, IOSectionState.minorWeight); }, ioParams: IOParams.minorWeight, style: 2, parent: minorParent.transform));

        minorList.Add(
        InstantiateBaseSection(IOParams.minorMinBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MinBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.minorMinBet - offsetIndex, IOSectionState.minorMinBet); }, ioParams: IOParams.minorMinBet, style: 2, parent: minorParent.transform));

        minorList.Add(
        InstantiateBaseSection(IOParams.minorMaxBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MaxBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.minorMaxBet - offsetIndex, IOSectionState.minorMaxBet); }, ioParams: IOParams.minorMaxBet, style: 2, parent: minorParent.transform));
        #endregion




        #region MajorJackpot

        //自定义父节点：
        GameObject majorParent = new GameObject();
        tempObjList.Add(majorParent);
        majorParent.AddComponent<VerticalLayoutGroup>();
        majorParent.transform.parent = transform;
        majorParent.transform.localScale = Vector3.one;
        var majorRect = majorParent.GetComponent<RectTransform>();
        majorRect.anchoredPosition = new Vector2(-450, -150);
        majorRect.sizeDelta = new Vector2(550, 300);

        List<IOBaseSection> majorList = new List<IOBaseSection>();

        majorList.Add(
        InstantiateBaseSection(IOParams.majorBaseValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].BaseValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.majorBaseValue - offsetIndex, IOSectionState.majorBaseValue); }, ioParams: IOParams.majorBaseValue, style: 2, parent: majorParent.transform));

        majorList.Add(
        InstantiateBaseSection(IOParams.majorMinTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MinTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.majorMinTriggerValue - offsetIndex, IOSectionState.majorMinTriggerValue); }, ioParams: IOParams.majorMinTriggerValue, style: 2, parent: majorParent.transform));

        majorList.Add(
        InstantiateBaseSection(IOParams.majorMaxTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MaxTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.majorMaxTriggerValue - offsetIndex, IOSectionState.majorMaxTriggerValue); }, ioParams: IOParams.majorMaxTriggerValue, style: 2, parent: majorParent.transform));

        // 这里不加如数组
        majorList.Add(
        InstantiateBaseSection(IOParams.majorWeight.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].Weight / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.majorWeight - offsetIndex, IOSectionState.majorWeight); }, ioParams: IOParams.majorWeight, style: 2, parent: majorParent.transform));

        majorList.Add(
        InstantiateBaseSection(IOParams.majorMinBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MinBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.majorMinBet - offsetIndex, IOSectionState.majorMinBet); }, ioParams: IOParams.majorMinBet, style: 2, parent: majorParent.transform));

        majorList.Add(
        InstantiateBaseSection(IOParams.majorMaxBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MaxBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.majorMaxBet - offsetIndex, IOSectionState.majorMaxBet); }, ioParams: IOParams.majorMaxBet, style: 2, parent: majorParent.transform));


        #endregion




        #region GrandJackpot

        //自定义父节点：
        GameObject grandParent = new GameObject();
        tempObjList.Add(grandParent);
        grandParent.AddComponent<VerticalLayoutGroup>();
        grandParent.transform.parent = transform;
        grandParent.transform.localScale = Vector3.one;
        var grandRect = grandParent.GetComponent<RectTransform>();
        grandRect.anchoredPosition = new Vector2(450, -150);
        grandRect.sizeDelta = new Vector2(550, 300);

        List<IOBaseSection> grandList = new List<IOBaseSection>();

        grandList.Add(
        InstantiateBaseSection(IOParams.grandBaseValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].BaseValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.grandBaseValue - offsetIndex, IOSectionState.grandBaseValue); }, ioParams: IOParams.grandBaseValue, style: 2, parent: grandParent.transform));

        grandList.Add(
        InstantiateBaseSection(IOParams.grandMinTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MinTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.grandMinTriggerValue - offsetIndex, IOSectionState.grandMinTriggerValue); }, ioParams: IOParams.grandMinTriggerValue, style: 2, parent: grandParent.transform));

        grandList.Add(
        InstantiateBaseSection(IOParams.grandMaxTriggerValue.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MaxTriggerValue / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.grandMaxTriggerValue - offsetIndex, IOSectionState.grandMaxTriggerValue); }, ioParams: IOParams.grandMaxTriggerValue, style: 2, parent: grandParent.transform));


        // 这里不加如数组
        grandList.Add(
        InstantiateBaseSection(IOParams.grandWeight.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].Weight / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.grandWeight - offsetIndex, IOSectionState.grandWeight); }, ioParams: IOParams.grandWeight, style: 2, parent: grandParent.transform));

        grandList.Add(
        InstantiateBaseSection(IOParams.grandMinBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MinBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.grandMinBet - offsetIndex, IOSectionState.grandMinBet); }, ioParams: IOParams.grandMinBet, style: 2, parent: grandParent.transform));

        grandList.Add(
        InstantiateBaseSection(IOParams.grandMaxBet.ToString(), IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MaxBet / 100, fontSize: 30,
            onClick: () => { OnSectionClick((int)IOParams.grandMaxBet - offsetIndex, IOSectionState.grandMaxBet); }, ioParams: IOParams.grandMaxBet, style: 2, parent: grandParent.transform));



        #endregion

        paramsSectionList.Add(grandList);
        paramsSectionList.Add(majorList);
        paramsSectionList.Add(minorList);
        paramsSectionList.Add(miniList);



        //InstantiateBlank();
        //InstantiateBlank();
        GameObject parent = new GameObject();
        tempObjList.Add(parent);
        parent.AddComponent<HorizontalLayoutGroup>();
        parent.transform.parent = transform;


#if OLD_MENU_RECTTRANSFORM_1

        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.anchoredPosition = new Vector2(0, -430);
        parentRect.sizeDelta = new Vector2(975, 100);
        parentRect.localScale = Vector3.one;
#else

        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.anchoredPosition = new Vector2(0, 60);
        parentRect.sizeDelta = new Vector2(975, 100);
        parentRect.localScale = Vector3.one;
        parentRect.anchorMin = new Vector2(0.5f, 0);
        parentRect.anchorMax = new Vector2(0.5f, 0);
        parentRect.pivot = new Vector2(0.5f, 0);
#endif




        InstantiateBaseBtn("Save", SaveJackpotConfig, parent: parent.transform, style: 2);
        InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2);
        curSelect = 0;
        SetCurSelect();


        // 设置当前页状态
        State = IOState.JackpotParams;
    }


    private void SaveJackpotConfig()
    {
        //为避免小数做的特殊处理
        IOCanvasModel.Instance.tempJackpotCfgData.TotalWeight = 0;
        for (int i = 0; i < IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem.Length; i++)
            IOCanvasModel.Instance.tempJackpotCfgData.TotalWeight += IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[i].Weight;

        //Debug.LogError(JsonConvert.SerializeObject(IOCanvasModel.Instance.tempJackpotCfgData));
        SBoxIdea.JackpotWriteConfig(IOCanvasModel.Instance.tempJackpotCfgData);
    }




    private void SetJackpotParamsGridLayout()
    {


#if OLD_MENU_RECTTRANSFORM_1

        gridLayout.cellSize = new Vector2(245, 0f);
        gridLayout.padding = new RectOffset(0, 0, 30, 0);
        gridLayoutRect = menuPanel.GetComponent<RectTransform>();
        gridLayoutRect.anchorMin = new Vector2(0.5f, 0);
        gridLayoutRect.anchorMax = new Vector2(0.5f, 0);
        gridLayoutRect.sizeDelta = new Vector2(1580f, 866f);
        gridLayoutRect.anchoredPosition = new Vector2(0, 530f);
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.spacing = new Vector2(0f, 0);
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;

#else


        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.spacing = new Vector2(0, 0);
        gridLayout.cellSize = new Vector2(300, 80);
        gridLayout.padding = new RectOffset(0, 0, 110, 100);  //RectOffset(左, 右, 上, 下);

        // 设置锚点：顶部拉伸
        gridLayoutRect.anchorMin = new Vector2(0, 0);
        gridLayoutRect.anchorMax = new Vector2(1, 1);
        // 四个边偏移 = 0 → left=0, right=0, top=0, bottom=0
        gridLayoutRect.offsetMin = new Vector2(0, 0);
        gridLayoutRect.offsetMax = new Vector2(0, 0);
        // Y 位置 = -610
        gridLayoutRect.pivot = new Vector2(0.5f, 0.5f);

#endif




    }


    /// <summary>
    /// 修改临时参数时，刷新UI
    /// </summary>
    public void UpdateParamsPanle()
    {
        //Debug.LogError($"UpdateParamsPanle:  jackpotData = {JsonConvert.SerializeObject(IOCanvasModel.Instance.tempJackpotCfgData)}  =");
        for (int i = 0; i < paramsSectionList.Count; i++)
        {
            //Debug.LogError($" paramsSectionList[{i}].Count  = {paramsSectionList[i].Count}");

            var jackpotData = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[i];
            paramsSectionList[i][0].contentText.text = (jackpotData.BaseValue / 100f).ToString();
            paramsSectionList[i][1].contentText.text = (jackpotData.MinTriggerValue / 100f).ToString();
            paramsSectionList[i][2].contentText.text = (jackpotData.MaxTriggerValue / 100f).ToString();
            paramsSectionList[i][3].contentText.text = (jackpotData.Weight / 100f).ToString();
            paramsSectionList[i][4].contentText.text = (jackpotData.MinBet / 100f).ToString();
            paramsSectionList[i][5].contentText.text = (jackpotData.MaxBet / 100f).ToString();
        }
    }






    /// <summary>
    /// 参数保存后-刷新UI时时调用
    /// </summary>
    private void RefresJackpotParamsPanelAtSave()
    {
        for (int i = 0; i < selectionList.Count; i++)
        {
            var selection = selectionList[i];

            int offsetIdx = (int)IOParams.miniBaseValue;

            switch (i + offsetIdx)
            {
                case (int)IOParams.miniBaseValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].BaseValue / 100;
                    break;
                case (int)IOParams.miniMinTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MinTriggerValue / 100;
                    break;
                case (int)IOParams.miniMaxTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MaxTriggerValue / 100;
                    break;
                case (int)IOParams.miniWeight:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].Weight / 100;
                    break;
                case (int)IOParams.miniMinBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MinBet / 100;
                    break;
                case (int)IOParams.miniMaxBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MaxBet / 100;
                    break;

                case (int)IOParams.minorBaseValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].BaseValue / 100;
                    break;
                case (int)IOParams.minorMinTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MinTriggerValue / 100;
                    break;
                case (int)IOParams.minorMaxTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MaxTriggerValue / 100;
                    break;
                case (int)IOParams.minorWeight:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].Weight / 100;
                    break;
                case (int)IOParams.minorMinBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MinBet / 100;
                    break;
                case (int)IOParams.minorMaxBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MaxBet / 100;
                    break;

                case (int)IOParams.majorBaseValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].BaseValue / 100;
                    break;
                case (int)IOParams.majorMinTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MinTriggerValue / 100;
                    break;
                case (int)IOParams.majorMaxTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MaxTriggerValue / 100;
                    break;
                case (int)IOParams.majorWeight:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].Weight / 100;
                    break;
                case (int)IOParams.majorMinBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MinBet / 100;
                    break;
                case (int)IOParams.majorMaxBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MaxBet / 100;
                    break;

                case (int)IOParams.grandBaseValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].BaseValue / 100;
                    break;
                case (int)IOParams.grandMinTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MinTriggerValue / 100;
                    break;
                case (int)IOParams.grandMaxTriggerValue:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MaxTriggerValue / 100;
                    break;
                case (int)IOParams.grandWeight:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].Weight / 100;
                    break;
                case (int)IOParams.grandMinBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MinBet / 100;
                    break;
                case (int)IOParams.grandMaxBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MaxBet / 100;
                    break;
            }
        }
    }




}
