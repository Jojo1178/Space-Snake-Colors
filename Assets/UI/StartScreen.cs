using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreen : MonoBehaviour
{
    public Button StartGameButton;
    public TextMeshProUGUI ScoreText;

    public Button PlayMusicButton;
    public Image PlayMusicImage;
    public Button PlaySoundEffectButton;
    public Image PlaySoundEffectImage;

    public Sprite MusicOn;
    public Sprite MusicOff;
    public Sprite SoundEffectsOn;
    public Sprite SoundEffectsOff;

    public void StartGameButtonCB()
    {
        GameController.INSTANCE.StartGame();
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        int hiScore = PlayerPrefs.GetInt(GameController.HIGHSCOREKEY, 0);
        if (hiScore > 0)
        {
            this.ScoreText.gameObject.SetActive(true);
            this.ScoreText.text = $"Best <color=yellow>{hiScore}</color>";
        }
        else
        {
            this.ScoreText.gameObject.SetActive(false);
        }

        this.UpdateSoundButtonImages();
    }

    private void UpdateSoundButtonImages()
    {
        this.PlayMusicImage.sprite = AudioController.INSTANCE.PlayMusic ? this.MusicOn : this.MusicOff;
        this.PlaySoundEffectImage.sprite = AudioController.INSTANCE.PlaySoundEffects ? this.SoundEffectsOn : this.SoundEffectsOff;
    }

    public void PlayMusicButtonCB()
    {
        AudioController.INSTANCE.PlayMusic = !AudioController.INSTANCE.PlayMusic;
        this.UpdateSoundButtonImages();
    }

    public void PlaySoundEffectButtonCB()
    {
        AudioController.INSTANCE.PlaySoundEffects = !AudioController.INSTANCE.PlaySoundEffects;
        this.UpdateSoundButtonImages();
    }
}