using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle 
{
    [MenuItem("Tools/Build AssetBundles")]
    public static void BulidAllAssetBundles()
    {
        string dir =  StartUpConfig.AssetBundlePath;
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.Android);
        Debug.Log("ab包打包完成...");
    }
}
