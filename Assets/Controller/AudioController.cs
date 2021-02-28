using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class AudioController : MonoBehaviour
{

    public static AudioController INSTANCE;
    public AudioSource OrbPickupAudioSource;
    public AudioSource PlayerDeathAudioSource;
    public AudioSource ObstacleDeathAudioSource;
    public AudioSource MainMusicSource;

    private bool playSoundEffects = true;
    public bool PlaySoundEffects
    {
        set
        {
            this.playSoundEffects = value;
            if (value)
            {
                PlayerPrefs.SetInt(this.playSoundEffectsKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(this.playSoundEffectsKey, 0);
            }
        }
        get { return this.playSoundEffects; }
    }


    private bool playMusic = true;
    public bool PlayMusic
    {
        set
        {
            this.playMusic = value;
            if (value)
            {
                this.MainMusicSource.Play();
                PlayerPrefs.SetInt(this.playMusicKey, 1);
            }
            else
            {
                this.MainMusicSource.Stop();
                PlayerPrefs.SetInt(this.playMusicKey, 0);
            }
        }
        get { return this.playMusic; }
    }

    private string playMusicKey = "music";
    private string playSoundEffectsKey = "sound";


    private AudioListener AudioListener;

    private void Awake()
    {
        INSTANCE = this;
        this.AudioListener = this.GetComponent<AudioListener>();
        int playMusic = PlayerPrefs.GetInt(this.playMusicKey, -1);
        int playEffects = PlayerPrefs.GetInt(this.playSoundEffectsKey, -1);
        this.PlayMusic = playMusic != 0;
        this.PlaySoundEffects = playEffects != 0;
    }

    public void PlayOrbPickupSound()
    {
        if (this.playSoundEffects)
        {
            this.OrbPickupAudioSource.Play();
        }
    }

    public void PlayPlayerDeathSound()
    {
        if (this.playSoundEffects && !this.PlayerDeathAudioSource.isPlaying)
        {
            this.PlayerDeathAudioSource.Play();
        }
    }

    public void PlayObstacleDeathSound()
    {
        if (this.playSoundEffects)
        {
            this.ObstacleDeathAudioSource.Play();
        }
    }

}

