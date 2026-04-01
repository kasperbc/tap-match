using System;
using UnityEngine;

public class Matchable
{
    public MatchableType type;
    public Vector2Int position;

    public event Action removed;
    
    
    public Matchable(MatchableType _type)
    {
        type = _type;
    }

    public void Remove()
    {
        removed?.Invoke();
    }
}
