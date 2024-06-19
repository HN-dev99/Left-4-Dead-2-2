using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrollingState : StateMachineBehaviour
{
    float timer;
    public float patrollingState = 10f;
    public float patrollingSpeed = 2f;
    public float detectionArea = 18f;

    Transform player;
    NavMeshAgent navAgent;
    List<Transform> waypointsList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = animator.GetComponent<NavMeshAgent>();
        timer = 0;
        navAgent.speed = patrollingSpeed;

        GameObject waypointHolder = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform waypoint in waypointHolder.transform)
        {
            waypointsList.Add(waypoint);
        }
        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        navAgent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Patrolling Sound
        if (SoundManager.Instance.zombieChanel.isPlaying == false)
        {
            SoundManager.Instance.zombieChanel.PlayOneShot(SoundManager.Instance.zombieWalking);
            SoundManager.Instance.zombieChanel.PlayDelayed(1f);
        }


        if (navAgent.remainingDistance <= navAgent.stoppingDistance && navAgent.enabled == true)
        {
            navAgent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > patrollingState)
        {
            animator.SetBool("isPatrolling", false);
        }
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}

