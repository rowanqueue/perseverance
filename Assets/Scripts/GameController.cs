using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //settings
    public Vector2Int offset;
    public int currentLevel;
    public LevelLoader levelLoader;
    //stuff
    public Rover rover;
    public Transform obstaclesParent;
    public GameObject obstaclePrefab;
    public GameObject cachePrefab;
    public Transform samplesParent;
    public GameObject samplePrefab;
    public bool isTutorial;
    public IntroManager intro;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeServices();
        Services.EventManager.Register<PlacedOnCache>(OnCachePlacement);
    }

    private void Start()
    {
        //Elizabeth's note: 0 is the tutorial scene, so no facts needed. If the scene index is not 0, pop open the fact box.
        /*if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            FactManager.instance.openFactBox();
        }*/
        levelLoader.LoadLevel(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
      
        if(ReferenceEquals(intro,null)){
            isTutorial = false;
        }else{
            isTutorial = true;
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

        Services.EventManager = new EventManager();
        
    }

    void OnCachePlacement(Eevent e){
        if(ReferenceEquals(intro,null) == false){
            //intro is still happening so don't go to next level
            return;
        }
        currentLevel++;
        levelLoader.LoadLevel(currentLevel);
    }
}
