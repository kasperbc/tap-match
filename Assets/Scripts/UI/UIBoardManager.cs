using System.Collections.Generic;
using UnityEngine;

public class UIBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject boardTileBackgroundPrefab;
    [SerializeField] private GameObject boardMatchablePrefab;

    private Dictionary<Vector2Int, RectTransform> tiles = new();
    private GameManager gameManager;
    
    public void OnGameSetup(GameManager game)
    {
        // create background tiles
        for (int y = 0; y < game.settings.boardSize.y; y++)
        {
            for (int x = 0; x < game.settings.boardSize.x; x++)
            {
                GameObject backgroundTile = Instantiate(boardTileBackgroundPrefab, transform);
                tiles.Add(new Vector2Int(x, y), backgroundTile.GetComponent<RectTransform>());
            }
        }

        game.matchableSpawned.AddListener(OnMatchableSpawn);
        gameManager = game;
    }

    public void OnMatchableSpawn(Matchable matchable, MatchableSpawnType spawnType)
    {
        UIMatchable uiMatchable = Instantiate(boardMatchablePrefab, tiles[matchable.position].transform).GetComponent<UIMatchable>();
        uiMatchable.SetMatchable(matchable);
        
        uiMatchable.clicked.AddListener(gameManager.OnMatchableClicked);
    }
}
