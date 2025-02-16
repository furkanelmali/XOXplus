using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class AIPlayer : MonoBehaviour
{
     int oNumber;
    public void MakeMove(TileState state, List<TileController> listTileController)
    {

        var winMove =CheckNextWinningMove(listTileController,state);
        if(winMove.Item1)
        {
            GameManager.Instance.oCountt++;
            oNumber = GameManager.Instance.oCountt;

            winMove.Item2.SetState(state,oNumber);
            winMove.Item2.oNumber = oNumber;
            return;
        }
        var blockMove =CheckNextWinningMove(listTileController,TileState.X);
        if(blockMove.Item1)
        {
            GameManager.Instance.oCountt++;
            oNumber = GameManager.Instance.oCountt;

            blockMove.Item2.SetState(state,oNumber);
            blockMove.Item2.oNumber = oNumber;
            return;
        }

        
        if(!blockMove.Item1)
        {
             GameManager.Instance.oCountt++;
             oNumber = GameManager.Instance.oCountt;

            blockMove.Item2.SetState(state,oNumber);
            blockMove.Item2.oNumber = oNumber;
            return;
        }
       

    }

    

    public (bool,TileController) CheckNextWinningMove(List<TileController> listTileController, TileState state)
    {
        foreach (var tile in listTileController)
        {
            if (tile.MyState != TileState.Empty) continue;
            
            TileState originalState = tile.MyState;
            tile.MyState = state;
            
            foreach (var direction in Enum.GetValues(typeof(Direction)))
            {
                var next = tile.GetNextTile((Direction)direction);
                if (!next) continue;
                
                if (next.MyState != state) continue;
                
                var last = next.GetNextTile((Direction)direction);
                if (!last) continue;
                
                if (last.MyState != state) continue;
                
                tile.MyState = originalState;
                return (true,tile);
            }
            
            tile.MyState = originalState;
        }
        
        return (false,GetEmptyCells(listTileController,state));
    }

    public TileController GetEmptyCells(List<TileController> listTileController, TileState state)
    {
        List<TileController> emptyCells = new List<TileController>();
       
        

        for (int i = 0; i < listTileController.Count; i++)
        {
            if (listTileController[i].MyState == TileState.Empty)
            {
                emptyCells.Add(listTileController[i]);
            }

            
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyCells.Count);
        return emptyCells[randomIndex];
    }
}
