using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Shooting")]
    public AudioSource shootingChanel;
    public AudioClip shootingSound;
    public AudioClip emptySound;

    [Header("Reload")]
    public AudioSource reloadChanel;
    public AudioClip reloadSound;

    [Header("Throwable")]
    public AudioSource throwableChanel;
    public AudioClip grenadeSound;
    public AudioClip smokeGrenadeSound;



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
