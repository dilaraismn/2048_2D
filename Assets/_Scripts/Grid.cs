using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
   public int size => cells.Length;
   public int height => rows.Length;
   public int width => size / height;
   public Row[] rows { get; private set; }
   public Cell[] cells { get; private set; }

   private void Awake()
   {
      rows = GetComponentsInChildren<Row>();
      cells = GetComponentsInChildren<Cell>();
   }

   private void Start()
   {
      for (int y = 0; y < rows.Length; y++)
      {
         for (int x = 0; x < rows[y].cells.Length; x++)
         {
            rows[y].cells[x].coordinates = new Vector2Int(x,y);
         }
      }
   }

   public Cell GetRandomEmptyCell()
   {
      int index = Random.Range(0, cells.Length);
      int startingIndex = index;
      
      while (cells[index].occupied)
      {
         index++;

         if (index >= cells.Length)
         {
            index = 0;
         }

         if (index == startingIndex)
         {
            return null;
         }
      }
      return cells[index];
   }

   public Cell GetCell(int x, int y)
   {
      if (x >= 0 && x < width && y >= 0 && y < height)
      {
         return rows[y].cells[x];
      }
      else
      {
         return null;
      }
   }

   public Cell GetCell(Vector2Int coordinates)
   {
      return GetCell(coordinates.x, coordinates.y);
   }
   public Cell GetAdjacentCell(Cell cell, Vector2Int direction)
   {
      Vector2Int coordinates = cell.coordinates;
      coordinates.x += direction.x;
      coordinates.y -= direction.y;

      return GetCell(coordinates);
   }
   
}
