using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class IOCanvasView : MonoSingleton<IOCanvasView>
{
    private Coroutine pressCorutine;
    private bool pressFlag;
    private bool pressing;

    private int curPage;
    private int maxPage;
    private int verCurSelect;
    private int horCurSelect;
    public const int PAGE_SIZE = 10;
    public const int MAX_HOR_SELECT = 2;
    private List<IOClientUSNDataSection> clientDataSections = new List<IOClientUSNDataSection>();

    private void OnKeyDown(KeyCode keyCode)
    {
        IOArrowRect.gameObject.SetActive(false);
        switch (keyCode)
        {
            case KeyCode.W:
                SelectUp();
                break;
            case KeyCode.S:
                SelectDown();
                break;
            case KeyCode.A:
                SelectLeft();
                break;
            case KeyCode.D:
                SelectRight();
                break;
            case KeyCode.F:
                OnConfirm();
                break;
            case KeyCode.G:
                OnCancle();
                break;
        }
    }

    private void OnKeyUp(KeyCode keyCode)
    {
        IOArrowRect.gameObject.SetActive(false);
        switch (keyCode)
        {
            case KeyCode.W:
                KeyUp(true);
                break;
            case KeyCode.S:
                KeyUp(false);
                break;
            case KeyCode.A:
                KeyUp(false);
                break;
            case KeyCode.D:
                KeyUp(true);
                break;

        }
    }

    private void OnHardwareKeyDown(ulong keyCode)
    {
        IOArrowRect.gameObject.SetActive(false);
        switch (keyCode)
        {
            case SBOX_SWITCH.SWITCH_KEYBOARD_UP:
                SelectUp();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_DOWN:
                SelectDown();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_LEFT:
                SelectLeft();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_RIGHT:
                SelectRight();
                break;
        }
    }

    private void OnHardwareKeyUp(ulong keyCode)
    {
        IOArrowRect.gameObject.SetActive(false);
        switch (keyCode)
        {
            case SBOX_SWITCH.SWITCH_KEYBOARD_UP:
                KeyUp(true);
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_DOWN:
                KeyUp(false);
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_LEFT:
                KeyUp(false);
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_RIGHT:
                KeyUp(true);
                break;
        }
    }

    private void OnHardwareClick(ulong keyCode)
    {
        IOArrowRect.gameObject.SetActive(false);
        switch (keyCode)
        {
            case SBOX_SWITCH.SWITCH_KEYBOARD_CONFIRM:
                OnConfirm();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_CANCLE:
                OnCancle();
                break;
        }
    }

    private void OnPasswordCurSelect(int curSelect)
    {
        if (selectionList.Count < 1)
            return;
        this.curSelect = curSelect;
        SetCurSelect(this.curSelect);
    }

    private void OnPasswordInput(int num)
    {
        string str = selectSection.baseSection.Content;
        str += num.ToString();
        selectSection.baseSection.Content = str;
    }

    private void OnPasswordDelete()
    {
        string str = selectSection.baseSection.Content;
        if (!string.IsNullOrEmpty(str))
        {
            str = str.Substring(0, str.Length - 1);
            selectSection.baseSection.Content = str;
        }
    }

    private void OnPasswordConfirm()
    {
        Debug.LogError("OnPasswordConfirm");

        string str = selectSection.baseSection.Content;
        if (string.IsNullOrEmpty(str))
            selectSection.baseSection.Content = "0";

        var section = selectionList[curSelect] as IOBaseSection;
        if (section != null)
        {
            section.Selected = !section.Selected;
            selectSection.selected = section.Selected;
            selectSection.baseSection = section;
            if (!selectSection.selected)
                SetCurSelect(curSelect);
            passwordPanel.Focuse(false);
            switch (selectSection.state)
            {
                case IOSectionState.LineId:
                    break;
                case IOSectionState.MacId:
                    break;
                case IOSectionState.ClientWinLock:
                    break;
                case IOSectionState.EditPassword:
                    break;
                default:
                    break;
            }
        }

    }

    private void OnRefreshIOCanvas()
    {
        Debug.LogError("OnRefreshIOCanvas");
        switch (State)
        {
            case IOState.Params:
                RefreshParamsPanel();
                break;
            case IOState.Bill:
            case IOState.Code:
                RefreshDiffcultyShow();
                break;
            case IOState.ProSetting:
                RefreshProSetting();
                break;
            case IOState.JackpotSetting:
                RefreshJackpotSetting();
                break;
            default:
                break;
        }
    }

    private void RefreshParamsPanel()
    {

        Debug.LogError($"@1 groupId: {IOCanvasModel.Instance.groupId}  tempGroupId: {IOCanvasModel.Instance.tempGroupId}");
        // ÉèÖÃ
        IOCanvasModel.Instance.groupId = IOCanvasModel.Instance.tempGroupId;

        Debug.LogError($"@2 groupId: {IOCanvasModel.Instance.groupId}  tempGroupId: {IOCanvasModel.Instance.tempGroupId}");

        for (int i = 0; i < selectionList.Count; i++)
        {
            var selection = selectionList[i];
            switch (i)
            {
                /*#seaweed# 
                case (int)IOParams.CountDown:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.CountDown;
                    break;
                case (int)IOParams.MinBet:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.MinBet;
                    break;
                */
                case (int)IOParams.CoinRatio:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.CoinValue;
                    break;
                case (int)IOParams.TicketRatio:
                    (selection as IOTicketRatioSection).SetCurIndex();
                    break;
                /*#seaweed# 
                case (int)IOParams.RefundMode:
                    (selection as IOBaseSection).CurIndex = (int)IOCanvasModel.Instance.tempCfgData.TicketMode;
                    break;
                case (int)IOParams.SkillMode:
                    (selection as IOSwitchSection).UpdateSwitch();
                    break;
                case (int)IOParams.ClientWinLock:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.PlayerWinLock;
                    break;
                case (int)IOParams.OffsetRatio:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.PulseValue;
                    break;
                case (int)IOParams.JackpotLimit:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.BetsMinOfJackpot;
                    break;
                */
                //case (int)IOParams.JackpotLevel:
                //    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempCfgData.JackpotLevel;
                //    break;
                case (int)IOParams.WaveGameCount:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.waveGameCount;
                    break;
            }
        }
    }

    private void RefreshProSetting()
    {
        if (IOCanvasModel.Instance.CfgData.MachineIdLock == 0 || IOCanvasModel.Instance.permissions == 3)
        {
            if (selectSection.selected)
            {
                IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
                int data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
                selectSection.baseSection.Content = data.ToString();
                SetCurSelect(selectSection.state == IOSectionState.EditLineId ? 0 : 1);
                selectSection.selected = false;
            }
        }
        else
            ReturnToFunction();
    }

    private void RefreshJackpotSetting()
    {
        for (int i = 0; i < selectionList.Count; i++)
        {
            var selection = selectionList[i];
            switch (i)
            {
                case (int)IOJackpotSetting.JackpotSwitch:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfg.jackpotSwitch;
                    break;
                case (int)IOJackpotSetting.JpPercent:
                    (selection as IOBaseSection).CurIndex = IOCanvasModel.Instance.tempJackpotCfg.jpPercent;
                    break;
            }
        }
    }

    public void EditPassword()
    {
        switch (selectSection.state)
        {
            case IOSectionState.EditPassword:
                passwordPanel.SetPlaceholderText(Utils.GetLanguage("Enter new password"));
                selectSection.state = IOSectionState.NewPassword;
                break;
            default:
                break;
        }
    }

    private void SelectUp()
    {
        if (!selectSection.selected)
        {
            if (State != IOState.Code &&
                 State != IOState.EditPassword &&
                 State != IOState.ProSetting)
            {
                int temp = curSelect - 1;
                if (temp > -1)
                    SetCurSelect(temp);
                else if (selectionList.Count > 0)
                {
                    temp = selectionList.Count - 1;
                    SetCurSelect(temp);
                }
            }
            else if (State == IOState.ProSetting)
            {
                if (IOCanvasModel.Instance.permissions == 3)
                {
                    int temp = curSelect - 1;
                    if (temp > 1 && temp < selectionList.Count - 1)
                        SetCurSelect(1);
                    else if (temp < 0)
                        SetCurSelect(selectionList.Count - 1);
                    else
                        SetCurSelect(temp);
                }
                else
                {
                    int temp = curSelect - 1;
                    if (temp > 1 && temp < selectionList.Count - 1)
                        SetCurSelect(0);
                    else if (temp < 0)
                        SetCurSelect(selectionList.Count - 1);
                    else
                        SetCurSelect(temp);
                }
            }
            else
            {
                if (curSelect == 12)
                    SetCurSelect(curSelect - 2);
            }
        }
        else
        {
            int temp;
            switch (selectSection.state)
            {
                case IOSectionState.DateTime:
                    if (curSelect > 7)
                    {
                        temp = curSelect - 1;
                        SetCurSelect(temp);
                        if (temp == 7)
                            selectSection.baseSection.CurIndex = 4;
                    }
                    else
                    {
                        if (selectSection.baseSection.CurIndex - 1 > -1)
                            selectSection.baseSection.CurIndex--;
                    }
                    break;
                case IOSectionState.SkillMode:
                    selectSection.baseSection.CurIndex--;
                    break;
                case IOSectionState.EditLineId:
                case IOSectionState.EditMacId:
                    int minSelect = IOCanvasModel.Instance.permissions == 3 ? 1 : 0;
                    temp = curSelect - 3;
                    if (curSelect == selectionList.Count - 1)
                        SetCurSelect(selectionList.Count - 3, false);
                    else if (temp > minSelect)
                        SetCurSelect(temp, false);
                    break;
            }
        }
    }

    private void SelectDown()
    {
        if (!selectSection.selected)
        {
            if (State != IOState.Code &&
                State != IOState.EditPassword &&
                State != IOState.ProSetting)
            {
                int temp = curSelect + 1;
                if (temp < selectionList.Count)
                    SetCurSelect(temp);
                else if (selectionList.Count > 0)
                    SetCurSelect();
            }
            else if (State == IOState.ProSetting)
            {
                if (IOCanvasModel.Instance.permissions == 3)
                {
                    int temp = curSelect + 1;
                    if (temp > 1 && temp < selectionList.Count - 1)
                        SetCurSelect(selectionList.Count - 1);
                    else if (temp > selectionList.Count - 1)
                        SetCurSelect();
                    else
                        SetCurSelect(temp);
                }
                else
                {
                    int temp = curSelect + 1;
                    if (temp > 0 && temp < selectionList.Count - 1)
                        SetCurSelect(selectionList.Count - 1);
                    else if (temp > selectionList.Count - 1)
                        SetCurSelect();
                    else
                        SetCurSelect(temp);
                }
            }
            else
            {
                if (curSelect > 8 && curSelect < 12)
                    SetCurSelect(12);
            }
        }
        else
        {
            int temp;
            switch (selectSection.state)
            {
                case IOSectionState.DateTime:
                    if (selectSection.baseSection.CurIndex + 1 > 4)
                    {
                        temp = curSelect + 1;
                        if (temp < selectionList.Count)
                            SetCurSelect(temp);
                    }
                    else
                        selectSection.baseSection.CurIndex++;
                    break;
                case IOSectionState.SkillMode:
                    selectSection.baseSection.CurIndex++;
                    break;
                case IOSectionState.EditLineId:
                case IOSectionState.EditMacId:
                    temp = curSelect + 3;
                    if (temp > selectionList.Count - 1)
                        SetCurSelect(selectionList.Count - 1, false);
                    else
                        SetCurSelect(temp, false);
                    break;
            }
        }
    }

    private void SelectLeft()
    {
        if (selectSection.selected)
            SelectedSectionDownAndLeft(true);
    }

    private void SelectRight()
    {
        if (selectSection.selected)
            SelectedSectionUpAndRight(true);
    }

    private void SelectedSectionUpAndRight(bool changeSkillValue)
    {
        int temp = 0;
        switch (selectSection.state)
        {
            case IOSectionState.NewGameMode:
                ValueChangeLoop(true, ref IOCanvasModel.Instance.tempCfgData.NewGameMode, Enum.GetValues(typeof(IONewGameMode)).Cast<int>().Min(), Enum.GetValues(typeof(IONewGameMode)).Cast<int>().Max());
                break;
            case IOSectionState.CountDown:
                pressCorutine = StartCoroutine(PressEnumerator(
               () => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.CountDown, IOCanvasModel.MIN_COUNT_DOWN, IOCanvasModel.MAX_COUNT_DOWN); }));
                break;
            case IOSectionState.MinBet:
                pressCorutine = StartCoroutine(PressEnumerator(
                (int offset) => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.MinBet, IOCanvasModel.MIN_MIN_BET, IOCanvasModel.MAX_MIN_BET, offset); }, deffaultOffset: 10));
                break;
            case IOSectionState.LimitBetsWins:
                pressCorutine = StartCoroutine(PressEnumerator(
                (int offset) => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.LimitBetsWins, IOCanvasModel.MIN_LIMIT_BETS_WINS, IOCanvasModel.MAX_LIMIT_BETS_WINS, offset); }, deffaultOffset: 10));
                break;
            case IOSectionState.CoinRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.CoinValue, IOCanvasModel.MIN_COIN_RATIO, IOCanvasModel.MAX_COIN_RATIO); }));
                break;
            case IOSectionState.TicketRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => {
                    if (IOCanvasModel.Instance.tempCfgData.TicketValue > 1)
                    {
                        IOCanvasModel.Instance.tempCfgData.TicketValue = IOCanvasModel.Instance.tempCfgData.TicketValue + 1 > IOCanvasModel.MAX_TICKET_VALUE ?
                        IOCanvasModel.Instance.tempCfgData.TicketValue : IOCanvasModel.Instance.tempCfgData.TicketValue + 1;
                    }
                    else
                    {
                        if (IOCanvasModel.Instance.tempCfgData.scoreTicket - 1 < 1)
                            IOCanvasModel.Instance.tempCfgData.TicketValue = 2;
                        else
                            IOCanvasModel.Instance.tempCfgData.scoreTicket--;
                    }
                    (selectSection.baseSection as IOTicketRatioSection).SetCurIndex();
                }));
                break;
            case IOSectionState.RefundMode:
                ValueChangeLoop(true, ref IOCanvasModel.Instance.tempCfgData.TicketMode, Enum.GetValues(typeof(IORefundMode)).Cast<int>().Min(), Enum.GetValues(typeof(IORefundMode)).Cast<int>().Max());
                break;
            case IOSectionState.SkillMode:
                if (changeSkillValue)
                    pressCorutine = StartCoroutine(PressEnumerator(
                    () =>
                    {
                        SkillValueChange(true,
                        IOCanvasModel.Instance.switchList,
                        selectSection.baseSection.CurIndex,
                        IOCanvasModel.Instance.SWITCH_LIMIT_MIN[selectSection.baseSection.CurIndex],
                        IOCanvasModel.Instance.SWITCH_LIMIT_MAX[selectSection.baseSection.CurIndex],
                        EventHandle.UPDATE_SWITCH);
                    }));
                else
                    selectSection.baseSection.CurIndex++;
                break;
            case IOSectionState.ScoreUpRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.ScoreUpUnit, IOCanvasModel.MIN_SCORE_UP_RATIO, IOCanvasModel.MAX_SCORE_UP_RATIO, offset: 1000); }));
                break;
            case IOSectionState.RecordMode:
                ValueChangeLoop(true, ref IOCanvasModel.Instance.tempCfgData.PrintMode, Enum.GetValues(typeof(IORecordMode)).Cast<int>().Min(), Enum.GetValues(typeof(IORecordMode)).Cast<int>().Max());
                break;
            case IOSectionState.ClientWinLock:
                pressCorutine = StartCoroutine(PressEnumerator(
                (int offset) => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.PlayerWinLock, IOCanvasModel.MIN_CLIENT_WIN_LOCK, IOCanvasModel.MAX_CLIENT_WIN_LOCK, offset); }, deffaultOffset: 1000));
                break;
            case IOSectionState.OffsetRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.PulseValue, IOCanvasModel.MIN_OFFSET_RATIO, IOCanvasModel.MAX_OFFSET_RATIO, 10); }));
                break;
            case IOSectionState.LEDBrightness:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { /*LEDBrightnessValueChange(true);*/ }));
                break;
            case IOSectionState.SoundVolumScale:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { SoundVolumnValueChange(true); }));
                break;
            case IOSectionState.JackpotLimit:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.BetsMinOfJackpot, IOCanvasModel.MIN_BETS_MIN_OF_JACKPOT, IOCanvasModel.MAX_BETS_MIN_OF_JACKPOT, offset: 50); }));
                break;
            //case IOSectionState.JackpotLevel:
            //    pressCorutine = StartCoroutine(PressEnumerator(
            //    () => { ValueChange(true, ref IOCanvasModel.Instance.tempCfgData.JackpotLevel, IOCanvasModel.MIN_JACKPOT_LEVEL, IOCanvasModel.MAX_JACKPOT_LEVEL, offset: 1); }));
            //    break;
            case IOSectionState.WaveGameCount:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(true, ref IOCanvasModel.Instance.tempWaveGamecout, IOCanvasModel.MIN_WAVE_GAME_COUNT, IOCanvasModel.MAX_WAVE_GAME_COUNT, offset: 1); }));
                break;
            case IOSectionState.DateTime:
                pressCorutine = StartCoroutine(PressEnumerator(
                    () =>
                    {
                        DateTimeValueChange(true,
                        selectSection.baseSection.CurIndex,
                        EventHandle.UPDATE_DATE_TIME);
                    }));
                break;
            case IOSectionState.Language:
                temp = (int)IOCanvasModel.Instance.curlanguage;
                ValueChangeLoop(true, ref temp, Enum.GetValues(typeof(Language)).Cast<int>().Min(), Enum.GetValues(typeof(Language)).Cast<int>().Max());
                IOCanvasModel.Instance.curlanguage = (Language)(Enum.Parse(typeof(Language), temp.ToString()));
                RefreshFunctionText();
                break;
            case IOSectionState.EditLineId:
            case IOSectionState.EditMacId:
                temp = curSelect + 1;
                if (temp < selectionList.Count)
                    SetCurSelect(temp, false);
                break;
            case IOSectionState.JackpotSwitch:
                ValueChangeLoop(true, ref IOCanvasModel.Instance.tempJackpotCfg.jackpotSwitch, Enum.GetValues(typeof(IOJackpotSwitch)).Cast<int>().Min(), Enum.GetValues(typeof(IOJackpotSwitch)).Cast<int>().Max());
                break;
            case IOSectionState.JackpotPercent:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(true, ref IOCanvasModel.Instance.tempJackpotCfg.jpPercent, IOCanvasModel.MIN_JACKPOT_PERCENT, IOCanvasModel.MAX_JACKPOT_PERCENT); }));
                break;

            default:
                break;
        }
    }

    private void SelectedSectionDownAndLeft(bool vertical)
    {
        int temp = 0;
        switch (selectSection.state)
        {
            case IOSectionState.NewGameMode:
                ValueChangeLoop(false, ref IOCanvasModel.Instance.tempCfgData.NewGameMode, Enum.GetValues(typeof(IONewGameMode)).Cast<int>().Min(), Enum.GetValues(typeof(IONewGameMode)).Cast<int>().Max());
                break;
            case IOSectionState.CountDown:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.CountDown, IOCanvasModel.MIN_COUNT_DOWN, IOCanvasModel.MAX_COUNT_DOWN); }));
                break;
            case IOSectionState.MinBet:
                pressCorutine = StartCoroutine(PressEnumerator(
                (int offset) => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.MinBet, IOCanvasModel.MIN_MIN_BET, IOCanvasModel.MAX_MIN_BET, offset); }, deffaultOffset: 10));
                break;
            case IOSectionState.LimitBetsWins:
                pressCorutine = StartCoroutine(PressEnumerator(
                (int offset) => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.LimitBetsWins, IOCanvasModel.MIN_LIMIT_BETS_WINS, IOCanvasModel.MAX_LIMIT_BETS_WINS, offset); }, 10));
                break;
            case IOSectionState.CoinRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.CoinValue, IOCanvasModel.MIN_COIN_RATIO, IOCanvasModel.MAX_COIN_RATIO); }));
                break;
            case IOSectionState.TicketRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => {
                    if (IOCanvasModel.Instance.tempCfgData.TicketValue > 1)
                        IOCanvasModel.Instance.tempCfgData.TicketValue--;
                    else
                    {
                        IOCanvasModel.Instance.tempCfgData.scoreTicket = IOCanvasModel.Instance.tempCfgData.scoreTicket + 1 > IOCanvasModel.MAX_VALUE_TICKET ?
                        IOCanvasModel.MAX_VALUE_TICKET : IOCanvasModel.Instance.tempCfgData.scoreTicket + 1;
                    }
                    (selectSection.baseSection as IOTicketRatioSection).SetCurIndex();
                }));
                break;
            case IOSectionState.RefundMode:
                ValueChangeLoop(false, ref IOCanvasModel.Instance.tempCfgData.TicketMode, Enum.GetValues(typeof(IORefundMode)).Cast<int>().Min(), Enum.GetValues(typeof(IORefundMode)).Cast<int>().Max());
                break;
            case IOSectionState.SkillMode:
                if (vertical)
                    pressCorutine = StartCoroutine(PressEnumerator(
                        () =>
                        {
                            SkillValueChange(false,
                            IOCanvasModel.Instance.switchList,
                            selectSection.baseSection.CurIndex,
                            IOCanvasModel.Instance.SWITCH_LIMIT_MIN[selectSection.baseSection.CurIndex],
                            IOCanvasModel.Instance.SWITCH_LIMIT_MAX[selectSection.baseSection.CurIndex],
                            EventHandle.UPDATE_SWITCH);
                        }));
                else
                    selectSection.baseSection.CurIndex--;
                break;
            case IOSectionState.ScoreUpRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.ScoreUpUnit, IOCanvasModel.MIN_SCORE_UP_RATIO, IOCanvasModel.MAX_SCORE_UP_RATIO, offset: 1000); }));
                break;
            //case IOSectionState.PrinterMode:
            //    ValueChangeLoop(false, ref IOCanvasModel.Instance.tempCfgData.PrintMode, Enum.GetValues(typeof(IOPrinterMode)).Cast<int>().Min(), Enum.GetValues(typeof(IOPrinterMode)).Cast<int>().Max());
            //    break;
            case IOSectionState.RecordMode:
                ValueChangeLoop(false, ref IOCanvasModel.Instance.tempCfgData.ShowMode, Enum.GetValues(typeof(IORecordMode)).Cast<int>().Min(), Enum.GetValues(typeof(IORecordMode)).Cast<int>().Max());
                break;
            //case IOSectionState.PrintTime:
            //    pressCorutine = StartCoroutine(PressEnumerator(
            //    () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.CheckTime, IOCanvasModel.MIN_PRINT_TIME, IOCanvasModel.MAX_PRINT_TIME); }));
            //    break;
            //case IOSectionState.PrintDensity:
            //    pressCorutine = StartCoroutine(PressEnumerator(
            //    (int offset) => { ValueChangeLoop(false, ref IOCanvasModel.Instance.tempCfgData.PrintLevel, IOCanvasModel.MIN_PRINT_DENSITY, IOCanvasModel.MAX_PRINT_DENSITY, offset); }));
            //    break;
            case IOSectionState.ClientWinLock:
                pressCorutine = StartCoroutine(PressEnumerator(
                (int offset) => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.PlayerWinLock, IOCanvasModel.MIN_CLIENT_WIN_LOCK, IOCanvasModel.MAX_CLIENT_WIN_LOCK, offset); }, deffaultOffset: 1000));
                break;
            //case IOSectionState.RoundWinLock:
            //    pressCorutine = StartCoroutine(PressEnumerator(
            //    (int offset) => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.LostLock, IOCanvasModel.MIN_CLIENT_WIN_LOCK, IOCanvasModel.MAX_CLIENT_WIN_LOCK, offset); }, deffaultOffset: 10));
            //    break;
            //case IOSectionState.NetSwitch:
            //    ValueChangeLoop(false, ref IOCanvasModel.Instance.tempNetSwitch, Enum.GetValues(typeof(IONetSwitch)).Cast<int>().Min(), Enum.GetValues(typeof(IONetSwitch)).Cast<int>().Max());
            //    break;
            case IOSectionState.OffsetRatio:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.PulseValue, IOCanvasModel.MIN_OFFSET_RATIO, IOCanvasModel.MAX_OFFSET_RATIO, 10); }));
                break;
            case IOSectionState.DateTime:
                pressCorutine = StartCoroutine(PressEnumerator(
                () =>
                {
                    DateTimeValueChange(false,
                    selectSection.baseSection.CurIndex,
                    EventHandle.UPDATE_DATE_TIME);
                }));
                break;

            case IOSectionState.Language:
                temp = (int)IOCanvasModel.Instance.curlanguage;
                ValueChangeLoop(false, ref temp, Enum.GetValues(typeof(Language)).Cast<int>().Min(), Enum.GetValues(typeof(Language)).Cast<int>().Max());
                IOCanvasModel.Instance.curlanguage = (Language)(Enum.Parse(typeof(Language), temp.ToString()));
                RefreshFunctionText();
                break;
            case IOSectionState.EditLineId:
            case IOSectionState.EditMacId:
                temp = curSelect - 1;
                if (temp > 1)
                    SetCurSelect(temp, false);
                break;
            case IOSectionState.JackpotSwitch:
                ValueChangeLoop(false, ref IOCanvasModel.Instance.tempJackpotCfg.jackpotSwitch, Enum.GetValues(typeof(IOJackpotSwitch)).Cast<int>().Min(), Enum.GetValues(typeof(IOJackpotSwitch)).Cast<int>().Max());
                break;
            case IOSectionState.JackpotPercent:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempJackpotCfg.jpPercent, IOCanvasModel.MIN_JACKPOT_PERCENT, IOCanvasModel.MAX_JACKPOT_PERCENT); }));
                break;
            case IOSectionState.LEDBrightness:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { /*LEDBrightnessValueChange(false);*/ }));
                break;
            case IOSectionState.SoundVolumScale:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { SoundVolumnValueChange(false); }));
                break;
            case IOSectionState.JackpotLimit:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.BetsMinOfJackpot, IOCanvasModel.MIN_BETS_MIN_OF_JACKPOT, IOCanvasModel.MAX_BETS_MIN_OF_JACKPOT, offset: 50); }));
                break;
            //case IOSectionState.JackpotLevel:
            //    pressCorutine = StartCoroutine(PressEnumerator(
            //    () => { ValueChange(false, ref IOCanvasModel.Instance.tempCfgData.JackpotLevel, IOCanvasModel.MIN_JACKPOT_LEVEL, IOCanvasModel.MAX_JACKPOT_LEVEL, offset: 1); }));
            //    break;
            case IOSectionState.WaveGameCount:
                pressCorutine = StartCoroutine(PressEnumerator(
                () => { ValueChange(false, ref IOCanvasModel.Instance.tempWaveGamecout, IOCanvasModel.MIN_WAVE_GAME_COUNT, IOCanvasModel.MAX_WAVE_GAME_COUNT, offset: 1); }));
                break;
            default:
                break;
        }
    }

    private IEnumerator PressEnumerator(Action onUpdate)
    {
        pressFlag = false;
        pressing = true;
        float pressTime = 0;
        while (pressing)
        {
            pressTime += 0.02f;
            if (!pressFlag && pressTime > 0.2f)
                pressFlag = true;
            if (pressFlag)
                onUpdate?.Invoke();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator PressEnumerator(Action<int> onUpdate, int deffaultOffset = 1)
    {
        pressFlag = false;
        pressing = true;
        float pressTime = 0;
        float step = 0.02f;
        int times = 0;
        int offset;
        while (pressing)
        {
            pressTime += 0.02f;
            if (!pressFlag && pressTime > 0.2f)
                pressFlag = true;
            if (pressFlag)
            {
                step = 0.12f;
                times++;
                if (times < 21)
                {
                    step = 0.06f;
                    offset = deffaultOffset;
                }
                else if (times < 41)
                    offset = deffaultOffset * 10;
                else if (times < 81)
                    offset = deffaultOffset * 100;
                else
                    offset = deffaultOffset * 1000;
                onUpdate?.Invoke(offset);
            }
            yield return new WaitForSeconds(step);
        }
    }

    private void KeyUp(bool isAdd)
    {
        Debug.LogError($"KeyUp isAdd:{isAdd}");

        if (selectSection.state == IOSectionState.GroupId)
        {
            ValueChange(isAdd, ref IOCanvasModel.Instance.tempGroupId, 0, 10);
            return;
        }

        if (!selectSection.selected || pressCorutine == null) return;
        if (!pressFlag)
            switch (selectSection.state)
            {
                case IOSectionState.GroupId:
                    Debug.LogError($"i am GroupId ");
                    break;
                case IOSectionState.NewGameMode:
                    ValueChangeLoop(isAdd, ref IOCanvasModel.Instance.tempCfgData.NewGameMode, Enum.GetValues(typeof(IONewGameMode)).Cast<int>().Min(), Enum.GetValues(typeof(IONewGameMode)).Cast<int>().Max());
                    break;
                case IOSectionState.CountDown:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.CountDown, IOCanvasModel.MIN_COUNT_DOWN, IOCanvasModel.MAX_COUNT_DOWN);
                    break;
                case IOSectionState.MinBet:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.MinBet, IOCanvasModel.MIN_MIN_BET, IOCanvasModel.MAX_MIN_BET, offset: 10);
                    break;
                case IOSectionState.LimitBetsWins:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.LimitBetsWins, IOCanvasModel.MIN_LIMIT_BETS_WINS, IOCanvasModel.MAX_LIMIT_BETS_WINS, offset: 10);
                    break;
                case IOSectionState.CoinRatio:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.CoinValue, IOCanvasModel.MIN_COIN_RATIO, IOCanvasModel.MAX_COIN_RATIO);
                    break;
                case IOSectionState.TicketRatio:
                    if (isAdd)
                    {
                        if (IOCanvasModel.Instance.tempCfgData.TicketValue > 1)
                        {
                            if (IOCanvasModel.Instance.tempCfgData.TicketValue + 1 > IOCanvasModel.MAX_TICKET_VALUE)
                                IOCanvasModel.Instance.tempCfgData.TicketValue = IOCanvasModel.MAX_TICKET_VALUE;
                            else
                                IOCanvasModel.Instance.tempCfgData.TicketValue++;
                        }
                        else
                        {
                            if (IOCanvasModel.Instance.tempCfgData.scoreTicket - 1 < 1)
                                IOCanvasModel.Instance.tempCfgData.TicketValue = 2;
                            else
                                IOCanvasModel.Instance.tempCfgData.scoreTicket--;
                        }
                    }
                    else
                    {
                        if (IOCanvasModel.Instance.tempCfgData.TicketValue > 1)
                            IOCanvasModel.Instance.tempCfgData.TicketValue--;
                        else
                        {
                            IOCanvasModel.Instance.tempCfgData.scoreTicket = IOCanvasModel.Instance.tempCfgData.scoreTicket + 1 > IOCanvasModel.MAX_VALUE_TICKET ?
                            IOCanvasModel.MAX_VALUE_TICKET : IOCanvasModel.Instance.tempCfgData.scoreTicket + 1;
                        }
                    }

                    (selectSection.baseSection as IOTicketRatioSection).SetCurIndex();
                    break;
                case IOSectionState.SkillMode:
                    SkillValueChange(isAdd, IOCanvasModel.Instance.switchList, selectSection.baseSection.CurIndex, IOCanvasModel.Instance.SWITCH_LIMIT_MIN[selectSection.baseSection.CurIndex], IOCanvasModel.Instance.SWITCH_LIMIT_MAX[selectSection.baseSection.CurIndex], EventHandle.UPDATE_SWITCH);
                    break;
                case IOSectionState.ScoreUpRatio:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.ScoreUpUnit, IOCanvasModel.MIN_SCORE_UP_RATIO, IOCanvasModel.MAX_SCORE_UP_RATIO, offset: 1000);
                    break;
                //case IOSectionState.PrinterMode:
                //    ValueChangeLoop(isAdd, ref IOCanvasModel.Instance.tempCfgData.PrintMode, Enum.GetValues(typeof(IOPrinterMode)).Cast<int>().Min(), Enum.GetValues(typeof(IOPrinterMode)).Cast<int>().Max());
                //    break;
                case IOSectionState.RecordMode:
                    ValueChangeLoop(isAdd, ref IOCanvasModel.Instance.tempCfgData.ShowMode, Enum.GetValues(typeof(IORecordMode)).Cast<int>().Min(), Enum.GetValues(typeof(IORecordMode)).Cast<int>().Max());
                    break;
                //case IOSectionState.PrintTime:
                //    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.CheckTime, IOCanvasModel.MIN_PRINT_TIME, IOCanvasModel.MAX_PRINT_TIME);
                //    break;
                //case IOSectionState.PrintDensity:
                //    ValueChangeLoop(isAdd, ref IOCanvasModel.Instance.tempCfgData.PrintLevel, IOCanvasModel.MIN_PRINT_DENSITY, IOCanvasModel.MAX_PRINT_DENSITY);
                //    break;
                case IOSectionState.ClientWinLock:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.PlayerWinLock, IOCanvasModel.MIN_CLIENT_WIN_LOCK, IOCanvasModel.MAX_CLIENT_WIN_LOCK, 1000);
                    break;
                //case IOSectionState.RoundWinLock:
                //    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.LostLock, IOCanvasModel.MIN_ROUND_WIN_LOCK, IOCanvasModel.MAX_ROUND_WIN_LOCK, 10);
                //    break;
                //case IOSectionState.NetSwitch:
                //    ValueChangeLoop(isAdd, ref IOCanvasModel.Instance.tempCfgData.NewGameMode, Enum.GetValues(typeof(IONetSwitch)).Cast<int>().Min(), Enum.GetValues(typeof(IONetSwitch)).Cast<int>().Max());
                //    break;
                case IOSectionState.OffsetRatio:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.PulseValue, IOCanvasModel.MIN_OFFSET_RATIO, IOCanvasModel.MAX_OFFSET_RATIO, 10);
                    break;
                case IOSectionState.DateTime:
                    DateTimeValueChange(isAdd, selectSection.baseSection.CurIndex, EventHandle.UPDATE_DATE_TIME);
                    break;

                case IOSectionState.Language:
                    int temp = (int)IOCanvasModel.Instance.curlanguage;
                    ValueChangeLoop(isAdd, ref temp, Enum.GetValues(typeof(Language)).Cast<int>().Min(), Enum.GetValues(typeof(Language)).Cast<int>().Max());
                    IOCanvasModel.Instance.curlanguage = (Language)(Enum.Parse(typeof(Language), temp.ToString()));
                    RefreshFunctionText();
                    break;
                case IOSectionState.JackpotSwitch:
                    ValueChangeLoop(isAdd, ref IOCanvasModel.Instance.tempJackpotCfg.jackpotSwitch, Enum.GetValues(typeof(IOJackpotSwitch)).Cast<int>().Min(), Enum.GetValues(typeof(IOJackpotSwitch)).Cast<int>().Max());
                    break;
                case IOSectionState.JackpotPercent:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempJackpotCfg.jpPercent, IOCanvasModel.MIN_JACKPOT_PERCENT, IOCanvasModel.MAX_JACKPOT_PERCENT);
                    break;
                case IOSectionState.LEDBrightness:
                    LEDBrightnessValueChange(isAdd);
                    break;
                case IOSectionState.SoundVolumScale:
                    SoundVolumnValueChange(isAdd);
                    break;
                case IOSectionState.JackpotLimit:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.BetsMinOfJackpot, IOCanvasModel.MIN_BETS_MIN_OF_JACKPOT, IOCanvasModel.MAX_BETS_MIN_OF_JACKPOT, offset: 50);
                    break;
                //case IOSectionState.JackpotLevel:
                //    ValueChange(isAdd, ref IOCanvasModel.Instance.tempCfgData.JackpotLevel, IOCanvasModel.MIN_JACKPOT_LEVEL, IOCanvasModel.MAX_JACKPOT_LEVEL, offset: 1);
                //    break;
                case IOSectionState.WaveGameCount:
                    ValueChange(isAdd, ref IOCanvasModel.Instance.tempWaveGamecout, IOCanvasModel.MIN_WAVE_GAME_COUNT, IOCanvasModel.MAX_WAVE_GAME_COUNT, offset: 1);
                    break;
            }
        pressing = false;
        pressFlag = false;
        StopCoroutine(pressCorutine);
        pressCorutine = null;
    }

    private void OnConfirm()
    {
        Debug.LogError("OnConfirm");

        IOBaseSection section;
        switch (State)
        {
            case IOState.CheckPermissions:
                break;
            case IOState.SelectFunction:
                switch (curSelect)
                {
                    case (int)IOFunction.Params:
                        InitParamsPanel();
                        break;
                    case (int)IOFunction.Bill:
                        InitBillPanel();
                        break;
                    case (int)IOFunction.Code:
                        InitCodePanel();
                        break;
                    case (int)IOFunction.EditPassword:
                        InitEditPasswordPanel();
                        break;
                    case (int)IOFunction.Language:
                        ChangeCurLanguage();
                        break;
                    case (int)IOFunction.ModifiedDate:
                        InitModeifedDatePanel();
                        break;
                    case (int)IOFunction.JackpotSetting:
                        InitJackpotSettingPanel();
                        break;
                    case (int)IOFunction.JackpotBet:
                        break;
                    case (int)IOFunction.JackpotWins:
                        break;
                    case (int)IOFunction.ProSetting:
                        if (IOCanvasModel.Instance.CfgData.MachineIdLock == 0 || IOCanvasModel.Instance.permissions == 3)
                            InitProSettingPanel();
                        else
                            ExitIOCanvas();
                        break;
                    case (int)IOFunction.Exit:
                        ExitIOCanvas();
                        break;
                }
                break;
            case IOState.Params:
                section = selectionList[curSelect] as IOBaseSection;
                if (section != null)
                {
                    section.Selected = !section.Selected;
                    selectSection.selected = section.Selected;
                    selectSection.baseSection = section;
                    if (!selectSection.selected)
                        SetCurSelect(curSelect);
                }
                switch (curSelect)
                {
                    /*#seaweed# 
                    case (int)IOParams.CountDown:
                        selectSection.state = IOSectionState.CountDown;
                        break;
                    case (int)IOParams.MinBet:
                        selectSection.state = IOSectionState.MinBet;
                        break;*/
                    case (int)IOParams.CoinRatio:
                        selectSection.state = IOSectionState.CoinRatio;
                        break;
                    case (int)IOParams.TicketRatio:
                        selectSection.state = IOSectionState.TicketRatio;
                        break;
                    /*#seaweed# 
                    case (int)IOParams.RefundMode:
                        selectSection.state = IOSectionState.RefundMode;
                        break;
                    case (int)IOParams.SkillMode:
                        selectSection.state = IOSectionState.SkillMode;
                        break;
                    case (int)IOParams.ClientWinLock:
                        selectSection.state = IOSectionState.ClientWinLock;
                        break;
                    case (int)IOParams.OffsetRatio:
                        selectSection.state = IOSectionState.OffsetRatio;
                        break;
                        */
                    case (int)IOParams.Save:
                        SaveConfig();
                        break;
                    case (int)IOParams.Return:
                        IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
                        ReturnToFunction();
                        break;
                    case (int)IOParams.OpenBox:
                        OpenBox();
                        break;
                    /*#seaweed# 
                    case (int)IOParams.JackpotLimit:
                        selectSection.state = IOSectionState.JackpotLimit;
                        break;
                    */
                    //case (int)IOParams.JackpotLevel:
                    //    selectSection.state = IOSectionState.JackpotLevel;
                    //    break;
                    case (int)IOParams.WaveGameCount:
                        selectSection.state = IOSectionState.WaveGameCount;
                        break;
                }
                break;
            case IOState.Bill:
                switch (curSelect)
                {
                    case (int)IOBill.Clients:
                        InitClientBillPanelNo0();
                        break;
                    case (int)IOBill.Return:
                        ReturnToFunction();
                        break;
                }
                break;
            case IOState.Clients:
                switch (curSelect)
                {
                    case 0:
                        InitClientBillPanelNo1(true);
                        break;
                    case 1:
                        InitClientBillPanelNo2(true);
                        break;
                    case 2:
                        ReturnToFunction();
                        break;
                }
                break;
            case IOState.Code:
                switch (curSelect)
                {
                    case 12:
                        ReturnToFunction();
                        break;
                }
                break;
            case IOState.EditPassword:
                switch (curSelect)
                {
                    case 12:
                        ReturnToFunction();
                        break;
                }
                break;
            case IOState.DateTime:
                if (IOCanvasModel.Instance.CfgData.MachineIdLock == 0 || IOCanvasModel.Instance.permissions == 3)
                    switch (curSelect)
                    {
                        case 9:
                            SaveDateTime();
                            break;
                        case 10:
                            CloseModifiedDatePanel();
                            break;
                    }
                else
                    switch (curSelect)
                    {
                        case 8:
                            SaveDateTime();
                            break;
                        case 9:
                            CloseModifiedDatePanel();
                            break;
                    }
                break;
            case IOState.JackpotSetting:
                section = selectionList[curSelect] as IOBaseSection;
                if (section != null)
                {
                    section.Selected = !section.Selected;
                    selectSection.selected = section.Selected;
                    selectSection.baseSection = section;
                    if (!selectSection.selected)
                        SetCurSelect(curSelect);
                }
                switch (curSelect)
                {
                    case (int)IOJackpotSetting.JackpotSwitch:
                        selectSection.state = IOSectionState.JackpotSwitch;
                        break;
                    case (int)IOJackpotSetting.JpPercent:
                        selectSection.state = IOSectionState.JackpotPercent;
                        break;
                    case (int)IOJackpotSetting.Save:
                        SaveJackpotCfg();
                        break;
                    case (int)IOJackpotSetting.Return:
                        ReturnToFunction();
                        break;
                }
                break;
            case IOState.ProSetting:
                int data;
                string str;
                if (IOCanvasModel.Instance.permissions == 3)
                    switch (curSelect)
                    {
                        case 0:
                        case 1:
                            section = selectionList[curSelect] as IOBaseSection;
                            section.Selected = true;
                            selectSection.selected = true;
                            selectSection.baseSection = section;
                            selectSection.state = curSelect == 0 ? IOSectionState.EditLineId : IOSectionState.EditMacId;
                            SetCurSelect(2, false);
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
                            str = data != 0 ? data.ToString() + (curSelect - 1).ToString() : (curSelect - 1).ToString();
                            if (str.Length > 9) break;
                            selectSection.baseSection.Content = str;
                            if (selectSection.state == IOSectionState.EditLineId)
                                IOCanvasModel.Instance.tempCfgData.LineId = int.Parse(str);
                            else
                                IOCanvasModel.Instance.tempCfgData.MachineId = int.Parse(str);
                            break;
                        case 11:
                            data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
                            str = data.ToString().Substring(0, data.ToString().Length - 1);
                            selectSection.baseSection.Content = str;
                            if (selectSection.state == IOSectionState.EditLineId)
                                IOCanvasModel.Instance.tempCfgData.LineId = string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
                            else
                                IOCanvasModel.Instance.tempCfgData.MachineId = string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
                            break;
                        case 12:
                            data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
                            str = data.ToString() + "0";
                            if (str.Length > 9)break;
                            selectSection.baseSection.Content = str;
                            if (selectSection.state == IOSectionState.EditLineId)
                                IOCanvasModel.Instance.tempCfgData.LineId = int.Parse(str);
                            else
                                IOCanvasModel.Instance.tempCfgData.MachineId = int.Parse(str);
                            break;
                        case 13:
                            SaveConfig();
                            break;
                        case 14:
                            if (selectSection.selected)
                            {
                                IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
                                data = selectSection.state == IOSectionState.EditLineId ? IOCanvasModel.Instance.tempCfgData.LineId : IOCanvasModel.Instance.tempCfgData.MachineId;
                                selectSection.baseSection.Content = data.ToString();
                                selectSection.selected = false;
                                SetCurSelect(selectSection.state == IOSectionState.EditLineId ? 0 : 1);
                            }
                            else
                                ReturnToFunction();
                            break;
                    }
                else
                    switch (curSelect)
                    {
                        case 0:
                            section = selectionList[curSelect] as IOBaseSection;
                            section.Selected = true;
                            selectSection.selected = true;
                            selectSection.baseSection = section;
                            selectSection.state = IOSectionState.EditMacId;
                            SetCurSelect(1, false);
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            data = IOCanvasModel.Instance.tempCfgData.MachineId;
                            str = data != 0 ? data.ToString() + curSelect.ToString() : curSelect.ToString();
                            selectSection.baseSection.Content = str;
                            IOCanvasModel.Instance.tempCfgData.MachineId = int.Parse(str);
                            break;
                        case 10:
                            data = IOCanvasModel.Instance.tempCfgData.MachineId;
                            str = data.ToString().Substring(0, data.ToString().Length - 1);
                            selectSection.baseSection.Content = str;
                            if (selectSection.state == IOSectionState.EditLineId)
                                IOCanvasModel.Instance.tempCfgData.LineId = string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
                            else
                                IOCanvasModel.Instance.tempCfgData.MachineId = string.IsNullOrEmpty(str) ? 0 : int.Parse(str);
                            break;
                        case 11:
                            data = IOCanvasModel.Instance.tempCfgData.MachineId;
                            str = data.ToString() + "0";
                            selectSection.baseSection.Content = str;
                            IOCanvasModel.Instance.tempCfgData.MachineId = int.Parse(str);
                            break;
                        case 12:
                            SaveConfig();
                            break;
                        case 13:
                            if (selectSection.selected)
                            {
                                IOCanvasModel.Instance.SetTempCfgData(IOCanvasModel.Instance.CfgData);
                                data = IOCanvasModel.Instance.tempCfgData.MachineId;
                                selectSection.baseSection.Content = data.ToString();
                                selectSection.selected = false;
                                SetCurSelect(0);
                            }
                            else
                                ReturnToFunction();
                            break;
                    }
                break;
            default:
                break;
        }
    }

    private void OnCancle()
    {
        Debug.LogError("OnCancle");

        if (State == IOState.Params)
        {
            var section = selectionList[curSelect] as IOBaseSection;
            if (section != null)
            {
                section.Selected = !section.Selected;
                selectSection.selected = section.Selected;
                selectSection.baseSection = section;
                if (!selectSection.selected)
                    SetCurSelect(curSelect);
            }
        }
        else if (State == IOState.Code
            || State == IOState.EditPassword)
        {
            ReturnToFunction();
        }
    }

    private void SaveConfig()
    {

        Debug.LogError("i am here SaveConfig");

        IOCanvasModel.Instance.tempCfgData.SwitchBetsUnitMin = IOCanvasModel.Instance.switchList[0];
        IOCanvasModel.Instance.tempCfgData.SwitchBetsUnitMid = IOCanvasModel.Instance.switchList[1];
        IOCanvasModel.Instance.tempCfgData.SwitchBetsUnitMax = IOCanvasModel.Instance.switchList[2];
        //SBoxIdea.WaveGameCount(1, IOCanvasModel.Instance.tempWaveGamecout);
        SBoxIdea.WriteConf(IOCanvasModel.Instance.tempCfgData);
    }

    private void OpenBox()
    {
        SBoxIdea.BattPrinterOpenBox(true, false);
    }

    private void SaveJackpotCfg()
    {
        return;

        PlayerPrefs.SetInt("JackpotSwitch", IOCanvasModel.Instance.tempJackpotCfg.jackpotSwitch);
        PlayerPrefs.SetInt("JackpotBetPercent", IOCanvasModel.Instance.tempJackpotCfg.betPercent);
        PlayerPrefs.SetInt("JackpotPercent", IOCanvasModel.Instance.tempJackpotCfg.jpPercent);
        var jackpotCfg = new JackpotConfig
        {
            jackpotSwitch = PlayerPrefs.GetInt("JackpotSwitch", 0),
            betPercent = PlayerPrefs.GetInt("JackpotBetPercent", 100),
            jpPercent = PlayerPrefs.GetInt("JackpotPercent", 5)
        };
        IOCanvasModel.Instance.jackpotCfg = jackpotCfg;

        if (!ClientWS.Instance.IsConnected && IOCanvasModel.Instance.jackpotCfg.jackpotSwitch == (int)IOJackpotSwitch.JackpotSwitchOn)
            NetMgr.Instance.SetNetAutoConnect(false);
        else if (ClientWS.Instance.IsConnected && IOCanvasModel.Instance.jackpotCfg.jackpotSwitch == (int)IOJackpotSwitch.JackpotSwitchOff)
            ClientWS.Instance.CloseSocket();
        OnRefreshIOCanvas();
        IOPopTips.Instance.ShowTips(Utils.GetLanguage("SaveSucceed"));
    }

    private void SkillValueChange(bool add, List<int> list, int index, int min, int max, string eventName)
    {
        int offset;
        switch (index)
        {
            case 0:
                offset = 10;
                break;
            case 1:
                offset = 50;
                break;
            case 2:
                offset = 100;
                break;
            default:
                offset = 10;
                break;
        }
        list[index] = add ?
        list[index] + offset > max ?
        max : list[index] + offset
        : list[index] - offset < min ?
        min : list[index] - offset;
        list[index] = list[index] == 11 ? 10 : list[index];
        EventCenter.Instance.EventTrigger(eventName);
    }

    private void DateTimeValueChange(bool add, int index, string eventName)
    {
        var dateTime = IOCanvasModel.Instance.IODateTime;
        int value = 0;
        int min = 1;
        int max = 9999;
        switch (index)
        {
            case 0:
                value = dateTime.Year;
                break;
            case 1:
                value = dateTime.Month;
                max = 12;
                break;
            case 2:
                value = dateTime.Day;
                max = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                break;
            case 3:
                value = dateTime.Hour;
                max = 23;
                min = 0;
                break;
            case 4:
                value = dateTime.Minute;
                max = 59;
                min = 0;
                break;
        }

        value = add ? value + 1 > max ? max : value + 1 : value - 1 < min ? min : value - 1;
        switch (index)
        {
            case 0:
                IOCanvasModel.Instance.IODateTime = new DateTime(value, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
                break;
            case 1:
                IOCanvasModel.Instance.IODateTime = new DateTime(dateTime.Year, value, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
                break;
            case 2:
                IOCanvasModel.Instance.IODateTime = new DateTime(dateTime.Year, dateTime.Month, value, dateTime.Hour, dateTime.Minute, 0);
                break;
            case 3:
                IOCanvasModel.Instance.IODateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, value, dateTime.Minute, 0);
                break;
            case 4:
                IOCanvasModel.Instance.IODateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, value, 0);
                break;
        }
        EventCenter.Instance.EventTrigger(eventName);
    }

    private void ValueChangeLoop(bool add, List<int> list, int index, int min, int max, string eventName, int offset = 1)
    {
        list[index] = add ?
        list[index] + offset > max ?
        min : list[index] + offset
        : list[index] - offset < min ?
        max : list[index] - offset;
        EventCenter.Instance.EventTrigger(eventName);
    }

    private void SoundVolumnValueChange(bool add, int offset = 1)
    {
        if (IOCanvasModel.Instance.SoundVolumScale == -1) return;
        IOCanvasModel.Instance.SoundVolumScale = add ?
        IOCanvasModel.Instance.SoundVolumScale + offset > IOCanvasModel.MAX_SOUND_VOLUM_SCALE ?
                IOCanvasModel.MAX_SOUND_VOLUM_SCALE : IOCanvasModel.Instance.SoundVolumScale + offset
                : IOCanvasModel.Instance.SoundVolumScale - offset < IOCanvasModel.MIN_SOUND_VOLUM_SCALE ?
                IOCanvasModel.MIN_SOUND_VOLUM_SCALE : IOCanvasModel.Instance.SoundVolumScale - offset;
        selectSection.baseSection.CurIndex = IOCanvasModel.Instance.SoundVolumScale;
    }

    private void LEDBrightnessValueChange(bool add, int offset = 1)
    {
        if (IOCanvasModel.Instance.LEDBrightness == -1) return;
        IOCanvasModel.Instance.LEDBrightness = add ?
        IOCanvasModel.Instance.LEDBrightness + offset > IOCanvasModel.MAX_LED_BRIGHTNESS ?
                IOCanvasModel.MAX_LED_BRIGHTNESS : IOCanvasModel.Instance.LEDBrightness + offset
                : IOCanvasModel.Instance.LEDBrightness - offset < IOCanvasModel.MIN_LED_BRIGHTNESS ?
                IOCanvasModel.MIN_LED_BRIGHTNESS : IOCanvasModel.Instance.LEDBrightness - offset;
        selectSection.baseSection.CurIndex = IOCanvasModel.Instance.LEDBrightness;
    }

    private void ValueChange(bool add, ref int value, int min, int max, int offset = 1)
    {
        if (value == -1) return;
        if (selectSection.state == IOSectionState.OffsetRatio && value == 1)
        {
            value = add ?
               value + 9 > max ?
               max : value + 9
               : value - offset < min ?
               min : value - offset;
        }
        else if (selectSection.state == IOSectionState.ScoreUpRatio && value == 100)
        {
            value = add ?
               value + 900 > max ?
               max : value + 900
               : value - offset < min ?
               min : value - offset;
        }
        else
        {
            value = add ?
                value + offset > max ?
                max : value + offset
                : value - offset < min ?
                min : value - offset;
            if (offset == 10 && value % 10 != 0)
                value = value / 10 * 10 < min ? min : value / 10 * 10;
        }
        selectSection.baseSection.CurIndex = value;
    }

    private void ValueChangeLoop(bool add, ref int value, int min, int max, int offset = 1)
    {
        if (value == -1) return;
        value = add ?
        value + offset > max ?
        min : value + offset
        : value - offset < min ?
        max : value - offset;
        selectSection.baseSection.CurIndex = value;
    }

    private void OnArrowUpPointerDown()
    {
        arrowUpImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1f);
        SelectedSectionUpAndRight(true);
    }

    private void OnArrowUpPointerUp()
    {
        arrowUpImg.color = Color.white;
        KeyUp(true);
    }

    private void OnArrowDownPointerDown()
    {
        arrowDownImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1f);
        SelectedSectionDownAndLeft(true);

    }

    private void OnArrowDownPointerUp()
    {
        arrowDownImg.color = Color.white;
        KeyUp(false);
    }
}
