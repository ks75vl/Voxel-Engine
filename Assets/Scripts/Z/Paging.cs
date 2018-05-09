using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Paging : MonoBehaviour {

	public int count;

	void Start() {

		GameObject g;
		GameObject p = new GameObject();

		for (int i = 0; i < this.count; i++) {
			g = new GameObject();
			g.transform.parent = p.transform;
			g.AddComponent<MeshFilter>();
			g.AddComponent<MeshRenderer>();
		}

	}

	void Update() {
		


	}
	

}
