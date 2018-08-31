using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : UnitySingleton<SoundManager> {


    private AudioSource audioPlayer;


    [Header("游戏开始结束音效")]
    public AudioClip appStart;
    public AudioClip gameStart;


    [Header("落在方块上的音效")]

    public AudioClip[] comboSounds;

    public AudioClip[] failSounds;
    public AudioClip perfectSound;

    [Header("按压音效")]
    public AudioClip pressSound;
    public AudioClip pressLoop;


    public AudioClip blockFalldown;

    private void Awake() {

        if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);


        audioPlayer = GetComponent<AudioSource>();
        GameManager.Instance.GameLoadedAction += OnGameLoaded;
        GameManager.Instance.GameStartAction += OnGameStart;
        GameManager.Instance.GameOverAction += OnGameOver;
    }



    public void OnGameLoaded() {
        audioPlayer.clip = appStart;
        audioPlayer.Play();
    }

    public void OnGameStart() {
        audioPlayer.clip = gameStart;
        audioPlayer.Play();
    }

    public void OnGameOver() {
        Instance.PlayFail();
    }

    public void PlayBlockFallDown() {
        audioPlayer.clip = blockFalldown;
        audioPlayer.loop = false;
        audioPlayer.Play();
    }


    public void PlayLanded(int perfectCount) {
        int clipIndex = 0;
        if (perfectCount > comboSounds.Length)
            clipIndex = comboSounds.Length - 1;
        else
            clipIndex = perfectCount;
        audioPlayer.clip = comboSounds[clipIndex];
        audioPlayer.Play();
    }


    public void PlaySnapSound() {
        audioPlayer.clip = pressSound;
        audioPlayer.Play();
    }

    public void PlaySnapLoop() {
        if (audioPlayer.clip != pressLoop) {
            audioPlayer.clip = pressLoop;
            audioPlayer.Play();
            audioPlayer.loop = true;
        }
    }

    public void Stop() {
        audioPlayer.Stop();
    }


    public void PlayFail() {
        audioPlayer.clip = failSounds[0];
        audioPlayer.Play();
    }
}


