using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AIStateId initialSate;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfigSO config;
    
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AIStateMachine(this);
        stateMachine.ChangeState(initialSate);
        stateMachine.RegisterState(new AIChasePlayerState());
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
