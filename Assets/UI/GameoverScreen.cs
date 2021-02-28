using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    public Button RetryButton;
    public Button MainMenuButton;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI HighScoreText;

    public string[] colorCode = new string[] {
        "red",
        "orange",
        "yellow",
        "green",
        "blue",
        "purple"
    };

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
        int randomStart = Random.Range(0, this.colorCode.Length);
        this.HighScoreText.text = $"Best {AddColorToScoreString(highScore, -1, randomStart)}";
    }

    private string AddColorToScoreString(int score, int colorStep = 1, int colorStart = 0)
    {
        string scoreStr = score.ToString("00");
        StringBuilder stringBuilder = new StringBuilder();
        int idx = colorStart;
        foreach (char c in scoreStr)
        {
            if (idx >= this.colorCode.Length)
            {
                idx = 0;
            }
            else if (idx < 0)
            {
                idx = this.colorCode.Length - 1;
            }
            stringBuilder.Append($"<color=\"{this.colorCode[idx]}\">{c}</color>");
            idx += colorStep;
        }
        return stringBuilder.ToString();
    }
}