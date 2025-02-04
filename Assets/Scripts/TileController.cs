using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour,IPointerDownHandler
{
    public TileState MyState{get; set;}
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color xColor,oColor;

    public Vector2 position;


    public void OnPointerDown(PointerEventData eventData)

    {
        Debug.Log("Tile clicked");
        var state = GameManager.Instance.turn%2 == 0 ? TileState.X : TileState.O;
        SetState(state);    
        GameManager.Instance.turn++;
        var result = GameManager.Instance.HasWinner();
        if(result.Item1)
        {
            Debug.Log($"Player {result.Item2} wins!");
        }
    }




    public void SetState(TileState state)
    {
        if(MyState != TileState.Empty) return;
        MyState = state;
        spriteRenderer.color = state == TileState.X ? xColor : oColor;
    }

    public TileController GetNextTile(Direction dir)
    {
        var nextTileCoordinate = position;
        switch(dir)
        {
            case Direction.up:
                nextTileCoordinate.y += 1;
                break;
            case Direction.upright:
                nextTileCoordinate.x += 1;
                nextTileCoordinate.y += 1;
                break;
            case Direction.right:
                nextTileCoordinate.x += 1;
                break;
            case Direction.downright:
                nextTileCoordinate.x += 1;
                nextTileCoordinate.y -= 1;
                break;
            case Direction.down:
                nextTileCoordinate.y -= 1;
                break;
            case Direction.downleft:
                nextTileCoordinate.x -= 1;
                nextTileCoordinate.y -= 1;
                break;
            case Direction.left:
                nextTileCoordinate.x -= 1;
                break;
            case Direction.upleft:
                nextTileCoordinate.x -= 1;
                nextTileCoordinate.y += 1;
                break; 
        }
        return GameManager.Instance.ListTileController.Find(t => t.position == nextTileCoordinate);
    }


}


public enum TileState

    {
        Empty,
        X,
        O
    }

public enum Direction
{
    up,
    upright,
    right,
    downright,
    down,
    downleft,
    left,
    upleft
}