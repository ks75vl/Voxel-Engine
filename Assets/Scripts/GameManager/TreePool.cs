using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreePool : Singleton<TreePool> {

	protected TreePool() { }


	private Transform parent;

	private GameObject[] treeType;
	private int typeLengh;

	private List<GameObject[]>[] treePool;
	private Queue<long>[] poolKey;
	private int poolSize = 64;



	public void Init(List<Layer> layer) {

		this.parent = new GameObject("TreePool").transform;

		this.typeLengh = layer.Count;

		this.treeType = new GameObject[this.typeLengh];
		this.treePool = new List<GameObject[]>[this.typeLengh];

		this.poolKey = new Queue<long>[this.typeLengh];
		
		int len = this.typeLengh;
		for (int i = 0; i < len; i++) {

			this.treeType[i] = layer[i].prefab;
			this.treePool[i] = new List<GameObject[]>();

			this.poolKey[i] = new Queue<long>();
		}
	}

	public long GetKey(int type) {

		if (type >= this.typeLengh || type < 0) {
			return 0;
		}

		long key;

		if (this.poolKey[type].Count == 0) {

			this.treePool[type].Add(new GameObject[this.poolSize]);

			int len = this.poolSize;
			int temp = this.treePool[type].Count - 1;
			GameObject g;


			for (int i = 0; i < len; i++) {

				g = GameObject.Instantiate(this.treeType[type], new Vector3(0, 0, 0), Quaternion.identity);
				g.transform.parent = this.parent;
				g.SetActive(false);

				this.treePool[type][temp][i] = g;

				key = temp;
				key <<= 10;
				key |= (long)i;
				key <<= 10;
				key |= (long)type;

				this.poolKey[type].Enqueue(key);
			}
		}

		return this.poolKey[type].Dequeue();
	}

	public GameObject GetTree(long key) {

		if (key < 0) {
			return null;
		}

		return this.treePool[key & 0x3FF][(int)((key >> 20) & 0xFFFFFFFF)][(key >> 10) & 0x3FF];
	}

	public void FreeTree(long key) {

		this.poolKey[key & 0x3FF].Enqueue(key);

		GameObject g = this.treePool[key & 0x3FF][(int)((key >> 20) & 0xFFFFFFFF)][(key >> 10) & 0x3FF];
		g.transform.parent = this.parent;
		g.SetActive(false);
	}
}
