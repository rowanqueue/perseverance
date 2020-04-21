using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
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
    
    //tutorial sprites
    public SpriteRenderer martianSurface;
    public SpriteRenderer grid;
    public SpriteRenderer rover;
    public Rigidbody2D roverArrow;
    public Rigidbody2D directionalArrow;
    public Rigidbody2D sendArrow;
    
    //rover UI
    public Button[] fadeableDirectionalIconArray;
    public Button[] fadeablePickupDropoffIconArray;
    public Image[] fadeableImageArray;

    // Start is called before the first frame update
    void Start()
    {
        //get correct components for various UI elements, then set them inactive/fade them
        playButtonText = playButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        tutorialText = tutorialBox.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        continueButton = tutorialBox.gameObject.GetComponentInChildren<Button>();
        /*continueButton.image.color = clearButtonColor;
        tutorialText.color = clearBlack;
        tutorialBox.color = clearWhite;
        tutorialBoxOutline.color = clearTutorialBoxOutline;*/
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
        continueButton.image.DOFade(1f, 2f);
    }

    public void continueButtonClick()
    {
        tutorialText.DOFade(0f, textFadeTime).OnComplete(() => tutorialText.DOFade(1f, textFadeTime));
        Invoke("loadNewTutorialText", textFadeTime);
    }

    void loadNewTutorialText()
    {
        StringArrayIndex++;
        if (StringArrayIndex == 5)
        {
            controlRoomPicture.DOFade(0f, 1f).OnComplete(() => martianSurface.DOFade(1f, 1f));
            grid.DOFade(1f, 1f).SetDelay(1f);
            rover.DOFade(1f, 1f);
            roverArrow.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 1f);
            roverArrow.transform.DOMove(new Vector2(0.45f,0.232f), 1).SetEase(Ease.InOutQuad).SetLoops(8);
        }
        
        if (StringArrayIndex == 6)
        {
            roverArrow.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 1f)
                .OnComplete(() => roverArrow.gameObject.SetActive(false));
            foreach (var butt in fadeableDirectionalIconArray)
            {
                butt.gameObject.SetActive(true);
                butt.image.DOFade(1f, 1f);
            }

            foreach (var UIBox in fadeableImageArray)
            {
                UIBox.gameObject.SetActive(true);
                UIBox.DOFade(.8f, 1f);
            }
            
            directionalArrow.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 1f);
            directionalArrow.transform.DOMove(new Vector2(-0.8462309f, -1.95f), 1).SetEase(Ease.InOutQuad).SetLoops(4)
                .OnComplete(() =>
                    sendArrow.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 1f))
                .OnComplete(() =>
                    sendArrow.transform.DOMove(new Vector2(-0.51f, 1.3f), 1).SetEase(Ease.InOutQuad).SetLoops(4)
                );

        }
        if (StringArrayIndex == tutorialStringArray.Length)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            tutorialText.text = tutorialStringArray[StringArrayIndex];
        }
    }
}
