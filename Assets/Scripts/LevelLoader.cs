using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile : byte{
    None,
    Rover,
    Obstacle,
    Sample,
    Cache
}
public class LevelLoader : MonoBehaviour
{
    public TextAsset[] levelTexts;
    public Transform gridParent;
    public GameObject gridSquarePrefab;
    Dictionary<char,Tile> char2Tile = new Dictionary<char, Tile>{
        {'.',Tile.None},
        {' ',Tile.None},
        {'@',Tile.Rover},
        {'#',Tile.Obstacle},
        {'*',Tile.Sample},
        {'h',Tile.Cache}
    };  
    public void DeleteLevel(){
        for(int i = gridParent.childCount-1; i >= 0; i--){
            Destroy(gridParent.GetChild(i).gameObject);
        }
        Services.SampleManager.ClearSamples();
        Services.ObstacleManager.ClearObstacles();
    }
    public Tile[,] LoadLevelFromString(string levelString){
        string[] lines = levelString.Split('\n');
        int width = lines[0].Length-3;
        int height = lines.Length-2;
        Tile[,] _level = new Tile[width,height];
        //checks for acceptable level
        bool hasRover = false;
        bool hasMultipleRover = false;
        bool hasCache = false;
        bool hasMultipleCache = false;
        bool hasSample = false;
        string[] splitLevel = levelString.Split(',');
        Debug.Log(levelString);
        Services.GameController.offset = new Vector2Int(-int.Parse(splitLevel[0]),-int.Parse(splitLevel[1]));
        Services.GameController.scoreToBeat = int.Parse(splitLevel[2]);
        for(int y = 0; y<height;y++){
            for(int x = 0; x<width;x++){
                char c = lines[y+1][x+1];
                Tile tile = char2Tile[c];
                bool acceptable = true;
                switch(tile){
                    case Tile.Rover:
                        if(hasRover){
                            hasMultipleRover = true;
                            acceptable = false;
                        }else{
                            hasRover = true;
                        }
                        break;
                    case Tile.Cache:
                        if(hasCache){
                            hasMultipleCache = true;
                            acceptable = false;
                        }else{
                            hasCache = true;
                        }
                        break;
                    case Tile.Sample:
                        hasSample = true;
                        break;
                }
                if(acceptable){
                    _level[x,y] = tile;
                }

                //Debug.Log(lines[x+1][y+1]);
            }
        }
        if(hasRover == false)
            Debug.Log("ERROR: No Rover");
        if(hasMultipleRover)
            Debug.Log("ERROR: Multiple Rovers");
        if(hasCache == false)
            Debug.Log("ERROR: No Cache");
        if(hasMultipleCache)
            Debug.Log("ERROR: Multiple Caches");
        if(hasSample == false)
            Debug.Log("ERROR: No Sample");
        return _level;
        //Debug.Log(_level[4,4]);
        //Debug.Log(lines[1][1]);
    }
    public void LoadLevel(string levelString){
        Tile[,] _level = LoadLevelFromString(levelString);
        LoadLevel(_level);
    }
    public void LoadLevel(Tile[,] _level){
        Services.GameController.endOfLevel = false;
        if (Services.GameController.currentLevel != 0)
        {
            LevelTransitionManager.instance.openStartFactBox();
        }
        Services.Rover.sendsThisLevel = 0;
        Services.Rover.movesThisLevel = 0;
        DeleteLevel();
        for(int x = 0;x<_level.GetLength(0);x++){
            for(int y = 0; y<_level.GetLength(1);y++){
                
                Vector2Int pos = new Vector2Int(x,_level.GetLength(1)-y-1)+Services.GameController.offset;
                //make a grid sqaure
                Vector3 gridPos = new Vector3(pos.x+0.5f,pos.y-0.5f,0f);
                GameObject obj = Instantiate(gridSquarePrefab,gridPos,Quaternion.identity,gridParent);
                //make the level bro
                switch(_level[x,y]){
                    case Tile.Obstacle:
                        Services.ObstacleManager.CreateObstacle(pos);
                        break;
                    case Tile.Rover:
                        Services.Rover.SetPosition(pos);
                        break;
                    case Tile.Cache:
                        Services.Cache.SetPosition(pos);
                        break;
                    case Tile.Sample:
                        Services.SampleManager.CreateSample(pos);
                        break;
                }
            }
        }
        Services.ObstacleManager.border = new Vector2Int(_level.GetLength(0),_level.GetLength(1));
    }
    public void LoadLevel(int levelNum){
        Services.GameController.currentLevel = levelNum;
        LoadLevel(levelTexts[levelNum].text);
    }
}
