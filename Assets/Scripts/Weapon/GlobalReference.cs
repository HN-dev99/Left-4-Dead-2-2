using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReference : MonoBehaviour
{
    public static GlobalReference Instance { get; private set; }

    public GameObject bulletEffectPrefab;

    //Throwable
    public GameObject smokeGrenadeEffect;
    public GameObject grenadeExplosionEffect;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
