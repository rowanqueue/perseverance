using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
            if (timer >= 3.5f)
            {
                timePassingText.text = timePassingArray[1];
            }

            if (timer >= 5f)
            {
                timePassingText.text = timePassingArray[2];
            }

            if (timer >= 6.5f)
            {
                timePassingText.text = timePassingArray[3];
            }

            if (timer >= 8f)
            {
                timePassingText.text = timePassingArray[4];
            }

            if (timer >= 9.5f)
            {
                timePassingText.text = "";
                SendingToRoverImage.gameObject.SetActive(false);
            }

            if (timer >= 10.5f)
            {
                Services.Rover.SendCommands();
                beamingUp = false;
                timer = 0;
            }
        }

    }

    public void OnForwardButtonPress()
    {
        thisRover.EnterCommand(Command.Forward);
        var newUpArrowIcon = Instantiate(UpArrowPrefab);
        newUpArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newUpArrowIcon);
        Debug.Log("The length of command list is " + commandList.Count);
    }

    public void OnBackwardButtonPress()
    {
        thisRover.EnterCommand(Command.Backward);
        var newDownArrowIcon = Instantiate(DownArrowPrefab);
        newDownArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newDownArrowIcon);
        Debug.Log("The length of command list is " + commandList.Count);

    }

    public void OnRightButtonPress()
    {
        thisRover.EnterCommand(Command.TurnRight);
        var newRightArrowIcon = Instantiate(RightArrowPrefab);
        newRightArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newRightArrowIcon);
    }

    public void OnLeftButtonPress()
    {
        thisRover.EnterCommand(Command.TurnLeft);
        var newLeftArrowIcon = Instantiate(LeftArrowPrefab);
        newLeftArrowIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newLeftArrowIcon);
    }

    public void OnPutdownButtonPress()
    {
        thisRover.EnterCommand(Command.PutDown);
        var newDropoffIcon = Instantiate(DropoffPrefab);
        newDropoffIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newDropoffIcon);
    }

    public void onPickupButtonPress()
    {
        thisRover.EnterCommand(Command.PickUp);
        var newPickupIcon = Instantiate(PickupPrefab);
        newPickupIcon.transform.SetParent(commandBox.transform);
        commandList.Add(newPickupIcon);
    }

    public void onSendButtonPress()
    {
        SendingToRoverImage.gameObject.SetActive(true);
        beamingUp = true;

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
    }

    
    
}
