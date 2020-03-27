using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle
{
    public Vector2Int pos;
    public GameObject obj;
    public Obstacle(Vector2Int pos){
        this.pos = pos;
        obj = GameObject.Instantiate(Services.ObstacleManager.prefab) as GameObject;
        obj.transform.parent = Services.ObstacleManager.parent;
        obj.transform.position = (Vector3Int)pos;
        obj.name = "Obs ("+pos.x+","+pos.y+")";
    }

    public void DestroySelf(){
        GameObject.Destroy(obj);
    }
}
