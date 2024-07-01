using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


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

    [Header("Zombie")]
    public AudioSource zombieChanel;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;

    public AudioSource zombieChanel1;
    public AudioClip zombieDeath;

    [Header("Player")]
    public AudioSource playerChanel;
    public AudioClip playerHurt;
    public AudioClip playerDeath;
    public AudioClip gameOverMusic;

    public static SoundManager Instance { get; private set; }
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
