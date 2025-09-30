using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float soundVolumScale = 1f; //音效声音大小比例
    public float SoundVolumScale
    {
        set
        {
            soundVolumScale = value;
            BgmVolumScale = bgmVolumScale;
        }
        get { return soundVolumScale; }
    }

    private float bgmVolumScale = 0.75f; //背景音乐声音大小比例
    public float BgmVolumScale
    {
        set
        {
            bgmVolumScale = value;
            musicAudio.volume = bgmVolumScale * soundVolumScale;
            PlayerPrefs.SetFloat("bgmVolumScale", bgmVolumScale);
        }
        get { return bgmVolumScale; }
    }

    private float effVolumScale = 1f;   //音效声音大小比例
    public float EffVolumScale
    {
        set
        {
            effVolumScale = value;
            PlayerPrefs.SetFloat("effVolumScale", effVolumScale);
        }
        get { return effVolumScale; }
    }

    private float voiceVolumScale = 1f; //人声声音大小比例
    public float VoiceVolumScale
    {
        set
        {
            voiceVolumScale = value;
            PlayerPrefs.SetFloat("voiceVolumScale", voiceVolumScale);
        }
        get { return voiceVolumScale; }
    }

    private AudioSource musicAudio;     //背景音乐播放器
    private Dictionary<string, AudioClip> audioClipResDic = new Dictionary<string, AudioClip>();
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = Init();
            return instance;
        }
    }
    // Start is called before the first frame update
    private static AudioManager Init()
    {
        GameObject obj = new GameObject("SoundManager");
        AudioManager soundManager = obj.AddComponent<AudioManager>();
        obj.AddComponent<AudioPool>();
        obj.AddComponent<AudioListener>();
        return soundManager;
    }
    private void Awake()
    {
        //存储关闭时的音量比例
        if (PlayerPrefs.HasKey("bgmVolumScale"))
            bgmVolumScale = PlayerPrefs.GetFloat("bgmVolumScale");
        if (PlayerPrefs.HasKey("effVolumScale"))
            effVolumScale = PlayerPrefs.GetFloat("effVolumScale");
        if (PlayerPrefs.HasKey("voiceVolumScale"))
            voiceVolumScale = PlayerPrefs.GetFloat("voiceVolumScale");

        musicAudio = gameObject.AddComponent<AudioSource>();
        musicAudio.playOnAwake = false;
        musicAudio.loop = true;
        musicAudio.volume = bgmVolumScale;
        DontDestroyOnLoad(gameObject);
    }

    //解暂停
    public void UnPause()
    {
        musicAudio.UnPause();
        AudioController.UnpauseAll();
    }
    //暂停所有声音
    public void Pause()
    {
        musicAudio.Pause();
        AudioController.PauseAll();
    }
    //停止所有声音
    public void Stop()
    {
        musicAudio.Stop();
        AudioController.StopAll();
    }
    //设置静音
    public void SetMute(bool isMute)
    {
        musicAudio.mute = isMute;
        AudioController.MuteAll();
    }

    //public float Get

    //播放背景音乐
    public void PlayMusic(string music)
    {
        if (IsPlayMusic(music))
            return;
        music = $"bgm/{music}";
        AudioClip clip = GetClipByName(music);
        PlayMusic(clip);
    }

    public void StopMusic()
    {
        musicAudio.Stop();
    }
    //是否正在播放music
    private bool IsPlayMusic(string music)
    {
        if (musicAudio.isPlaying && musicAudio.clip != null)
            return musicAudio.clip.name == music;

        return false;
    }

    public bool IsPlaySoundEff(string str)
    {
        if (AudioController.IsPlaying(str))
            return true;
        return false;
    }

    //播放背景音乐
    private void PlayMusic(AudioClip clip)
    {
        musicAudio.loop = true;
        musicAudio.clip = clip;
        musicAudio.Play();
    }

    //播放2d音效  eff  loop 是否循环
    public void PlaySoundEff(string eff, bool loop = false)
    {
        string path = "sfx/" + eff;
        AudioClip clip = GetClipByName(path);
        PlaySoundEff(clip, loop);
    }

    //播放2d音效  clip  loop 是否循环
    private void PlaySoundEff(AudioClip clip, bool loop)
    {
        if (clip == null)
            return;
        AudioController audioController = AudioPool.Instance.GetController();
        audioController.SetSourceProperties(clip, effVolumScale * soundVolumScale * 0.5f, 1, loop, 0);
        audioController.Play();
    }

    public void PlayVoiceEff(string voice, bool loop = false)
    {
        string path = IOCanvasModel.Instance.curlanguage == Language.en ? $"voice/en_{voice}" : $"voice/ch_{voice}";
        AudioClip clip = GetClipByName(path);
        PlayVoiceEff(clip, loop);
    }

    private void PlayVoiceEff(AudioClip clip, bool loop)
    {
        if (clip == null)
            return;
        AudioController audioController = AudioPool.Instance.GetController();
        audioController.SetSourceProperties(clip, voiceVolumScale * soundVolumScale * 0.5f, 1, loop, 0);
        audioController.Play();
    }

    public void StopAllSoudEff()
    {
        AudioController.StopAll();
    }

    public void StopSoundEff(string name)
    {
        AudioController.Stop(name);
    }

    //停止某音效
    public void StopSoundEff(AudioClip clip)
    {
        AudioController.Stop(clip);
    }

    //设置音效音量
    public void SetSoundEffVolume(string eff, float volume)
    {
        AudioController.SetVolume(eff, volume);
    }

    public AudioClip GetClipByName(string name)
    {
        if (!audioClipResDic.ContainsKey(name))
            audioClipResDic[name] = ResMgr.Instance.Load<AudioClip>($"sound/{name}");
        return audioClipResDic[name];
    }
}
