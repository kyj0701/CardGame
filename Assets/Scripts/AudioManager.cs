using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public AudioSource audioSource;

    [SerializeField]
    private float _bgmChangeTime = 20.0f;
    private bool _isStarted = false;
    private float _timer = 0.0f;
    

    private void Start()
    {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    private void Update()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            _timer = 0.0f;
        }

        if (_isStarted)
        {
            _timer += Time.deltaTime;

            if (_timer >= _bgmChangeTime)
            {
                audioSource.pitch = 2.0f;
            }
        }

        if (Time.timeScale == 0)
        {
            audioSource.Stop();
        }

    }
}
