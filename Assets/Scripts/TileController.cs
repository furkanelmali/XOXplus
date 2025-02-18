using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TileController : MonoBehaviour,IPointerDownHandler
{
    public TileState MyState{get; set;}
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite xSprite,oSprite;
    [SerializeField] public int xNumber,oNumber;
    [SerializeField] private Color xColor,oColor,emptyColor;

    [SerializeField] private GameObject GameUI;
    public Animation animation;
    private AudioSource audioSource;

   




    public Vector2 position;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnPointerDown(PointerEventData eventData)

    {
        if(!GameUI.activeSelf) return;
        Debug.Log("Tile clicked");
        audioSource.Play();
        var state = StateChooser();
        int currentNumber;
        if(state == TileState.X)
        {
            GameManager.Instance.xCountt++;
            xNumber = GameManager.Instance.xCountt;
            currentNumber = xNumber;

        }
        else
        {
            GameManager.Instance.oCountt++;
            oNumber = GameManager.Instance.oCountt;
            currentNumber = oNumber;
        }
        SetState(state, currentNumber); 
        animation.Play();

        GameManager.Instance.turn++;
        GameManager.Instance.checkNumber();
        var result = GameManager.Instance.HasWinner();
        if(GameManager.Instance.gameMode == 1)
        {
            StartCoroutine(AITurnWithDelay());
        }

        if(result.Item1)
        {
            GameManager.Instance.WinState(result.Item2);
        }
    }


    public void SetState(TileState state, int number)
    {
        if(MyState != TileState.Empty) return;
        MyState = state;
        spriteRenderer.color = state == TileState.X ? xColor : oColor;
        spriteRenderer.sprite = state == TileState.X ? xSprite : oSprite;
        
     
    }
    

    private IEnumerator AITurnWithDelay()
{
    yield return new WaitForSeconds(0.5f); // 0.5 saniyelik gecikme
    
    var state = StateChooser();
    AIPlayer aiPlayer = FindObjectOfType<AIPlayer>();
    aiPlayer.MakeMove(state, GameManager.Instance.ListTileController);
    

    GameManager.Instance.turn++;
    GameManager.Instance.checkNumber();
    var result = GameManager.Instance.HasWinner();
    
    if(result.Item1)
    {
        GameManager.Instance.WinState(result.Item2);
    }
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

public void ResetTile()
{
    MyState = TileState.Empty;
    spriteRenderer.color = emptyColor;
    spriteRenderer.sprite = null;
    xNumber = 0;
    oNumber = 0;
}


TileState StateChooser()
{
    if(GameManager.Instance.stateChooser == 0)
    {
        var state = GameManager.Instance.turn%2 == 0 ? TileState.X : TileState.O;
        return state;
    }
    else
    {
        var state = GameManager.Instance.turn%2 == 0 ? TileState.O : TileState.X;
        return state;
    }
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