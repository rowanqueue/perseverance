using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public GameController myGameController;
    private AudioSource myAudioSource;
    public AudioClip backgroundMusic1;
    public AudioClip backgroundMusic2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = gameObject.GetComponent<AudioSource>();
        myAudioSource.clip = backgroundMusic1;
        myAudioSource.Play();
        myAudioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myGameController.currentLevel != 0 && myAudioSource.clip != backgroundMusic2)
        {
            myAudioSource.clip = backgroundMusic2;
            myAudioSource.Play();
            myAudioSource.loop = true;
        }
    }
}
