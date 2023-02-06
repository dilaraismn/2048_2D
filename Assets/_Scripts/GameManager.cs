using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    void Start()
    {
        Button_NewGame();
    }

    public void Button_NewGame()
    {
       board.ClearBoard();
       board.CreateTile();
       board.CreateTile();
       board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        print("game over");
    }
}
