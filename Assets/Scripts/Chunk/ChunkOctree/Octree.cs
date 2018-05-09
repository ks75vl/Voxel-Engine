using UnityEngine;

public class Octant {

	Octant[] octants;
	Vector3 octantPosition;
	float octantSize;
	int depth;

	public Vector3 data;
	public int objectCount;

	public Octant(Vector3 octantPosition, float octantSize, int depth) {

		this.octants = null;
		this.octantPosition = octantPosition;
		this.octantSize = octantSize;
		this.depth = depth;

		this.objectCount = 0;
	}


	public bool Subdivide() {

		if (this.depth == 0) {
			
			return false;
		}

		this.octants = new Octant[8];

		float quarterSize = this.octantSize * 0.25f;
		float haftSize = this.octantSize * 0.5f;
		Vector3 newPosition;

		for (int i = 0; i < 8; i++) {
			newPosition = this.octantPosition;

			if ((i & 4) == 4) {		//Make new octant position
				newPosition.y += quarterSize;
			} else {
				newPosition.y -= quarterSize;
			}

			if ((i & 2) == 2) {
				newPosition.x += quarterSize;
			} else {
				newPosition.x -= quarterSize;
			}

			if ((i & 1) == 1) {
				newPosition.z += quarterSize;
			} else {
				newPosition.z -= quarterSize;
			}

			octants [i] = new Octant (newPosition, haftSize, this.depth - 1);
		}

		return true;
	}



	public Vector3 Position {
		get {
			return this.octantPosition;
		}
	}

	public float OctantSize {
		get {
			return this.octantSize;
		}
	}

	public Octant[] Octants {
		get {
			return this.octants;
		}
	}

	public bool IsLeaf() {

		return octants == null;
	}
}