using UnityEngine;
using UnityEngine.UI;

public class IOBaseBtn : IOBaseBtnSelection
{
    public override Color Color
    {
        get
        {
            if (style == 0)
                return image.color;
            else
                return titleText.color;
        }
        set
        {
            if (style != 0)
                titleText.color = value;
        }
    }

    [HideInInspector]
    public Text titleText;

    public int style;

    protected override void Awake()
    {
        base.Awake();
        titleText = transform.Find("Text").GetComponent<Text>();
    }
}
