using Newtonsoft.Json;
using SBoxApi;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SBoxIdeaHelper : MonoSingleton<SBoxIdeaHelper>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnDestroy()
    {

        base.OnDestroy();
    }
    void Update()
    {
        
    }


    bool isTest => Application.isEditor;

    string clsName => isTest ? "SBoxIdeaTests" : "SBoxIdea";

    public void LogRpcUp(string name, object obj)
    {
        string data = $"{obj}";

        if (obj is string)
        {
            data = (string)obj;
        }
        else
        {
            try
            {
                string str = JsonConvert.SerializeObject(obj);
                data = str;
            }
            catch { }
        }

        Debug.Log($"¡¾{clsName}¡¿<color=green>rpc up</color> name: {name} ; req: {JsonConvert.SerializeObject(data)}");
    }

    public void LogRpcDown(string name, object obj)
    {
        string data = $"{obj}";

        if (obj is string)
        {
            data = (string)obj;
        }
        else
        {
            try
            {
                string str = JsonConvert.SerializeObject(obj);
                data = str;
            }
            catch { }
        }

        Debug.Log($"¡¾{clsName}¡¿<color=yellow>rpc down</color> name: {name} ; req: {JsonConvert.SerializeObject(data)}");
    }



    public static void ChangePassword(int password)
    {
        Instance.LogRpcUp(MethodBase.GetCurrentMethod().Name, password);
        if (Instance.isTest)
            SBoxIdeaTester.ChangePassword(password);
        else
            SBoxIdea.ChangePassword(password);

    }
    void OnChangePassword(SBoxPermissionsData res) => Instance.LogRpcDown(MethodBase.GetCurrentMethod().Name.Substring(2), res);


    public static void CheckPassword(int password)
    {
        Instance.LogRpcUp(MethodBase.GetCurrentMethod().Name, password);
        if (Instance.isTest)
            SBoxIdeaTester.CheckPassword(password);
        else
            SBoxIdea.CheckPassword(password);
    }
    void OnCheckPassword(SBoxPermissionsData res) => Instance.LogRpcDown(MethodBase.GetCurrentMethod().Name.Substring(2), res);


}
