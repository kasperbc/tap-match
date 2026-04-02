using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Connects the "logical" GameBoard to Unity, handles invoking Matchable events.
/// </summary>
public class GameManager : MonoBehaviour
{
    private GameBoard board;
    public GameSettings settings;

    public UnityEvent<Matchable, MatchableSpawnType> matchableSpawned;

    private bool canInteract = true;

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
        if (!canInteract)
            return;
        canInteract = false;
        
        DOTween.Sequence().
            AppendCallback(() => { RemoveConnectedMatchablesTo(matchable); }). // remove connected
            AppendInterval(0.2f).
            AppendCallback(ApplyGravity). // gravity
            AppendInterval(0.5f).
            AppendCallback(() => { FillBoard(MatchableSpawnType.Fall); }). // fill empty tiles
            AppendInterval(0.5f).
            AppendCallback(() => { canInteract = true; });
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
            // this could have been done in GameBoard but figured it was better to keep the events in one place
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