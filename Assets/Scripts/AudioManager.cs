using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip bgm;
    private AudioSource[] audioSources;
    private AudioSource bgmAudioSource;   // �̹� �������� BGM ������
    private bool isBGMOn = true;
    private bool isSFXOn = true;


    private void Awake()
    {
        // �̱���
        if(Instance == null)
        {
            Instance = this;
        }
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        audioSources = new AudioSource[5];
        for (int i=0; i < audioSources.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }
    

    public void PlayClip(AudioClip clip)
    {
        float playSpeed = 1.0f;
        PlayClip(clip, playSpeed);
    }
    public void PlayClip(AudioClip clip, float playSpeed)
    {
        if (!isSFXOn) return;
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.pitch = playSpeed;
                source.Play();
                return;
            }
        }
    }

    public void PlayBGM()
    {
        float playSpeed = 1.0f;
        PlayBGM(bgm, playSpeed);
    }
    public void PlayBGM(float playSpeed)
    {
        PlayBGM(bgm, playSpeed);
    }
    public void PlayBGM(AudioClip clip)
    {
        float playSpeed = 1.0f;
        PlayBGM(clip, playSpeed);
    }
    public void PlayBGM(AudioClip clip, float playSpeed)
    {
        if( playSpeed > 0f ) bgmAudioSource.pitch = playSpeed;
        if(!isBGMOn) return;
        if(bgmAudioSource.isPlaying)bgmAudioSource.Stop();
        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        if(bgmAudioSource.isPlaying)bgmAudioSource.Stop();
    }
    
    public void TurnOffBGM()
    {
        isBGMOn = false;
        StopBGM();
    }

    public void TurnOffSFX()
    {
        isSFXOn = false;
        for(int i=0; i < audioSources.Length; i++)
        {
            if(audioSources[i].isPlaying) audioSources[i].Stop();
        }
    }

    public void TurnOnBGM()
    {
        isBGMOn = true;
        if(!bgmAudioSource.isPlaying) PlayBGM(-1f);  // �����ӵ�����
    }

    public void TurnOnSFX()
    {
        isSFXOn = true;
    }

    public void PauseSound()
    {
        for(int i=0; i < audioSources.Length; i++)
        {
            bgmAudioSource.Pause();
            audioSources[i].Pause();
        }
    }

    public void UnPauseSound()
    {
        for(int i=0; i < audioSources.Length; i++)
        {
            bgmAudioSource.UnPause();
            audioSources[i].UnPause();
        }
    }

    public void SetVolume(bool bgmMute, bool sfxMute, float bgmVolume, float sfxVolume)
    {
        if(bgmMute) TurnOffBGM();
        if(sfxMute) TurnOffSFX();
        if(!bgmMute) TurnOnBGM();
        if(!sfxMute) TurnOnSFX();

        bgmAudioSource.volume = bgmVolume;
        for(int i=0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = sfxVolume;
        }
    }
}
