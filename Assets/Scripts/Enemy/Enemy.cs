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
        if (isDead == true) return;
        HP -= damage;
        if (HP > 0)
        {
            animator.SetTrigger("DAMAGE");
            SoundManager.Instance.zombieChanel.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
        else
        {
            isDead = true;
            int randomValue = Random.Range(0, 1);
            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            SoundManager.Instance.zombieChanel1.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        Debug.Log(HP);
    }

}
