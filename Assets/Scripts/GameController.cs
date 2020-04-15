using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        FactManager.instance.openFactBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }
    void InitializeServices(){
        Services.GameController = this;
        Services.ObstacleManager = new ObstacleManager();
        Services.ObstacleManager.Initialize(obstaclesParent,obstaclePrefab);

        Services.SampleManager = new SampleManager();
        Services.SampleManager.Initialize(samplesParent,samplePrefab);

        Services.Rover = rover;
        Services.Cache = new Cache(Vector2Int.zero);
        
    }
}
