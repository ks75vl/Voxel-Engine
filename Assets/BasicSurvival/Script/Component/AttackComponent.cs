using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour {

    public BoxCollider attackBox;

    public float damage = 33;

	// Use this for initialization
	void Start () {
		if (attackBox == null)
        {
            Debug.Log("attackBox = NULL !!!");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
