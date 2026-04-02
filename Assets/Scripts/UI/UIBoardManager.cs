using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages UIMatchables and keeping the player visuals in check with the game board, mainly by using events from Matchables.
/// </summary>
public class UIBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject boardTileBackgroundPrefab;
    [SerializeField] private GameObject boardMatchablePrefab;

    private Dictionary<Vector2Int, RectTransform> tiles = new();
    private GameManager gameManager;

    private Dictionary<Matchable, UIMatchable> uiMatchables = new();
    
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

    private void OnMatchableSpawn(Matchable matchable, MatchableSpawnType spawnType)
    {
        UIMatchable uiMatchable = Instantiate(boardMatchablePrefab, tiles[matchable.position].transform).GetComponent<UIMatchable>();
        uiMatchable.SetMatchable(matchable);

        if (spawnType == MatchableSpawnType.Fall)
        {
            var uiImg = uiMatchable.GetComponent<Image>();

            var uiColor = matchable.type.color;
            uiColor.a = 0;
            uiImg.color = uiColor;
            uiImg.DOFade(matchable.type.color.a, 0.15f);

            uiMatchable.GetComponent<RectTransform>().localPosition += Vector3.up * 400;
                
            uiMatchable.BounceToParentTile();
        }
        
        // subscribe to events
        uiMatchable.clicked.AddListener(gameManager.OnMatchableClicked);
        matchable.Removed += OnMatchableRemoved;
        matchable.Moved += OnMatchableMoved;
        
        uiMatchables.Add(matchable, uiMatchable);
    }

    private void OnMatchableRemoved(object sender, EventArgs _)
    {
        if (!(sender is Matchable matchable && uiMatchables.ContainsKey(matchable))) // matchable exists
            return;
        
        uiMatchables[matchable].OnRemoved();
        uiMatchables.Remove(matchable);
    }

    private void OnMatchableMoved(object sender, Vector2Int newPosition)
    {
        if (!(sender is Matchable matchable && uiMatchables.ContainsKey(matchable))) // matchable exists
            return;

        if (!tiles.ContainsKey(newPosition))
            return;
        
        uiMatchables[matchable].OnMoved(tiles[newPosition]);
    }
}
