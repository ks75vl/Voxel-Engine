using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Layer {


	public GameObject prefab;

	public int percent = 0;

	[Range(0, 100)]
	public int maxLevel = 0;

	[Range(0, 100)]
	public int minLevel = 0;

	
	//public Layer() {

	//	Debug.Log(this.minLevel);
	//}

		
	public bool ContainPrefab(short level) {

		short temp = (short)(((level - TerrainHandle.Instance.minValue) / (float)TerrainHandle.Instance.diffrentValue) * 100);
		
		if (temp >= this.minLevel && temp <= this.maxLevel) {
			return true;
		}

		return false;
	}





}
