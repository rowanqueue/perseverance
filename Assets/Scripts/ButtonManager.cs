using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public Rover thisRover;

    public GameObject commandBox;

    public Image UpArrowPrefab;

    public Image DownArrowPrefab;

    public Image LeftArrowPrefab;

    public Image RightArrowPrefab;

    public Image PickupPrefab;

    public Image DropoffPrefab;

    private List<Image> commandList;
    // Start is called before the first frame update
    void Start()
    {    
        commandList = new List<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnForwardButtonPress()
    {
        thisRover.EnterCommand(Command.Forward);
        Instantiate(UpArrowPrefab).transform.SetParent(commandBox.transform);
    }

    public void OnBackwardButtonPress()
    {
        thisRover.EnterCommand(Command.Backward);
        Instantiate(DownArrowPrefab).transform.SetParent(commandBox.transform);
    }

    public void OnRightButtonPress()
    {
        thisRover.EnterCommand(Command.TurnRight);
        Instantiate(LeftArrowPrefab).transform.SetParent(commandBox.transform);
    }

    public void OnLeftButtonPress()
    {
        thisRover.EnterCommand(Command.TurnLeft);
        Instantiate(RightArrowPrefab).transform.SetParent(commandBox.transform);
    }

    public void OnPutdownButtonPress()
    {
        thisRover.EnterCommand(Command.PutDown);
        Instantiate(DropoffPrefab).transform.SetParent(commandBox.transform);
    }

    public void onPickupButtonPress()
    {
        thisRover.EnterCommand(Command.PickUp);
        Instantiate(PickupPrefab).transform.SetParent(commandBox.transform);
    }

    public void onSendButtonPress()
    {
        thisRover.EnterCommand(Command.Send);
    }
}
