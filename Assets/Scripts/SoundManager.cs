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


    }

    public void PlayCombine(int combineID) {

    }

    public void PlayPerfect() {
        audioPlayer.clip = perfectSound;
        audioPlayer.Play();
    }




}


