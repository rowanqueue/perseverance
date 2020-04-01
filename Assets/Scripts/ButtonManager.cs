using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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
        thisRover.EnterCommand(Command.Send);
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
