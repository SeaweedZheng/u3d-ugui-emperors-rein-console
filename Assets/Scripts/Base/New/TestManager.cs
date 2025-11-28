using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoSingleton<MonoBehaviour>   
{

    public GameObject goAnchor;

    public Button btnBase, btnHitJP1, btnHitJP2;

    void Start()
    {
        goAnchor.SetActive(false);

        btnBase.onClick.AddListener(OnClickButtonBase);

        btnHitJP1.onClick.AddListener(OnClickButtonHitJP1);

        btnHitJP2.onClick.AddListener(OnClickButtonHitJP2);
    }


    void OnClickButtonBase()
    {
        goAnchor.SetActive(!goAnchor.active);
    }

    void OnClickButtonHitJP1()
    {
        EventCenter.Instance.EventTrigger(GlobalEvent.TEST_HIT_JP1);
    }
    void OnClickButtonHitJP2()
    {
        EventCenter.Instance.EventTrigger(GlobalEvent.TEST_HIT_JP2);
    }
}
