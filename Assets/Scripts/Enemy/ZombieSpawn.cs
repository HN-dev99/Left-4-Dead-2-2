using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public ObjectPool zombiePool;
    public float delayTime = 5f;
    public int initialZombiesPerWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 20f;

    public bool inCooldown;
    public float cooldownCounter = 0; // use this for testing and the UI;

    public List<Enemy> currentZombieAlive;

    private void Start()
    {
        currentZombiePerWave = initialZombiesPerWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombieAlive.Clear();

        currentWave++;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiePerWave; i++)
        {
            // Generate a random offset within a specified range
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            if (zombiePool != null)
            {
                //Get Zombie From Pool
                var zombie = zombiePool.GetBulletFromPool(spawnPosition, Quaternion.identity);

                // Get Enemy Script 
                Enemy enemyScript = zombie.GetComponent<Enemy>();
                enemyScript.isDead = false;
                // Track this zombie
                currentZombieAlive.Add(enemyScript);
                yield return new WaitForSeconds(spawnDelay);
            }

        }
    }

    private void Update()
    {
        // Get all dead zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombieAlive)
        {
            if (zombie.isDead)
            {
                Debug.Log($"Zombie {zombie.name} is dead. Starting coroutine to unset active.");
                StartCoroutine(UnSetActiveZombieAfterDelay(delayTime, zombie));
                zombiesToRemove.Add(zombie);
            }
        }

        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombieAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        // Start Cooldown if all zombies are dead
        if (!inCooldown && currentZombieAlive.Count == 0)
        {
            // Start cooldown for next wave
            StartCoroutine(WaveCooldown());
        }

        // Run the cooldown counter
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            //Reset the counter
            cooldownCounter = waveCooldown;
        }
    }

    private IEnumerator UnSetActiveZombieAfterDelay(float delay, Enemy zombie)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"Zombie {zombie.name} is being returned to the pool.");
        zombiePool.ReturnBulletToPool(zombie.gameObject);
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        currentZombiePerWave += 5;
        StartNextWave();
    }
}









