using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; set;}
    public int turn{get; set;}
    public List<TileController> ListTileController => listTileController;
    [SerializeField] private List<TileController> listTileController;

    [SerializeField] private TextMeshProUGUI winText, restartText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] public int xCountt, oCountt;
    


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

    public void checkNumber()
    {
        if (GetTotalXCount() > 3)
    {
        
        var firstX = ListTileController
            .Where(t => t.MyState == TileState.X)
            .OrderBy(t => t.xNumber)
            .FirstOrDefault();
            
        if (firstX != null)
        {
            firstX.ResetTile();
        }
    }

    if (GetTotalOCount() > 3)
    {
       
        var firstO = ListTileController
            .Where(t => t.MyState == TileState.O)
            .OrderBy(t => t.oNumber)
            .FirstOrDefault();
            
        if (firstO != null)
        {
            firstO.ResetTile();
        }
    }
            
    }

    public void WinState(TileState result)
    {
            Debug.Log($"Player {result} wins!");
            winText.text = $"Player {result} wins!";
            restartText.gameObject.SetActive(true);
            restartButton.SetActive(true);
            winText.gameObject.SetActive(true);
    }

    public void Restart()
    {
       foreach(var tile in listTileController)
       {
        tile.ResetTile();
       }
       restartText.gameObject.SetActive(false);
       restartButton.SetActive(false);
       winText.gameObject.SetActive(false);

    }

    public int GetTotalXCount()
    {
        return ListTileController.Count(t => t.MyState == TileState.X);
    }

    public int GetTotalOCount()
    {
        return ListTileController.Count(t => t.MyState == TileState.O);
    }

}   
