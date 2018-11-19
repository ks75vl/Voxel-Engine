using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public int health;
    bool bFall;
    

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if (health <= 0 && !bFall)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
            StartCoroutine(destroyTree());
            bFall = true;
        }
	}

    IEnumerator destroyTree()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    public void GetHit()
    {
        health -= 1;

    }
}
