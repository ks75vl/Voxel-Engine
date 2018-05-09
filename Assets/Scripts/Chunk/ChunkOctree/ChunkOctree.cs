using UnityEngine;


public class ChunkOctree : IChunkOctree {

	Octant rootOctant;

	public ChunkOctree (float octantSize, int depth) {
		
		this.rootOctant = new Octant (Vector3.zero, octantSize, depth);
	}


#region IChunkOctree
	int GetIndexOfPosition(Vector3 localPosition, Vector3 octantPosition) {

		int index = 0;

		index |= (localPosition.y > octantPosition.y) ? 4 : 0;
		index |= (localPosition.x > octantPosition.x) ? 2 : 0;
		index |= (localPosition.z > octantPosition.z) ? 1 : 0;

		return index;
	}


	public void AddBlockToOctree (Vector3 localPosition, Octant octant) {

		if (!octant.IsLeaf ()) {

			this.AddBlockToOctree (localPosition, octant.Octants [this.GetIndexOfPosition (localPosition, octant.Position)]);

		} else {
			if (octant.objectCount == 0) {
			
				octant.data = localPosition;

			} else if (octant.Subdivide ()) {
				
				this.AddBlockToOctree (localPosition, octant.Octants [this.GetIndexOfPosition (localPosition, octant.Position)]);
				this.AddBlockToOctree (octant.data, octant.Octants [this.GetIndexOfPosition (octant.data, octant.Position)]);

			}
		}

		octant.objectCount++;
	}



	public void RemoveBlockFromOctree(Vector3 localPosition, Octant octant) {

		if (!octant.IsLeaf ()) {
			
			this.RemoveBlockFromOctree (localPosition, octant.Octants [this.GetIndexOfPosition (localPosition, octant.Position)]);

		} else {

		}

		octant.objectCount--;
	}



	public Octant OctreeHandle {
		get {
			return this.rootOctant;
		}
	}
#endregion

}

