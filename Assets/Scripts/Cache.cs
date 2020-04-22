using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache
{
    public Vector2Int pos;
    public GameObject obj;
    public Cache(Vector2Int pos){
        this.pos = pos;
        obj = GameObject.Instantiate(Services.GameController.cachePrefab);
        obj.transform.position = (Vector3Int)pos;
        obj.transform.parent = Services.GameController.transform.parent;
    }
    public void SetPosition(Vector2Int pos){
        this.pos = pos;
        obj.transform.position = (Vector3Int)pos;
    }
}
