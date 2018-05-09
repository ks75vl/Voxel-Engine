using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public GameObject player;

	public Vector3Int landSize;
	public Vector3Int cacheShiftSize;
	public Vector3Int viewDistance;

	public int freqUpdate = 500;

	public ChunkMetaData chunkMetaData;


	WorldLoader loader;

	Vector3Int position;
	Vector3 playerPosition;
	float saveTime = 0.0f;

	ChunkLoader chunkLoader;


	public void ttt(int demo) {

		demo--;
	}

	Vector3 test;

	void Awake() {

		ChunkMetaData.Instance = this.chunkMetaData;
		VoxelEngine.Instance.Init(this.landSize);
	}

	void Start () {
		

		this.chunkMetaData.chunkSize = 16;	//Hehe
		this.chunkMetaData.depth = 4;
		this.chunkMetaData.textureLoader = new TextureLoader (this.chunkMetaData);

		

		this.playerPosition = new Vector3 (0, 0, 0);
		this.loader = new WorldLoader (new Vector3 (0, 0, 0), this.landSize, this.cacheShiftSize, this.viewDistance);

		Simplex.Noise.Seed = 123;
		this.test = new Vector3(0, 0, 0);
	}


	void Update () {

		if (Input.GetKeyDown(KeyCode.T)) {
			this.test.x += 3;
			this.loader.UpdateViewDistance(this.test);
		}

		if (Input.GetKeyDown(KeyCode.Y)) {
			this.test.x -= 3;
			this.loader.UpdateViewDistance(this.test);
		}

		if (Input.GetKeyDown(KeyCode.U)) {
			this.test.z += 3;
			this.loader.UpdateViewDistance(this.test);
		}

		if (Input.GetKeyDown(KeyCode.J)) {
			this.test.z -= 3;
			this.loader.UpdateViewDistance(this.test);
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			this.loader.Dbg();
		}

		if (this.playerPosition != this.player.transform.position) {	//Player moving
			if ((Time.realtimeSinceStartup - this.saveTime) * 1000 >= this.freqUpdate) {
				this.playerPosition = this.player.transform.position;
				this.loader.UpdateViewDistance (this.playerPosition);
				Debug.Log("3Liming");
				this.saveTime = Time.realtimeSinceStartup;
			}
		}

	}


	public IWorldAccess Loader {
		get {
			return null;
		}
	}
}
