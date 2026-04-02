using System;
using UnityEngine;

public class Matchable
{
    public MatchableType type;
    public Vector2Int position;

    public event EventHandler Removed;
    public event EventHandler<Vector2Int> Moved;
    
    public Matchable(MatchableType _type)
    {
        type = _type;
    }

    public void OnRemoved()
    {
        Removed?.Invoke(this, null);
    }

    public void OnMoved()
    {
        Moved?.Invoke(this, position);
    }
}
