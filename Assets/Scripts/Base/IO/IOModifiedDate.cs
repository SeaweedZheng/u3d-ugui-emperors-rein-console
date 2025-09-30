using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOModifiedDate : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("IOArrow");
        transform.Find("dateTimeSection");
        transform.Find("btn");
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
