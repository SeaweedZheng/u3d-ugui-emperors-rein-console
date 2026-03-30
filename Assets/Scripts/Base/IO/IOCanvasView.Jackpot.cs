using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class IOCanvasView
{


    /// <summary>
    /// 彩金参数设置面板
    /// </summary>
    private void InitJackpotParamsPanel()
    {
        title.text = Utils.GetLanguage("Params");
        title.fontStyle = FontStyle.Bold;
        title.fontSize = 78;
        //titleRect.anchoredPosition = new Vector2(0, -50f);
        ClearMenuBtn();
        SetJackpotParamsGridLayout();

        // 参数备份
        IOCanvasModel.Instance.SetTempJackCfgData(IOCanvasModel.Instance.JackCfgData);
    }


    private void SetJackpotParamsGridLayout()
    {
        gridLayout.cellSize = new Vector2(245, 0f);
        gridLayout.padding = new RectOffset(0, 0, 30, 0);
        gridLayoutRect = menuPanel.GetComponent<RectTransform>();
        gridLayoutRect.anchorMin = new Vector2(0.5f, 0);
        gridLayoutRect.anchorMax = new Vector2(0.5f, 0);
        gridLayoutRect.sizeDelta = new Vector2(1580f, 827f);
        gridLayoutRect.anchoredPosition = new Vector2(0, 520f);
        gridLayout.childAlignment = TextAnchor.UpperCenter;
        gridLayout.spacing = new Vector2(0f, 0);
        gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
    }
}
