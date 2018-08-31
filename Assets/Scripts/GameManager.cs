using System.Collections;
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

    public bool isAutoPlay;

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

    public int totalScore;

    public void GameReLoad() {
        gameUI.gameObject.SetActive(false);
        gameUI.IniUI();
        mainUI.gameObject.SetActive(true);
        mainUI.FadeIn();
        gameOverUI.gameObject.SetActive(false);

        plane.transform.localPosition = Vector3.zero;

        road.RoadReset();
        role.RoleReset();
        cf.transform.localPosition = new Vector3(15,15,-8);
        cf.transform.eulerAngles = new Vector3(35, -45);
        gameStat = GameStat.Loaded;
        totalScore = 0;
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

    void OnChangeBlock(int perfectCount) {
        road.SpawnNextBlock();
        
        plane.localPosition = new Vector3(
            road.currentBlock.transform.localPosition.x,
            -3, 
            road.currentBlock.transform.localPosition.z);
        cf.StartFollow(road.GetFollowPoint());
        int scoreAdd = Mathf.Min( (int)Mathf.Pow(2, perfectCount),256);
        AddScore(scoreAdd);

        SoundManager.Instance.PlayLanded(perfectCount);
    }

    public void AddScore(int scoreAdd) {
        totalScore += scoreAdd;
        gameUI.ShowScoreAddHud(scoreAdd);
        gameUI.SetTotalScore(totalScore);
    }



    public void StartGame(bool autoGame) {
        isAutoPlay = autoGame;
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
        gameOverUI.socre.text = totalScore.ToString();
        gameStat = GameStat.GameOver;
        GameOverAction?.Invoke();
    }


    public  Block standBlock {
        get {
            return role.standBlock;
        }
    }

    /// <summary>
    /// 最佳跳跃距离
    /// </summary>
    /// <returns></returns>
    public Vector3 PerfectJumpVector() {
        return road.currentBlock.perfect.position - role.transform.position;
    }
}
