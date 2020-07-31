using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionManager : MonoBehaviour
{

    public static LevelTransitionManager instance;
    
    public GameObject startFactHolder;

    private TextMeshProUGUI startFactText;

    public GameObject finishFactHolder;
    public Image[] stars;
    public Sprite[] starSprites;

    public TextMeshProUGUI finishFactText;

    public TextMeshProUGUI scoreLevelDesignationText;

    public Image blackBackground;

    public TextAsset startFacts;

    private string[] startFactArray;

    public TextAsset finishFacts;

    private string[] finishFactArray;

    private int startFactIndex = 0;

    private int finishFactIndex = 0;

    public SpriteRenderer martianSurface;

    public SpriteRenderer missionControl;

    public string[] levelDesignations;

    public GameObject buttonHolder;

    private void Awake()
    {
        instance = this;
        startFactText = startFactHolder.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        //startFacts = Resources.Load<TextAsset>("StartFacts");
        startFactArray = startFacts.text.Split('\n');
        Debug.Log("The length of startFactArray is " + startFactArray.Length);

        //finishFacts = Resources.Load<TextAsset>("FinishFacts");
        finishFactArray = finishFacts.text.Split('\n');

    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    }


    public void openStartFactBox()
    {
        ButtonManager.instance.buttonHolder.gameObject.SetActive(true);
        ButtonManager.instance.commandBox.gameObject.transform.parent.gameObject.SetActive(true);
        martianSurface.gameObject.SetActive(true);

        startFactHolder.gameObject.SetActive(true);
        blackBackground.gameObject.SetActive(true);
        startFactText.text = startFactArray[startFactIndex];
    }
    
    public void onStartFactBoxButtonPress()
    {
        startFactHolder.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        startFactIndex++;
        if (startFactIndex == startFactArray.Length)
        {
            startFactIndex = 0;
        }
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
    }

    public void openFinishFactBox()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.levelCompleteSound);
        blackBackground.gameObject.SetActive(true);
        finishFactHolder.SetActive(true);
        finishFactText.text = finishFactArray[finishFactIndex];
        if (SoundManager.instance.RoverAudioSource.isPlaying)
        {
            SoundManager.instance.RoverAudioSource.Stop();
        }
    }

    public void onLevelScreenButtonPress()
    {
        finishFactIndex++;
        if (finishFactIndex == finishFactArray.Length)
        {
            finishFactIndex = 0;
        }
        ButtonManager.instance.buttonHolder.gameObject.SetActive(false);
        ButtonManager.instance.commandBox.gameObject.transform.parent.gameObject.SetActive(false);
        finishFactHolder.SetActive(false);
        startFactHolder.gameObject.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        missionControl.gameObject.SetActive(true);
        martianSurface.gameObject.SetActive(false);

        ButtonManager.instance.buttonHolder.gameObject.SetActive(false);
        ButtonManager.instance.commandBox.gameObject.transform.parent.gameObject.SetActive(false);

        buttonHolder.SetActive(true);

        Services.GameController.levelLoader.DeleteLevel();

    }

}
