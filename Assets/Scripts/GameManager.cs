﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager> {

    public enum GameStat {
        Loaded,
        Playing,
        GameOver,
    }

    public GameStat gameStat;

    public Road road;
    public Role role;


    public Transform plane;

    [Space(30)]
    [Header("UI")]
    public GameUI gameUI;
    public MainUI mainUI;
    public GameOverUI gameOverUI;


    [Header("Camera")]
    public CameraFollow cf;



    [Header("事件")]
    public UnityAction GameLoadedAction;
    public UnityAction GameStartAction;
    public UnityAction GameOverAction;



    public void GameReLoad() {
        gameUI.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);
        mainUI.FadeIn();
        gameOverUI.gameObject.SetActive(false);

        plane.transform.localPosition = Vector3.zero;

        road.RoadReset();
        role.RoleReset();
        cf.transform.localPosition = new Vector3(15,15,-8);
        cf.transform.eulerAngles = new Vector3(35, -45);
        gameStat = GameStat.Loaded;
    }


    private void Awake() {
        DontDestroyOnLoad(this);
        role.ChangeBlockAction += OnChangeBlock;
    }


    private void Start() {
        GameReLoad();
        GameLoadedAction?.Invoke();
        gameStat = GameStat.Loaded;
        plane.localPosition = Vector3.zero;
    }

    void OnChangeBlock(bool isPerfect) {
        road.SpawnNextBlock();
        
        plane.localPosition = new Vector3(
            road.currentBlock.transform.localPosition.x,
            -3, 
            road.currentBlock.transform.localPosition.z);
        cf.StartFollow(road.GetFollowPoint());
        if (isPerfect) {
            gameUI.AddScore(2);
            SoundManager.Instance.PlayPerfect();
        } else
            gameUI.AddScore(1);
    }



    public void StartGame() {
        if (gameStat != GameStat.Loaded) return;
        mainUI.FadeOut();
        gameUI.FadeIn();
        road.SpawnFirstTwo();
        plane.localPosition = new Vector3(
            road.currentBlock.transform.localPosition.x,
            -3,
            road.currentBlock.transform.localPosition.z);
        role.StartEnter();
        gameStat = GameStat.Playing;
        GameStartAction?.Invoke();
        //plane.transform.localPosition = Vector3.zero;
    }

    public void CheckGameOver() {
        if(gameStat == GameStat.Playing) {
            GameOver();
        }
    }


    public void GameOver() {
        gameUI.FadeOut();
        gameOverUI.gameObject.SetActive(true);
        gameStat = GameStat.GameOver;
        GameOverAction?.Invoke();
    }


    public  Block standBlock {
        get {
            return role.standBlock;
        }
    }
}
