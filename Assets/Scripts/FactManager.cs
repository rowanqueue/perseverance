using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactManager : MonoBehaviour
{
    public static FactManager instance;
    
    public GameObject startFactHolder;

    private TextMeshProUGUI startFactText;

    public GameObject finishFactHolder;

    private TextMeshProUGUI scoreText;

    private TextMeshProUGUI finishFactText;

    public Image blackBackground;

    private TextAsset startFacts;

    private string[] startFactArray;

    private TextAsset finishFacts;

    private string[] finishFactArray;

    private int startFactIndex = 0;

    private int finishFactIndex = 0;

    public SpriteRenderer martianSurface;

    public SpriteRenderer missionControl;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        startFactHolder.SetActive(false);
        finishFactHolder.SetActive(false);
        missionControl.gameObject.SetActive(false);
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

    public void onLevelSelectButtonPress()
    {
        finishFactIndex++;
        finishFactHolder.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        missionControl.gameObject.SetActive(true);
        martianSurface.gameObject.SetActive(false);

    }
}
