using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCameraSwitch : MonoBehaviour
{
    void Awake()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
        }
    }
}
