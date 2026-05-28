using System;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public void MakeMove(TileState state, List<TileController> listTileController)
    {
        if (state == TileState.Empty) return;

        // 1) Kazandıran hamle
        var winMove = CheckNextWinningMove(listTileController, state);
        if (winMove.found && winMove.tile)
        {
            winMove.tile.Place(state);
            return;
        }

        // 2) Rakibin kazandıran hamlesini engelle
        var opponent = state == TileState.X ? TileState.O : TileState.X;
        var blockMove = CheckNextWinningMove(listTileController, opponent);
        if (blockMove.found && blockMove.tile)
        {
            blockMove.tile.Place(state);
            return;
        }

        // 3) Rastgele boş hücre (boş yoksa null)
        var random = GetRandomEmptyCell(listTileController);
        if (random) random.Place(state);
    }

    public (bool found, TileController tile) CheckNextWinningMove(List<TileController> listTileController, TileState state)
    {
        foreach (var tile in listTileController)
        {
            if (tile.MyState != TileState.Empty) continue;

            var originalState = tile.MyState;
            tile.MyState = state;

            foreach (var direction in GameManager.DirectionsForSearch)
            {
                var next = tile.GetNextTile(direction);
                if (!next) continue;

                if (next.MyState != state) continue;

                var last = next.GetNextTile(direction);
                if (!last) continue;

                if (last.MyState != state) continue;

                tile.MyState = originalState;
                return (true, tile);
            }

            tile.MyState = originalState;
        }

        return (false, null);
    }

    public TileController GetRandomEmptyCell(List<TileController> listTileController)
    {
        var emptyCount = 0;
        for (int i = 0; i < listTileController.Count; i++)
            if (listTileController[i].MyState == TileState.Empty) emptyCount++;

        if (emptyCount == 0) return null;

        var pick = UnityEngine.Random.Range(0, emptyCount);
        for (int i = 0; i < listTileController.Count; i++)
        {
            if (listTileController[i].MyState != TileState.Empty) continue;
            if (pick-- == 0) return listTileController[i];
        }

        return null;
    }
}
