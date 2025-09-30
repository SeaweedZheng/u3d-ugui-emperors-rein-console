using SBoxApi;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static SBoxApi.SBoxSandbox;

public class IOPasswordPanel : MonoBehaviour
{
    private GameObject baseBtnPrefabs;

    private int curSelect;
    private ulong curPassword = 0;
    private InputField inputField;
    private Transform btnArea;
    private List<IOBaseBtnSelection> btnList = new List<IOBaseBtnSelection>();
    private Text placeholderText;
    private bool inputEnable = true;
    private bool needMoreInput = false;
    private bool editSectionMode = false;

    private void Awake()
    {
        ResMgr.Instance.LoadAssetBundle("io", "baseButton0", (obj) => baseBtnPrefabs = (GameObject)obj);
        inputField = transform.Find("password").GetComponent<InputField>();
        placeholderText = inputField.placeholder.GetComponent<Text>();
        btnArea = transform.Find("btnArea");
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<KeyCode>(EventHandle.KEY_DOWN, OnKeyCode);
        EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_DOWN, OnHardwareDown);
        EventCenter.Instance.AddEventListener<ulong>(EventHandle.HARDWARE_KEY_CLICK, OnHardwareClick);

        for (int i = 0; i < 12; i++)
        {
            if (i < 9)
            {
                int num = i + 1;
                int tempIndex = i;
                InstantiateBaseBtn(num.ToString(), () => { OnNumBtnClcik(num, tempIndex); }, true, TextAnchor.MiddleCenter);
            }
            else
            {
                int fontSize = IOCanvasModel.Instance.curlanguage == Language.en ? 52 : 80;
                switch (i)
                {
                    case 9:
                        InstantiateBaseBtn(Utils.GetLanguage("DELETE"), () => { OnDeleteBtnClick(); }, true, TextAnchor.MiddleCenter, fontSize: fontSize);
                        break;
                    case 10:
                        InstantiateBaseBtn("0", () => { OnNumBtnClcik(0, 10); }, true, TextAnchor.MiddleCenter);
                        break;
                    case 11:
                        InstantiateBaseBtn(Utils.GetLanguage("CONFIRM"), () => { OnConfirmClick(); }, true, TextAnchor.MiddleCenter, fontSize: fontSize);
                        break;
                }
            }
        }
        SetCurSelect(0);
    }

    public void SetParams(InputField.ContentType contentType = InputField.ContentType.Password, string placeholderStr = "Please enter password", bool noSelect = false, float localScal = 1f)
    {
        inputField.contentType = contentType;
        if (contentType == InputField.ContentType.Password)
        {
            inputField.textComponent.fontSize = 170;
            inputField.textComponent.alignment = TextAnchor.MiddleCenter;
            inputField.textComponent.rectTransform.anchoredPosition = new Vector2(0f, -36f);
        }
        else
        {
            inputField.textComponent.fontSize = 110;
            inputField.textComponent.alignment = TextAnchor.UpperLeft;
            inputField.textComponent.rectTransform.anchoredPosition = new Vector2(18, -20f);
        }

        placeholderText.fontSize = IOCanvasModel.Instance.curlanguage == Language.cht ? 55 : 58;
        placeholderText.text = Utils.GetLanguage(placeholderStr);

        if (noSelect)
        {
            for (int i = 0; i < btnList.Count; i++)
                btnList[i].Color = IOCanvasModel.Instance.imageNormalColor;
            inputEnable = false;
        }
        transform.localScale = Vector3.one * localScal;
    }

    public void SetPlaceholderText(string str)
    {
        placeholderText.text = Utils.GetLanguage(str);
    }

    public List<IOBaseBtnSelection> GetSelectionList()
    {
        return btnList;
    }

    private void InstantiateBaseBtn(string str, UnityAction unityAction, bool showBg = true, TextAnchor textAnchor = TextAnchor.MiddleCenter, int fontSize = 100)
    {
        var btn = Instantiate(baseBtnPrefabs, btnArea).GetComponent<IOBaseBtn>();
        btn.ShowBg = showBg;
        btn.titleText.text = str;
        btn.titleText.alignment = textAnchor;
        btn.titleText.fontSize = fontSize;
        if (IOCanvasModel.Instance.mouseClick)
            btn.button.onClick.AddListener(unityAction);
        btnList.Add(btn);
    }

    public void SetCurSelect(int index, bool isSelf = true)
    {
        curSelect = index;
        if (index < 12)
        {
            for (int i = 0; i < btnList.Count; i++)
                btnList[i].SetSelected(false);
            btnList[index].SetSelected(true);
        }
        else
            for (int i = 0; i < btnList.Count; i++)
                btnList[i].SetSelected(false);
        if ((isSelf && IOCanvasModel.Instance.state == IOState.Code)
            || (isSelf && IOCanvasModel.Instance.state == IOState.EditPassword))
            EventCenter.Instance.EventTrigger(EventHandle.PASSWORD_CUR_SELECT, curSelect);
    }

    public void Focuse(bool focuse, bool editSectionMode = true)
    {
        if (focuse)
        {
            curSelect = 0;
            SetCurSelect(curSelect);
            needMoreInput = true;
            this.editSectionMode = editSectionMode;
        }
        else
        {
            for (int i = 0; i < btnList.Count; i++)
                btnList[i].Color = IOCanvasModel.Instance.imageNormalColor;
        }
        inputEnable = focuse;
    }

    private void OnHardwareDown(ulong sboxSwitch)
    {
        if (!inputEnable) return;
        switch (sboxSwitch)
        {
            case SBOX_SWITCH.SWITCH_KEYBOARD_UP:
                SelectUp();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_LEFT:
                SelectLeft();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_DOWN:
                SelectDown();
                break;
            case SBOX_SWITCH.SWITCH_KEYBOARD_RIGHT:
                SelectRight();
                break;

        }
    }

    private void OnHardwareClick(ulong sboxSwitch)
    {
        if (!inputEnable) return;
        switch (sboxSwitch)
        {
            case SBOX_SWITCH.SWITCH_KEYBOARD_CONFIRM:
                if (curSelect < 9)
                    OnNumBtnClcik(curSelect + 1, curSelect);
                else
                {
                    switch (curSelect)
                    {
                        case 9:
                            OnDeleteBtnClick();
                            break;
                        case 10:
                            OnNumBtnClcik(0, 10);
                            break;
                        case 11:
                            OnConfirmClick();
                            break;
                    }
                }
                break;
        }
    }

    private void OnKeyCode(KeyCode keyCode)
    {
        if (inputEnable)
        {
            switch (keyCode)
            {
                case KeyCode.W:
                    SelectUp();
                    break;
                case KeyCode.A:
                    SelectLeft();
                    break;
                case KeyCode.S:
                    SelectDown();
                    break;
                case KeyCode.D:
                    SelectRight();
                    break;
                case KeyCode.F:
                    if (needMoreInput)
                    {
                        needMoreInput = false;
                        return;
                    }
                    if (curSelect < 9)
                        OnNumBtnClcik(curSelect + 1, curSelect);
                    else
                        switch (curSelect)
                        {
                            case 9:
                                OnDeleteBtnClick();
                                break;
                            case 10:
                                OnNumBtnClcik(0, 10);
                                break;
                            case 11:
                                OnConfirmClick();
                                break;
                            default:
                                break;
                        }
                    break;
            }
        }

    }

    private void SelectUp()
    {
        if (curSelect == 12)
        {
            SetCurSelect(10);
        }
        else
        {
            if (curSelect - 3 > -1)
                SetCurSelect(curSelect - 3);
        }

    }

    private void SelectDown()
    {
        if (curSelect == 12)
            return;
        if (curSelect + 3 < btnList.Count)
            SetCurSelect(curSelect + 3);
    }

    private void SelectLeft()
    {
        if (curSelect - 1 > -1)
            SetCurSelect(curSelect - 1);
    }

    private void SelectRight()
    {
        if (curSelect == 12)
            return;
        if (curSelect + 1 < btnList.Count)
            SetCurSelect(curSelect + 1);
    }

    private void OnNumBtnClcik(int num, int index)
    {
        SetCurSelect(index);
        if (editSectionMode)
            EventCenter.Instance.EventTrigger(EventHandle.PASSWORD_INPUT, num);
        else
        {
            ulong.TryParse(curPassword.ToString() + num.ToString(), out ulong tempPassword);
            curPassword = curPassword < tempPassword ? tempPassword : curPassword;
            inputField.text = curPassword.ToString();
        }
    }

    private void OnDeleteBtnClick()
    {
        SetCurSelect(9);
        if (editSectionMode)
            EventCenter.Instance.EventTrigger(EventHandle.PASSWORD_DELETE);
        else
        {
            curPassword /= 10;
            inputField.text = curPassword == 0 ? "" : curPassword.ToString();
        }
    }

    private void OnConfirmClick()
    {
        SetCurSelect(11);
        if (editSectionMode)
        {
            EventCenter.Instance.EventTrigger(EventHandle.PASSWORD_CONFIRM);
            curPassword = 0;
            inputField.text = "";
        }
        else
        {
            EventCenter.Instance.EventTrigger(EventHandle.CONFIRM_PASSWORD, curPassword);
            curPassword = 0;
            inputField.text = "";
        }
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<KeyCode>(EventHandle.KEY_DOWN, OnKeyCode);
        EventCenter.Instance.RemoveEventListener<ulong>(EventHandle.HARDWARE_KEY_DOWN, OnHardwareDown);
        EventCenter.Instance.RemoveEventListener<ulong>(EventHandle.HARDWARE_KEY_CLICK, OnHardwareClick);
    }
}
