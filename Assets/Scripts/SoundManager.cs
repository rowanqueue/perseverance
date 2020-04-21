using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    private AudioSource myAudioSource;
    public AudioClip backgroundMusic;

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
        myAudioSource.clip = backgroundMusic;
        myAudioSource.Play();
        myAudioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
