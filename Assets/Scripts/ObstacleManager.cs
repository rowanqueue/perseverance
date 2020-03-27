using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager
{
    public List<Obstacle> obstacles;
    public Transform parent;
    public GameObject prefab;
    public Vector2Int border;
    // Start is called before the first frame update
    public void Initialize(Transform obstaclesParent, GameObject prefab)
    {
        this.parent = obstaclesParent;
        this.prefab = prefab;
        obstacles = new List<Obstacle>();
    }
    public void CreateObstacle(Vector2Int pos){
        Obstacle obs = new Obstacle(pos);
        obstacles.Add(obs);
    }
    public void ClearObstacles(){
        foreach(Obstacle obs in obstacles){
            obs.DestroySelf();
        }
        obstacles.Clear();
    }
    public bool IsObstacleHere(Vector2Int pos){
        foreach(Obstacle obs in obstacles){
            if(obs.pos == pos){
                return true;
            }
        }
        pos -= Services.GameController.offset;
        //check for leaving the boundaries
        if(pos.x < 0 || pos.y < 0 || pos.x >= border.x || pos.y >= border.y){
            return true;
        }
        return false;
    }
}
