using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    void Start()
    {
        Button_NewGame();
    }

    public void Button_NewGame()
    {
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;
        
       board.ClearBoard();
       board.CreateTile();
       board.CreateTile();
       board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOverCanvasGroup.interactable = true;
        StartCoroutine(Fade(gameOverCanvasGroup, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
