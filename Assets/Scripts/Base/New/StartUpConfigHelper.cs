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

        get
        {
            try
            {
                Type cls = typeof(StartUpConfigHelper);
                bool flg = _isRelease;
                FieldInfo field = cls.GetField("isRelease", BindingFlags.Static | BindingFlags.Public);
                if (field != null)
                {
                    flg = (bool)field.GetValue(null);
                }
                //bool flg = StartUpConfig.isRelease;  // 这样写会报错,(try-catch也解决不了)！
                return flg;
            }
            catch(Exception ex)
            {
                return _isRelease;
            }
        }
    }

}
