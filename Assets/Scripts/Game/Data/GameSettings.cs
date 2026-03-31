using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Tap Match/Game Settings")]
public class GameSettings : ScriptableObject
{
    public Vector2Int boardSize;
    public MatchableType[] matchableTypes;
}
