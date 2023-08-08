using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip bgm;
    private AudioSource[] audioSources;
    private AudioSource bgmAudioSource;   // 이미 실행중인 BGM 추적용

    private void Awake()
    {
        // 싱글톤
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
        if(bgmAudioSource.isPlaying)bgmAudioSource.Stop();
        bgmAudioSource.clip = clip;
        bgmAudioSource.pitch = playSpeed;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }
}
