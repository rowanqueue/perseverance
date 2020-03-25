using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Command : byte{
    Up,
    Right,
    Down,
    Left,
    PickUp,
    PutDown
}
public class Rover : MonoBehaviour
{
    public bool waitingForInput;
    public int numMoves;
    public int lastDirection;
    List<float> angles = new List<float>{0,-90,-180,-270};
    List<Vector2Int> directions = new List<Vector2Int>{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    public TaskManager taskManager = new TaskManager();
    public List<MoveRover> moves = new List<MoveRover>();
    bool doMoves = false;
    float elapsedTime;

    public TextMeshPro text;
    public Transform obstaclesParent;
    List<Vector2Int> obstacles;
    //sample stuff

    public Sample sampleCarried;
    public Transform samplesParent;
    List<Sample> samples;

    void Start(){
        lastDirection = 0;
        obstacles = new List<Vector2Int>();
        samples = new List<Sample>();
        foreach(Transform child in obstaclesParent){
            obstacles.Add(new Vector2Int((int)child.transform.position.x,(int)child.transform.position.y));
        }
        foreach(Transform child in samplesParent){
            samples.Add(child.GetComponent<Sample>());
        }
    }
    void Update()
    {
        //set direction
        transform.GetChild(0).localEulerAngles = new Vector3(0,0,angles[lastDirection]);
        waitingForInput = !doMoves;
        taskManager.Update();
        if(doMoves){
            elapsedTime+=Time.deltaTime;
            //taskManager.Update();
            if(elapsedTime >= numMoves*0.25f){
                doMoves = false;
                text.text = "";
            }
        }else{
            if(moves.Count >= numMoves){
                doMoves = true;
                elapsedTime = 0;
                //fill commands here
                var move = moves[0];
                for(int i = 1; i < numMoves; i++){
                    moves[i-1].Then(moves[i]);
                }
                taskManager.Do(moves[0]);
                moves.Clear();
            }else{
                if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
                    EnterCommand(Command.Up);
                }
                if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
                     EnterCommand(Command.Right);
                }
                if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                     EnterCommand(Command.Down);
                }
                if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
                     EnterCommand(Command.Left);
                }
                if(Input.GetKeyDown(KeyCode.Q)){
                    EnterCommand(Command.PickUp);
                }
                if(Input.GetKeyDown(KeyCode.E)){
                    EnterCommand(Command.PutDown);
                }
            }
        }
        
    }
    public void EnterCommand(Command command){
        if(waitingForInput){
            switch(command){
                case Command.Up:
                    moves.Add(new MoveRover(this, Vector2.up));
                    text.text +="Up ";
                    break;
                case Command.Right:
                    moves.Add(new MoveRover(this, Vector2.right));
                    text.text +="Right ";
                    break;
                case Command.Left:
                    moves.Add(new MoveRover(this, Vector2.left));
                    text.text +="Left ";
                    break;
                case Command.Down:
                    moves.Add(new MoveRover(this, Vector2.down));
                    text.text +="Down ";
                    break;
                case Command.PickUp:
                    moves.Add(new MoveRover(this, Vector2.up*2));
                    text.text +="PickUp ";
                    break;
                case Command.PutDown:
                    moves.Add(new MoveRover(this, Vector2.up*-2));
                    text.text +="PutDown ";
                    break;
            }
        }
        
    }
    public class MoveRover : Task
    {
        private float duration = 1.0f;
        private Rover rover;
        private float elapsedTime = 0;
        public Vector2 direction;
        Vector2 target;
        Vector2 start;
        bool obstacleHere;
        //pick up stuff
        bool pickUpCommand;
        bool foundSample;
        Sample sampleToPickUp;
        //end pick up stuff
        //put down stuff
        bool putDownCommand;
        bool canPutDown;
        Vector2Int putDownDirection;
        //end put down stuff
        public MoveRover(Rover rover, Vector2 direction)
        {
            this.direction = direction;
            this.rover = rover;
            duration = 0.25f;
            elapsedTime = 0f;
            if(direction == Vector2.up*2){
                Debug.Log("Pickup");
                pickUpCommand = true;
            }
            if(direction == Vector2.up*-2){
                Debug.Log("PutDown");
                pickUpCommand = true;
                putDownCommand = true;
            }
        }
        protected override void Initialize(){
            if(pickUpCommand){
                if(putDownCommand){
                    if(rover.sampleCarried == null){
                        canPutDown = false;
                        return;
                    }
                    canPutDown = true;
                    putDownDirection = Vector2Int.down;
                    //here is where we would figure out if you can put it down, will figure out later
                }else{
                    if(rover.sampleCarried != null){
                        return;
                    }
                    foreach(Sample sample in rover.samples){
                        Debug.Log(sample);
                        Debug.Log(rover);
                        if(Vector2.Distance(sample.transform.position,rover.transform.position) <= 1){
                            sampleToPickUp = sample;
                            foundSample = true;
                            break;
                        }
                    }
                }
                
            }else{
                if(direction == Vector2.up){
                    rover.lastDirection = 0;
                }
                if(direction == Vector2.right){
                    rover.lastDirection = 1;
                }
                if(direction == Vector2.down){
                    rover.lastDirection = 2;
                }
                if(direction == Vector2.left){
                    rover.lastDirection = 3;
                }
                start = (Vector2)rover.transform.position;
                target = (Vector2)rover.transform.position+direction;
                target.x = Mathf.RoundToInt(target.x);
                target.y = Mathf.RoundToInt(target.y);
                Vector2Int targetExact = new Vector2Int((int)target.x,(int)target.y);
                if(rover.obstacles.Contains(targetExact)){
                    obstacleHere = true;
                    //SetStatus(TaskStatus.Success);
                }
                foreach(Sample sample in rover.samples){
                    if(sample != rover.sampleCarried){
                        if((Vector2)sample.transform.position == targetExact){
                            obstacleHere = true;
                        }
                    }
                }
            }
            
        }
        internal override void Update(){
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= duration){
                if(pickUpCommand){
                    if(putDownCommand){
                        if(canPutDown){
                            rover.sampleCarried.transform.position = (Vector2)rover.transform.position + putDownDirection;
                            rover.sampleCarried.transform.parent = rover.samplesParent;
                            rover.sampleCarried = null;
                        }
                    }else{
                        if(foundSample){
                            rover.sampleCarried = sampleToPickUp;
                            sampleToPickUp.transform.parent = rover.transform;
                            sampleToPickUp.transform.localPosition = Vector2.zero;
                        }
                    }
                    
                   
                }else{
                    if(obstacleHere){
                        rover.transform.position = start;
                    }else{
                        rover.transform.position = target;
                    }
                }
                
                SetStatus(TaskStatus.Success);
                return;
            }
            if(pickUpCommand){

            }else{
                if(obstacleHere){
                    if(elapsedTime < duration*0.5f){
                        rover.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
                    }else{
                        rover.transform.position = Vector2.Lerp(target,start,elapsedTime/duration);
                    }
                }else{
                    rover.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
                }
            }
            
            
        }
        protected override void OnSuccess(){
            Debug.Log("SUCCESS");
            //rover.transform.position = target;
        }
    }

}
