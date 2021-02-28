using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : MonoBehaviour
{
    public Button ChangeColorButton;
    public TextMeshProUGUI ScoreText;

    [Header("Tuto")]
    public GameObject InstructionGameobject;
    public TextMeshProUGUI InstructionText;
    public Image SwitchColorPanel;
    public float TypingSpeedSec = 0.125f;

    private void OnEnable()
    {
        this.SetTutoScreen(false);
    }

    public void ChangeColorCB()
    {
        GameController.INSTANCE.ChangeColor();
    }

    public IEnumerator TypeInstruction(string instruction, float waitForSeconds= 0)
    {
        StringBuilder stringBuilder = new StringBuilder();
        bool richTextFound = false;
        foreach(char c in instruction)
        {
            if(c == '<')
            {
                richTextFound = true;
            }
            stringBuilder.Append(c);
            if (!richTextFound)
            {
                yield return new WaitForSeconds(this.TypingSpeedSec);
                this.InstructionText.text = stringBuilder.ToString();
            }
            if (c == '>')
            {
                richTextFound = false;
                this.InstructionText.text = stringBuilder.ToString();
            }
        }
        this.InstructionText.text = instruction;
        if (waitForSeconds > 0)
        {
            yield return new WaitForSeconds(waitForSeconds);
        }
    }

    public void SetTutoScreen(bool value)
    {
            this.InstructionGameobject.SetActive(value);
            this.SwitchColorPanel.gameObject.SetActive(value);
            this.SwitchColorPanel.color = Color.clear;
    }
}