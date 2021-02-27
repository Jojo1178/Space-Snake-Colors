using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameScreen : MonoBehaviour
{
    public Button ChangeColorButton;
    public TextMeshProUGUI ScoreText;

    public void ChangeColorCB()
    {
        GameController.INSTANCE.ChangeColor();
    }
}
