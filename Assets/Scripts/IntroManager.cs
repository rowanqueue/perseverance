using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{

    public SpriteRenderer titlePicture;

    public SpriteRenderer controlRoomPicture;

    public SpriteRenderer blackBackground;

    public Button playButton;

    public TextMeshProUGUI[] titleTextArray;

    private TextMeshProUGUI playButtonText;
    // Start is called before the first frame update
    void Start()
    {
        playButtonText = playButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        controlRoomPicture.DOFade(0f, .1f);
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
    }
}
