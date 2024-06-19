using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public GameObject bloodyScreen;
    LevelManager levelManager;
    public int HP = 100;
    public bool isDead;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        HP -= damage;
        if (HP > 0)
        {
            // Damage player
            StartCoroutine(BloodyEffect());
            SoundManager.Instance.playerChanel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
        else
        {
            // Player Die
            PlayerDead();
        }
    }

    private void PlayerDead()
    {
        isDead = true;
        levelManager.LoadGameOver();
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        SoundManager.Instance.playerChanel.PlayOneShot(SoundManager.Instance.playerDeath);

        // Set up Cursor to None
        Cursor.lockState = CursorLockMode.None;

        // Dying Animaiton
        GetComponentInChildren<Animator>().enabled = true;

    }

    private IEnumerator BloodyEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        //Set the initial alpha value to 1 (fully visible)
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            //Caculate the new alpha value using Lerp
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elaped time
            elapsedTime += Time.deltaTime;

            yield return null;

        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("ZombieHand"))
        {
            TakeDamage(collider.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
