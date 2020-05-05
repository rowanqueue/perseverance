using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private bool isMenuOpen = false;
    public GameObject menu;
    public LevelLoader myLevelLoader;
    public GameController myGameController;
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
        myLevelLoader.LoadLevel(myGameController.currentLevel);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
    }

    public void onLevelScreenClick()
    {
        LevelTransitionManager.instance.onLevelScreenButtonPress();
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);

    }

    public void onQuitButtonClick()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
        Application.Quit();
    }
}
