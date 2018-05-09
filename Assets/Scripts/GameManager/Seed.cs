using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

	public static Seed Instance;
	int percent;

	void Awake() {
		
		if (Instance != null && Instance != this) {
			Destroy(this.gameObject);
		} else {
			Instance = this;
		}
	}

	public int Get(int x, int y, int z) {

		if (Random.Range (0, 100) <= this.percent) {
			return 1;
		}

		return 0;
	}

	public void SetPercent(int percent) {

		this.percent = percent;
	}
}