using Newtonsoft.Json;
using SBoxApi;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    private GameObject IOCanvasObj;

    public void OnSettingBtnClick()
    {
        ResMgr.Instance.LoadAssetBundle("io", "IOCanvas", (obj) =>
        { IOCanvasObj = Instantiate(obj) as GameObject; });
    }

    public void Start()
    {
    }

}
