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
        List<TileController> emptyCells = new List<TileController>();
       
        

        for (int i = 0; i < listTileController.Count; i++)
        {
            if (listTileController[i].MyState == TileState.Empty)
            {
                emptyCells.Add(listTileController[i]);
            }

            
        }
        if (emptyCells.Count > 0)
            { 
                GameManager.Instance.oCountt++;
                oNumber = GameManager.Instance.oCountt;
                int randomIndex = UnityEngine.Random.Range(0, emptyCells.Count);
                emptyCells[randomIndex].oNumber = oNumber;
                
                
                emptyCells[randomIndex].SetState(state,oNumber);
            }
       
    }
}
