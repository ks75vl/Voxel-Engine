using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public GameObject player;
	public GameObject plane;

	public bool isCollide = true;

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
		VoxelEngine.Instance.Init(this.landSize, this.resolution, this.isCollide);
	}

	void Start () {
		

		this.chunkMetaData.chunkSize = 16;	//Hehe
		this.chunkMetaData.depth = 4;
		this.chunkMetaData.textureLoader = new TextureLoader (this.chunkMetaData);

		this.playerPosition = new Vector3 (0, 0, 0);
		this.loader = new WorldLoader (new Vector3 (0, 0, 0), this.landSize, this.cacheShiftSize, this.viewDistance);



		if (File.Exists(this.seed.ToString())) {
			VoxelEngine.Instance.binaryReader = new BinaryReader(File.Open(this.seed.ToString(), FileMode.Open));
		} else {
			Debug.LogWarning("Missing Seed database");
			Application.Quit();
		}


		if (this.isCollide) {
			this.plane.SetActive(false);
		} else {
			this.plane.SetActive(true);
		}

	}

	void Update () {

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
