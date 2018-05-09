using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelEngine : Singleton<VoxelEngine> {

	protected VoxelEngine() {}

	public void Init( Vector3 worldPos) {

		this.worldPosition = worldPos;
		this.CalculateWorldOfset();
		this.CalculatecChunkLocalPosition();
	}

	Vector3 worldPosition;

	public Vector3[] chunkLocalPosition;

	public Vector3Int worldOffset;

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

	void CalculatecChunkLocalPosition() {

		this.chunkLocalPosition = new Vector3[4096];

		int ni, nj;
		float blockSize = ChunkMetaData.Instance.blockSize;

		for (int i = 0; i < 16; i++) {
			ni = i * 256;
			for (int j = 0; j < 16; j++) {
				nj = ni + j * 16;
				for (int k = 0; k < 16; k++) {
					this.chunkLocalPosition[nj + k] = new Vector3(blockSize * (j - localOffset), blockSize * (i - localOffset), blockSize * (k - localOffset));
				}
			}
		}

	}


	void CalculateWorldOfset() {

		this.worldOffset = new Vector3Int ((int)(this.worldPosition.x * 0.5f), (int)(this.worldPosition.y * 0.5f), (int)(this.worldPosition.z * 0.5f));
	}
}