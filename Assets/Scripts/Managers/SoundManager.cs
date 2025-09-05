using System;
using UnityEngine;
using System.Collections.Generic;
using Service_Locator;

public class SoundManager : Singleton<SoundManager>
{
    [Header("SFX Pool Settings")]
    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private GameObject audioSourcePrefab; 
    
    [Space(20)] 
    [SerializeField] private AudioSource _backgroundMusic;
    public AudioSource BackgroundMusic => _backgroundMusic;
    
    private Queue<AudioSource> sfxPool = new();
    private List<AudioSource> activeSfx = new();

    public override void Awake()
    {
        ServiceLocator.Global.Register(this);
        base.Awake();
        InitializePool();
    }

    private void Start()
    {
        if(_backgroundMusic != null && _backgroundMusic.loop == false) _backgroundMusic.loop = true;
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject go = new GameObject("PooledAudioSource");
        go.transform.SetParent(transform);
        AudioSource newSource = go.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        sfxPool.Enqueue(newSource);
        newSource.gameObject.SetActive(false);
        return newSource;
    }

    public void PlaySFX(AudioClip clip, Vector2 position = default, float volume = 0.7f, float pitch = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundManager: Attempted to play a null AudioClip.");
            return;
        }

        AudioSource sourceToPlay;
        if (sfxPool.Count > 0)
        {
            sourceToPlay = sfxPool.Dequeue();
        }
        else
        {
            sourceToPlay = CreateNewAudioSource(); // Expand pool if needed
            sourceToPlay.gameObject.SetActive(true);
        }

        sourceToPlay.clip = clip;
        sourceToPlay.volume = volume;
        sourceToPlay.pitch = pitch;
        sourceToPlay.gameObject.SetActive(true); // Ensure it's active

        sourceToPlay.Play();
        activeSfx.Add(sourceToPlay);

        // Schedule returning to pool after clip finishes
        StartCoroutine(ReturnToPoolAfterDelay(sourceToPlay, clip.length));
    }

    private System.Collections.IEnumerator ReturnToPoolAfterDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (activeSfx.Contains(source))
        {
            activeSfx.Remove(source);
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
            sfxPool.Enqueue(source);
        }
    }

    public void PlayBackgroundMusic()
    {
        _backgroundMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        _backgroundMusic.Stop();
    }
}