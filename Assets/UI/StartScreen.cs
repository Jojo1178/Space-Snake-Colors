using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button StartGameButton;

    public void StartGameButtonCB()
    {
        GameController.INSTANCE.StartGame();
        this.gameObject.SetActive(false);
    }
}
