using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalReference : MonoBehaviour
{
    public static GlobalReference Instance { get; private set; }

    public GameObject bulletEffectPrefab;

    //Throwable
    public GameObject smokeGrenadeEffect;
    public GameObject grenadeExplosionEffect;

    public GameObject bloodSprayEffect;



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
