using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public LevelLoader myLevelLoader;
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
    public int StringArrayIndex = 0;//5,8,9
    
    //tutorial sprites
    public SpriteRenderer martianSurface;
    public SpriteRenderer grid;
    public SpriteRenderer rover;
    
    //pointer arrows
    public SpriteRenderer roverArrow;
    public SpriteRenderer directionalArrow;
    public SpriteRenderer sendArrow;
    public SpriteRenderer sampleArrow;
    public SpriteRenderer pickupArrow;
    public SpriteRenderer cacheArrow;
    public SpriteRenderer dropoffArrow;
    public SpriteRenderer obstacleArrow1;
    public SpriteRenderer obstacleArrow2;
    public SpriteRenderer menuArrow;

    public float arrowMoveAmount = .25f;
    private Vector2 roverArrowVector;
    private Vector2 directionalArrowVector;
    private Vector2 sendArrowVector;
    private Vector2 sampleArrowVector;
    private Vector2 pickupArrowVector;
    private Vector2 cacheArrowVector;
    private Vector2 dropoffArrowVector;
    private Vector2 obstacleArrow1Vector;
    private Vector2 obstacleArrow2Vector;
    private Vector2 menuArrowVector;
    
    //rover UI
    public Button[] fadeableDirectionalIconArray;
    public Button pickupButton;
    public Button dropoffButton;
    public Image[] fadeableImageArray;
    public GameObject game;
    public GameController gameController;
    public bool canHitContinue;
    public bool alreadyHit;

    // Start is called before the first frame update
    void Start()
    {
        canHitContinue = true;
        if(gameController.currentLevel > 0){
            game.SetActive(true);
            GameObject.Destroy(gameObject);
            gameController.intro = null;
            pickupButton.image.DOFade(1f, 1f);
            dropoffButton.image.DOFade(1f, 1f);
            return;
        }
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
        
        //set arrow Vector2s
        roverArrowVector = roverArrow.gameObject.transform.position;
        directionalArrowVector = directionalArrow.gameObject.transform.position;
        sendArrowVector = sendArrow.gameObject.transform.position;
        sampleArrowVector = sampleArrow.gameObject.transform.position;
        pickupArrowVector = pickupArrow.gameObject.transform.position;
        cacheArrowVector = cacheArrow.gameObject.transform.position;
        dropoffArrowVector = dropoffArrow.gameObject.transform.position;
        obstacleArrow1Vector = obstacleArrow1.gameObject.transform.position;
        obstacleArrow2Vector = obstacleArrow2.gameObject.transform.position;
        menuArrowVector = menuArrow.gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(canHitContinue){
            continueButton.gameObject.SetActive(true);
        }else{
            continueButton.gameObject.SetActive(false);
            if(alreadyHit == false){
                if(StringArrayIndex == 5){
                    if(Services.Rover.waitingForInput == false){
                        fakeContinueButtonClick();
                        alreadyHit = true;
                    }
                }
                if(StringArrayIndex == 8){
                    if(Services.Rover.carryingSample){
                        fakeContinueButtonClick();
                        alreadyHit = true;
                    }
                }
            }
            
        }
    }
    void OnCachePlacement(Eevent e){
        if(alreadyHit == false && StringArrayIndex == 9){
            fakeContinueButtonClick();
            alreadyHit = true;
            Services.EventManager.Unregister<PlacedOnCache>(OnCachePlacement);
        }
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
        LevelTransitionManager.instance.missionControl.gameObject.SetActive(false);
    }
    public void fakeContinueButtonClick(){
        tutorialText.DOFade(0f, textFadeTime).OnComplete(() => tutorialText.DOFade(1f, textFadeTime));
        Invoke("loadNewTutorialText", textFadeTime);
    }
    public void continueButtonClick()
    {
        tutorialText.DOFade(0f, textFadeTime).OnComplete(() => tutorialText.DOFade(1f, textFadeTime));
        Invoke("loadNewTutorialText", textFadeTime);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
    }

    void loadNewTutorialText()
    {
        alreadyHit = false;
        StringArrayIndex++;
        if(StringArrayIndex == 5 || StringArrayIndex == 8 || StringArrayIndex == 9){
            canHitContinue = false;
            if(StringArrayIndex == 9){
                Services.EventManager.Register<PlacedOnCache>(OnCachePlacement);
            }
        }else{
            canHitContinue = true;
        }
        if (StringArrayIndex == 4)
        {
            controlRoomPicture.DOFade(0f, 1f).OnComplete(() => martianSurface.DOFade(1f, 1f));
            game.SetActive(true);
            grid.DOFade(.4f, 1f).SetDelay(1f);
            rover.DOFade(1f, 1f);
            roverArrow.DOFade(1f, 1f).SetDelay(1f);
            roverArrow.transform.DOMove(new Vector2(roverArrowVector.x, roverArrowVector.y - arrowMoveAmount), 1).SetEase(Ease.InOutQuad).SetLoops(4).SetDelay(1f)
                .OnComplete(() => roverArrow.DOFade(0f, 1f).
                    OnComplete(() => roverArrow.gameObject.SetActive(false)));
        }
        
        if (StringArrayIndex == 5)
        {
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
            
            directionalArrow.DOFade(1f, 1f);
            directionalArrow.transform.DOMove(new Vector2(directionalArrowVector.x, directionalArrowVector.y - arrowMoveAmount), 1).SetEase(Ease.InOutQuad).SetLoops(4)
                .OnComplete(() => directionalArrow.DOFade(0f, 1f)
                    .OnComplete(() => directionalArrow.gameObject.SetActive(false)));
            sendArrow.DOFade(1f, 1f).SetDelay(4f);
            sendArrow.transform.DOMove(new Vector2(sendArrowVector.x - arrowMoveAmount, sendArrowVector.y), 1).SetEase(Ease.InOutQuad).SetLoops(4).SetDelay(4f)
                .OnComplete(() => sendArrow.DOFade(0f, 1f).
                    OnComplete(() => sendArrow.gameObject.SetActive(false)));

        }

        if (StringArrayIndex == 6)
        {
            //reset the rover
        }

        if (StringArrayIndex == 7)
        {
            myLevelLoader.LoadLevel(Services.GameController.currentLevel);
            obstacleArrow1.DOFade(1f, 1f);
            obstacleArrow2.DOFade(1f, 1f);
            obstacleArrow1.transform
                .DOMove(new Vector2(obstacleArrow1Vector.x, obstacleArrow1Vector.y - arrowMoveAmount), 1)
                .SetEase(Ease.InOutQuad).SetLoops(4)
                .OnComplete(() => obstacleArrow1.DOFade(0f, 1f));
            obstacleArrow2.transform
                .DOMove(new Vector2(obstacleArrow2Vector.x, obstacleArrow2Vector.y - arrowMoveAmount), 1)
                .SetEase(Ease.InOutQuad).SetLoops(4)
                .OnComplete(() => obstacleArrow2.DOFade(0f, 1f));
        }

        if (StringArrayIndex == 8)
        {
            pickupButton.image.DOFade(1f, 1f);
            sampleArrow.DOFade(1f, 1f);
            sampleArrow.transform.DOMove(new Vector2(sampleArrowVector.x - arrowMoveAmount, sampleArrowVector.y),1).SetEase(Ease.InOutQuad).SetLoops(4)
                .OnComplete(() => sampleArrow.DOFade(0f, 1f).
                    OnComplete(() => sampleArrow.gameObject.SetActive(false)));
            pickupArrow.DOFade(1f, 1f).SetDelay(3f);
            pickupArrow.transform.DOMove(new Vector2(pickupArrowVector.x + arrowMoveAmount, pickupArrowVector.y), 1).SetEase(Ease.InOutQuad).SetLoops(4)
                .SetDelay(3f)
                .OnComplete(() => pickupArrow.DOFade(0f, 1f).
                    OnComplete(() => pickupArrow.gameObject.SetActive(false)));
        }

        if (StringArrayIndex == 9)
        {
            dropoffButton.image.DOFade(1f, 1f);
            cacheArrow.DOFade(1f, 1f);
            cacheArrow.transform.DOMove(new Vector2(cacheArrowVector.x - arrowMoveAmount, cacheArrowVector.y),1).SetEase(Ease.InOutQuad).SetLoops(4)
                .OnComplete(() => cacheArrow.DOFade(0f, 1f).
                    OnComplete(() => cacheArrow.gameObject.SetActive(false)));
            dropoffArrow.DOFade(1f, 1f).SetDelay(3f);
            dropoffArrow.transform.DOMove(new Vector2(dropoffArrowVector.x, dropoffArrowVector.y - arrowMoveAmount), 1).SetEase(Ease.InOutQuad).SetLoops(4)
                .SetDelay(3f)
                .OnComplete(() => dropoffArrow.DOFade(0f, 1f).
                    OnComplete(() => dropoffArrow.gameObject.SetActive(false)));

        }

        if (StringArrayIndex == 13)
        {
            menuArrow.DOFade(1f, 1f);
            menuArrow.transform.DOMove(new Vector2(menuArrowVector.x, menuArrowVector.y + arrowMoveAmount), 1).SetEase(Ease.InOutQuad).SetLoops(4)
               .OnComplete(() => menuArrow.DOFade(0f, 1f).
                   OnComplete(() => menuArrow.gameObject.SetActive(false)));

        }
        if (StringArrayIndex == tutorialStringArray.Length)
        {
            //SceneManager.LoadScene(1);
            Services.GameController.intro = null;
            Destroy(gameObject);
            Services.GameController.currentLevel++;
            Services.GameController.levelLoader.LoadLevel(Services.GameController.currentLevel);
        }
        else
        {
            tutorialText.text = tutorialStringArray[StringArrayIndex];
        }
    }
}
