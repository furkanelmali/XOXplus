using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] public int stateChooser;

    public int gameMode;
    public bool isitPlayersTurn;

    [Header("Cached references")]
    [SerializeField] private AIPlayer aiPlayer;
    [SerializeField] private FullPageAd fullPageAd;

    private readonly Queue<TileController> xMoves = new Queue<TileController>(4);
    private readonly Queue<TileController> oMoves = new Queue<TileController>(4);
    public static readonly Direction[] DirectionsForSearch = new[]
    {
        Direction.up, Direction.upright, Direction.right, Direction.downright,
        Direction.down, Direction.downleft, Direction.left, Direction.upleft
    };

    public AIPlayer AIPlayer => aiPlayer;
    


    private void Awake()
    {
        Instance = this;
        gameMode = PlayerPrefs.GetInt("gameMode", 0);

        // Mobilde daha stabil frame pacing.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (!aiPlayer) aiPlayer = FindObjectOfType<AIPlayer>();
        if (!fullPageAd) fullPageAd = FindObjectOfType<FullPageAd>();
        isitPlayersTurn = true;
    }

    public (bool,TileState) HasWinner()
    {  
        foreach(var tile in listTileController)
        {
            if(tile.MyState == TileState.Empty) continue;
            foreach(var direction in DirectionsForSearch)
            {
               var next =  tile.GetNextTile(direction);
               if(!next) continue;

               if(next.MyState != tile.MyState) continue;
               
               var last = next.GetNextTile(direction);
               if(!last) continue;

               if(last.MyState != tile.MyState) continue;

               return (true,tile.MyState);
            }
        }
        return (false,TileState.Empty);

    }

    public int NextNumber(TileState state)
    {
        if (state == TileState.X) return ++xCountt;
        if (state == TileState.O) return ++oCountt;
        return 0;
    }

    public void RegisterMove(TileController tile, TileState state)
    {
        if (state == TileState.X)
        {
            xMoves.Enqueue(tile);
            if (xMoves.Count > 3) xMoves.Dequeue().ResetTile();
            return;
        }

        if (state == TileState.O)
        {
            oMoves.Enqueue(tile);
            if (oMoves.Count > 3) oMoves.Dequeue().ResetTile();
        }
    }

    public void WinState(TileState result)
    {
            Debug.Log($"Player {result} wins!");
            winText.text = $"Player {result} wins!";
            restartText.gameObject.SetActive(true);
            restartButton.SetActive(true);
            winText.gameObject.SetActive(true);
            StartCoroutine(LoadFullPageAd());
            
    }

    public void Restart()
    {
       foreach(var tile in listTileController)
       {
        tile.ResetTile();
       }
       xMoves.Clear();
       oMoves.Clear();
       xCountt = 0;
       oCountt = 0;
       turn = 0;
       isitPlayersTurn = true;
       restartText.gameObject.SetActive(false);
       restartButton.SetActive(false);
       winText.gameObject.SetActive(false);

    }

    private IEnumerator LoadFullPageAd()
    {
        yield return new WaitForSeconds(1f);
        if (!fullPageAd) fullPageAd = FindObjectOfType<FullPageAd>();
        if (fullPageAd) fullPageAd.ShowInterstitialAd();
    }

}   
