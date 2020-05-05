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

    private TextMeshProUGUI scoreText;

    private TextMeshProUGUI finishFactText;

    private TextMeshProUGUI scoreLevelDesignationText;

    public Image blackBackground;

    private TextAsset startFacts;

    private string[] startFactArray;

    private TextAsset finishFacts;

    private string[] finishFactArray;

    private int startFactIndex = 0;

    private int finishFactIndex = 0;

    public SpriteRenderer martianSurface;

    public SpriteRenderer missionControl;

    public string[] levelDesignations;

    public Button[] levelButtons;

    private void Awake()
    {
        instance = this;
        startFactText = startFactHolder.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        startFacts = Resources.Load<TextAsset>("StartFacts");
        startFactArray = startFacts.text.Split('\n');

        finishFacts = Resources.Load<TextAsset>("FinishFacts");
        finishFactArray = finishFacts.text.Split('\n');

        scoreText = finishFactHolder.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        finishFactText = finishFactHolder.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        scoreLevelDesignationText = finishFactHolder.gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void openStartFactBox()
    {
        startFactHolder.gameObject.SetActive(true);
        blackBackground.gameObject.SetActive(true);
        startFactText.text = startFactArray[startFactIndex];
    }
    
    public void onStartFactBoxButtonPress()
    {
        startFactHolder.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        startFactIndex++;
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
    }

    public void openFinishFactBox()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.levelCompleteSound);
        blackBackground.gameObject.SetActive(true);
        finishFactHolder.SetActive(true);
        finishFactText.text = finishFactArray[finishFactIndex];
    }

    public void onLevelScreenButtonPress()
    {
        finishFactIndex++;
        finishFactHolder.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        missionControl.gameObject.SetActive(true);
        martianSurface.gameObject.SetActive(false);

        ButtonManager.instance.buttonHolder.gameObject.SetActive(false);
        ButtonManager.instance.commandBox.gameObject.SetActive(false);

        foreach (var butt in levelButtons)
        {
            butt.gameObject.SetActive(true);
        }

    }

}
