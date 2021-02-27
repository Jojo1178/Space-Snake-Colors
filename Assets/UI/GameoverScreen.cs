using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    public Button RetryButton;
    public Button MainMenuButton;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI HighScoreText;

    public StartScreen StartScreen;

    public void RetryButtonCB()
    {
        GameController.INSTANCE.StartGame();
        this.gameObject.SetActive(false);
    }

    public void MainMenuButtonCB()
    {
        GameController.INSTANCE.ResetWorld();
        this.StartScreen.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SetScore(int score, int highScore)
    {
        this.FinalScoreText.text = score.ToString("00");
        this.HighScoreText.text = $"Best {highScore.ToString("00")}";
    }
}
