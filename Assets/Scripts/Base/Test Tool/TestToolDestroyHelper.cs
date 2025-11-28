using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToolDestroyHelper : MonoBehaviour
{
    void Start()
    {
        if(StartUpConfigHelper.isRelease)
            GameObject.Destroy(gameObject);
    }
}
