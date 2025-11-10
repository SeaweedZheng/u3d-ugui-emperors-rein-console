using SBoxApi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    private Text descText;
    private void Start()
    {
        descText = GameObject.Find("DescText")?.GetComponent<Text>();
        EventCenter.Instance.AddEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnReadConfig);
        if (descText != null)
            descText.text = "正在检测硬件...";

        if (false && Application.isEditor)
        {
            Debug.LogError("i am here1234...");
            /*  IOCanvasModel.Instance.CfgData = new SBoxConfData
              {

              };

              ResMgr02.Instance.LoadAssetBundle("io", "IOCanvas", (UnityEngine.Object obj) =>
              {
                  IOCanvasViewObj = obj as GameObject;
                  GameObject gameObject = Instantiate(IOCanvasViewObj, transform);
              });*/

            IOCanvasModel.Instance.CfgData = new SBoxConfData
            {

            };
            // 分辨率： 1280x720
            //StartCoroutine(LoadAssetbundle());

            ScenesMgr.Instance.LoadSceneAsyn("game", () => ResMgr.Instance.LoadAssetBundle("game", "ui", (obj) =>
            {
                Controller.Instance.Init();
                Instantiate(obj);
            }));
            //ResMgr.Instance.LoadAssetBundle("io", "IOCanvas", (prefab) =>
            //{
            //    if (prefab == null) return;
            //    GameObject IOCanvasPrefab = (GameObject)prefab;
            //    GameObject gameObject = Instantiate(IOCanvasPrefab, transform);

            //});
        }
        else
        {
            SBoxInit.Instance.Init("192.168.3.225", OnInitSBox);
        }
    }
    private GameObject IOCanvasViewObj;

    IEnumerator LoadAssetbundle()
    {
        if (ResMgr.Instance.bundleDic.ContainsKey("io"))
        {
            AssetBundle bundle = ResMgr.Instance.bundleDic["io"];
            bundle.LoadAllAssets();
            GameObject go = bundle.LoadAsset<GameObject>("IOCanvas");
            GameObject gameObject = Instantiate(IOCanvasViewObj, transform);
            yield break;
        }
        else
        {
            bool isNext = false;
            AssetBundleCreateRequest createRequest = null;
            StartUpUtils.GetFromStreamingAssets("AssetBundles/io", (data) =>
            {
                createRequest = AssetBundle.LoadFromMemoryAsync(data);
                isNext = true;
            });
            yield return new WaitUntil(()=>isNext == true);
            //yield return createRequest;
            AssetBundle bundle = createRequest.assetBundle;
            bundle.LoadAllAssets();
            GameObject go = bundle.LoadAsset<GameObject>("IOCanvas");
            GameObject gameObject = Instantiate(IOCanvasViewObj, transform);
        }

    }










    private void OnInitSBox()
    {
        if (descText != null)
            descText.text = "正在读取游戏配置...";
        SBoxIdea.ReadConf();
    }

    private void OnReadConfig(SBoxConfData sBoxConfData)
    {
        IOCanvasModel.Instance.CfgData = sBoxConfData;
        IOCanvasModel.Instance.netSwitch = PlayerPrefs.GetInt("NetSwitch", 0);
        IOCanvasModel.Instance.tempNetSwitch = IOCanvasModel.Instance.netSwitch;
        OnSBoxReadConfigComplete();
    }

    private void OnSBoxReadConfigComplete()
    {
        IOCanvasModel.Instance.CurLanguage = (Language)(Enum.Parse(typeof(Language), PlayerPrefs.GetInt("CurLanguage", 0).ToString()));
        EventCenter.Instance.RemoveEventListener<SBoxConfData>(SBoxEventHandle.SBOX_READ_CONF, OnReadConfig);
        NetMessageController.Instance.Init();
        InternetDeviceMsg.Instance.Init();
        SQLite.Instance.Init();

        LoadData();

        Model.Instance.highestWinsOrderData[0].Sort((a, b) => b.wins.CompareTo(a.wins));
        Model.Instance.DealWithUnFinishOrder(SQLite.Instance.ReadUnfinishOrderData());
        //if (SBoxModel.Instance.isNetServer)
        //    InternetServerMgr.Instance.DeviceConnectServer();
        //else
            NetMgr.Instance.SetNetAutoConnect(true);
        PreloadSound();
        if (descText != null)
            descText.text = "正在载入...";
        ScenesMgr.Instance.LoadSceneAsyn("game", () => ResMgr.Instance.LoadAssetBundle("game", "ui", (obj) =>
        {
            Controller.Instance.Init();
            Instantiate(obj);
        }));
    }

    private void LoadData()
    {
        Model.Instance.highestWinsOrderData = new Dictionary<int, List<OrderData>>();
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Fix] = SQLite.Instance.LoadDataWithHighestWins();
        Model.Instance.AddFakeData();
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Grand] = SQLite.Instance.LoadDataWithHighestWins(0);
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Major] = SQLite.Instance.LoadDataWithHighestWins(1);
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Minor] = SQLite.Instance.LoadDataWithHighestWins(2);
        Model.Instance.highestWinsOrderData[(int)OrderDataMode.Mini] = SQLite.Instance.LoadDataWithHighestWins(3);
        Model.Instance.curOrderIdx = SQLite.Instance.LoadCurIndex();
        IOCanvasModel.Instance.BetDataDic = SQLite.Instance.ReadAllBetData();
        SQLiteModel.Instance.PlayerIdDataClientIdDic = SQLite.Instance.LoadPlayerData();
        SQLiteModel.Instance.PlayerIdDataLogicIdDic = SQLite.Instance.LoadPlayerData1();
    }

    private void PreloadSound()
    {

    }

}
