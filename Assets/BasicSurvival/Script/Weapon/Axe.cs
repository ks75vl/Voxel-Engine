using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IWeapon {

    public float publicDamage;
    public float damage { get; set; }
    //public BoxCollider boxCollider;

	// Use this for initialization
	void Start () {
        damage = publicDamage;
        //boxCollider.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Mobs")
            return;

        HPComponent targetHP = col.GetComponent<HPComponent>();
        targetHP.DoDelta(-damage);
    }

    public void OnStartAttack()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true;
    }

    public void OnEndAttack()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
}
