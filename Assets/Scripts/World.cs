using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public GameObject player;

	[Header("World")]
	public Vector3Int landSize;
	public Vector3Int cacheShiftSize;
	public Vector3Int viewDistance;

	public int seed = 1234;
	public int resolution = 3000;

	public int freqUpdate = 500;

	public ChunkMetaData chunkMetaData;

	
	

	WorldLoader loader;

	Vector3Int position;
	Vector3 playerPosition;
	float saveTime = 0.0f;

	ChunkLoader chunkLoader;


	void Awake() {

		ChunkMetaData.Instance = this.chunkMetaData;
		VoxelEngine.Instance.Init(this.landSize, this.resolution);
	}

	void Start () {
		

		this.chunkMetaData.chunkSize = 16;	//Hehe
		this.chunkMetaData.depth = 4;
		this.chunkMetaData.textureLoader = new TextureLoader (this.chunkMetaData);

		

		this.playerPosition = new Vector3 (0, 0, 0);
		this.loader = new WorldLoader (new Vector3 (0, 0, 0), this.landSize, this.cacheShiftSize, this.viewDistance);

		Noise.Seed(seed);
	}

	void Update () {

		//if (Input.GetKeyDown(KeyCode.T)) {
		//	this.test.x += 3;
		//	this.loader.UpdateViewDistance(this.test);
		//}

		//if (Input.GetKeyDown(KeyCode.Y)) {
		//	this.test.x -= 3;
		//	this.loader.UpdateViewDistance(this.test);
		//}

		//if (Input.GetKeyDown(KeyCode.U)) {
		//	this.test.z += 3;
		//	this.loader.UpdateViewDistance(this.test);
		//}

		//if (Input.GetKeyDown(KeyCode.J)) {
		//	this.test.z -= 3;
		//	this.loader.UpdateViewDistance(this.test);
		//}

		//if (Input.GetKeyDown(KeyCode.S)) {
		//	for (int i = 0; i < 5; i++) {
		//		for (int j = 0; j < 5; j++) {
		//			for (int k = 0; k < 5; k++) {
		//				Debug.Log((i * 25 + j * 5 + k).ToString() + " " + (i * 36 + j * 6 + k).ToString() + " " + ((i * 25 + j * 5 + k) - (i * 36 + j * 6 + k)).ToString());
		//			}
		//		}
		//	}
		//}

		if (this.playerPosition != this.player.transform.position) {	//Player moving
			if ((Time.realtimeSinceStartup - this.saveTime) * 1000 >= this.freqUpdate) {
				this.playerPosition = this.player.transform.position;
				this.loader.UpdateViewDistance (this.playerPosition);
				//.Log("3Liming");
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
