using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class ZombieChasingState : StateMachineBehaviour
{
    public float attackingDistance = 4f;
    public float chaseSpeed = 5f;
    public float stopChasingDistance = 21f;
    Transform player;
    NavMeshAgent navAgent;
    Enemy zombie;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();
        navAgent.speed = chaseSpeed;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //Chasing Sound
        if (SoundManager.Instance.zombieChanel.isPlaying == false)
        {
            SoundManager.Instance.zombieChanel.PlayOneShot(SoundManager.Instance.zombieChase);

        }

        navAgent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.SetDestination(animator.transform.position);
        SoundManager.Instance.zombieChanel.Stop();
    }


}
