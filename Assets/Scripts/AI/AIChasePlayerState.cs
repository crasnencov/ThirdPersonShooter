using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
   
    public Transform playerTransform;
    
    private float timer = 0.0f;
    public AIStateId GetId()
    {
        return AIStateId.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("player " + playerTransform);
    }

    public void Update(AIAgent agent)
    {
        // Debug.Log("ai" );
        //agent.navMeshAgent.destination = playerTransform.position;
       /* if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = playerTransform.position;
                }
                
            }

            timer = agent.config.maxTime;

        }
        */
        // MoveToNextLocation();
        
    }

    public void Exit(AIAgent agent)
    {
        
    }
}
