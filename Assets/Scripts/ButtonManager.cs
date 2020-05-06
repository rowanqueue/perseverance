using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

using Debug = UnityEngine.Debug;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public static ButtonManager instance;

    public Rover thisRover;

    public GameObject commandBox;

    public Image UpArrowPrefab;

    public Image DownArrowPrefab;

    public Image LeftArrowPrefab;

    public Image RightArrowPrefab;

    public Image PickupPrefab;

    public Image DropoffPrefab;

    public Image SendingToRoverImage;

    private TextAsset timePassingFile;

    private TextMeshProUGUI timePassingText;

    private float timer = 0;

    private bool beamingUp = false;

    private string[] timePassingArray;

    [HideInInspector]public List<Image> commandList;
    public Transform buttonHolder;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        commandList = new List<Image>();
        timePassingText = SendingToRoverImage.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        timePassingText.text = "";
        SendingToRoverImage.gameObject.SetActive(false);
        timePassingFile = Resources.Load<TextAsset>("DataLines");
        timePassingArray = timePassingFile.text.Split('\n');

    }

    // Update is called once per frame
    void Update()
    {
       
        if (beamingUp)
        {
            SendingToRoverImage.gameObject.SetActive(true);
            timePassingText.text = timePassingArray[0];
            timePassingText.gameObject.SetActive(false);
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timePassingText.gameObject.SetActive(true);
            }
            if (timer >= 3f)
            {
                timePassingText.text = timePassingArray[1];
            }

            if (timer >= 4f)
            {
                timePassingText.text = timePassingArray[2];
            }

            if (timer >= 5f)
            {
                timePassingText.text = timePassingArray[3];
            }

            if (timer >= 6f)
            {
                timePassingText.text = timePassingArray[4];
            }

            if (timer >= 7f)
            {
                timePassingText.text = "";
                SendingToRoverImage.gameObject.SetActive(false);
                SoundManager.instance.UIAudioSource.Stop();
                SoundManager.instance.UIAudioSource.loop = false;
            }

            if (timer >= 8f)
            {
                Services.Rover.SendCommands();
                beamingUp = false;
                timer = 0;
            }
        }

    }

    public void OnForwardButtonPress()
    {
        if(commandList.Count >= 6){return;}
        thisRover.EnterCommand(Command.Forward);
        var newUpArrowIcon = Instantiate(UpArrowPrefab);
        newUpArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newUpArrowIcon);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
    }

    public void OnBackwardButtonPress()
    {
        if(commandList.Count >= 6){return;}
        thisRover.EnterCommand(Command.Backward);
        var newDownArrowIcon = Instantiate(DownArrowPrefab);
        newDownArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newDownArrowIcon);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);

    }

    public void OnRightButtonPress()
    {
        if(commandList.Count >= 6){return;}
        thisRover.EnterCommand(Command.TurnRight);
        var newRightArrowIcon = Instantiate(RightArrowPrefab);
        newRightArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newRightArrowIcon);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);
    }

    public void OnLeftButtonPress()
    {
        if(commandList.Count >= 6){return;}
        thisRover.EnterCommand(Command.TurnLeft);
        var newLeftArrowIcon = Instantiate(LeftArrowPrefab);
        newLeftArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newLeftArrowIcon);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);

    }

    public void OnPutdownButtonPress()
    {
        if(commandList.Count >= 6){return;}
        thisRover.EnterCommand(Command.PutDown);
        var newDropoffIcon = Instantiate(DropoffPrefab);
        newDropoffIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newDropoffIcon);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);

    }

    public void onPickupButtonPress()
    {
        if(commandList.Count >= 6){return;}
        thisRover.EnterCommand(Command.PickUp);
        var newPickupIcon = Instantiate(PickupPrefab);
        newPickupIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newPickupIcon);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound2);

    }

    public void onSendButtonPress()
    {
        SendingToRoverImage.gameObject.SetActive(true);
        beamingUp = true;
        SoundManager.instance.PlaySendingToRoverSound();
    }

    public void OnDeleteButtonPress()
    {
        if (commandList.Count == 0)
        {
            //if there is nothing in the list, it will stop running the function right here
            return;
        }
        
        Destroy(commandList[commandList.Count - 1].gameObject);
        commandList.RemoveAt(commandList.Count-1);
        thisRover.moves.RemoveAt(thisRover.moves.Count-1);
        SoundManager.instance.PlayUISound(SoundManager.instance.buttonClickSound1);
    }

    
    
}
