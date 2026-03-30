using System.Collections.Generic;
using UnityEngine;

public static class StartUpConfig
{
    /*
    static bool _isMachine = false;
    public static bool isMachine
    {
        get => _isMachine;
    }*/


    /// <summary> 是否是正式包 (下个版本加)</summary>
    public static bool isRelease => true;
    
    public static string url => isRelease? "http://chresouce.oss-cn-guangzhou.aliyuncs.com/luomajp" : // 正式服
        "http://chresouce.oss-cn-guangzhou.aliyuncs.com/luomajp/Debug"; // 测试服




    public static Dictionary<string, AssetBundle> bundleDic = new Dictionary<string, AssetBundle>();
    public static string DllPath
    {
        get
        {
            string path = Application.dataPath + "/StreamingAssets/Lib";
            if (!Application.isEditor)
                path = Application.persistentDataPath;
            return path;
        }
    }

    public static string AssetBundlePath
    {
        get
        {
            string path = Application.dataPath + "/StreamingAssets/AssetBundles";
            if (!Application.isEditor)
                path = Application.persistentDataPath;
            return path;
        }
    }

    public static string VersionPath
    {
        get
        {
            string path = Application.dataPath + "/StreamingAssets/Version.txt";
            if (!Application.isEditor)
                path = Application.persistentDataPath + "/Version.txt";
            return path;
        }
    }

    public static string LanguagePath
    {
        get
        {
            string path = Application.dataPath + "/StreamingAssets/language.json";
            if (!Application.isEditor)
                path = Application.persistentDataPath + "/language.json";
            return path;
        }
    }





}
