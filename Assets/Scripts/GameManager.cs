using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; set;}
    public int turn{get; set;}
    public List<TileController> ListTileController => listTileController;
    [SerializeField] private List<TileController> listTileController;
    

    private void Awake()
    {
        Instance = this;
    }

    public (bool,TileState) HasWinner()
    {  
        foreach(var tile in listTileController)
        {
            if(tile.MyState == TileState.Empty) continue;
            foreach(var direction in Enum.GetValues(typeof(Direction)))
            {
               var next =  tile.GetNextTile((Direction)direction);
               if(!next) continue;

               if(next.MyState != tile.MyState) continue;
               
               var last = next.GetNextTile((Direction)direction);
               if(!last) continue;

               if(last.MyState != tile.MyState) continue;

               return (true,tile.MyState);
            }
        }
        return (false,TileState.Empty);

    }

}   
