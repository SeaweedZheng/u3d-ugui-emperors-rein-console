using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// 这个等打下个包时拿掉
/// </summary>
public static class StartUpConfigHelper
{
    public static bool _isRelease = false;
    public static bool isRelease
    {
        // SelfAOT
        get
        {
            try
            {
                //bool flg = StartUpConfig.isRelease;  // 这样写会报错,(try-catch也解决不了)！

                //Type cls = typeof(StartUpConfig);
              
                Assembly hotfixAssembly = Assembly.Load("SelfAOT");
                Type cls = hotfixAssembly.GetType("StartUpConfig");

                bool flg = _isRelease;
                PropertyInfo prop = cls.GetProperty("isRelease", BindingFlags.Static | BindingFlags.Public);
                if (prop != null)
                {
                    // 2. 读取静态属性的值（静态成员传 null）
                    flg = (bool)prop.GetValue(null);
                }
                return flg;
            }
            catch(Exception ex)
            {
                return _isRelease;
            }
        }
    }

}
