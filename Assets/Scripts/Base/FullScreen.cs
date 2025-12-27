using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    private void Awake()
    {
        //获取设置当前屏幕分辩率 
        Resolution[] resolutions = Screen.resolutions;
        //设置当前分辨率 
        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);

        Screen.fullScreen = true;

    }
}
