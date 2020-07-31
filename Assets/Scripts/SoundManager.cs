using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public GameController myGameController;
    private AudioSource soundtrackAudioSource;
    [HideInInspector] public AudioSource UIAudioSource;
    [HideInInspector] public AudioSource RoverAudioSource;
    public AudioClip[] soundtrackMusic;
    public AudioClip buttonClickSound1;
    public AudioClip buttonClickSound2;
    public AudioClip sendingToRoverSound;
    public AudioClip roverBeginMoveForwardSound;
    public AudioClip roverMoveForwardSound;
    public AudioClip roverFinishMoveForwardSound;
    public AudioClip roverTurnSound;
    public AudioClip roverDropSound;
    public AudioClip roverPickupSound;
    public AudioClip roverHitObstacleSound;
    public AudioClip levelCompleteSound;
    public bool soundOn = true;
    public Sprite[] soundSprites;
    public Image soundImage;

    [HideInInspector] public bool roverSoundPlaying = false;
    private float sendingtimer = 0;

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
        soundtrackAudioSource = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        UIAudioSource = gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        RoverAudioSource = gameObject.transform.GetChild(2).GetComponent<AudioSource>();
        if(soundOn){
            soundtrackAudioSource.clip = soundtrackMusic[0];
            soundtrackAudioSource.Play();
            soundtrackAudioSource.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (soundOn && myGameController.currentLevel != 0 && soundtrackAudioSource.clip != soundtrackMusic[1])
        {
            soundtrackAudioSource.clip = soundtrackMusic[1];
            soundtrackAudioSource.Play();
            soundtrackAudioSource.loop = true;
        }
        soundImage.sprite = soundOn ? soundSprites[0] : soundSprites[1];

    }

    public void PlayUISound(AudioClip clip)
    {
        if(soundOn){
            UIAudioSource.PlayOneShot(clip);
        }
        
    }

    public void PlaySendingToRoverSound()
    {
        if(soundOn){
            UIAudioSource.clip = sendingToRoverSound;
            UIAudioSource.Play();
            UIAudioSource.loop = true;
        }
    }

    public void playRoverSound(AudioClip clip)
    {
        if(soundOn){
            Debug.Log("Playing rover sound");
            RoverAudioSource.PlayOneShot(clip);
        }
        
    }
    public void SoundOff(){
        soundOn = !soundOn;
    }
}
