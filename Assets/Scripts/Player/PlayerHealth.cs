using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int HP = 100;
    // public Animator animator;
    private void Start()
    {
        // animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP > 0)
        {
            // Damage player
            Debug.Log("Player Hit");
        }
        else
        {
            // Player Die
            Debug.Log("Player Dead");

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
