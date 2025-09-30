using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class Utils
{
    private static System.Random _rand;
    private static Material _uiGreyMat = null;
    public static void SetRandomSeed()
    {
        _rand = new System.Random((int)System.DateTime.Now.Ticks);
    }
    public static int GetRandom(int from, int to)
    {
        if (_rand == null) { SetRandomSeed(); }
        return _rand.Next(from, to);
    }

    public static byte[] ToByteArray(string str)
    {
        byte[] send = System.Text.Encoding.UTF8.GetBytes(str);
        byte[] old = send;
        send = new byte[old.Length + 5];
        System.BitConverter.GetBytes(old.Length + 1).CopyTo(send, 0);
        send[4] = 0;
        old.CopyTo(send, 5);
        return send;
    }

    public static string LocalIP()
    {
        string AddressIP = string.Empty;
        string IP = "";
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());   //Dns.GetHostName()获取本机名Dns.GetHostAddresses()根据本机名获取ip地址组
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                IP = ip.ToString();  //ipv4
            }
        }
        return IP;
    }

    public static void Post(string url, Dictionary<string, string> dic, Action<bool, string> actionResult = null)
    {
        //StringBuilder builder = new StringBuilder();
        //int i = 0;
        //foreach (var item in dic)
        //{
        //    if (i > 0)
        //        builder.Append("&");
        //    builder.AppendFormat("{0}={1}", item.Key, item.Value);
        //    i++;
        //}
        //byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
        //MonoMgr.Instance.StartCoroutine(_Post(url, data, actionResult));
    }

    private static IEnumerator _Post(string url, byte[] data, Action<bool, string> action)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(data);
            request.SetRequestHeader("content-type", "application/x-www-form-urlencoded");
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            string resstr = "";
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                resstr = request.error;
            else
                resstr = request.downloadHandler.text;

            if (action != null)
                action(request.result == UnityWebRequest.Result.ConnectionError, resstr);
        }
    }

    public static string Post(string url, string jsonData, Action<string> onSucceed = null, Action<string> onError = null)
    {
        try
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "text/plain";
            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes(AESManager.Instance.TryLocalEncrypt(jsonData));
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            onSucceed?.Invoke(result);
            return result;
        }
        catch
        (Exception e)
        {
            onError?.Invoke(e.Message);
            return e.Message;
        }
    }

    public static long GetTimeStamp()
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds;
        return timeStamp;
    }

    public static int GetEnumLength<T>(T enumType)
    {
        string[] enumLength = Enum.GetNames(enumType.GetType());
        return enumLength.Length;
    }

    /// <summary>
    /// 按长度分割字符串，汉字按一个字符算
    /// </summary>
    /// <param name="SourceString"></param>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static List<string> SplitLength(string SourceString, int Length)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < SourceString.Trim().Length; i += Length)
        {
            if ((SourceString.Trim().Length - i) >= Length)
                list.Add(SourceString.Trim().Substring(i, Length));
            else
                list.Add(SourceString.Trim().Substring(i, SourceString.Trim().Length - i));
        }
        return list;
    }

    public static void FileWriteByCreate(string content, string outFilePath)
    {
        FileStream fs = new FileStream(outFilePath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(content);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    public static void SaveObjectToJsonFile<T>(T data, string path)
    {
        TextWriter tw = new StreamWriter(path);
        if (tw == null)
        {
            Debug.LogError("Cannot write to " + path);
            return;
        }

        string jsonStr = JsonConvert.SerializeObject(data);
        tw.Write(jsonStr);
        tw.Flush();
        tw.Close();
    }

    public static T LoadObjectFromJsonFile<T>(string path)
    {
        TextReader reader = new StreamReader(path);
        if (reader == null)
        {
            Debug.LogError("Cannot find " + path);
            reader.Close();
            return default(T);
        }

        T data = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        if (data == null)
        {
            Debug.LogError("Cannot read data from " + path);
        }

        reader.Close();
        return data;
    }

    public static void ReadGameData()
    {
        //string url = "http://8.138.140.180:8124/SoloHotfix/JackpotDevice/gameData.json";
        //UnityWebRequest www;
        //www = UnityWebRequest.Get(url);
        //www.timeout = 3;
        //var operation = www.SendWebRequest();
        //while (!operation.isDone) { }
        //if (www.result == UnityWebRequest.Result.ConnectionError
        //    || www.result == UnityWebRequest.Result.ProtocolError)
        //{
        //    Debug.LogError("getGameData:"+www.error);
        //    string localPath;
        //    if (Application.platform == RuntimePlatform.Android)
        //        localPath = Application.streamingAssetsPath + "/gameData.json";
        //    else
        //        localPath = "file:///" + Application.streamingAssetsPath + "/gameData.json";
        //    www = UnityWebRequest.Get(localPath);
        //    operation = www.SendWebRequest();
        //    while (!operation.isDone)
        //    { }
        //    if (www.result == UnityWebRequest.Result.ConnectionError
        //        || www.result == UnityWebRequest.Result.ProtocolError)
        //        Debug.LogError(www.error);
        //    else
        //        IOCanvasModel.Instance.gameDataDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(www.downloadHandler.text);
        //}
        //else
        //    IOCanvasModel.Instance.gameDataDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(www.downloadHandler.text);
    }

    private static void ReadLanguage()
    {
        string url = "http://8.138.140.180:8124/SoloHotfix/JackpotDevice/language.json";
        UnityWebRequest www;
        www = UnityWebRequest.Get(url);
        www.timeout = 3;
        var operation = www.SendWebRequest();
        while (!operation.isDone) { }
        if (www.result == UnityWebRequest.Result.ConnectionError
            || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
            string localPath;
            if (Application.platform == RuntimePlatform.Android)
                localPath = Application.streamingAssetsPath + "/language.json";
            else
                localPath = "file:///" + Application.streamingAssetsPath + "/language.json";
            www = UnityWebRequest.Get(localPath);
            operation = www.SendWebRequest();
            while (!operation.isDone)
            { }
            if (www.result == UnityWebRequest.Result.ConnectionError
                || www.result == UnityWebRequest.Result.ProtocolError)
                Debug.LogError(www.error);
            else
                IOCanvasModel.Instance.languageDic = JsonConvert.DeserializeObject<Dictionary<Language, Dictionary<string, string>>>(www.downloadHandler.text);
        }
        else
            IOCanvasModel.Instance.languageDic = JsonConvert.DeserializeObject<Dictionary<Language, Dictionary<string, string>>>(www.downloadHandler.text);
    }

    public static string GetLanguage(string str)
    {
        if (IOCanvasModel.Instance.languageDic == null)
            ReadLanguage();
        IOCanvasModel.Instance.languageDic[IOCanvasModel.Instance.curlanguage].TryGetValue(str, out string temp);
        if (!string.IsNullOrEmpty(temp))
            return temp;
        return str;
    }

    public static string[] GetEnumNames(Type enumType)
    {
        string[] names = Enum.GetNames(enumType);
        for (int i = 0; i < names.Length; i++)
            names[i] = GetLanguage(names[i]);
        return names;
    }

    private static Material UIGreyMat
    {
        get
        {
            if (!_uiGreyMat)
            {
                var shader = Shader.Find("UI/UIGray");
                if (shader)
                {
                    _uiGreyMat = new Material(shader);
                    _uiGreyMat.hideFlags = HideFlags.HideAndDontSave;
                }
            }
            return _uiGreyMat;
        }
    }
    public static void SetUIGray(GameObject uiObj, bool isGrey = true)
    {
        var graphics = uiObj.GetComponentsInChildren<Graphic>();
        int length = graphics.Length;
        for (int i = 0; i < length; i++)
        {
            graphics[i].material = isGrey ? UIGreyMat : null;
        }
    }

    public static T DeepCopy<T>(T obj)
    {
        if (obj is string || obj.GetType().IsValueType) return obj;
        object retval = Activator.CreateInstance(obj.GetType());
        System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(System.Reflection.BindingFlags.Public
            | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
        foreach (var field in fields)
        {
            try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
            catch { }
        }
        return (T)retval;
    }
}
