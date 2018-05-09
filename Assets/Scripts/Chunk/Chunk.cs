using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk {

	short[] chunkData;

	List<Vector3> vertices;
	List<int> triangles;
	List<Vector2> uv0;

	int chunkIndexX;
	int chunkIndexY;
	int chunkIndexZ;

	int powChunkSize;

	public GameObject chunkObject;
	MeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Mesh mesh;

	Chunk[] chunkNeighbors;
	int chunkNbMask;

	bool loaded;
	bool needRender;
	bool needUpdateMesh;
	public bool isActive;

	VoxelEngine transport;

	public Chunk(int x, int y, int z) {

		this.chunkData = new short[4096];

		this.vertices = new List<Vector3> ();
		this.triangles = new List<int> ();
		this.uv0 = new List<Vector2> ();

		this.chunkIndexX = x;
		this.chunkIndexY = y;
		this.chunkIndexZ = z;

		this.chunkObject = new GameObject();
		this.chunkObject.SetActive(false);
		this.chunkObject.transform.parent = WorldLoader.Instance.WorldObject.transform;
		this.chunkObject.transform.localPosition = ((new Vector3(this.chunkIndexX, this.chunkIndexY, this.chunkIndexZ)) - VoxelEngine.Instance.worldOffset) * VoxelEngine.Instance.chunkOffset;

		this.meshRenderer = chunkObject.AddComponent<MeshRenderer>();
		this.meshRenderer.sharedMaterial = ChunkMetaData.Instance.material;
		this.meshRenderer.enabled = true;

		this.mesh = chunkObject.AddComponent<MeshFilter>().mesh;
		this.mesh.Clear();

		//this.meshCollider = chunkObject.AddComponent<MeshCollider> ();
		//this.localOffset = -Vector3.one * (float)((ChunkMetaData.Instance.chunkSize - 1) * 0.5f * ChunkMetaData.Instance.blockSize);

		this.powChunkSize = ChunkMetaData.Instance.chunkSize * ChunkMetaData.Instance.chunkSize;

		this.chunkNeighbors = new Chunk[6];
		this.chunkNbMask = 0;

		this.loaded = true;
		this.needRender = false;
		this.needUpdateMesh = false;
		this.isActive = false;

		this.transport = VoxelEngine.Instance;
	}



	public bool Loaded {
		get {
			return this.loaded;
		}
	}

	public bool NeedRender {
		get {
			return this.needRender;
		}
	}

	public bool NeedUpdateMesh {
		get {
			return this.needUpdateMesh;
		}
		set {
			this.needUpdateMesh = value;
		}
	}

	#region Render method

	public void Clear() {

		this.mesh.Clear();
	}

	public void Render() {

		this.CaculateMesh ();
		this.ApplyMesh ();
	}

	public void CaculateMesh() {
		
		lock (this.vertices) {
			lock (this.triangles) {
				lock (this.uv0) {
					this.triangles.Clear();
					this.uv0.Clear();
					this.vertices.Clear ();
				}
			}
		}

		int blockType;
		int flatIndex, temp;
		int ni, nj;

		for (int i = 0; i < 16; i++) {
			ni = i * 256;
			for (int j = 0; j < 16; j++) {
				nj = ni + j * 16;
				for (int k = 0; k < 16; k++) {
					flatIndex = nj + k;

					blockType = this.chunkData [flatIndex];		//Get block type
					if (blockType == 0) {
						continue;
					}

					temp = flatIndex;

					//Forward
					flatIndex += 1;
					if (k == 15) {
						if (this.chunkNeighbors [0] != null && this.chunkNeighbors[0].loaded) {
							if (this.chunkNeighbors [0].chunkData [flatIndex - 16] == 0) {
								this.AddMeshProperites (temp, blockType, 0);
							}
						} else {
							this.AddMeshProperites (temp, blockType, 0);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 0);
					}


					//Back
					flatIndex -= 2;
					if (k == 0) {
						if (this.chunkNeighbors[1] != null && this.chunkNeighbors[1].loaded) {
							if (this.chunkNeighbors [1].chunkData [flatIndex + 16] == 0) {
								this.AddMeshProperites (temp, blockType, 1);
							}
						} else {
							this.AddMeshProperites (temp, blockType, 1);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 1);
					}


					//Right
					flatIndex += 17;
					if (j == 15) {
						if (this.chunkNeighbors [5] != null && this.chunkNeighbors[5].loaded) {
							if (this.chunkNeighbors [5].chunkData [flatIndex - 256] == 0) {
								this.AddMeshProperites (temp, blockType, 5);
							}
						} else {
							this.AddMeshProperites (temp, blockType, 5);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 5);
					}


					//Left
					flatIndex -= 32;
					if (j == 0) {
						if (this.chunkNeighbors [4] != null && this.chunkNeighbors[4].loaded) {
							if (this.chunkNeighbors [4].chunkData [flatIndex + 256] == 0) {
								this.AddMeshProperites (temp, blockType, 4);
							}
						} else {
							this.AddMeshProperites (temp, blockType, 4);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 4);
					}


					//Down
					flatIndex -= 240;
					if (i == 0) {
						if (this.chunkNeighbors [3] != null && this.chunkNeighbors[3].loaded) {
							if (this.chunkNeighbors [3].chunkData [flatIndex + 4096] == 0) {
								this.AddMeshProperites (temp, blockType, 3);
							}
						} else {
							this.AddMeshProperites (temp, blockType, 3);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 3);
					}


					//Top
					flatIndex += 512;
					if (i == 15) {
						if (this.chunkNeighbors [2] != null && this.chunkNeighbors[2].loaded) {
							if (this.chunkNeighbors [2].chunkData [flatIndex - 4096] == 0) {
								this.AddMeshProperites (temp, blockType, 2);
							}
						} else {
							this.AddMeshProperites (temp, blockType, 2);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 2);
					}
				}
			}
		}

		this.needRender = true;
		this.loaded = true;
	}

	public void ApplyMesh() {

		if (!this.needRender) {
			return;
		}

		lock (this.vertices) {
			lock (this.triangles) {
				lock (this.uv0) {

					this.mesh.triangles = null;

					this.mesh.vertices = this.vertices.ToArray();
					this.mesh.triangles = this.triangles.ToArray();
					this.mesh.uv = this.uv0.ToArray();

					this.mesh.RecalculateNormals();
					this.mesh.RecalculateBounds();

					//this.meshCollider.sharedMesh = mesh;
				}
			}
		}

		this.needRender = false;
	}

	public void CaculateNeighborsMesh() {

		for (int i = 0; i < 6; i++) {
			if (this.chunkNeighbors[i] != null && this.chunkNeighbors[i].isActive && this.chunkNeighbors[i].needUpdateMesh) {
				this.chunkNeighbors[i].CaculateMesh();
				this.chunkNeighbors[i].needUpdateMesh = false;
				this.chunkNeighbors[i].chunkNbMask |= (1 << i);
			}
		}
	}

	public void ApplyNeighborsMesh() {

		for (int i = 0; i < 6; i++) {
			if (((this.chunkNeighbors[i].chunkNbMask >> i) & 1) == 1) {
				this.chunkNeighbors[i].ApplyMesh();
				this.chunkNeighbors[i].chunkNbMask &= ~(1 << i);
			}
		}
	}

	public void SetLoadedEvent() {

		for (int i = 0; i < 6; i++) {
			this.chunkNeighbors[i].needUpdateMesh = true;
		}
	}



	void AddMeshProperites(int chunkIndex, int blockType, int index) {

		Vector3 newPosition;
		Vector3 position = this.transport.chunkLocalPosition[chunkIndex];
		float halfSize = ChunkMetaData.Instance.halfBlockSize;
		int[] data = this.transport.trianglesIndex [index];


		lock (this.vertices) {
			lock (this.triangles) {
				lock (this.uv0) {

					int count = this.vertices.Count;

					//Add vertices
					for (int i = 0; i < 4; i++) {
						newPosition = position;

						if ((data [i] & 4) == 4) {
							newPosition.y += halfSize;
						} else {
							newPosition.y -= halfSize;
						}

						if ((data [i] & 2) == 2) {
							newPosition.x += halfSize;
						} else {
							newPosition.x -= halfSize;
						}

						if ((data [i] & 1) == 1) {
							newPosition.z += halfSize;
						} else {
							newPosition.z -= halfSize;
						}

						this.vertices.Add (newPosition);
					}


					//Add triangles
					this.triangles.Add (count + 1);
					this.triangles.Add (count);
					this.triangles.Add (count + 2);
					this.triangles.Add (count + 1);
					this.triangles.Add (count + 2);
					this.triangles.Add (count + 3);

					//Add uv
					TextureRect _temp = ChunkMetaData.Instance.textureLoader.GetBlockTexture(blockType - 1, index);
					this.uv0.Add (_temp.bottomLeft);
					this.uv0.Add (_temp.bottomRight);
					this.uv0.Add (_temp.topLeft);
					this.uv0.Add (_temp.topRight);
				}
			}
		}

	}

	#endregion



	public void CountVertices() {
		if (this.mesh.vertices.Length != 0) {
			Debug.Log("Chunk " + this.chunkIndexX.ToString() + " " + this.chunkIndexY.ToString() + " " + this.chunkIndexZ.ToString() + " :" + this.mesh.vertices.Length.ToString());
		}
	}


#region Chunk access

	public void SetActive(bool state) {

		this.chunkObject.SetActive(state);
		//this.meshRenderer.enabled = state;
		//this.meshCollider.enabled = state;

		this.isActive = state;
	}


	public void SetPosition(int x, int y, int z) {

		this.chunkIndexX = x;
		this.chunkIndexY = y;
		this.chunkIndexZ = z;

		this.chunkObject.transform.localPosition = ((new Vector3 (x, y, z)) - VoxelEngine.Instance.worldOffset) * VoxelEngine.Instance.chunkOffset;
	}



	public void Fill(bool render = true) {
		
		//float[,] test = Simplex.Noise.Calc2D (this.chunkIndexX * 16, this.chunkIndexZ * 16, 16, 16, 0.020f);
		short type;
		int offset;



		//for (int i = 0; i < 16; i++) {
		//	for (int j = 0; j < 16; j++) {

		//		offset = (int)((test [i, j] / 256.0f) * 16) + 250 * 16;

		//		for (int k = 0; k < 16; k++) {
					
		//			if ((k + this.chunkIndexY * 16) < offset) {
		//				type = 2;
		//			} else {
		//				type = 0;
		//			}
		//			this.chunkData [k * 256 + i * 16 + j] = type;
		//		}
		//	}
		//}

		type = 0;
		if (this.chunkIndexY <= 248) {
			type = 2;
		}

		for (int i = 0; i < 16; i++) {
			for (int j = 0; j < 16; j++) {
				for (int k = 0; k < 16; k++) {
					this.chunkData [i * 256 + j * 16 + k] = type;
				}
			}
		}
	}


	public void SetBlock(Vector3Int blockPosition, int type, bool autoUpdate = true) {

		if (autoUpdate) {

			if (type == 0) {
				this.RemoveBlock (blockPosition.x, blockPosition.y, blockPosition.z);
			} else {
				this.AddBlock (blockPosition.x, blockPosition.y, blockPosition.z, type);
			}

			this.Render ();
		} else {
			if (type == 0) {
				this.RemoveBlock (blockPosition.x, blockPosition.y, blockPosition.z);
			} else {
				this.AddBlock (blockPosition.x, blockPosition.y, blockPosition.z, type);
			}
		}

	}

	public void AddBlock(int x, int y, int z, int type) {

		this.chunkData [x * 256 + y * 16 + z] = (short)type;
		//this.chunkData [x * 256 + y * 16 + z] |= (short)(type & 0x3FF);
	}

	public void RemoveBlock(int x, int y, int z) {

		this.chunkData [x * 256 + y * 16 + z] = 0;
	}


	public void SetNeighbor(Chunk chunk, int relationship) {

		this.chunkNeighbors[relationship] = chunk;
	}


#endregion








#region Position converter
	int CustomRound(float f) {

		int end = ((int)(f * 10)) % 10;
		if (Mathf.Abs(end) >= 5) {
			return ((int)f + ((end >= 0) ? 1 : -1));
		}

		return (int)f;
	}

	//public Vector3 ConvertBlockPosToLocalPos(int x, int y, int z) {

	//	Vector3 _offset = this.localOffset;
	//	float _blockSize = ChunkMetaData.Instance.blockSize;

	//	return new Vector3 (x * _blockSize + _offset.x, y * _blockSize + _offset.y, z * _blockSize + _offset.z);
	//}

	//public Vector3Int ConvertLocalPosToBlockPos(Vector3 localPosition) {

	//	Vector3 _offset = this.localOffset;
	//	float _blockSize = ChunkMetaData.Instance.blockSize;

	//	return new Vector3Int ((int)((localPosition.x - _offset.x) / _blockSize), (int)((localPosition.y - _offset.y) / _blockSize), (int)((localPosition.z - _offset.z) / _blockSize));
	//}
#endregion



}