using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private bool isMenuOpen = false;
    public GameObject menu;
    public LevelLoader myLevelLoader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMenuButtonClick()
    {
        if (!isMenuOpen)
        {
            menu.SetActive(true);
            isMenuOpen = true;
            SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
        }
        else
        {
            menu.SetActive(false);
            isMenuOpen = false;
            SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);

        }
    }

    public void onRestartButtonClick()
    {
        myLevelLoader.LoadLevel(Services.GameController.currentLevel);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
        menu.SetActive(false);
    }

    public void onLevelScreenClick()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
        menu.SetActive(false);
        Services.GameController.menuButton.SetActive(false);
        if(Services.GameController.intro != null){
            IntroManager.instance.onSkipButtonPress();
        }else{
            LevelTransitionManager.instance.onLevelScreenButtonPress();
        }
        

    }

    public void onQuitButtonClick()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
        menu.SetActive(false);
        Application.Quit();
    }
}
