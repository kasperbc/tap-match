using UnityEngine.UI;

public class UIBoardAspectRatioFitter : AspectRatioFitter
{
    public void OnGameSetup(GameManager game) => 
        aspectRatio = (float)game.settings.boardSize.x / game.settings.boardSize.y;
}
