using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldLoader : IWorldAccess {

	public static IWorldAccess Instance;

	GameObject worldObject;
	ChunkLoader chunkLoader;

	Chunk[] chunkStore;
	long[] tiles;
	ulong[] nonce;

	ulong workingNonce;

	float blockSize = 0.25f;

	int newPositionX;
	int newPositionY;
	int newPositionZ;

	int oldPositionX;
	int oldPositionY;
	int oldPositionZ;

	int cacheShiftX;
	int cacheShiftY;
	int cacheShiftZ;

	int cacheSizeX;
	int cacheSizeY;
	int cacheSizeZ;
    
	int cacheMaskX;
	int cacheMaskY;
	int cacheMaskZ;

	int worldSizeLimitX;
	int worldSizeLimitY;
	int worldSizeLimitZ;

	int viewX;
	int viewY;
	int viewZ;

	int chunkShiftX;
	int chunkShiftY;
	int chunkShiftZ;

	int chunkMaskX;
	int chunkMaskY;
	int chunkMaskZ;

	int currentChunkX;
	int currentChunkY;
	int currentChunkZ;

	Vector3 worldPosition;

	float worldOffsetX;
	float worldOffsetY;
	float worldOffsetZ;

	int xz;
	int worldXZ;
	int worldZ;

	public WorldLoader(Vector3 worldPosition, Vector3Int worldSize, Vector3Int cacheSize, Vector3Int viewDistance) {

		Instance = this;

		this.worldObject = new GameObject (worldPosition.ToString ());
		this.worldObject.transform.position = worldPosition;

		this.chunkLoader = this.worldObject.AddComponent<ChunkLoader> ();
		Debug.Log (VoxelEngine.Instance.worldOffset);
		this.worldPosition = worldPosition;

		this.worldOffsetX = worldPosition.x - worldSize.x * ChunkMetaData.Instance.blockSize * ChunkMetaData.Instance.chunkSize * 0.5f;
		this.worldOffsetY = worldPosition.y - worldSize.y * ChunkMetaData.Instance.blockSize * ChunkMetaData.Instance.chunkSize * 0.5f;
		this.worldOffsetZ = worldPosition.z - worldSize.z * ChunkMetaData.Instance.blockSize * ChunkMetaData.Instance.chunkSize * 0.5f;

		this.cacheShiftX = cacheSize.x;
		this.cacheShiftY = cacheSize.y;
		this.cacheShiftZ = cacheSize.z;

		this.cacheSizeX = 1 << this.cacheShiftX;
		this.cacheSizeY = 1 << this.cacheShiftY;
		this.cacheSizeZ = 1 << this.cacheShiftZ;

		this.cacheMaskX = this.cacheSizeX - 1;
		this.cacheMaskY = this.cacheSizeY - 1;
		this.cacheMaskZ = this.cacheSizeZ - 1;

		int temp = this.cacheSizeX * this.cacheSizeY * this.cacheSizeZ;
		
		this.chunkStore = new Chunk[temp];
		this.tiles = new long[temp];
		this.nonce = new ulong[temp];

		this.xz = this.cacheSizeX * this.cacheSizeZ;
		this.worldXZ = ((worldSize.x >> cacheSize.x) * (worldSize.z >> cacheSize.z));
		this.worldZ = worldSize.z >> cacheSize.z;

		this.workingNonce = 0;

		this.worldSizeLimitX = worldSize.x;
		this.worldSizeLimitY = worldSize.y;
		this.worldSizeLimitZ = worldSize.z;

		this.viewX = viewDistance.x;
		this.viewY = viewDistance.y;
		this.viewZ = viewDistance.z;

		this.chunkShiftX = 4;
		this.chunkShiftY = 4;
		this.chunkShiftZ = 4;

		this.chunkMaskX = 15;
		this.chunkMaskY = 15;
		this.chunkMaskZ = 15;

		this.newPositionX = 0;
		this.newPositionY = 0;
		this.newPositionZ = 0;

		this.oldPositionX = 0;
		this.oldPositionY = 0;
		this.oldPositionZ = 0;

		
		for (int i = 0; i < temp; i++) {
			this.chunkStore[i] = new Chunk(0, 0, 0);
			this.tiles[i] = 0;
		}

		int ni, nj, flatIndex;
		for (int i = this.cacheMaskY; i >= 0; i--) {
			ni = i * this.xz;
			for (int j = this.cacheMaskX; j >= 0; j--) {
				nj = ni + j * this.cacheSizeZ;
				for (int k = this.cacheMaskZ; k >= 0; k--) {
					flatIndex = nj + k;

					//Forward
					if (k != this.cacheMaskZ) {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex + 1], 0);
					} else {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex - this.cacheMaskZ], 0);
					}

					//Back
					if (k != 0) {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex - 1], 1);
					} else {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex + this.cacheMaskZ], 1);
					}

					//Top
					if (i != this.cacheMaskY) {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex + this.xz], 2);
					} else {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex - i * this.xz], 2);
					}

					//Down
					if (i != 0) {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex - this.xz], 3);
					} else {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex + this.cacheMaskY * this.xz], 3);
					}

					//Right
					if (j != this.cacheMaskX) {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex + this.cacheSizeZ], 5);
					} else {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex - j * this.cacheSizeZ], 5);
					}

					//Left
					if (j != 0) {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex - this.cacheSizeZ], 4);
					} else {
						this.chunkStore[flatIndex].SetNeighbor(this.chunkStore[flatIndex + this.cacheMaskX * this.cacheSizeZ], 4);
					}
				}
			}
		}

	}

	public void Dbg() {

		for (int i = 0; i < this.chunkStore.Length; i++) {
			this.chunkStore[i].Clear();
		}
	}

	public GameObject WorldObject {

		get {
			return this.worldObject;
		}

		set {
			this.worldObject = value;
		}
	}

	public Chunk GetChunk (int x, int y, int z) {

		long tile;

		//Calculate cache tile
		tile = (x >> this.cacheShiftX);
		tile <<= 20;
		tile |= (y >> this.cacheShiftY);
		tile <<= 20;
		tile |= (z >> this.cacheShiftZ);

		//Calculcate flatten index
		int flatIndex = (y & this.cacheMaskY) * this.xz + (x & this.cacheMaskX) * this.cacheSizeZ + (z & this.cacheMaskZ);

		if (this.tiles [flatIndex] == tile) {	//Check for valid chunk
			return this.chunkStore [flatIndex];
		}

		return null;
	}

	public void UpdateViewDistance(Vector3 playerPosition) {

		//Debug.Log ("3");
		//Convert player position to chunk position
		
		int newChunkX = (int)(playerPosition.x - this.worldOffsetX) >> 2;
		int newChunkY = (int)(playerPosition.y - this.worldOffsetY) >> 2;
		int newChunkZ = (int)(playerPosition.z - this.worldOffsetZ) >> 2;

        //Debug.Log("Offset " + this.worldOffsetX.ToString());
		//Debug.Log ("Debbug 1: " + newChunkX.ToString () + " " + newChunkY.ToString () + " " + newChunkZ.ToString ());

		//Avoid if chunk index out of world range
		if (newChunkX < this.viewX || newChunkX >= (this.worldSizeLimitX - this.viewX)) {
			return;
		}
		if (newChunkY < this.viewY || newChunkY >= (this.worldSizeLimitY - this.viewY)) {
			return;
		}
		if (newChunkZ < this.viewZ || newChunkZ >= (this.worldSizeLimitZ - this.viewZ)) {
			return;
		}

		//Vector moving
		int deltaX = newChunkX - this.newPositionX;
		int deltaY = newChunkY - this.newPositionY;
		int deltaZ = newChunkZ - this.newPositionZ;

		//Only update when player reach new chunk
		if (deltaX == 0) {
			if (deltaZ == 0) {
				if (deltaY == 0) {
					return;
				}
			}
		}

		this.oldPositionX = this.newPositionX;
		this.oldPositionY = this.newPositionY;
		this.oldPositionZ = this.newPositionZ;

		this.newPositionX = newChunkX;
		this.newPositionY = newChunkY;
		this.newPositionZ = newChunkZ;


		if (deltaX != 0) {
			if (deltaX > 0) {
				deltaX = this.viewX;
			} else {
				deltaX = -this.viewX;
			}
		}

		if (deltaY != 0) {
			if (deltaY > 0) {
				deltaY = this.viewY;
			} else {
				deltaY = -this.viewY;
			}
		}

		if (deltaZ != 0) {
			if (deltaZ > 0) {
				deltaZ = this.viewZ;
			} else {
				deltaZ = -this.viewZ;
			}
		}


		//Debug.Log ("Debbug 1: " + newChunkX.ToString () + " " + newChunkY.ToString () + " " + newChunkZ.ToString ());
		//Debug.Log ("Debbug 2: " + (newChunkX + deltaX).ToString () + " " + (newChunkY + deltaY).ToString () + " " + (newChunkZ + deltaZ).ToString ());
		//Debug.Log ("Debbug 3: " + (this.oldPositionX - deltaX).ToString () + " " + (this.oldPositionY - deltaY).ToString () + " " + (this.oldPositionZ - deltaZ).ToString ());
		//Debug.Log ("================");


		this.workingNonce++;    //Change nonce

		//this.chunkLoader.InitLoader();
		this.CalculateViewDistance (newChunkX + deltaX, newChunkY + deltaY, newChunkZ + deltaZ);
		this.CalculateViewDistance (this.oldPositionX - deltaX, this.oldPositionY - deltaY, this.oldPositionZ - deltaZ);
		this.chunkLoader.Load();
		//Update chua cai ria ra
	}

	void CalculateViewDistance(int chunkX, int chunkY, int chunkZ) {
		
		Queue<long> queue = new Queue<long> ();
		long data, tile;

		int x, y, z;		//Chunk index
		int ckx, cky, ckz;	//Cache key
		int cix, ciy, ciz;	//Cache index
		int vx, vy, vz;		//
		int flatIndex, newFlatIndex;


		bool inNewRadius, inOldRadius;

		//Init queue
		data = chunkX;
		data <<= 20;
		data |= chunkY;
		data <<= 20;
		data |= chunkZ;

		queue.Enqueue (data);
		
		while (true) {

			if (queue.Count == 0) {
				break;
			}
			
			//Get chunk index
			data = queue.Dequeue ();

			//Get chunk index from big long number
			x = (int)((data >> 40) & 0xFFFFF);
			y = (int)((data >> 20) & 0xFFFFF);
			z = (int)(data & 0xFFFFF);



			//Check for chunk in new radius
			vx = x - this.newPositionX;
			vy = y - this.newPositionY;
			vz = z - this.newPositionZ;

			if (vx < 0) {
				vx = -vx;
			}
			if (vy < 0) {
				vy = -vy;
			}
			if (vz < 0) {
				vz = -vz;
			}

			inNewRadius = false;
			if (vx <= this.viewX) {
				if (vy <= this.viewY) {
					if (vz <= this.viewZ) {
						inNewRadius = true;
					}
				}
			}

			//Chech for chunk in old radius
			vx = x - this.oldPositionX;
			vy = y - this.oldPositionY;
			vz = z - this.oldPositionZ;

			if (vx < 0) {
				vx = -vx;
			}
			if (vy < 0) {
				vy = -vy;
			}
			if (vz < 0) {
				vz = -vz;
			}

			inOldRadius = false;
			if (vx <= this.viewX) {
				if (vy <= this.viewY) {
					if (vz <= this.viewZ) {
						inOldRadius = true;
					}
				}
			}


			//Avoid when chunk index in both new and old radius
			if (inNewRadius == inOldRadius) {
				continue;
			}


			//Get cache index from chunk index
			cix = x & this.cacheMaskX;
			ciy = y & this.cacheMaskY;
			ciz = z & this.cacheMaskZ;

			//Get cache key from key index
			ckx = x >> this.cacheShiftX;
			cky = y >> this.cacheShiftY;
			ckz = z >> this.cacheShiftZ;

			//
			flatIndex = ciy * this.xz + cix * this.cacheSizeZ + ciz;

			//Calculate cache tile
			tile = ckx;
			tile <<= 20;
			tile |= cky;
			tile <<= 20;
			tile |= ckz;


			if (inNewRadius) {
				
				if (this.tiles [flatIndex] != tile) {

					this.chunkStore [flatIndex].SetPosition (x, y, z);
					this.tiles [flatIndex] = tile;
					this.chunkLoader.AddChunk(this.chunkStore[flatIndex]);

				} else if (!this.chunkStore[flatIndex].Loaded) {
					this.chunkLoader.AddChunk(this.chunkStore[flatIndex]);
				} else if (this.chunkStore[flatIndex].NeedRender) {
					this.chunkStore[flatIndex].ApplyMesh();
				}

				this.chunkStore [flatIndex].SetActive (true);

			} else {

				if (this.tiles [flatIndex] == tile) {
					this.chunkStore [flatIndex].SetActive (false);
					//Debug.Log ("Hide");
				}
			}


			//flatIndex = ciy * this.xz + cix * this.cacheSizeZ + ciz;

			//Forward
			if (ciz == this.cacheMaskZ) {
				newFlatIndex = flatIndex - ciz;
			} else {
				newFlatIndex = flatIndex + 1;
			}
				
			if (this.nonce [newFlatIndex] != this.workingNonce) {
				this.nonce [newFlatIndex] = this.workingNonce;
				queue.Enqueue (data + 1);
			}


			//Back
			if (ciz == 0) {
				newFlatIndex = flatIndex + this.cacheMaskZ;
			} else {
				newFlatIndex = flatIndex - 1;
			}

			if (this.nonce [newFlatIndex] != this.workingNonce) {
				this.nonce [newFlatIndex] = this.workingNonce;
				queue.Enqueue (data - 1);
			}


			//Top
			if (ciy == this.cacheMaskY) {
				newFlatIndex = flatIndex - ciy * this.xz;
			} else {
				newFlatIndex = flatIndex + this.xz;
			}

			if (this.nonce [newFlatIndex] != this.workingNonce) {
				this.nonce [newFlatIndex] = this.workingNonce;
				queue.Enqueue (data + 0x100000);
			}


			//Down
			if (ciy == 0) {
				newFlatIndex = flatIndex + this.cacheMaskY * this.xz;
			} else {
				newFlatIndex = flatIndex - this.xz;
			}

			if (this.nonce [newFlatIndex] != this.workingNonce) {
				this.nonce [newFlatIndex] = this.workingNonce;
				queue.Enqueue (data - 0x100000);
			}


			//Right
			if (cix == this.cacheMaskX) {
				newFlatIndex = flatIndex - cix * this.cacheSizeZ;
			} else {
				newFlatIndex = flatIndex + this.cacheSizeZ;
			}

			if (this.nonce [newFlatIndex] != this.workingNonce) {
				this.nonce [newFlatIndex] = this.workingNonce;
				queue.Enqueue (data + 0x10000000000);
			}


			//Left
			if (cix == 0) {
				newFlatIndex = flatIndex + this.cacheMaskX * this.cacheSizeZ;
			} else {
				newFlatIndex = flatIndex - this.cacheSizeZ;
			}

			if (this.nonce [newFlatIndex] != this.workingNonce) {
				this.nonce [newFlatIndex] = this.workingNonce;
				queue.Enqueue (data - 0x10000000000);
			}

		}

	}

	public void SetBlock(int x, int y, int z, int type) {
        
	}
}