using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySelectBlock : MonoBehaviour {

	public Text textBlockName;
	public Text prevButton;
	public Text nextButton;

	Spawner spawner;
	ChunkMetaData chunkMetaData;

	int currentSelectedBlockIndex;
	int len;
	bool flag;

	void Start () {

		this.spawner = this.GetComponent<Spawner> ();
		this.chunkMetaData = this.GetComponent<World> ().chunkMetaData;

		this.len = this.chunkMetaData.blockMetaData.Count;
		this.currentSelectedBlockIndex = 0;

		this.flag = true;
	}
	

	void Update () {

		if (Input.GetKeyDown (KeyCode.Q) && this.currentSelectedBlockIndex > 0) {

			if (this.currentSelectedBlockIndex >= (this.len - 1)) {
				this.nextButton.enabled = true;
			} else if (this.currentSelectedBlockIndex <= 1) {
				this.prevButton.enabled = false;
			}

			this.currentSelectedBlockIndex--;
			this.flag = true;
		}

		if (Input.GetKeyDown (KeyCode.E) && this.currentSelectedBlockIndex < (this.len - 1)) {

			if (this.currentSelectedBlockIndex <= 0) {
				this.prevButton.enabled = true;
			} else if (this.currentSelectedBlockIndex >= (this.len - 2)) {
				this.nextButton.enabled = false;
			}

			this.currentSelectedBlockIndex++;
			this.flag = true;
		}

		if (this.flag) {

			this.textBlockName.text = this.spawner.SelectBlock (this.chunkMetaData.blockMetaData [this.currentSelectedBlockIndex].name);
			this.flag = false;
		}
	}
}
