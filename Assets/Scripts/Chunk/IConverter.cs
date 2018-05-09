using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IConverter {

	Vector3 ConvertBlockPosToLocalPos(int x, int y, int z);
	Vector3Int ConvertLocalPosToBlockPos(Vector3 localPosition);
	bool InChunkRange(Vector3Int pos);
}


