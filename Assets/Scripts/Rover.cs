using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Command : byte{
    Forward,
    Backward,
    TurnRight,
    TurnLeft,
    PickUp,
    PutDown,
    Send
}
public class Rover : MonoBehaviour
{
    public bool waitingForInput;
    public int numMoves;
    public int direction;
    List<float> angles = new List<float>{0,-90,-180,-270};
    List<Vector2Int> directions = new List<Vector2Int>{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    public TaskManager taskManager = new TaskManager();
    public List<RoverCommand> moves = new List<RoverCommand>();
    bool doMoves = false;
    float elapsedTime;

    public TextMeshPro text;
    public Transform obstaclesParent;
    List<Vector2Int> obstacles;
    //sample stuff

    public bool carryingSample;
    public Sample sampleCarried;
    public Transform samplesParent;
    List<Sample> samples;

    void Start(){
        direction = 0;
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
        //transform.GetChild(0).localEulerAngles = new Vector3(0,0,angles[direction]);
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
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
                EnterCommand(Command.Forward);
            }
            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
                    EnterCommand(Command.TurnRight);
            }
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                    EnterCommand(Command.Backward);
            }
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
                    EnterCommand(Command.TurnLeft);
            }
            if(Input.GetKeyDown(KeyCode.Q)){
                EnterCommand(Command.PickUp);
            }
            if(Input.GetKeyDown(KeyCode.E)){
                EnterCommand(Command.PutDown);
            }
            if(Input.GetKeyDown(KeyCode.F)){
                EnterCommand(Command.Send);
            }
        }
        
    }
    public void EnterCommand(Command command){
        if(waitingForInput){
            switch(command){
                case Command.Forward:
                    moves.Add(new MoveRover(this, true));
                    text.text +="Forward ";
                    break;
                case Command.Backward:
                    moves.Add(new MoveRover(this, false));
                    text.text +="Backward ";
                    break;
                case Command.TurnLeft:
                    moves.Add(new TurnRover(this, false));
                    text.text +="Left ";
                    break;
                case Command.TurnRight:
                    moves.Add(new TurnRover(this, true));
                    text.text +="Right ";
                    break;
                case Command.PickUp:
                    moves.Add(new PickUpSample(this));
                    text.text +="PickUp ";
                    break;
                case Command.PutDown:
                    moves.Add(new DropSample(this));
                    text.text +="PutDown ";
                    break;
                case Command.Send:
                    SendCommands();
                    break;
            }
        }
        
    }
    public void SendCommands(){
        doMoves = true;
        numMoves = moves.Count;
        elapsedTime = 0;
        //fill commands here
        var move = moves[0];
        for(int i = 1; i < numMoves; i++){
            moves[i-1].Then(moves[i]);
        }
        taskManager.Do(moves[0]);
        moves.Clear();
    }
    public class RoverCommand : Task
    {
        public float duration = 1.0f;
        public Rover rover;
        public float elapsedTime = 0;
        public RoverCommand(Rover rover){
            this.rover = rover;
        }
        protected override  void Initialize(){

        }
        internal override void Update(){
            elapsedTime+=Time.deltaTime;
            if(elapsedTime >=duration){
                SetStatus(TaskStatus.Success);
                return;
            }
        }
        protected override void OnSuccess(){

        }
    }
    public class MoveRover : RoverCommand
    {
        public bool forward;
        Vector2 target;
        Vector2 start;
        bool obstacleHere;
        public MoveRover(Rover rover, bool forward) : base(rover){
            this.rover = rover;
            this.forward = forward;
        }
        protected override void Initialize(){
            Vector2 direction= Vector2.zero;
            if(forward){
                direction = rover.directions[rover.direction];
            }else{
                direction = rover.directions[(rover.direction+2)%4];
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
        internal override void Update(){
            base.Update();
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
    public class TurnRover : RoverCommand
    {
        public bool right;
        float targetAngle;
        float startAngle;
        public TurnRover(Rover rover, bool right) : base(rover){
            this.rover = rover;
            this.right = right;
        }
        protected override void Initialize(){
            bool wrappedAround = false;
            startAngle = rover.angles[rover.direction];
            if(right){
                rover.direction+=1;
                if(rover.direction == 4){
                    wrappedAround = true;
                }
            }else{
                rover.direction-=1;
                if(rover.direction == -1){
                    wrappedAround = true;
                }
            }
            rover.direction = (rover.direction+4)%4;
            //if(rover.direction)
            Debug.Log(rover.direction);
            targetAngle = rover.angles[rover.direction];
            if(wrappedAround){
                targetAngle+= right ? -360 : 360;
            }
        }
        internal override void Update(){
            base.Update();
            rover.transform.GetChild(0).eulerAngles 
            = Vector3.Lerp(new Vector3(0,0,startAngle),
                           new Vector3(0,0,targetAngle),elapsedTime/duration);
        }
    }
    public class PickUpSample : RoverCommand
    {
        bool foundSample;
        Sample sampleToPickUp;
        Vector2 start;
        Vector2 target;
        public PickUpSample(Rover rover) : base(rover){
            this.rover = rover;
        }
        protected override void Initialize(){
            duration *= 0.1f;
            foundSample = false;
            Vector2 pickUpPosition = (Vector2)rover.transform.position+rover.directions[rover.direction];
            if(rover.carryingSample){
                return;
            }
            foreach(Sample sample in rover.samples){
                if(Vector2.Distance(sample.transform.position,pickUpPosition) <= 0.5){
                    sampleToPickUp = sample;
                    foundSample = true;
                    break;
                }
            }
            if(foundSample){
                start = sampleToPickUp.transform.position;
                target = rover.transform.position;
            }
        }
        internal override void Update(){
            base.Update();
            if(foundSample){
                sampleToPickUp.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
            }else{
                elapsedTime = duration;
            }
            
        }
       protected override void OnSuccess(){
            if(foundSample){
                rover.carryingSample = true;
                rover.sampleCarried = sampleToPickUp;
                sampleToPickUp.transform.parent = rover.transform;
                sampleToPickUp.transform.localPosition = Vector2.zero;
            }
        }
    }
    public class DropSample : RoverCommand
    {
        bool canDrop;
        Vector2Int dropPosition;
        Vector2 start;
        Vector2 target;
        public DropSample(Rover rover) : base(rover){
            this.rover = rover;
        }
        protected override void Initialize(){
            duration *= 0.1f;
            canDrop = false;
            Debug.Log(rover.carryingSample);
            if(rover.carryingSample == false){
                Debug.Log("not carrying anthing");
                return;
            }
            dropPosition = new Vector2Int((int)rover.transform.position.x,(int)rover.transform.position.y)+rover.directions[rover.direction];
            if(!rover.obstacles.Contains(dropPosition)){
                canDrop = true;
            }
            if(canDrop){
                start = rover.transform.position;
                target = dropPosition;
            }
        }
        internal override void Update(){
            base.Update();
            if(canDrop && rover.sampleCarried != null){
                rover.sampleCarried.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
            }else{
                elapsedTime = duration;
            }
            
        }
        protected override void OnSuccess(){
            if(canDrop){
                Debug.Log("DROPPED");
                rover.carryingSample = false;
                rover.sampleCarried.transform.position = (Vector2)dropPosition;
                rover.sampleCarried.transform.parent = rover.samplesParent;
                rover.sampleCarried = null;
            }
        }
    }
    
    /*public class MoveRoverT : RoverCommand
    {
        private float duration = 1.0f;
        private Rover rover;
        private float elapsedTime = 0;
        public Vector2 direction;
        Vector2 target;
        Vector2 start;
        bool obstacleHere;
        //pick up stuff
        /*bool pickUpCommand;
        bool foundSample;
        Sample sampleToPickUp;
        //end pick up stuff
        //put down stuff
        bool putDownCommand;
        bool canPutDown;
        Vector2Int putDownDirection;*/
        //end put down stuff
        /*public MoveRover(Rover rover, Vector2 direction)
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
    }*/

}
