using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
   [SerializeField] private int size => cells.Length;
   [SerializeField] private int height => rows.Length;
   [SerializeField] private int width => size / height;
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
}
