using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IWorldAccess	{
    
    Chunk GetChunk (int x, int y, int z);
	GameObject WorldObject { get; set; }
}

