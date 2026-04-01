using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private GameBoard board;
    public GameSettings settings;

    public UnityEvent<Matchable, MatchableSpawnType> matchableSpawned;
    void Start()
    {
        // game setup
        board = new GameBoard();

        board.InitializeBoard(settings.boardSize.x, settings.boardSize.y);
        BroadcastMessage("OnGameSetup", this);
        
        // initial tiles
        foreach (var spawnedMatchable in board.FillEmptyTiles(settings.matchableTypes))
        {
            matchableSpawned.Invoke(spawnedMatchable, MatchableSpawnType.Appear);
        }
        
        print(board.GetBoardString());
    }

    public void OnMatchableClicked(Matchable matchable)
    {
        Matchable[] connectedMatchables = board.GetConnectedMatchablesAt(matchable.position.x, matchable.position.y);

        foreach (var connection in connectedMatchables)
        {
            connection.Remove();
        }
    }
}
