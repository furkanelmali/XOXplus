using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour,IPointerDownHandler
{
    public TileState MyState{get; set;}
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite xSprite,oSprite;
    [SerializeField] public int xNumber,oNumber;
    [SerializeField] private Color xColor,oColor,emptyColor;

    [SerializeField] private GameObject GameUI;
    private AudioSource audioSource;
    private Vector3 baseScale;
    private Coroutine popRoutine;

    

   




    public Vector2 position;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        baseScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(this.MyState != TileState.Empty) return;
        if(!GameManager.Instance.isitPlayersTurn) return;
        if(!GameUI.activeSelf) return;
        if (audioSource) audioSource.Play();

        var state = StateChooser();
        Place(state);

        GameManager.Instance.turn++;
        var result = GameManager.Instance.HasWinner();
        if(GameManager.Instance.gameMode == 1)
        {
            
            StartCoroutine(AITurnWithDelay());
            
        }

        if(result.Item1)
        {
            GameManager.Instance.isitPlayersTurn = false;
            GameManager.Instance.WinState(result.Item2);
        }
    }

    public void Place(TileState state)
    {
        if (MyState != TileState.Empty) return;

        var currentNumber = GameManager.Instance.NextNumber(state);
        if (state == TileState.X) xNumber = currentNumber;
        else oNumber = currentNumber;

        SetState(state, currentNumber);
        PlayPop();

        GameManager.Instance.RegisterMove(this, state);
    }


    public void SetState(TileState state, int number)
    {
        if(MyState != TileState.Empty) return;
        MyState = state;
        spriteRenderer.color = state == TileState.X ? xColor : oColor;
        spriteRenderer.sprite = state == TileState.X ? xSprite : oSprite;
        
     
    }

    private void PlayPop()
    {
        if (popRoutine != null) StopCoroutine(popRoutine);
        popRoutine = StartCoroutine(PopRoutine());
    }

    private IEnumerator PopRoutine()
    {
        // Basit, allocationsız scale pop. Legacy Animation yerine.
        const float duration = 0.12f;
        var start = baseScale * 0.85f;
        var peak = baseScale * 1.08f;

        transform.localScale = start;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            var a = Mathf.Clamp01(t / duration);
            transform.localScale = Vector3.LerpUnclamped(start, peak, a);
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            var a = Mathf.Clamp01(t / duration);
            transform.localScale = Vector3.LerpUnclamped(peak, baseScale, a);
            yield return null;
        }

        transform.localScale = baseScale;
        popRoutine = null;
    }
    
    private IEnumerator AITurnWithDelay()
    {
        GameManager.Instance.isitPlayersTurn = false;
        yield return new WaitForSeconds(0.5f); // 0.5 saniyelik gecikme
    
        var state = StateChooser();
        var aiPlayer = GameManager.Instance.AIPlayer;
        if (aiPlayer) aiPlayer.MakeMove(state, GameManager.Instance.ListTileController);
    

        GameManager.Instance.turn++;
        var result = GameManager.Instance.HasWinner();
    
        if(result.Item1)
        {
            GameManager.Instance.isitPlayersTurn = false;
            GameManager.Instance.WinState(result.Item2);
        }
        
        if(!result.Item1) GameManager.Instance.isitPlayersTurn = true;
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
    if (popRoutine != null)
    {
        StopCoroutine(popRoutine);
        popRoutine = null;
    }
    transform.localScale = baseScale;
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