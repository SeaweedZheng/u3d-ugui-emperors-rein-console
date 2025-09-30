using Newtonsoft.Json;
using SBoxApi;
using System;
using System.Collections.Generic;
using UnityEngine;

public class IOCanvasManager : BaseManager<IOCanvasManager>
{
    public IOCanvasView view;
    public void Init(IOCanvasView view)
    {
        this.view = view;
        IOCanvasModel.Instance.horizontal = true;
        AddEventListener();
    }

    private void AddEventListener()
    {
        EventCenter.Instance.AddEventListener<ulong>(EventHandle.CONFIRM_PASSWORD, OnConfirmPassword);
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CHECK_PASSWORD, OnCheckPassword);
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CHANGE_PASSWORD, OnChangePassword);
        EventCenter.Instance.AddEventListener<int>(SBoxEventHandle.SBOX_SANDBOX_SET_DATETIME, OnSetDate);
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_WRITE_CONF, OnWriteConf);
        EventCenter.Instance.AddEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnReadConfig);
        EventCenter.Instance.AddEventListener<SBoxCoderData>(SBoxEventHandle.SBOX_REQUEST_CODER, OnRequestCoder);
        EventCenter.Instance.AddEventListener<SBoxPermissionsData>(SBoxEventHandle.SBOX_CODER, OnSBoxCode);
        EventCenter.Instance.AddEventListener<DateTime>(SBoxEventHandle.SBOX_SANDBOX_GET_DATETIME, OnSandboxGetDateTime);
    }

    private void OnSandboxGetDateTime(DateTime dateTime)
    {
        IOCanvasModel.Instance.IODateTime = dateTime;
        EventCenter.Instance.EventTrigger(EventHandle.GET_SANDBOX_DATE);
    }

    private void OnSBoxCode(SBoxPermissionsData sBoxPermissionsData)
    {
        Debug.LogError($"OnSBoxCode:{JsonConvert.SerializeObject(sBoxPermissionsData)}");
        if (sBoxPermissionsData.result < 0)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
            return;
        }
        else if (sBoxPermissionsData.result == 1)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("VerificationCodeRequired"));
            return;
        }
        IOPopTips.Instance.ShowTips(Utils.GetLanguage("CodeSucceed"));
        EventCenter.Instance.EventTrigger(EventHandle.CODE_SUCCEED);
        if (sBoxPermissionsData.permissions % 1000 > 0)
        {
            for (int i = 1; i < 11; i++)
            {
                PlayerPrefs.SetInt($"player{i}coinInCount", -1);
                PlayerPrefs.GetInt($"player{i}coinInCount", -1);
            }
            SQLite.Instance.ClearAllData();
        }
        SBoxIdea.RequestCoder(0);
        SBoxIdea.ReadConf();
    }

    private void OnRequestCoder(SBoxCoderData sBoxCoderData)
    {
        if (sBoxCoderData.result < 0)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
            return;
        }
        else if (sBoxCoderData.result > 0)
        {
            IOPopTips.Instance.ShowTips($"{Utils.GetLanguage("Wrong")}:{sBoxCoderData.result}");
            return;
        }

        IOCanvasModel.Instance.coderData = sBoxCoderData;
        EventCenter.Instance.EventTrigger(EventHandle.INIT_CODER_VIEW);
    }

    private void OnReadConfig(SBoxConfData sBoxConfData)
    {
        if (sBoxConfData.result < 0)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
            return;
        }
        else if (sBoxConfData.result > 0)
        {
            IOPopTips.Instance.ShowTips($"{Utils.GetLanguage("Wrong")}:{sBoxConfData.result}");
            return;
        }
        IOCanvasModel.Instance.CfgData = sBoxConfData;

        IOCanvasModel.Instance.netSwitch = PlayerPrefs.GetInt("NetSwitch", 0);
        IOCanvasModel.Instance.tempNetSwitch = IOCanvasModel.Instance.netSwitch;

        #region todo: 读取配置后同步给各个分机
        //var msgConfig = new MsgConfig
        //{
        //    coinValue = sBoxConfData.CoinValue,
        //    ticketValue = sBoxConfData.TicketValue,
        //    scoreTicket = sBoxConfData.scoreTicket,
        //    pulseValue = sBoxConfData.PulseValue,
        //    switchList = IOCanvasModel.Instance.switchList,
        //    countDown = sBoxConfData.CountDown,
        //    betsMinOfJackpot = sBoxConfData.BetsMinOfJackpot,
        //};

        //var msgInfo = new MsgInfo
        //{
        //    cmd = (int)S2C_CMD.S2C_UpdateConfig,
        //    id = -1,
        //    jsonData = JsonConvert.SerializeObject(msgConfig),
        //};
        //NetMgr.Instance.SendToAllClient(JsonConvert.SerializeObject(msgInfo));
        #endregion

        EventCenter.Instance.EventTrigger(EventHandle.REFRESH_IOCANVAS);
    }

    private void OnConfirmPassword(ulong password)
    {
        switch (IOCanvasModel.Instance.state)
        {
            case IOState.CheckPermissions:
                CheckPassword(password);
                break;
            case IOState.Code:
                    SBoxIdea.Coder(0, (ulong)password);
                break;
            case IOState.EditPassword:
                switch (view.selectSection.state)
                {
                    case IOSectionState.EditPassword:
                        IOCanvasModel.Instance.tempPassword = password;
                        view.selectSection.state = IOSectionState.NewPassword;
                        view.passwordPanel.SetPlaceholderText(Utils.GetLanguage("Please enter new password again"));
                        break;
                    case IOSectionState.NewPassword:
                        if (IOCanvasModel.Instance.tempPassword == password)
                            ChangePassword(password);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private void CheckPassword(ulong password)
    {
        SBoxIdea.CheckPassword((int)password);
    }

    private void ChangePassword(ulong password)
    {
        SBoxIdea.ChangePassword((int)password);
    }

    private void OnCheckPassword(SBoxPermissionsData sBoxPermissionsData)
    {
        if (sBoxPermissionsData.result < 0)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
            return;
        }
        else if (sBoxPermissionsData.result == 1)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("VerificationCodeRequired"));
            return;
        }
        IOCanvasModel.Instance.permissions = sBoxPermissionsData.permissions;
     
        if (Application.isEditor)
            sBoxPermissionsData.permissions = 2;
        if (sBoxPermissionsData.permissions > 0)
        {
            switch (IOCanvasModel.Instance.state)
            {
                case IOState.CheckPermissions:
                    view.DestroyManagerPasswordPanel();
                    view.InstantiateMenuPanel();
                    IOCanvasModel.Instance.state = IOState.SelectFunction;
                    IOCanvasModel.Instance.isInitMenu = true;
                    break;
                case IOState.EditPassword:
                    view.EditPassword();
                    break;
                default:
                    break;
            } 
        }
        else
            IOPopTips.Instance.ShowTips($"{Utils.GetLanguage("PasswordWrong")}:{sBoxPermissionsData.result}");
    }

    private void OnChangePassword(SBoxPermissionsData sBoxPermissionsData)
    {
        if (sBoxPermissionsData.result < 0)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
            return;
        }
        else if (sBoxPermissionsData.result == 1)
        {
            IOPopTips.Instance.ShowTips(Utils.GetLanguage("VerificationCodeRequired"));
            return;
        }
        IOPopTips.Instance.ShowTips(Utils.GetLanguage("EditSucceed"));
        view.selectSection.selected = false;
        view.passwordPanel.SetPlaceholderText(Utils.GetLanguage("Please enter new password"));
        //view.passwordPanel.Focuse(false);
        view.SetCurSelect();
        view.selectSection.state = IOSectionState.EditPassword;
        IOCanvasModel.Instance.permissions = sBoxPermissionsData.permissions;
    }

    private void OnWriteConf(SBoxPermissionsData sBoxPermissionsData)
    {
        switch (sBoxPermissionsData.result)
        {
            case -1:
                IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
                break;
            case 0:
                IOPopTips.Instance.ShowTips(Utils.GetLanguage("SaveSucceed"));

                IOCanvasModel.Instance.netSwitch = IOCanvasModel.Instance.tempNetSwitch;
                PlayerPrefs.SetInt("NetSwitch", IOCanvasModel.Instance.netSwitch);
                break;
            case 1:
                IOPopTips.Instance.ShowTips(Utils.GetLanguage("VerificationCodeRequired"));
                break;
            case 2:
                IOPopTips.Instance.ShowTips(Utils.GetLanguage("NeedCode"));
                break;
        }
        IOCanvasModel.Instance.permissions = sBoxPermissionsData.permissions;
        SBoxIdea.ReadConf();
    }

    private void OnSetDate(int result)
    {
        switch (result)
        {
            case -1:
                IOPopTips.Instance.ShowTips(Utils.GetLanguage("ParamsWrong"));
                break;
            case 0:
                IOPopTips.Instance.ShowTips(Utils.GetLanguage("SaveSucceed"));
                view.CloseModifiedDatePanel();
                break;
        }
    }
}