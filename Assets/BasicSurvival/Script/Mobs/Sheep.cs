using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour {

    private NavMeshAgent _agent;

    public GameObject Player;
    public float enemyDistance = 4.0f;

    private HPComponent hpComponent;

	// Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        hpComponent = GetComponent<HPComponent>();
	}
	
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(this.transform.position, Player.transform.position);

        if (distance < enemyDistance)
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;

            Vector3 NewPos = this.transform.position + dirToPlayer;

            _agent.SetDestination(NewPos);
        }

        IsDead();
	}

    public void IsDead()
    {
        if (hpComponent.currentHP <= 0)
        {
            DropItems drop = GetComponent<DropItems>();
            drop.DropPerformance();
            Destroy(gameObject);
        }
    }

}
