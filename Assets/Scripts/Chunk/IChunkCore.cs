using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IChunkCore	{

	void CreateMesh ();
	void Fill (bool render = true);

	void AddBlock(int x, int y, int z, int type);
	void RemoveBlock(int x, int y, int z);

	//bool AddBlockEventListenner(Vector3Int blockPosition, byte index);
	//bool RemoveBlockEventListenner(Vector3Int blockPosition, byte index);

	List<Vector3> Vertices { get; }
	List<int> Triangles { get; }
	List<Vector2> UV0 { get;}
}

