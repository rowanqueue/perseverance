using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtonClick : MonoBehaviour
{
    public int buttonNumber;
    public LevelLoader myLevelLoader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonClick()
    {
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
        myLevelLoader.LoadLevel(buttonNumber);
        LevelTransitionManager.instance.missionControl.gameObject.SetActive(false);
        LevelTransitionManager.instance.buttonHolder.SetActive(false);

        ButtonManager.instance.verticalBox.SetActive(true);
        ButtonManager.instance.horizontalBox.SetActive(true);
        LevelTransitionManager.instance.startFactHolder.gameObject.SetActive(true);
        LevelTransitionManager.instance.blackBackground.gameObject.SetActive(true);
        LevelTransitionManager.instance.martianSurface.gameObject.SetActive(true);

        Services.GameController.menuButton.SetActive(true);
    }
}
