using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicHeight : MonoBehaviour
{
    public AudioSource audiosource;
    public Slider slider;
    public static bool musiccomplete = false;//保存操作数据，当musiccomplete的值为true时，操作完成
    public static float musicheight;//保存当前滚动条的值

    private void Start()
    {
        slider.value = audiosource.volume;//滚动条的初始值为音频的音量大小
    }

    void Update()
    {
        music_height();
    }

    public void music_height()
    {
        audiosource.volume = slider.value;
        musiccomplete = true;
        musicheight = slider.value;
    }
}

