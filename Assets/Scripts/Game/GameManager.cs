using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoard Board;
    public GameSettings settings;
    
    void Start()
    {
        Board = new GameBoard();
        
        Board.InitializeBoard(settings.boardSize.x, settings.boardSize.y);
        Board.FillEmptyTiles(settings.matchableTypes);

        Debug.Log(Board.GetBoardString());
    }
}
