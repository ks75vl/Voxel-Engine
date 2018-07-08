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

	public GameObject chunkObject;
	MeshCollider meshCollider;

	Chunk[] chunkNeighbors;
	int chunkNbMask;

	bool loaded = false;
	bool needRender = false;
	bool needUpdateMesh = false;
	bool needUpdateTransform = false;
	bool componentReady = false;
	bool isActive = false;


	public Chunk(int x, int y, int z) {

		this.chunkIndexX = x;
		this.chunkIndexY = y;
		this.chunkIndexZ = z;
		this.needUpdateTransform = true;

		this.chunkNeighbors = new Chunk[6];
		this.chunkNbMask = 0;

		//this.loaded = true;
		//this.needRender = false;
		//this.needUpdateMesh = false;
		//this.needUpdateTransform = false;
		//this.isActive = false;

		//this.componentReady = false;
	}

	void AddComponent() {

		this.chunkData = new short[4096];

		this.vertices = new List<Vector3> ();
		this.triangles = new List<int> ();
		this.uv0 = new List<Vector2> ();

		this.chunkObject = new GameObject();

		this.chunkObject.SetActive(false);
		this.chunkObject.transform.parent = WorldLoader.Instance.WorldObject.transform;
		this.chunkObject.transform.localPosition = ((new Vector3(this.chunkIndexX, this.chunkIndexY, this.chunkIndexZ)) - VoxelEngine.Instance.worldOffset) * VoxelEngine.Instance.chunkOffset;
		
		this.chunkObject.AddComponent<MeshRenderer>().sharedMaterial = ChunkMetaData.Instance.material;
		this.chunkObject.AddComponent<MeshFilter>().sharedMesh = new Mesh();

		this.meshCollider = this.chunkObject.AddComponent<MeshCollider> ();
		this.meshCollider.enabled = true;
		
		this.componentReady = true;
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

	#region Render method


	//<document>
	//	<Params>		Void
	//	<Return>		Void
	//	<Description>	Calulalte and apply mesh to chunk
	//	<Note>			Split to 2 method because ApplyMesh() must be called from main thread
	//</document>
	public void Render() {

		this.CaculateMesh ();
		this.ApplyMesh ();
	}


	//<document>
	//	<Description>	Detect side of block, which visible to render 
	//	<Note>
	//</document>
	public void CaculateMesh() {

		if (!this.needUpdateMesh) {
			return;
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

					temp = flatIndex + 33 * i + j;	//Convert 16*16*16 matrix to 17*17*17 matrix

					//Forward
					flatIndex += 1;
					if (k == 15) {
						if (this.chunkNeighbors [0] != null && this.chunkNeighbors[0].isActive) {
							if (this.chunkNeighbors [0].chunkData [flatIndex - 16] == 0) {
								this.AddMeshProperites (temp, blockType, 0);
							}
						} else {
							//this.AddMeshProperites (temp, blockType, 0);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 0);
					}


					//Back
					flatIndex -= 2;
					if (k == 0) {
						if (this.chunkNeighbors[1] != null && this.chunkNeighbors[1].isActive) {
							if (this.chunkNeighbors [1].chunkData [flatIndex + 16] == 0) {
								this.AddMeshProperites (temp, blockType, 1);
							}
						} else {
							//this.AddMeshProperites (temp, blockType, 1);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 1);
					}


					//Right
					flatIndex += 17;
					if (j == 15) {
						if (this.chunkNeighbors [5] != null && this.chunkNeighbors[5].isActive) {
							if (this.chunkNeighbors [5].chunkData [flatIndex - 256] == 0) {
								this.AddMeshProperites (temp, blockType, 5);
							}
						} else {
							//this.AddMeshProperites (temp, blockType, 5);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 5);
					}


					//Left
					flatIndex -= 32;
					if (j == 0) {
						if (this.chunkNeighbors [4] != null && this.chunkNeighbors[4].isActive) {
							if (this.chunkNeighbors [4].chunkData [flatIndex + 256] == 0) {
								this.AddMeshProperites (temp, blockType, 4);
							}
						} else {
							//this.AddMeshProperites (temp, blockType, 4);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 4);
					}


					//Down
					flatIndex -= 240;
					if (i == 0) {
						if (this.chunkNeighbors [3] != null && this.chunkNeighbors[3].isActive) {
							if (this.chunkNeighbors [3].chunkData [flatIndex + 4096] == 0) {
								this.AddMeshProperites (temp, blockType, 3);
							}
						} else {
							//this.AddMeshProperites (temp, blockType, 3);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 3);
					}


					//Top
					flatIndex += 512;
					if (i == 15) {
						if (this.chunkNeighbors [2] != null && this.chunkNeighbors[2].isActive) {
							if (this.chunkNeighbors [2].chunkData [flatIndex - 4096] == 0) {
								this.AddMeshProperites (temp, blockType, 2);
							}
						} else {
							//this.AddMeshProperites (temp, blockType, 2);
						}
					} else if ((this.chunkData [flatIndex]) == 0) {
						this.AddMeshProperites (temp, blockType, 2);
					}
				}
			}
		}

		this.needRender = true;
		this.loaded = true;
		this.needUpdateMesh = false;
	}


	//<document>
	//	<Description>	Apply vertices, triangles, uv to mesh component of this chunk
	//	<Note>			lock() is require because of deadblock with child thread
	//					Clear mesh data after apply to chunk (vertices, triangles, uv)
	//</document>
	public void ApplyMesh() {

		if (!this.needRender) {
			return;
		}

		if (this.needUpdateTransform) {
			this.chunkObject.transform.localPosition = ((new Vector3(this.chunkIndexX, this.chunkIndexY, this.chunkIndexZ)) - VoxelEngine.Instance.worldOffset) * VoxelEngine.Instance.chunkOffset;
			this.needUpdateTransform = false;
		}

		if (!this.componentReady) {
			this.AddComponent();
		}

		lock (this.vertices) {
			lock (this.triangles) {
				lock (this.uv0) {

					Mesh m = this.chunkObject.GetComponent<MeshFilter>().sharedMesh;
					m.Clear();
					
					m.SetVertices(this.vertices);
					m.SetTriangles(this.triangles, 0);
					m.SetUVs(0, this.uv0);

					m.RecalculateNormals();
					//m.RecalculateBounds();

					this.meshCollider.sharedMesh = null;
					this.meshCollider.sharedMesh = m;

					this.triangles.Clear();
					this.uv0.Clear();
					this.vertices.Clear();
				}
			}
		}

		this.needRender = false;
	}




	public void CaculateNeighborsMesh() {

		for (int i = 0; i < 6; i++) {
			if (this.chunkNeighbors[i] != null && this.chunkNeighbors[i].isActive && this.chunkNeighbors[i].needUpdateMesh) {
				this.chunkNeighbors[i].CaculateMesh();
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


	//<document>
	//	<Description>	Mark neighbor chunk need update mesh when this chunk was spawned
	//	<Note>
	//</document>
	public void SetLoadedEvent() {

		for (int i = 0; i < 6; i++) {
			this.chunkNeighbors[i].needUpdateMesh = true;
		}
	}


	//<document>
	//	<Description>	Calculate mesh data: vertices, triangles, uv
	//	<Note>			This chunk was filled
	//</document>
	void AddMeshProperites(int chunkIndex, int blockType, int index) {

		lock (this.vertices) {
			lock (this.triangles) {
				lock (this.uv0) {

					int count = this.vertices.Count;

					//Add vertices
					for (int i = 0; i < 4; i++) {
						this.vertices.Add(VoxelEngine.Instance.chunkLocalPosition[chunkIndex + VoxelEngine.Instance.trianglesIndex1[index][i]]);
					}

					//Add triangles
					this.triangles.Add(count + 1);
					this.triangles.Add(count);
					this.triangles.Add(count + 2);
					this.triangles.Add(count + 1);
					this.triangles.Add(count + 2);
					this.triangles.Add(count + 3);

					//Add uv
					TextureRect _temp = ChunkMetaData.Instance.textureLoader.GetBlockTexture(blockType - 1, index);
					this.uv0.Add(_temp.bottomLeft);
					this.uv0.Add(_temp.bottomRight);
					this.uv0.Add(_temp.topLeft);
					this.uv0.Add(_temp.topRight);
				}
			}
		}

	}

#endregion



	public void CountVertices() {
		
	}


#region Chunk access

	public void SetActive(bool state) {


		if (!this.componentReady) {
			this.AddComponent();
		}

		this.chunkObject.SetActive(state);
		//this.meshRenderer.enabled = state;
		//this.meshCollider.enabled = state;

		this.isActive = state;
	}

	public void SetPosition(int x, int y, int z) {

		this.chunkIndexX = x;
		this.chunkIndexY = y;
		this.chunkIndexZ = z;

		this.needUpdateTransform = true;
	}



	//<document>
	//	<Params>		Not use
	//	<Return>		Void
	//	<Description>	Fill all blocks in chunk using Simplex Noise
	//	<Note>			Only called from child thread, cause lagging if using from main thread
	//</document>
	public void Fill(bool render = true) {
		
		short type;
		int offset;
		float temp;
		float step = 1 / (float)VoxelEngine.Instance.worldResolution;
		Vector3 point = new Vector3(this.chunkIndexX * 16 * step, this.chunkIndexZ * 16 * step, 0);

		for (int i = 0; i < 16; i++) {
			point.x += step;
			for (int j = 0; j < 16; j++) {
				point.y += step;
				
				temp = Noise.Sum(point, 5f, 6, 2f, 0.5f).value + 0.6f;
				offset = (int)(temp * 128 + 250 * 16);
				
				for (int k = 0; k < 16; k++) {
					if ((k + this.chunkIndexY * 16) < offset) {
						type = 2;
					} else {
						type = 0;
					}
					this.chunkData[k * 256 + i * 16 + j] = type;
				}
			}
			point.y -= (step * 16);
		}

		//Debug: Flat terrain
		//type = 0;
		//if (this.chunkIndexY <= 248) {
		//	type = 2;
		//}

		//for (int i = 0; i < 16; i++) {
		//	for (int j = 0; j < 16; j++) {
		//		for (int k = 0; k < 16; k++) {
		//			this.chunkData [i * 256 + j * 16 + k] = type;
		//		}
		//	}
		//}
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

}