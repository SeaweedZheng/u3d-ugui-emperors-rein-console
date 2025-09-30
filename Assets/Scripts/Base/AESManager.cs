using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class AESManager : BaseManager<AESManager>
{
    private readonly string localKey = "a4d58c5f125f2c16dd65edd0d4ab45e2485a95042741a740e5012f88e6e1df64";
    private readonly string localIv = "be7369b599f7d5bc2e102a3db2a4bfdd";

    private byte[] localKeyBytes;
    private byte[] localIvBytes;

    private byte[] keyBytes;
    private byte[] ivBytes;

    private bool isInit;

    public void Init()
    {
        localKeyBytes = StringToByteArray(localKey);
        localIvBytes = StringToByteArray(localIv).Take(16).ToArray();

        keyBytes = StringToByteArray(localKey);
        ivBytes = StringToByteArray(localIv).Take(16).ToArray();

        isInit = true;
    }

    public void RefreshKey(string key, string iv)
    {
        keyBytes = StringToByteArray(key);
        ivBytes = StringToByteArray(iv).Take(16).ToArray();
    }

    public string TryLocalEncrypt(string plainText)
    {
        if (!isInit) Init();
        return UnityEncrypt(plainText, localKeyBytes, localIvBytes);
    }

    public string TryLocalDecrypt(string cipherText)
    {
        if (!isInit) Init();
        return UnityDecrypt(cipherText, localKeyBytes, localIvBytes);
    }

    public string TryEncrypt(string plainText)
    {
        if (!isInit) Init();
        return UnityEncrypt(plainText, keyBytes, ivBytes);
    }

    public string TryDecrypt(string cipherText)
    {
        if (!isInit) Init();
        return UnityDecrypt(cipherText, keyBytes, ivBytes);
    }

    public string UnityEncrypt(string plainText, byte[] keyBytes, byte[] ivBytes)
    {
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public string UnityDecrypt(string cipherText, byte[] keyBytes, byte[] ivBytes)
    {
        try
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Decode Error: {cipherText}\n{e.Message}");
            return "";
        }
        
    }

    private byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
