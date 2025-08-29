using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ParticleSystemManager : Singleton<ParticleSystemManager>
{
    [Header("Particle Pool Settings")]
    private Queue<ParticleSystem> particlePool = new Queue<ParticleSystem>();
    private List<ParticleSystem> activeParticles = new List<ParticleSystem>(); // To track active particles for returning to pool

    public override void Awake()
    {
        base.Awake();
        InitializePool();
    }

    private void InitializePool()
    {
        // No prefab specified, so we'll assume we're pooling existing particle systems
        // or that the PlayParticles method will receive a prefab to instantiate.
        // For a generic pool, we might need a base ParticleSystem prefab.
        // For now, we'll just ensure the pool is ready to receive and manage.
    }

    public ParticleSystem GetPooledParticleSystem(ParticleSystem prefab)
    {
        ParticleSystem psToPlay;
        if (particlePool.Count > 0)
        {
            psToPlay = particlePool.Dequeue();
        }
        else
        {
            // If pool is empty, instantiate a new one from the prefab
            psToPlay = Instantiate(prefab, this.transform);
            psToPlay.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Ensure it's stopped and clear
        }
        psToPlay.gameObject.SetActive(true);
        return psToPlay;
    }

    public void ReturnPooledParticleSystem(ParticleSystem ps)
    {
        if (ps != null && activeParticles.Contains(ps))
        {
            activeParticles.Remove(ps);
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.gameObject.SetActive(false);
            particlePool.Enqueue(ps);
        }
    }

    public void PlayParticles(ParticleSystem prefab, Transform parent = null, Vector3 position = default, Quaternion rotation = default, bool asTransient = false)
    {
        if (prefab == null)
        {
            Debug.LogWarning("ParticleSystemManager: Attempted to play a null ParticleSystem prefab.");
            return;
        }

        ParticleSystem ps = GetPooledParticleSystem(prefab);
        if (parent != null)
        {
            ps.transform.SetParent(parent);
        }
        else
        {
            ps.transform.SetParent(this.transform);
        }
        ps.transform.position = position;
        ps.transform.rotation = rotation;
        ps.Play();
        activeParticles.Add(ps);
        // Schedule returning to pool after its duration
        StartCoroutine(ReturnToPoolAfterDelay(ps, ps.main.duration + ps.main.startLifetime.constantMax, asTransient));
    }

    private System.Collections.IEnumerator ReturnToPoolAfterDelay(ParticleSystem ps, float delay, bool asTransient)
    {
        yield return new WaitForSeconds(delay);
        if (asTransient)
        {
            Destroy(ps.gameObject);
        }
        else
        {
            ReturnPooledParticleSystem(ps);
        }
    }
}