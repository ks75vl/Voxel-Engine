using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour {

    private NavMeshAgent _agent;
    private HPComponent hpComponent;
    public GameObject Player;
    public float enemyDistance = 4.0f;
    public float attackRange = 1.5f;
    public float rotationSpeed = 10.0f;

    private Animator anim;
    bool bChase;

    // Use this for initialization
    void Start () {
        _agent = GetComponent<NavMeshAgent>();
        hpComponent = GetComponent<HPComponent>();
        anim = GetComponent<Animator>();
        //_agent.updatePosition = false;
        _agent.updateRotation = true;
    }
	
	// Update is called once per frame
	void Update () {

        anim.SetBool("bChase", bChase);

        ChaseToAttack();
        RotateTowards(Player.transform);
       
        IsDead();
    }

    public void ChaseToAttack()
    {
        float distance = Vector3.Distance(this.transform.position, Player.transform.position);

        if (distance < enemyDistance)
        {     
            if (IsInAttackRangeOf(Player.transform))
            {
                anim.SetTrigger("bAttack");
            }
            else
            {
                MoveTowards(Player.transform);
            }
        }
    }

    private bool IsInAttackRangeOf(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < attackRange;
    }

    private void MoveTowards(Transform target)
    {
        _agent.SetDestination(target.position);
       
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion oldRotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.rotation.SetEulerAngles(oldRotation.x, transform.rotation.y, oldRotation.z);
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
