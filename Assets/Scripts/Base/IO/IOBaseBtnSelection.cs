using UnityEngine;
using UnityEngine.UI;

public abstract class IOBaseBtnSelection : IOBaseSelection
{

    [HideInInspector]
    public Button button;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    public void AddListener(UnityEngine.Events.UnityAction action)
    {
        if (action != null && IOCanvasModel.Instance.mouseClick)
            button.onClick.AddListener(action);
    }
}
