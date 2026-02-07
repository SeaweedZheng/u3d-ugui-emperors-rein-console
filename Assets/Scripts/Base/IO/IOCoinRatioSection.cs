using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOCoinRatioSection : IOBaseSection
{
    public override int CurIndex
    {
        get { return _curIndex; }
        set
        {
            _curIndex = value;
            if (IOCanvasModel.Instance.tempCfgData.CoinValue > 1)
                _curIndex = IOCanvasModel.Instance.tempCfgData.CoinValue - 1;
            else
                _curIndex = -IOCanvasModel.Instance.tempCfgData.ScoreValue + 1;
            UpdataContent();
        }
    }

    private void UpdataContent()
    {
        if (_curIndex > -1)
            contentText.text = $"{_curIndex + 1} {Utils.GetLanguage("Coin")} / 1 {Utils.GetLanguage("score")}";
        else
            contentText.text = $"1 {Utils.GetLanguage("Coin")} / {Mathf.Abs(_curIndex) + 1} {Utils.GetLanguage("score")}";
    }

    public void SetCurIndex()
    {
        if (IOCanvasModel.Instance.tempCfgData.CoinValue > 1)
            _curIndex = IOCanvasModel.Instance.tempCfgData.CoinValue - 1;
        else
            _curIndex = -IOCanvasModel.Instance.tempCfgData.ScoreValue + 1;
        UpdataContent();
    }
}