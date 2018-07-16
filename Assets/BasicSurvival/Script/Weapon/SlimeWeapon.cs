using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeWeapon : MonoBehaviour {


    BoxCollider boxCollider;
    public float damage = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player")
            return;

        PlayerInventory target = col.GetComponent<PlayerInventory>();
        target.currentHealth -= damage;

        if (target.currentHealth < 0)
            target.currentHealth = 0;
        target.UpdateHPBar();
    }
}
