using UnityEngine;
using UnityEngine.UI;

public abstract class IOBaseSelection : MonoBehaviour
{
    protected bool _showBg;
    public bool ShowBg
    {
        get { return _showBg; }
        set
        {
            _showBg = value;
            if (ShowBg)
                image.color = new Color(1, 1, 1, 1);
            else
                image.color = new Color(1, 1, 1, 1 / 255f);
        }
    }

    public Vector2 ImgRect
    {
        set
        {
            image.rectTransform.sizeDelta = value;
        }
    }

    [HideInInspector]
    public string title;

    public abstract Color Color { get; set; }

    protected Image image;

    protected Image select;


    protected virtual void Awake()
    {
        image = transform.Find("Image").GetComponent<Image>();
        select = transform.Find("Select").GetComponent<Image>();
    }

    public void SetParentAndReset(Transform parent)
    {
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    public void SetSelected(bool selected)
    {
        select.gameObject.SetActive(selected);
    }
}
