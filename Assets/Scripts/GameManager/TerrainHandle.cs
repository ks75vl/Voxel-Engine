using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHandle : Singleton<TerrainHandle> {
	
	protected TerrainHandle() { }



	public BinaryReader binaryReader;

	public Vector3Int terrainSize;

	public int seed;

	public int maxValue;
	public int minValue;
	public int diffrentValue;

	public List<Layer> layer;

	public void Init(Vector3Int terrainSize, List<Layer> layer) {

		this.terrainSize = terrainSize;

		this.layer = layer;
	}


	//<document>
	//	Terrain file struct: |...Simplex.noise.value...|WorldX|WorldZ|Seed|MinValue|MaxValue|
	//</document>
	public bool SetTerrainData(string fileName) {

		if (File.Exists(fileName)) {

			this.binaryReader = new BinaryReader(File.Open(fileName, FileMode.Open));
			this.binaryReader.BaseStream.Seek(this.binaryReader.BaseStream.Length - 4 * 4, SeekOrigin.Begin);

			if (this.binaryReader.ReadInt32() == this.terrainSize.x) {
				if (this.binaryReader.ReadInt32() == this.terrainSize.z) {

					this.seed = int.Parse(fileName);

					if (this.binaryReader.ReadInt32() == this.seed) {

						this.minValue = this.binaryReader.ReadInt16();
						this.maxValue = this.binaryReader.ReadInt16();
						this.diffrentValue = this.maxValue - this.minValue;

						Debug.Log(this.minValue);
						Debug.Log(this.maxValue);

						return true;
					}
				}
			}

			binaryReader.Close();
		}

		return false;
	}
}
