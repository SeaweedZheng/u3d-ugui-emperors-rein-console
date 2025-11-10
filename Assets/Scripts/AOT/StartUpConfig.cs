using System.Collections.Generic;
using UnityEngine;

public static class StartUpConfig
{

    // 测试服
    //public const string url = "http://8.138.140.180:8124/EmperorsRein200/ConsoleTest";

    // 正式服
    public const string url = "http://chresouce.oss-cn-guangzhou.aliyuncs.com/luomajp";


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




    static bool _isMachine = false;
    public static bool isMachine
    {
        get => _isMachine;
    }

}
