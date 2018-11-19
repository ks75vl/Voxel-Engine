using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUpItemMovement : MonoBehaviour {

    public float height;
    public float amplitude = 5;
    float velocity = 0;
    Vector3 pos;

	// Use this for initialization
	void Start () {
        pos = gameObject.transform.position;
        pos.y += 1;
        gameObject.transform.position = pos;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 vec3 = pos;
        vec3.y = pos.y + Mathf.Sin(Time.time)*0.5f;

        gameObject.transform.position = vec3;
    }
}
