using UnityEngine;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    [Header("SFX Pool Settings")]
    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private GameObject audioSourcePrefab; 

    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private List<AudioSource> activeSfx = new List<AudioSource>();

    public override void Awake()
    {
        base.Awake();
        InitializePool();
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
            source.clip = null; // Clear clip
            source.gameObject.SetActive(false); // Deactivate
            sfxPool.Enqueue(source);
        }
    }
}