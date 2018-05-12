using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Paging : MonoBehaviour {

	public int count;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;

	List<Vector3> vertices;
	List<int> triangles;

	void Start() {

		this.meshFilter = this.gameObject.AddComponent<MeshFilter>();
		this.meshRenderer = this.gameObject.AddComponent<MeshRenderer>();

		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();

		this.meshFilter.mesh = new Mesh();
		this.meshFilter.sharedMesh.SetVertices(this.vertices);
		this.meshFilter.sharedMesh.SetTriangles(this.triangles, 0);
		//this.meshFilter.sharedMesh.SetUVs(this.uv)

		this.vertices.Add(new Vector3(0, 0, 0));
		this.vertices.Add(new Vector3(1, 0, 0));
		this.vertices.Add(new Vector3(0, 1, 0));
		this.vertices.Add(new Vector3(1, 1, 0));

		this.triangles.Add(1);
		this.triangles.Add(0);
		this.triangles.Add(2);
		this.triangles.Add(1);
		this.triangles.Add(2);
		this.triangles.Add(3);

		//this.meshFilter.sharedMesh.RecalculateNormals();
		//this.meshFilter.sharedMesh.RecalculateBounds();
		//this.meshFilter.sharedMesh.UploadMeshData(false);
	}

	void Update() {
		
		if (Input.GetKeyDown(KeyCode.T) || true) {
			this.meshFilter.sharedMesh.SetVertices(this.vertices);
			this.meshFilter.sharedMesh.SetTriangles(this.triangles, 0);
		}

	}
	

}
