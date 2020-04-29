using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactManager : MonoBehaviour
{
    public static FactManager instance;
    
    public Image factHolderImage;

    private TextMeshProUGUI factText;

    public Image factHolderTitle;

    public Image blackBackground;

    public string[] startFacts;

    [HideInInspector] public int startFactIndex = 0;

    private void Awake()
    {
        instance = this;
        factText = factHolderImage.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openFactBox()
    {
        factHolderImage.gameObject.SetActive(true);
        factHolderTitle.gameObject.SetActive(true);
        blackBackground.gameObject.SetActive(true);
        factText.text = startFacts[startFactIndex];
    }
    
    public void onFactBoxButtonPress()
    {
        factHolderImage.gameObject.SetActive(false);
        factHolderTitle.gameObject.SetActive(false);
        blackBackground.gameObject.SetActive(false);
        startFactIndex++;
        SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound1);
    }
}
