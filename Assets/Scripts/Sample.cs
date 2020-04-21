using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample
{
    public Vector2Int pos;
    public GameObject obj;
    public bool beingCarried;
    public Sample(Vector2Int pos){
        this.pos = pos;
        obj = GameObject.Instantiate(Services.SampleManager.prefab) as GameObject;
        obj.transform.parent = Services.SampleManager.parent;
        obj.transform.position = (Vector3Int)pos;
        obj.name = "Samp";
    }
    public void PickUp(){
        obj.transform.parent = Services.Rover.transform;
        obj.transform.localPosition = Vector2.zero;
    }
    public void Drop(Vector2Int dropPosition){
        pos = dropPosition;
        obj.transform.position = (Vector3Int)pos;
        obj.transform.parent = Services.SampleManager.parent;
        if(Services.Cache.pos == pos){
            Debug.Log("SUCCEED");
            Services.EventManager.Fire(new PlacedOnCache());
        }
    }

    public void DestroySelf(){
        GameObject.Destroy(obj);
    }
}
