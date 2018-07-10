using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubjectNerd.Utilities;

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

	[Reorderable]
	public List<Layer> layer;
	

	WorldLoader loader;

	Vector3Int position;
	Vector3 playerPosition;
	float saveTime = 0.0f;

	ChunkLoader chunkLoader;

	long key;

	void Awake() {
		
		ChunkMetaData.Instance = this.chunkMetaData;

		VoxelEngine.Instance.Init(this.landSize, this.resolution, this.isCollide);
		TerrainHandle.Instance.Init(this.landSize, this.layer);
		TreePool.Instance.Init(this.layer);
	}

	void Start () {
		

		this.chunkMetaData.chunkSize = 16;	//Hehe
		this.chunkMetaData.depth = 4;
		this.chunkMetaData.textureLoader = new TextureLoader (this.chunkMetaData);

		this.playerPosition = new Vector3 (0, 0, 0);
		this.loader = new WorldLoader (new Vector3 (0, 0, 0), this.landSize, this.cacheShiftSize, this.viewDistance);


		if (!TerrainHandle.Instance.SetTerrainData(this.seed.ToString())) {
			Debug.LogError("Missing or invalid format terrain data of seed '" + this.seed.ToString() + "'");
		}


		if (this.isCollide) {
			this.plane.SetActive(false);
		} else {
			this.plane.SetActive(true);
		}


		//Noise.Seed(5555);
	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.T)) {

			this.key = TreePool.Instance.GetKey(1);
			GameObject g = TreePool.Instance.GetTree(this.key);
			g.SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.Y)) {

			TreePool.Instance.FreeTree(this.key);
		}

		if (this.playerPosition != this.player.transform.position) {	//Player moving
			if ((Time.realtimeSinceStartup - this.saveTime) * 1000 >= this.freqUpdate) {

				this.playerPosition = this.player.transform.position;
				this.loader.UpdateViewDistance (this.playerPosition);
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
