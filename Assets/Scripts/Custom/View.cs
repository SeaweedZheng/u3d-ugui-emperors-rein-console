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
        if ( IOCanvasObj != null )
            IOCanvasObj.SetActive(true);
        else
            ResMgr.Instance.LoadAssetBundle("io", "IOCanvas", (obj) =>
            { IOCanvasObj = Instantiate(obj) as GameObject; });
    }

    public void Start()
    {


        // 显示信息
        Debug.Log($"包信息：isRelease:{StartUpConfigHelper.isRelease}");

        // 创建工具
    }

}
