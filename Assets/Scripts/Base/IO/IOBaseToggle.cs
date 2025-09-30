using UnityEngine;
using UnityEngine.UI;

public class IOBaseToggle : IOBaseSelection
{
    [HideInInspector]
    public Toggle toggle;

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
        toggle = GetComponent<Toggle>();
        titleText = transform.Find("Text").GetComponent<Text>();
    }

    public void AddListener(UnityEngine.Events.UnityAction<bool> action)
    {
        if (action != null)
            toggle.onValueChanged.AddListener(action);
    }
}
