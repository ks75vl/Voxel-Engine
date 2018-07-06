using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Paging : MonoBehaviour {

	class demo {
		int[] a;

		public demo() {
			this.a = new int[100000];

			for (int i = 0; i < 100000; i++) {
				this.a[i] = i;
			}
		}

		public void change() {

			this.a[100] = 200;
		}

		public int get() {

			return this.a[100];
		}
	}

	demo[] test;
	demo[] lone;
	List<demo> list;

	void dbg(demo d, int i) {
		lone[i] = d;
	}

	void Start() {

		test = new demo[10];
		for (int i = 0; i < 10; i++) {
			test[i] = new demo();
		}

		//lone = new demo[10];
		//for (int i = 0; i < 10; i++) {
		//	dbg(test[i], i);
		//}

		//Debug.Log(test[1].get());
		//lone[1].change();
		//Debug.Log(test[1].get());

		list = new List<demo>();
		for (int i = 0; i < 10; i++) {
			list.Add(test[i]);
		}

		Debug.Log(test[1].get());
		list[1].change();
		Debug.Log(test[1].get());
	}

	void Update() {
		
		

	}
	

}
