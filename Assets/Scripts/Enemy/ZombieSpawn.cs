using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int initialZombiesPerWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10f;

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
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instantiate the Zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // Get Enemy Script
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            // Track this zombie
            currentZombieAlive.Add(enemyScript);
            yield return new WaitForSeconds(spawnDelay);
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
                zombiesToRemove.Add(zombie);
            }
        }

        // Actually remove all dead zombies
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombieAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        // Start Cooldown if all zombies are dead
        if (currentZombieAlive.Count == 0 && inCooldown == false)
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

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        currentZombiePerWave *= 2;
        StartNextWave();
    }
}
