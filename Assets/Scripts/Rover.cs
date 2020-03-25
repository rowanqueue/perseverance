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
    public TaskManager taskManager = new TaskManager();
    public List<MoveRover> moves = new List<MoveRover>();
    bool doMoves = false;
    float elapsedTime;

    public TextMeshPro text;

    void Update()
    {
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
        public MoveRover(Rover rover, Vector2 direction)
        {
            this.direction = direction;
            this.rover = rover;
            duration = 0.25f;
            elapsedTime = 0f;
        }
        protected override void Initialize(){
            start = (Vector2)rover.transform.position;
            target = (Vector2)rover.transform.position+direction;
            target.x = Mathf.RoundToInt(target.x);
            target.y = Mathf.RoundToInt(target.y);
        }
        internal override void Update(){
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= duration){
                Debug.Log("E");
                SetStatus(TaskStatus.Success);
                return;
            }
            rover.transform.position = Vector2.Lerp(start,target,elapsedTime/duration);
        }
        protected override void OnSuccess(){
            Debug.Log("SUCCESS");
            rover.transform.position = target;
        }
    }

}
