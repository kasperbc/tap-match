using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles logic for the game board.
/// </summary>
public class GameBoard
{
    private readonly Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down
    };

    private Matchable[][] _gameBoard;
    public Vector2Int BoardSize
    {
        get
        {
            if (_gameBoard == null)
                return new Vector2Int(0, 0);
            return new Vector2Int(_gameBoard[0].Length, _gameBoard.Length);
        }
    }

    /// <summary>
    /// Creates a new game board with a specified size.
    /// Must be done before manipulating the board.
    /// Note: Clears any matchables on the board.
    /// </summary>
    public void InitializeBoard(int sizeX, int sizeY)
    {
        _gameBoard = new Matchable[sizeY][];
        for (var i = 0; i < sizeY; i++)
        {
            _gameBoard[i] = new Matchable[sizeX];
        }
    }
    
    #region Basic Functions
    
    /// <returns>The matchable at the specified tile, null if the tile is empty.</returns>
    public Matchable GetMatchableAtPosition(int x, int y)
    {
        if (!IsValidPosition(x, y))
        {
            Debug.LogError("Matchable position is out of bounds!");
            return null;
        }
        
        return _gameBoard[y][x];
    }

    public void SetMatchableAtPosition(int x, int y, Matchable matchable)
    {
        if (!IsValidPosition(x, y))
        {
            Debug.LogError("Matchable position is out of bounds!");
            return;
        }
        
        _gameBoard[y][x] = matchable;
        if (matchable != null)
            matchable.position = new(x, y); // do this here to keep the positions synchronized
    }

    public void RemoveMatchableAtPosition(int x, int y) => SetMatchableAtPosition(x, y, null);

    public void RemoveMatchable(Matchable matchable) =>
        RemoveMatchableAtPosition(matchable.position.x, matchable.position.y);

    public void MoveMatchableToPosition(int oldX, int oldY, int newX, int newY)
    {
        Matchable matchable = GetMatchableAtPosition(oldX, oldY);
        
        RemoveMatchableAtPosition(oldX, oldY);
        SetMatchableAtPosition(newX, newY, matchable);
    }
    
    /// <returns>If the position is within the game board.</returns>
    public bool IsValidPosition(int x, int y)
    {
        return x < BoardSize.x 
               && y < BoardSize.y
               && Mathf.Min(x, y) >= 0; // position cannot be negative
    }
    
    /// <returns>If the position does not contain a matchable, or is invalid</returns>
    public bool TileIsEmptyOrInvalid(int x, int y)
    {
        if (!IsValidPosition(x, y))
            return true;
        
        return GetMatchableAtPosition(x, y) == null;
    }

    // for debugging purposes
    public string GetBoardString()
    {
        string result = string.Empty;
        for (int y = 0; y < BoardSize.y; y++)
        {
            for (int x = 0; x < BoardSize.x; x++)
            {
                if (TileIsEmptyOrInvalid(x, y))
                {
                    result += "□";
                    continue;
                }
                
                Matchable matchable = GetMatchableAtPosition(x, y);
                result += $"<color=#{ColorUtility.ToHtmlStringRGB(matchable.type.color)}>■</color>";
            }

            result += "\n";
        }

        return result;
    }
    
    #endregion
    
    #region Sophisticated Functions

    /// <param name="pool">Matchables that can be generated</param>
    /// <returns>Newly generated matchables</returns>
    public Matchable[] FillEmptyTiles(MatchableType[] pool)
    {
        List<Matchable> newMatchables = new List<Matchable>();

        for (int y = 0; y < BoardSize.y; y++)
        {
            for (int x = 0; x < BoardSize.x; x++)
            {
                if (!TileIsEmptyOrInvalid(x, y))
                    continue;

                MatchableType randomType = pool[Mathf.FloorToInt(Random.value * pool.Length)];
                Matchable newMatchable = new Matchable(randomType);
                
                SetMatchableAtPosition(x, y, newMatchable);
                newMatchables.Add(newMatchable);
            }
        }
        
        return newMatchables.ToArray();
    }
    
    /// <summary>
    /// Recursively searches the adjacent tiles (from the coordinates provided) for matchables of the same type.
    /// </summary>
    /// <returns>Positions of connected matchables in grid, (0,0) being top left</returns>
    public Vector2Int[] GetConnectedMatchablePositionsAt(int x, int y)
    {
        if (TileIsEmptyOrInvalid(x, y))
        {
            Debug.LogError("Tile position is invalid!");
            return null;
        }
        
        List<Vector2Int> connectedPositions = new List<Vector2Int> { new(x,y) };
        
        int iterations = 0;
        while(true)
        {
            List<Vector2Int> newConnections = new();
            
            // iterate over each existing connection and search for any new ones
            foreach (var position in connectedPositions)
            {
                Matchable currentMatchable = GetMatchableAtPosition(position.x, position.y);
                // iterate every direction
                foreach (var direction in directions)
                {
                    Vector2Int adjacentPosition = position + direction;
                    
                    // skip if invalid
                    if (TileIsEmptyOrInvalid(adjacentPosition.x, adjacentPosition.y))
                        continue;
                    
                    // skip if not a new connection
                    bool alreadySearched = false;
                    foreach (var _position in connectedPositions)
                    {
                        if (adjacentPosition == _position)
                        {
                            alreadySearched = true;
                            break;
                        }
                    }
                    if (alreadySearched)
                        continue;


                    
                    // does it connect
                    Matchable adjacentMatchable = GetMatchableAtPosition(adjacentPosition.x, adjacentPosition.y);
                    
                    if (adjacentMatchable.type.Equals(currentMatchable.type))
                        newConnections.Add(adjacentPosition);
                }

                iterations += 1;
            }
            
            // if new connections were not found, we are done
            if (newConnections.Count <= 0)
                break;
            
            foreach (var newConnection in newConnections)
                connectedPositions.Add(newConnection);
            
            if (iterations > Mathf.Pow(BoardSize.x * BoardSize.y, 2)) // the code has probably gotten stuck
            {
                Debug.LogWarning($"Something has gone wrong in searching connected tiles at {x},{y}");
                break;
            }
        }

        return connectedPositions.ToArray();
    }
    
    public Matchable[] GetConnectedMatchablesAt(int x, int y)
    {
        List<Matchable> result = new();
        foreach (var position in GetConnectedMatchablePositionsAt(x, y))
        {
            result.Add(GetMatchableAtPosition(position.x, position.y));
        }

        return result.ToArray();
    }
    
    /// <summary>
    /// All tiles above empty tile(s) on the provided X position are moved down.
    /// </summary>
    /// <returns>Effected matchables</returns>
    public Matchable[] ApplyGravityAtColumn(int x)
    {
        if (!IsValidPosition(x, 0))
        {
            Debug.LogError("Board column is out of bounds!");
            return Array.Empty<Matchable>();
        }

        int gravity = 0;
        List<Matchable> effectedMatchables = new();
        
        for (int y = BoardSize.y - 1; y >= 0; y--)
        {
            if (TileIsEmptyOrInvalid(x, y))
            {
                gravity += 1;
            }
            else if (gravity > 0)
            {
                effectedMatchables.Add(GetMatchableAtPosition(x,y));
                MoveMatchableToPosition(x,y, x,y + gravity);
            }
        }
        
        return effectedMatchables.ToArray();
    }

    /// <summary>
    /// All tiles above empty tile(s) are moved down.
    /// </summary>
    /// <returns>Effected matchables</returns>
    public Matchable[] ApplyGravity()
    {
        List<Matchable> effectedMatchables = new();
        for (int x = 0; x < BoardSize.x; x++)
        {
            effectedMatchables.AddRange(ApplyGravityAtColumn(x));
        }

        return effectedMatchables.ToArray();
    }

    #endregion
}
