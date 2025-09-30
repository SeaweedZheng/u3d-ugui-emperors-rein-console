using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IODateTimeSection : IOBaseSection
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
            textList[_curIndex].color = IOCanvasModel.Instance.selectedColor;
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

    private List<Text> textList = new List<Text>();

    private Transform contentList;

    private Text yearText;
    private Text monthText;
    private Text dayText;
    private Text hourText;
    private Text minuteText;

    protected override void Awake()
    {
        base.Awake();
        titleText = transform.Find("Title").GetComponent<Text>();
        contentList = transform.Find("Content");
        yearText = transform.Find("Content/year").GetComponent<Text>();
        monthText = transform.Find("Content/month").GetComponent<Text>();
        dayText = transform.Find("Content/day").GetComponent<Text>();
        hourText = transform.Find("Content/hour").GetComponent<Text>();
        minuteText = transform.Find("Content/min").GetComponent<Text>();

        textList.Add(yearText);
        textList.Add(monthText);
        textList.Add(dayText);
        textList.Add(hourText);
        textList.Add(minuteText);
    }

    public void InitDateTimeView()
    {
        yearText.text = IOCanvasModel.Instance.curlanguage == Language.en ? 
            IOCanvasModel.Instance.IODateTime.Year.ToString() + ", "
            : IOCanvasModel.Instance.IODateTime.Year.ToString() + Utils.GetLanguage("Year");
        monthText.text = IOCanvasModel.Instance.curlanguage == Language.en ?
            (Month)(IOCanvasModel.Instance.IODateTime.Month - 1) + ". " 
            : IOCanvasModel.Instance.IODateTime.Month.ToString() + Utils.GetLanguage("Month");
        dayText.text = IOCanvasModel.Instance.curlanguage == Language.en ?
            IOCanvasModel.Instance.IODateTime.Day.ToString() + " "
            : IOCanvasModel.Instance.IODateTime.Day.ToString() + Utils.GetLanguage("Day1");
        hourText.text = IOCanvasModel.Instance.IODateTime.Hour < 10 ?
            "0" + IOCanvasModel.Instance.IODateTime.Hour
            : IOCanvasModel.Instance.IODateTime.Hour.ToString();
        minuteText.text = IOCanvasModel.Instance.IODateTime.Minute < 10 ?
            "0" + IOCanvasModel.Instance.IODateTime.Minute 
            : IOCanvasModel.Instance.IODateTime.Minute.ToString();
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
        EventCenter.Instance.AddEventListener(EventHandle.UPDATE_DATE_TIME, InitDateTimeView);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(EventHandle.UPDATE_DATE_TIME, InitDateTimeView);
    }
}
