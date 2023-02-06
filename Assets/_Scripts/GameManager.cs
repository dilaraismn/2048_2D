using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private TextMeshProUGUI scoreText, highScoreText;
    private int score; 
    void Start()
    {
        Button_NewGame();
    }

    public void Button_NewGame()
    {
        SetScore(0);
        highScoreText.text = LoadHigScore().ToString();
        
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

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }
    
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        int highScore = LoadHigScore();
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    private int LoadHigScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }
}
