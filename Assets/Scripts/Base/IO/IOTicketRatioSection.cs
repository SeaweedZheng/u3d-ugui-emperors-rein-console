using UnityEngine;

public class IOTicketRatioSection : IOBaseSection
{
    public override int CurIndex
    {
        get { return _curIndex; }
        set
        {
            _curIndex = value;
            if (IOCanvasModel.Instance.tempCfgData.TicketValue > 1)
                _curIndex = IOCanvasModel.Instance.tempCfgData.TicketValue - 1;
            else
                _curIndex = -IOCanvasModel.Instance.tempCfgData.scoreTicket + 1;
            UpdataContent();  
        }
    }

    private void UpdataContent()
    {
        if (_curIndex > -1)
            contentText.text = $"{_curIndex + 1} {Utils.GetLanguage("score")} / 1 {Utils.GetLanguage("tickets")}";
        else
            contentText.text = $"1 {Utils.GetLanguage("score")} / {Mathf.Abs(_curIndex) + 1} {Utils.GetLanguage("tickets")}";
    }

    public void SetCurIndex()
    {
        if (IOCanvasModel.Instance.tempCfgData.TicketValue > 1)
            _curIndex = IOCanvasModel.Instance.tempCfgData.TicketValue - 1;
        else
            _curIndex = -IOCanvasModel.Instance.tempCfgData.scoreTicket + 1;
        UpdataContent();
    }
}
