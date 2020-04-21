using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public List<Sprite> directionalSprites;
    List<float> angles = new List<float>{0,-90,-180,-270};
    List<Vector2Int> directions = new List<Vector2Int>{Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    public TaskManager taskManager = new TaskManager();
    public List<RoverCommand> moves = new List<RoverCommand>();
    bool doMoves = false;
    float elapsedTime;
    float timeForCommands;

    public TextMeshPro text;
    //sample stuff

    public bool carryingSample;
    public Sample sampleCarried;
    SpriteRenderer spriteRenderer;

    void Start(){
        spriteRenderer  = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        //set direction
        if(sampleCarried != null){
            sampleCarried.pos = new Vector2Int((int)transform.position.x,(int)transform.position.y);
        }
        spriteRenderer.sprite = directionalSprites[direction];
        //transform.GetChild(0).localEulerAngles = new Vector3(0,0,angles[direction]);
        waitingForInput = !doMoves;
        taskManager.Update();
        if(doMoves){
            elapsedTime+=Time.deltaTime;
            //taskManager.Update();
            if(elapsedTime >= timeForCommands){
                foreach (Image command in ButtonManager.instance.commandList)
                {
                    Destroy(command.gameObject);
                }
                ButtonManager.instance.commandList.Clear();
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
        if(numMoves == 0){
            Debug.Log("ERROR: No moves input");
            return;
        }
        elapsedTime = 0;
        //fill commands here
        var move = moves[0];
        for(int i = 1; i < numMoves; i++){
            moves[i-1].Then(moves[i]);
        }
        taskManager.Do(moves[0]);
        timeForCommands = 0;
        for(int i = 0; i < numMoves;i++){
            timeForCommands += moves[i].duration;
        }
        moves.Clear();
    }
    public void SetPosition(Vector2Int pos){
        transform.position = (Vector3Int)pos;
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
            if(Services.ObstacleManager.IsObstacleHere(targetExact)){
                obstacleHere = true;
                //SetStatus(TaskStatus.Success);
            }
            if(Services.SampleManager.IsSampleHere(targetExact)){
                obstacleHere = true;
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
            duration = 0.25f;
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
            /*rover.transform.GetChild(0).eulerAngles 
            = Vector3.Lerp(new Vector3(0,0,startAngle),
                           new Vector3(0,0,targetAngle),elapsedTime/duration);*/
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
            Vector2Int pickUpPosition = new Vector2Int((int)rover.transform.position.x,(int)rover.transform.position.y) + rover.directions[rover.direction];
            if(rover.carryingSample){
                return;
            }
            if(Services.SampleManager.IsSampleHere(pickUpPosition)){
                sampleToPickUp = Services.SampleManager.GetSampleHere(pickUpPosition);
                foundSample = true;
            }
            if(foundSample){
                start = sampleToPickUp.pos;
                target = rover.transform.position;
            }
        }
        internal override void Update(){
            base.Update();
            if(foundSample){
                sampleToPickUp.obj.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
            }else{
                elapsedTime = duration;
            }
            
        }
       protected override void OnSuccess(){
            if(foundSample){
                rover.carryingSample = true;
                rover.sampleCarried = sampleToPickUp;
                rover.sampleCarried.PickUp();
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
            if(!Services.ObstacleManager.IsObstacleHere(dropPosition)){
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
                rover.sampleCarried.obj.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
            }else{
                elapsedTime = duration;
            }
            
        }
        protected override void OnSuccess(){
            if(canDrop){
                Debug.Log("DROPPED");
                rover.carryingSample = false;
                rover.sampleCarried.Drop(dropPosition);

                rover.sampleCarried = null;
            }
        }
    }

}
