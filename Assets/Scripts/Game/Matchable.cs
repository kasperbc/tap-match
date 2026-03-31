using UnityEngine;

public class Matchable
{
    public MatchableType type;
    public Vector2Int position;

    public Matchable(MatchableType _type)
    {
        type = _type;
    }
}
