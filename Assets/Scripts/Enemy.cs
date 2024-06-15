using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    public int zombieDamage = 25;
    public bool isDead;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP > 0)
        {
            isDead = false;
            animator.SetTrigger("DAMAGE");
        }
        else
        {
            int randomValue = Random.Range(0, 1);
            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            isDead = true;

        }
        Debug.Log(HP);
    }

}
