using UnityEngine;
using UnityEngine.UI;

public class IOBaseSection : IOBaseBtnSelection
{

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
                contentText.color = value;
            }
        }
    }

    protected int _curIndex;
    public virtual int CurIndex
    {
        get { return _curIndex; }
        set
        {
            _curIndex = value;
            if (_curIndex < 0)
                contentText.text = "NoData";
            else
            {
                if (contents.Length > 0)
                    contentText.text = contents[_curIndex];
                else
                {
                    string str = _curIndex.ToString();
                    switch (ioParams)
                    {
                        /*#seaweed# 
                        case IOParams.CountDown:
                            str += Utils.GetLanguage("Seconds");
                            break;
                        */
                        case IOParams.CoinRatio:
                            str = $"1{Utils.GetLanguage("Coin")}{str}{Utils.GetLanguage("Score")}";
                            break;
                        /*#seaweed# 
                        case IOParams.ClientWinLock:
                            if (_curIndex == 0)
                            {
                                switch (IOCanvasModel.Instance.curlanguage)
                                {
                                    case Language.chs:
                                        str = "无限制";
                                        break;
                                    case Language.cht:
                                        str = "無限制";
                                        break;
                                    case Language.en:
                                        str = "No limit";
                                        break;
                                }
                            }
                            break;
                        */
                    }
                    contentText.text = str;
                }
            }
        }
    }

    public IOParams ioParams;
    protected bool _selected;
    public virtual bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            if (_selected)
            {
                image.color = _showBg ? IOCanvasModel.Instance.selectedColor : image.color;
                titleText.color = IOCanvasModel.Instance.textNormalColor;
                contentText.color = IOCanvasModel.Instance.textSelectedColor;
            }
            else
            {
                image.color = _showBg ? IOCanvasModel.Instance.imageNormalColor : image.color;
                titleText.color = IOCanvasModel.Instance.textNormalColor;
                contentText.color = IOCanvasModel.Instance.textNormalColor;
            }
        }
    }

    [HideInInspector]
    public string[] contents;

    [HideInInspector]
    public Text titleText;

    [HideInInspector]
    public Text contentText;

    public string Content
    {
        get { return contentText.text; }
        set
        {
            /*#seaweed# 
            if (ioParams == IOParams.CountDown)
                contentText.text = value + Utils.GetLanguage("Seconds");
            else
                contentText.text = value;
            */
            contentText.text = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        titleText = transform.Find("Title").GetComponent<Text>();
        contentText = transform.Find("Content").GetComponent<Text>();
    }
}
