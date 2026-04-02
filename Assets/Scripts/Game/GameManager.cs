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
        
        FillBoard(MatchableSpawnType.Appear);

        print(board.GetBoardString());
    }

    public void OnMatchableClicked(Matchable matchable)
    {
        RemoveConnectedMatchablesTo(matchable);
        ApplyGravity();
        FillBoard(MatchableSpawnType.Fall);
    }

    private void RemoveConnectedMatchablesTo(Matchable matchable)
    {
        Matchable[] connectedMatchables = board.GetConnectedMatchablesAt(matchable.position.x, matchable.position.y);
        foreach (var connection in connectedMatchables)
        {
            board.RemoveMatchable(connection);
            connection.OnRemoved();
        }
    }

    private void ApplyGravity()
    {
        Matchable[] fallenMatchables = board.ApplyGravity();

        foreach (var matchable in fallenMatchables)
        {
            matchable.OnMoved();
        }
    }

    private void FillBoard(MatchableSpawnType method)
    {
        foreach (var spawnedMatchable in board.FillEmptyTiles(settings.matchableTypes))
        {
            matchableSpawned.Invoke(spawnedMatchable, method);
        }
    }
}