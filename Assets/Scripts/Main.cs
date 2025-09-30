using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    /// <summary> 是否更新: 网络版本比对 /// </summary>
    private bool needUpdateNet = false;

    private UnityWebRequest www;

    public Text versionText;
    public Text descText;
    public Text downloadDescText;
    public ProgressBarUV progressSlider;
    private ProgressMonitor monitor;

    private const float initNetworkTime = 10;
    private const float requestTimeout = 15;
    private const float descShowTime = 2;

    private string downObjName;

    private VersionData curVersionData = new VersionData
    {
        Version = "0.0.0",
    };

    private VersionData netVersionData = new VersionData
    {
        Version = "0.0.0",
    };

    private VersionData localVersionData = new VersionData
    {
        Version = "0.0.0",
    };

    private List<string> dllList = new List<string>
    {
        "UnityWebSocket.Runtime.dll",
        "Base.dll",
        "Custom.dll"  //"Game.dll"
    };

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        if (Application.isEditor)
            Application.runInBackground = true;

        monitor = new ProgressMonitor();
        monitor.OnProgressChanged += (progress) =>
        {
            progressSlider.SetProgress(progress);
        };

        GetCurVersion();
        yield return InitNetwork();
        yield return RefTypes.LoadMetadataForAOTAssemblies();
        yield return CheckVersion();
    }

    private IEnumerator InitNetwork()
    {
        descText.text = "正在等待网络初始化...";

        float time = 0f;
        float checkInterval = 0.5f;

        monitor.Progress = 0f;

        while (Application.internetReachability == NetworkReachability.NotReachable && time < initNetworkTime)
        {
            yield return null;
            time += Time.deltaTime;
            monitor.Progress = time / initNetworkTime * 0.1f;
        }

        monitor.Progress = 0.1f;
    }

    private IEnumerator CheckVersion()
    {
        descText.text = "正在网络版本检测...";
        yield return new WaitForSeconds(descShowTime);
        string versionUrl = StartUpConfig.url + "/Version.txt";
        www = UnityWebRequest.Get(versionUrl);
        www.timeout = 20;
        downObjName = "Version.txt";
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError
           || www.result == UnityWebRequest.Result.ProtocolError)
        {
            descText.text = $"网络请求超时:{www.error}";
            yield return new WaitForSeconds(descShowTime);
            yield return CompareVersion();
        }
        else if (www.isDone)
        {
            netVersionData = JsonConvert.DeserializeObject<VersionData>(www.downloadHandler.text);
            yield return CompareVersion();
        }
    }

    private IEnumerator CompareVersion()
    {
        monitor.Progress = 0.15f;
        descText.text = "正在版本比对...";
        yield return new WaitForSeconds(descShowTime);
        GetLocalVersion();
        needUpdateNet = StartUpUtils.ParseVersion(netVersionData.Version) > StartUpUtils.ParseVersion(curVersionData.Version);
        if (needUpdateNet)
        {
            if (StartUpUtils.ParseVersion(netVersionData.Version) > StartUpUtils.ParseVersion(localVersionData.Version))
                yield return UpdateFromNet();
            else
                yield return UpdateFromLocal();
        }
        else
        {
            if (StartUpUtils.ParseVersion(localVersionData.Version) > StartUpUtils.ParseVersion(curVersionData.Version))
                yield return UpdateFromLocal();
            else
                yield return LoadFromMemory();
        }
    }

    private void GetCurVersion()
    {
        if (File.Exists(StartUpConfig.VersionPath))
        {
            string versionJson = File.ReadAllText(StartUpConfig.VersionPath);
            curVersionData = JsonConvert.DeserializeObject<VersionData>(versionJson);
            PlayerPrefs.SetString("CurVersion", curVersionData.Version);
            versionText.text = $"Version: {curVersionData.Version}";
        }
    }

    private void GetLocalVersion()
    {
        StartUpUtils.GetFromStreamingAssets("Version.txt", (data) =>
        {
            string versionJson = System.Text.Encoding.UTF8.GetString(data);
            localVersionData = JsonConvert.DeserializeObject<VersionData>(versionJson);
        });
    }

    private IEnumerator UpdateFromNet()
    {
        descText.text = "正在网络更新...";
        monitor.Progress = 0.4f;

        string languageUrl = StartUpConfig.url + "/language.json";
        www = UnityWebRequest.Get(languageUrl);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError
        || www.result == UnityWebRequest.Result.ProtocolError)
        {
            descText.text = $"网络更新多语言文件错误:{www.error}";
        }
        else if (www.isDone)
            File.WriteAllBytes(StartUpConfig.LanguagePath, www.downloadHandler.data);

        for (int i = 0; i < dllList.Count; i++)
        {
            string dllUrl = StartUpConfig.url + "/Lib/" + dllList[i];
            downObjName = $"{dllList[i]}";
            www = UnityWebRequest.Get(dllUrl);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError
            || www.result == UnityWebRequest.Result.ProtocolError)
            {
                descText.text = $"网络更新代码文件错误:{www.error}";
            }
            else if (www.isDone)
                File.WriteAllBytes(StartUpConfig.DllPath + "/" + dllList[i], www.downloadHandler.data);
        }

        LoadDllFromMemory();
        StartCoroutine(LoadAssetBundleFromNet());
    }

    private IEnumerator LoadAssetBundleFromNet()
    {
        descText.text = "正在替换网络更新资源...";
        www = UnityWebRequestAssetBundle.GetAssetBundle(StartUpConfig.url + "/AssetBundles/AssetBundles");
        downObjName = $"AssetBundles";
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError
            || www.result == UnityWebRequest.Result.ProtocolError)
        {
            descText.text = $"网络更新资源文件错误:{www.error}";
        }
        else if (www.isDone)
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] names = manifest.GetAllAssetBundles();
            AssetBundle.UnloadAllAssetBundles(true);
            www = UnityWebRequest.Get(StartUpConfig.url + "/AssetBundles/AssetBundles");
            downObjName = $"AssetBundles";
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError
            || www.result == UnityWebRequest.Result.ProtocolError)
            {
                descText.text = $"读取资源依赖文件错误:{www.error}";
            }
            else if (www.isDone)
                File.WriteAllBytes(StartUpConfig.AssetBundlePath + "/AssetBundles", www.downloadHandler.data);
            for (int i = 0; i < names.Length; i++)
            {
                www = UnityWebRequest.Get(StartUpConfig.url + "/AssetBundles/" + names[i]);
                downObjName = $"{names[i]}";
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError
                || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    descText.text = $"网络更新资源文件错误:{www.error}";
                }
                else if (www.isDone)
                    File.WriteAllBytes(StartUpConfig.AssetBundlePath + "/" + names[i], www.downloadHandler.data);
            }
            monitor.Progress = 0.8f;
            yield return LoadAssetBundleFromMemoryAsync(true);
        }
    }

    private IEnumerator UpdateFromLocal()
    {
        monitor.Progress = 0.4f;
        descText.text = "正在本地更新...";
        yield return new WaitForSeconds(descShowTime);

        descText.text = "正在替换本地更新资源...";
        CopyLanguageFromStreamingAssets();
        CopyDllFromStreamingAssets();
        LoadDllFromMemory();
        yield return new WaitForSeconds(descShowTime);
        yield return CopyAssetBundleFromStreamingAssets();
    }

    private IEnumerator LoadFromMemory()
    {
        LoadDllFromMemory();
        yield return LoadAssetBundleFromMemoryAsync(false);
    }

    private void LoadDllFromMemory()
    {
        for (int i = 0; i < dllList.Count; i++)
        {
            string path = StartUpConfig.DllPath + "/" + dllList[i];
            if (!File.Exists(path))
            {
                StartUpUtils.GetFromStreamingAssets("Lib/" + dllList[i], (data) =>
                {
                    File.WriteAllBytes(path, data);
                    Assembly.Load(data);
                });
            }
            else
                Assembly.Load(File.ReadAllBytes(path));
        }
    }

    private void CopyLanguageFromStreamingAssets()
    {
        StartUpUtils.GetFromStreamingAssets("language.json", (data) =>
        {
            Debug.Log("Get language.json FromStreamingAssets succeed");
            File.WriteAllBytes(StartUpConfig.LanguagePath, data);
        });
    }

    private void CopyDllFromStreamingAssets()
    {
        for (int i = 0; i < dllList.Count; i++)
        {
            string path = StartUpConfig.DllPath + "/" + dllList[i];
            StartUpUtils.GetFromStreamingAssets("Lib/" + dllList[i], (data) =>
            {
                File.WriteAllBytes(path, data);
            });
        }
    }

    private IEnumerator CopyAssetBundleFromStreamingAssets()
    {
        AssetBundleCreateRequest createRequest = null;
        StartUpUtils.GetFromStreamingAssets("AssetBundles/AssetBundles", (data) =>
        {
            File.WriteAllBytes(StartUpConfig.AssetBundlePath + "/AssetBundles", data);
            createRequest = AssetBundle.LoadFromMemoryAsync(data);
        });
        monitor.Progress = 0.6f;
        progressSlider.step *= 5;
        yield return createRequest;
        if (createRequest != null)
        {
            AssetBundle bundle = createRequest.assetBundle;
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] names = manifest.GetAllAssetBundles();
            GameObject prefab = null;
            descText.text = "正在加载资源...";
            for (int i = 0; i < names.Length; i++)
            {
                createRequest = null;
                StartUpUtils.GetFromStreamingAssets("AssetBundles/" + names[i], (data) =>
                {
                    File.WriteAllBytes(StartUpConfig.AssetBundlePath + "/" + names[i], data);
                    createRequest = AssetBundle.LoadFromMemoryAsync(data);
                });
                yield return createRequest;
                if (createRequest != null)
                {
                    bundle = createRequest.assetBundle;
                    StartUpConfig.bundleDic[names[i]] = bundle;
                    if (names[i] == "load")
                        prefab = bundle.LoadAsset<GameObject>("load");
                }
            }
            if (prefab != null)
            {
                File.WriteAllText(StartUpConfig.VersionPath, JsonConvert.SerializeObject(localVersionData));
                curVersionData = localVersionData;
                PlayerPrefs.SetString("CurVersion", curVersionData.Version);
                PlayerPrefs.GetString("CurVersion", "0.0.0");
                versionText.text = $"Version: {curVersionData.Version}";
                monitor.Progress = 1f;
                yield return new WaitForSeconds(descShowTime);
                GameObject gameObject = Instantiate(prefab, transform);
            }
        }
    }

    private IEnumerator LoadAssetBundleFromMemoryAsync(bool isNetUpdate)
    {
        monitor.Progress = 0.6f;
        progressSlider.step *= 5;
        descText.text = "正在加载资源...";
        AssetBundleCreateRequest createRequest = null;
        bool needCopy = !File.Exists(StartUpConfig.AssetBundlePath + "/AssetBundles");
        if (needCopy)
        {
            StartUpUtils.GetFromStreamingAssets("AssetBundles/AssetBundles", (data) =>
            {
                File.WriteAllBytes(StartUpConfig.AssetBundlePath + "/AssetBundles", data);
                createRequest = AssetBundle.LoadFromMemoryAsync(data);
            });
        }
        else
            createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(StartUpConfig.AssetBundlePath + "/AssetBundles"));
        yield return createRequest;
        GameObject prefab = null;
        if (createRequest != null)
        {
            AssetBundle bundle = createRequest.assetBundle;
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] names = manifest.GetAllAssetBundles();
            for (int i = 0; i < names.Length; i++)
            {
                createRequest = null;
                needCopy = !File.Exists(StartUpConfig.AssetBundlePath + "/" + names[i]);
                if (needCopy)
                {
                    StartUpUtils.GetFromStreamingAssets("AssetBundles/" + names[i], (data) =>
                    {
                        File.WriteAllBytes(StartUpConfig.AssetBundlePath + "/" + names[i], data);
                        createRequest = AssetBundle.LoadFromMemoryAsync(data);
                    });
                }
                else
                    createRequest = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(StartUpConfig.AssetBundlePath + "/" + names[i]));
                
                yield return createRequest;
                if (createRequest != null)
                {
                    bundle = createRequest.assetBundle;
                    StartUpConfig.bundleDic[names[i]] = bundle;
                    if (names[i] == "load")
                        prefab = bundle.LoadAsset<GameObject>("load");
                }
            }
            if (prefab != null)
            {
                if (isNetUpdate)
                {
                    curVersionData = netVersionData;
                    File.WriteAllText(StartUpConfig.VersionPath, JsonConvert.SerializeObject(curVersionData));
                    PlayerPrefs.SetString("CurVersion", curVersionData.Version);
                    versionText.text = $"Version: {curVersionData.Version}";
                }
                monitor.Progress = 1f;
                yield return new WaitForSeconds(descShowTime);
                GameObject gameObject = Instantiate(prefab, transform);
            }
        }
    }

    private void Update()
    {
        if (www != null && www.downloadProgress != 1 && www.downloadProgress != 0)
        {
            downloadDescText.text = $"{downObjName}:" + (www.downloadProgress * 100).ToString("#0") + "%";
            Debug.Log(descText.text + ":" + www.downloadProgress);
        }
        else
            downloadDescText.text = "";
    }
}
