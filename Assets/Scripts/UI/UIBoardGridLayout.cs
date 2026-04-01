using UnityEngine;
using UnityEngine.UI;

public class UIBoardGridLayout : GridLayoutGroup
{
    public void OnGameSetup(GameManager game) => SetGridParams(game.settings.boardSize);

    public void SetGridParams(Vector2Int boardSize)
    {
        constraint = Constraint.FixedColumnCount;
        startAxis = Axis.Horizontal;
        startCorner = Corner.UpperLeft;
        
        constraintCount = boardSize.x;

        float _cellSize = (rectTransform.rect.width - spacing.x * (boardSize.x - 1) - (padding.left + padding.right)) / boardSize.x;
        cellSize = Vector2.one * _cellSize;
    }
}
