using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    //音频管理器 存储所有的音频并且可以播放和停止
    [Serializable]
    public class Sound
    {
        [Header("音频剪辑")] public AudioClip clip;

        [Header("音频分组")] public AudioMixerGroup outputGroup;

        [Header("音频音量")][Range(0, 1)] public float volume;

        [Header("音频是否自启动")] public bool PlayOnAwake;

        [Header("音频是否要循环播放")] public bool loop;
    }

    public List<Sound> sounds; //存储所有音频的信息

    private Dictionary<string, AudioSource> audioDic; //每一个音频的名称组件

    public static string BGM_MainManu = "标题BGM和视觉小说最后一段BGM";
    public static string Effect_TOUCH = "TOUCH";
    public static string Effect_Change = "消弹转换触发";
    public static string BGM_Fight = "战斗部分BGM";
    public static string Effect_Victory = "战斗胜利";
    public static string Effect_Failed = "战斗失败";
    public static string Effect_UIClick = "游戏内UI按下音效";


    public static string BGM_Novel = "视觉小说BGM";
    public static string Effect_NovelClick = "视觉小说部分点击选项音效";

    public static string Effect_BossHited = "对BOSS造成伤害";
    public static string Effect_PlayerHited = "玩家受到伤害";


    private string _current = "";



    private void Start()
    {
        audioDic = new Dictionary<string, AudioSource>();
        foreach (var sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.playOnAwake = sound.PlayOnAwake;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.outputGroup;


            if (sound.PlayOnAwake)
            {
                source.Play();
            }
            audioDic.Add(sound.clip.name, source);
        }
    }


    public static void PlayBgm(string name)
    {
        PlayAudio(name, true, true);
    }

    public static void PlayEffect(string name)
    {
        PlayAudio(name, false, false, true);
    }

    /// <summary>
    /// 是否循环，是否打断当前播放
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isLoop"></param>
    /// <param name="isStopCurrent"></param>
    public static void PlayAudio(string name, bool isLoop = false, bool isStopCurrent = false, bool isEffect = false)
    {
        Debug.Log(name);
        if (!Instance.audioDic.ContainsKey(name))
        {
            //不存在音频
            Debug.LogError("不存在" + name + "音频");
            return;
        }
        if (isEffect)
        {
            Debug.Log("Effect");
            Instance.audioDic[name].PlayOneShot(Instance.audioDic[name].clip);
            return;
        }
        if (Instance.audioDic[name].isPlaying)
        {
            return;
        }
        //直接播放
        if (isStopCurrent)
        {
            StopCurrent();
        }
        Instance.audioDic[name].loop = isLoop;
        Instance.audioDic[name].Play();
        Instance._current = name;
    }

    private static void StopCurrent()
    {
        foreach (AudioSource source in Instance.audioDic.Values)
        {
            source.Stop();
        }
        //if (Instance.audioDic.ContainsKey(Instance._current))
        //{
        //    Instance.audioDic[Instance._current].Stop();
        //}
    }


    //停止音频的播放
    public static void StopMute(string name)
    {
        if (!Instance.audioDic.ContainsKey(name))
        {
            //不存在次音频
            Debug.LogError("不存在" + name + "音频");
            return;
        }
        else
        {
            Instance.audioDic[name].Stop();
        }
    }
}