using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubjectNerd.Utilities;


[System.Serializable]
public class ChunkMetaData {

	public static ChunkMetaData Instance;

	public int chunkSize;
	public float blockSize;
	public float halfBlockSize;
	public int blockSizeDiv;
	public int depth;
	public Material material;
	public int textureSize;

	[Range(0, 100)]
	public int delta = 0;

	public TextureLoader textureLoader;


	[Reorderable]
	public List<ChunkMetaData.BlockMetaData> blockMetaData;

	public ChunkMetaData() {
		this.chunkSize = 16;
		this.blockSize = 0.5f;
		this.halfBlockSize = 0.25f;
		this.blockSizeDiv = 2;
		this.depth = 4;

		Instance = this;
	}

	[System.Serializable]
	public class BlockMetaData {
		public string name;
		public Vector2Int forward;
		public Vector2Int back;
		public Vector2Int top;
		public Vector2Int down;
		public Vector2Int left;
		public Vector2Int right;
	}
}