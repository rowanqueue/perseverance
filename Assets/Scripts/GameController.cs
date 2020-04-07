using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //settings
    public Vector2Int offset;
    //stuff
    public Rover rover;
    public Transform obstaclesParent;
    public GameObject obstaclePrefab;
    public GameObject cachePrefab;
    public Transform samplesParent;
    public GameObject samplePrefab;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeServices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeServices(){
        Services.GameController = this;
        Services.ObstacleManager = new ObstacleManager();
        Services.ObstacleManager.Initialize(obstaclesParent,obstaclePrefab);

        Services.SampleManager = new SampleManager();
        Services.SampleManager.Initialize(samplesParent,samplePrefab);

        Services.Rover = rover;
        Services.Cache = new Cache(Vector2Int.zero);
        
        FactManager.instance.openFactBox();
    }
}
