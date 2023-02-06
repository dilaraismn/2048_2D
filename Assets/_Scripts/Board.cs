using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileState[] tileStates;
    private Grid grid;
    private List<Tile> tiles;

    private void Awake()
    {
        grid = GetComponentInChildren<Grid>();
        tiles = new List<Tile>(16); //capacity specification for optimization
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform); //grid as parent
        tile.SetState(tileStates[0], 2);
        
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
}
