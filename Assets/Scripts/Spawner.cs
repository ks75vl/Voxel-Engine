using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	World world;
	string selectedBlockName;

	void Start () {
		
		this.selectedBlockName = "Dirt";
		this.world = this.GetComponent<World> ();
	}


	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 30.0f)) {
				//this.world.Loader.RemoveBlock (hit.point, hit.normal);
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 30.0f)) {
				//this.world.Loader.SpawnBlock (hit.point, hit.normal, this.selectedBlockName);
			}
		}

	}


	public string SelectBlock(string blockName) {

		this.selectedBlockName = blockName;

		return blockName;
	}
}
