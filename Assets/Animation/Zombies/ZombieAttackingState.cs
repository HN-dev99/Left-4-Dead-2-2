// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class ZombieAttackingState : StateMachineBehaviour
// {
//     public float stopAttackingDistance = 4f;
//     Transform player;
//     NavMeshAgent navAgent;

//     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {

//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         navAgent = animator.GetComponent<NavMeshAgent>();
//     }

//     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         LookAtPlayer();

//         float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
//         if (distanceFromPlayer > stopAttackingDistance)
//         {
//             animator.SetBool("isAttacking", false);
//         }
//     }

//     private void LookAtPlayer()
//     {
//         Vector3 direction = player.position - navAgent.transform.position;
//         navAgent.transform.rotation = Quaternion.LookRotation(direction);

//         var yRotation = navAgent.transform.eulerAngles.y;
//         navAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
//     }
// }










using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    public float stopAttackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // if (SoundManager.Instance.zombieChannel.isPlaying == false)
        // {
        //     SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zomebieAttack);
        // }


        LookAtPlayer();
        // --- Checking if the agent should stop Attacking--- //

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }

    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // SoundManager.Instance.zombieChannel.Stop();

    }

}
