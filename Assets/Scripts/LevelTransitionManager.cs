using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelTransitionManager : MonoBehaviour
{

    public static LevelTransitionManager instance;
    
    public GameObject startFactHolder;

    public TextMeshProUGUI startFactText;

    public GameObject finishFactHolder;
    public Image[] stars;
    public Sprite[] starSprites;

    public TextMeshProUGUI finishFactText;

    public TextMeshProUGUI scoreLevelDesignationText;

    public Image blackBackground;

    public TextAsset startFacts;

    private List<string> startFactList = new List<string>();

    public TextAsset finishFacts;

    private List<string> finishFactList = new List<string>();

    //private int startFactIndex = 0;

    //private int finishFactIndex = 0;

    public SpriteRenderer martianSurface;

    public SpriteRenderer missionControl;

    public string[] levelDesignations;

    public GameObject buttonHolder;

    private void Awake()
    {
        instance = this;
        //startFactText = startFactHolder.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        //startFacts = Resources.Load<TextAsset>("StartFacts");
        startFactList = startFacts.text.Split('\n').ToList();

        //finishFacts = Resources.Load<TextAsset>("FinishFacts");
        finishFactList = finishFacts.text.Split('\n').ToList();

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("The length of cleanStartFactList is " + startFactList.Count);
        Debug.Log("The length of cleanFinishFactList is " + finishFactList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        for(var i = 0; i < 3;i++){
            if(i < Services.GameController.score){
                stars[i].sprite = starSprites[0];
            }else{
                stars[i].sprite = starSprites[1];
            }
        }
        //startFactText.text = startFactArray[startFactIndex];

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            if (finishFactHolder.activeInHierarchy == true)
            {
                FinishFactTest();
            }
        }*/
    }


    public void openStartFactBox()
    {
        ButtonManager.instance.verticalBox.gameObject.SetActive(true);
        ButtonManager.instance.horizontalBox.gameObject.transform.parent.gameObject.SetActive(true);
        martianSurface.gameObject.SetActive(true);

        startFactHolder.gameObject.SetActive(true);
        blackBackground.gameObject.SetActive(true);
        var random = new System.Random();
        int myRandomStartNum = random.Next(startFactList.Count);
        startFactText.text = startFactList[myRandomStartNum];
        startFactList.Remove(startFactList[myRandomStartNum]);
        Debug.Log("I have played and removed the " + myRandomStartNum + " from the startFactList");

    }
    
    public void onStartFactBoxButtonPress()
    {
        startFactHolder.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        if (startFactList.Count == 0)
        {
            RefreshFactList("start");
        }
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
    }

    public void openFinishFactBox()
    {
        if (finishFactList.Count == 0)
        {
            RefreshFactList("finish");
        }
        SoundManager.instance.PlayUISound(SoundManager.instance.levelCompleteSound);
        blackBackground.gameObject.SetActive(true);
        finishFactHolder.SetActive(true);
        var random = new System.Random();
        var myRandomFinishNum = random.Next(finishFactList.Count);
        finishFactText.text = finishFactList[myRandomFinishNum];
        scoreLevelDesignationText.text = levelDesignations[Services.GameController.score-1 >= 0 ? Services.GameController.score-1 : 0];
        finishFactList.Remove(finishFactList[myRandomFinishNum]);
        Debug.Log("I have played and removed the " + myRandomFinishNum + " from the finishFactList");

        if (SoundManager.instance.RoverAudioSource.isPlaying)
        {
            SoundManager.instance.RoverAudioSource.Stop();
        }
    }

    /*public void FinishFactTest()
    {
        if (finishFactList.Count == 0)
        {
            RefreshFactList("finish");
        }
        var random = new System.Random();
        var myRandomFinishNum = random.Next(finishFactList.Count);
        finishFactText.text = finishFactList[myRandomFinishNum];
        scoreLevelDesignationText.text = levelDesignations[Services.GameController.score - 1 >= 0 ? Services.GameController.score - 1 : 0];
        finishFactList.Remove(finishFactList[myRandomFinishNum]);
        Debug.Log("I have played and removed the " + myRandomFinishNum + " from the finishFactList");   

        if (SoundManager.instance.RoverAudioSource.isPlaying)
        {
            SoundManager.instance.RoverAudioSource.Stop();
        }
    }*/

    public void onLevelScreenButtonPress()
    {
        if (finishFactList.Count == 0)
        {
            RefreshFactList("finish");
        }
        ButtonManager.instance.verticalBox.gameObject.SetActive(false);
        ButtonManager.instance.horizontalBox.gameObject.SetActive(false);
        finishFactHolder.SetActive(false);
        startFactHolder.gameObject.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        missionControl.gameObject.SetActive(true);
        martianSurface.gameObject.SetActive(false);


        buttonHolder.SetActive(true);

        Services.GameController.levelLoader.DeleteLevel();

    }

    public void RefreshFactList(string factListPicker)
    {
        if (factListPicker == "start")
        {
            startFactList = startFacts.text.Split('\n').ToList();
            Debug.Log("Resetting startFactList...");

        }
        else if (factListPicker == "finish")
        {
            finishFactList = finishFacts.text.Split('\n').ToList();
            Debug.Log("Resetting finishFactList...");
        }
    }

}
