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
        myLevelLoader.LoadLevel(buttonNumber);
    }
}
