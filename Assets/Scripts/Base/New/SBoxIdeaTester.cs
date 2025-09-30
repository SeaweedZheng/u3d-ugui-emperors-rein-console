using SBoxApi;
using GameUtil;
using UnityEngine;

public partial class SBoxIdeaTester 
{


    public static void ChangePassword(int password)
    {
        int result = permissions >= 1 ? 0 : 1;
        if (permissions == 1)
        {
            passwordUser = password;
        }
        else if (permissions == 2)
        {
            passwordManager = password;
        }
        else if (permissions == 3)
        {
            passwordAdmin = password;
        }

        SBoxPermissionsData sBoxPermissionsData = new SBoxPermissionsData()
        {
            result = result,
            permissions = permissions
        };
        EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_CHANGE_PASSWORD, sBoxPermissionsData);
    }



    public static void CheckPassword(int password)
    {
        Timer.DelayAction(0.1f, () =>
        {
            int result = 1;
            if (password == passwordUser)
            {
                result = 0;
                permissions = 1;
            }
            else if (password == passwordManager)
            {
                result = 0;
                permissions = 2;
            }
            else if (password == passwordAdmin)
            {
                result = 0;
                permissions = 3;
            }
            // // 0：无任何修改参数的权限，1：普通密码权限，2：管理员密码权限，3：超级管理员密码权限
            SBoxPermissionsData sBoxPermissionsData = new SBoxPermissionsData()
            {
                result = result,
                permissions = permissions
            };

            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_CHECK_PASSWORD, sBoxPermissionsData);
        });
    }


}



public partial class SBoxIdeaTester
{

    static int permissions = 0;



    static int passwordUser
    {
        get
        {
            if (_passwordUser == null)
                _passwordUser = PlayerPrefs.GetInt(TEST_SBOX_PASSWORD_USER, 666666);
            return (int)_passwordUser;
        }
        set
        {
            _passwordUser = value;
            PlayerPrefs.SetInt(TEST_SBOX_PASSWORD_USER, value);
        }
    }
    const string TEST_SBOX_PASSWORD_USER = "TEST_SBOX_PASSWORD_USER";
    static int? _passwordUser;



    static int passwordManager
    {
        get
        {
            if (_passwordManager == null)
                _passwordManager = PlayerPrefs.GetInt(TEST_SBOX_PASSWORD_MANAGER, 88888888);
            return (int)_passwordManager;
        }
        set
        {
            _passwordManager = value;
            PlayerPrefs.SetInt(TEST_SBOX_PASSWORD_MANAGER, value);
        }
    }
    const string TEST_SBOX_PASSWORD_MANAGER = "TEST_SBOX_PASSWORD_MANAGER";
    static int? _passwordManager;


    static int passwordAdmin
    {
        get
        {
            if (_passwordAdmin == null)
                _passwordAdmin = PlayerPrefs.GetInt(TEST_SBOX_PASSWORD_ADMIN, 187653214);
            return (int)_passwordAdmin;
        }
        set
        {
            _passwordAdmin = value;
            PlayerPrefs.SetInt(TEST_SBOX_PASSWORD_ADMIN, value);
        }
    }
    const string TEST_SBOX_PASSWORD_ADMIN = "TEST_SBOX_PASSWORD_ADMIN";
    static int? _passwordAdmin;
}