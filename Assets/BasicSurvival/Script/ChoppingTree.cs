using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingTree : MonoBehaviour {

    PlayerInventory inven;
    public GameObject text;
    

    void Start () {
        inven = GetComponent<PlayerInventory>();
    }
	
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {                
                if (hit.collider.tag == "Tree")
                {               
                    hit.transform.gameObject.GetComponent<Tree>().GetHit();
                    inven.mainInventory.addItemToInventory(4);
                    GameObject textDisplay = Instantiate(text, hit.transform.position, transform.rotation);
                    textDisplay.GetComponent<TextParticle>().SetText("1 Lumber");

                }
            }
        }
	}
}
