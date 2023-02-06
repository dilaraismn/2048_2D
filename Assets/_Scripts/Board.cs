using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
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

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform); //grid as parent
        tile.SetState(tileStates[0], 2);
        
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        
        tiles.Clear();
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
                if (CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
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

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.isLocked;
    }

    private void Merge(Tile a, Tile b)
    {
        //destroy one adjust second
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) +1, 0, tileStates.Length -1);
        int number = b.number * 2;
        
        b.SetState(tileStates[index], number);

    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }
    
    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(.1f);
        waiting = false;

        foreach (var tile in tiles)
        {
            tile.isLocked = false;
        }

        if (tiles.Count != grid.size)
        {
            CreateTile();
        }

        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (tiles.Count != grid.size)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            Cell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            Cell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            Cell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);
            Cell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);

            if (up != null && CanMerge(tile,up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile,down.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile,right.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile,left.tile))
            {
                return false;
            }
        }

        return true;
    }
}
