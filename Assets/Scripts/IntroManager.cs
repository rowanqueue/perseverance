using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    //title screen elements
    public SpriteRenderer titlePicture;
    public Button playButton;
    public TextMeshProUGUI[] titleTextArray;
    private TextMeshProUGUI playButtonText;
    public SpriteRenderer blackBackground;

    public SpriteRenderer controlRoomPicture;

    //tutorial UI elements
    public Image tutorialBox;
    public Image tutorialBoxOutline;
    private TextMeshProUGUI tutorialText;
    private Button continueButton;
    public float textFadeTime = .25f;

    //tutorial text elements
    private TextAsset tutorialLines;
    private string[] tutorialStringArray;
    private int StringArrayIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //get correct components for various UI elements, then set them inactive/fade them
        playButtonText = playButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        tutorialText = tutorialBox.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        continueButton = tutorialBox.gameObject.GetComponentInChildren<Button>();
        tutorialBox.gameObject.SetActive(false);
        tutorialBoxOutline.gameObject.SetActive(false);
        controlRoomPicture.DOFade(0f, .1f);
        
        //set up the tutorial lines array
        tutorialLines = Resources.Load<TextAsset>("tutorialLines");
        tutorialStringArray = tutorialLines.text.Split('\n');
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPlayButtonPress()
    {
        titlePicture.DOFade(0f, 1f).OnComplete(() => titlePicture.gameObject.SetActive(false));
        playButton.image.DOFade(0f, 1f).OnComplete(() => playButton.gameObject.SetActive(false));
        playButtonText.DOFade(0f, 1f).OnComplete(() => tutorialSetup());
        foreach (var titleAsset in titleTextArray)
        {
            titleAsset.DOFade(0f, 1f);
        }
    }

    void tutorialSetup()
    {
        controlRoomPicture.DOFade(1f, 1f);
        tutorialBox.gameObject.SetActive(true);
        tutorialBoxOutline.gameObject.SetActive(true);
        tutorialBox.DOFade(1f, 2f);
        tutorialBoxOutline.DOFade(1f, 2f);
        tutorialText.text = tutorialStringArray[StringArrayIndex];
        tutorialText.DOFade(1f, 2f);
    }

    public void continueButtonClick()
    {
        tutorialText.DOFade(0f, textFadeTime).OnComplete(() => tutorialText.DOFade(1f, textFadeTime));
        Invoke("loadNewTutorialText", textFadeTime);
    }

    void loadNewTutorialText()
    {
        StringArrayIndex++;
        tutorialText.text = tutorialStringArray[StringArrayIndex];
    }
}
