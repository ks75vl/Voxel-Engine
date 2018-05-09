using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IChunkOctree {

	void AddBlockToOctree (Vector3 localPosition, Octant octant);
	void RemoveBlockFromOctree (Vector3 localPosition, Octant octant);
	Octant OctreeHandle { get;}
}

