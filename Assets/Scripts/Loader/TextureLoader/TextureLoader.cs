using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextureLoader {
	
	List<TextureRect[]> blockTexture;
	float tiling;
	float delta;

	public TextureLoader(ChunkMetaData chunkMetaData) {
		
		this.tiling = (float)(1.0f / (float)chunkMetaData.textureSize);
		this.delta = this.tiling * (chunkMetaData.delta / 100.0f);

		this.blockTexture = new List<TextureRect[]> ();
		int len = chunkMetaData.blockMetaData.Count;

		for (int i = 0; i < len; i++) {

			TextureRect[] rect = new TextureRect[6];

			rect [0] = this.GetTextureRect (chunkMetaData.blockMetaData [i].forward);
			rect [1] = this.GetTextureRect (chunkMetaData.blockMetaData [i].back);
			rect [2] = this.GetTextureRect (chunkMetaData.blockMetaData [i].top);
			rect [3] = this.GetTextureRect (chunkMetaData.blockMetaData [i].down);
			rect [4] = this.GetTextureRect (chunkMetaData.blockMetaData [i].left);
			rect [5] = this.GetTextureRect (chunkMetaData.blockMetaData [i].right);

			this.blockTexture.Add (rect);
		}
	}

	TextureRect GetTextureRect(Vector2Int index) {

		float _tiling = this.tiling;
		float _delta = this.delta;
		Vector2 temp;
		TextureRect ret;

		temp.x = _tiling * index.x + _delta;
		temp.y = _tiling * index.y + _delta;
		ret.bottomLeft = temp;

		temp.x += (_tiling - _delta * 2);
		ret.bottomRight = temp;

		temp.y += (_tiling - _delta * 2);
		ret.topRight = temp;

		temp.x -= (_tiling - _delta * 2);
		ret.topLeft = temp;

		return ret;
	}

	public TextureRect GetBlockTexture(int blocktype, int face) {

		return this.blockTexture[blocktype][face];
	}

}
	
public struct TextureRect {

	public Vector2 bottomLeft;
	public Vector2 bottomRight;
	public Vector2 topLeft;
	public Vector2 topRight;
}