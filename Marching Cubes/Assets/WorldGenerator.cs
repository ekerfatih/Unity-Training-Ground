using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public int WorldSizeInChunks = 10;
    private Dictionary<Vector3Int, Chunk> _chunks = new Dictionary<Vector3Int, Chunk>();
    private void Start() {
        Generate();
    }

    void Generate() {
        for (int x = 0; x < WorldSizeInChunks; x++) {
            for (int z = 0; z < WorldSizeInChunks; z++) {
                Vector3Int chunkPos = new Vector3Int(x * GameData.chunkWidth, 0, z * GameData.chunkWidth);
                _chunks.Add(chunkPos,new Chunk(chunkPos,false,true));
                _chunks[chunkPos].chunkObject.transform.SetParent(World.Instance.transform);
            }
        }
        Debug.Log(string.Format("{0} x {0} World Generated",(WorldSizeInChunks * GameData.chunkWidth)));
    }

    public Chunk GetChunkFromVector3(Vector3 pos) {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        return _chunks[new Vector3Int(x, y, z)];
    }

}