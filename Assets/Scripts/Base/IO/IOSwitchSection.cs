using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOSwitchSection : IOBaseSection
{
    public override int CurIndex
    {
        get { return _curIndex; }
        set
        {
            UpdateTextListColor(IOCanvasModel.Instance.textNormalColor);
            if (value > textList.Count - 1)
                _curIndex = 0;
            else if (value < 0)
                _curIndex = textList.Count - 1;
            else
                _curIndex = value;
            textList[_curIndex].color = IOCanvasModel.Instance.textSelectedColor;
        }
    }

    public override Color Color 
    {
        get
        {
            if (_showBg)
                return image.color;
            else
                return titleText.color;
        }
        set
        {
            if (_showBg)
                image.color = value;
            else
            {
                titleText.color = value;
                UpdateTextListColor(value);
            }
        }
    }

    public override bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            if (_selected)
            {
                image.color = _showBg ? IOCanvasModel.Instance.selectedColor : image.color;
                titleText.color = IOCanvasModel.Instance.textNormalColor;
                UpdateTextListColor(IOCanvasModel.Instance.textNormalColor);
                _curIndex = 0;
                textList[_curIndex].color = IOCanvasModel.Instance.selectedColor;
            }
            else
            {
                image.color = _showBg ? IOCanvasModel.Instance.textNormalColor : image.color;
                titleText.color = IOCanvasModel.Instance.selectedColor;
                UpdateTextListColor(IOCanvasModel.Instance.selectedColor);
            }
        }
    }

    private List<int> _dataList;

    public List<int> DataList
    {
        get { return _dataList; }
        set
        {
            _dataList = value;
            UpdateTextListColor(Color);
        }
    }

    private List<int> _orignalList;
    public List<int> OriganlList
    {
        get { return _orignalList; }
        set
        {
            _orignalList = value;
            InitList();
        }
    }

    private List<Text> textList = new List<Text>();


    private Transform contentList;

    protected override void Awake()
    {
        base.Awake();
        titleText = transform.Find("Title").GetComponent<Text>();
        contentList = transform.Find("Content");
    }

    private void InitList()
    {
        IOCanvasModel.Instance.switchList.ForEach(InstantiateListItem);
    }

    private void InstantiateListItem(int data)
    {
        Text text = new GameObject("text", typeof(Text)).GetComponent<Text>();
        text.transform.SetParent(contentList);
        text.transform.localPosition = Vector3.zero;
        text.transform.localScale = Vector3.one;
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        text.fontSize = titleText.fontSize;
        text.color = IOCanvasModel.Instance.textNormalColor;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleLeft;
        text.text = data.ToString();
        textList.Add(text);
    }

    private void UpdateTextListColor(Color color)
    {
        if (color == IOCanvasModel.Instance.selectedColor)
            textList.ForEach(text => text.color = IOCanvasModel.Instance.selectedColor);
        else
        {
            textList.ForEach(txt =>
            {
                txt.color = IOCanvasModel.Instance.textNormalColor;
            });
        }
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(EventHandle.UPDATE_SWITCH, UpdateSwitch);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(EventHandle.UPDATE_SWITCH, UpdateSwitch);
    }

    public void UpdateSwitch()
    {
        for (int i = 0; i < textList.Count; i++)
            textList[i].text = IOCanvasModel.Instance.switchList[i].ToString();
    }

    public void OnConfirm()
    {
        //int temp = int.Parse(textList[_curIndex].text);
        //if (_dataList.Contains(temp))
        //{
        //    _dataList.Remove(temp);
        //    IOCanvasModel.Instance.switchList.Remove(temp);
        //}
        //else
        //{
        //    _dataList.Add(temp);
        //    IOCanvasModel.Instance.switchList.Add(temp);
        //}
    }
}
