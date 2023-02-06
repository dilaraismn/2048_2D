using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileState[] tileStates;
    private List<Tile> tiles;
    private Grid grid;
    private bool waiting;

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

    private void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTileCheck(Vector2Int.up, 0,1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTileCheck(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTileCheck(Vector2Int.left, 1,1,0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTileCheck(Vector2Int.right, grid.width - 2, -1, 0,1);
            }
        }
    }

    private void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform); //grid as parent
        tile.SetState(tileStates[0], 2);
        
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void MoveTileCheck(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                Cell cell = grid.GetCell(x, y);
                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newCell = null;
        Cell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveToCell(newCell);
            return true;
        }

        return false;
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(.1f);
        waiting = false;
        
        //create new tile
        //check for game over
    }
}
