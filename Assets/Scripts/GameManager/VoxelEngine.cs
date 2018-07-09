using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelEngine : Singleton<VoxelEngine> {

	protected VoxelEngine() {}

	public void Init( Vector3Int worldSize, int resolution, bool collider) {

		this.worldSize = worldSize;
		this.CalculateWorldOfset();
		this.CalculatecChunkLocalPosition();

		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();
		this.uv0 = new List<Vector2>();

		this.worldResolution = resolution;
		this.isCollide = collider;
	}

	public Vector3Int worldSize;
	public Vector3[] chunkLocalPosition;
	public Vector3Int worldOffset;


	public int worldResolution;
	public bool isCollide;


	public int powChunkSize = ChunkMetaData.Instance.chunkSize * ChunkMetaData.Instance.chunkSize;
	public int chunkOffset = (int)(ChunkMetaData.Instance.chunkSize * ChunkMetaData.Instance.blockSize);
	public float localOffset = (ChunkMetaData.Instance.chunkSize - 1) * 0.5f;


	public List<int[]> trianglesIndex = new List<int[]> {
		new int[] {3, 1, 7, 5},	//1, 3, 7, 1, 7, 5
		new int[] {0, 2, 4, 6},	//2, 0, 4, 2, 4, 6
		new int[] {4, 6, 5, 7},	//6, 4, 5, 6, 5, 7
		new int[] {1, 3, 0, 2},	//3, 1, 0, 3, 0, 2
		new int[] {1, 0, 5, 4},	//0, 1, 5, 0, 5, 4
		new int[] {2, 3, 6, 7}	//3, 2, 6, 3, 6, 7
	};
	public List<int[]> trianglesIndex1 = new List<int[]> {
		new int[] {18, 1, 307, 290},	//1, 3, 7, 1, 7, 5
		new int[] {0, 17, 289, 306},	//2, 0, 4, 2, 4, 6
		new int[] {289, 306, 290, 307},	//6, 4, 5, 6, 5, 7
		new int[] {1, 18, 0, 17},	//3, 1, 0, 3, 0, 2
		new int[] {1, 0, 290, 289},	//0, 1, 5, 0, 5, 4
		new int[] {17, 18, 306, 307}	//3, 2, 6, 3, 6, 7
	};
	public List<Vector3> vertices;
	public List<int> triangles;
	public List<Vector2> uv0;

	

	public BinaryReader binaryReader;



	void CalculatecChunkLocalPosition() {

		this.chunkLocalPosition = new Vector3[4913];	//17*17*17

		int ni, nj;
		float blockSize = ChunkMetaData.Instance.blockSize;
		float haftSize = ChunkMetaData.Instance.blockSize * 0.5f;

		//for (int i = 0; i < 17; i++) {
		//	ni = i * 256;
		//	for (int j = 0; j < 17; j++) {
		//		nj = ni + j * 16;
		//		for (int k = 0; k < 17; k++) {
		//			//this.chunkLocalPosition[nj + k] = new Vector3(blockSize * (j - localOffset), blockSize * (i - localOffset), blockSize * (k - localOffset));
		//			this.chunkLocalPosition[nj + k] = new Vector3(blockSize * (j - localOffset), blockSize * (i - localOffset), blockSize * (k - localOffset))
		//		}
		//	}
		//}
		
		for (int i = 0; i < 17; i++) {
			ni = i * 289;	//17*17
			for (int j = 0; j < 17; j++) {
				nj = ni + j * 17;
				for (int k = 0; k < 17; k++) {
					this.chunkLocalPosition[nj + k] = new Vector3(blockSize * (j - this.localOffset) - haftSize, blockSize * (i - this.localOffset) - haftSize, blockSize * (k - this.localOffset) - haftSize);
				}
			}
		}
		
	}

	void CalculateWorldOfset() {

		this.worldOffset = new Vector3Int ((int)(this.worldSize.x * 0.5f), (int)(this.worldSize.y * 0.5f), (int)(this.worldSize.z * 0.5f));
	}
}