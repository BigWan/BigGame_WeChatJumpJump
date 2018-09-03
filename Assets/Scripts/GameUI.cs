using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase {

    public float scoreFadeTime = 0.4f;

    [Header("Components Refs")]
    public Text scoreText;

    public Text scoreHud;

    public Button pauseButton;
    public Button endGameButton;



    public void IniUI() {
        SetScore(0);
        scoreHud.gameObject.SetActive(false);
    }

    private void Awake() {
        cg = GetComponent<CanvasGroup>();
        if (scoreText == null) return;
        
    }

    public void SetScore(float score) {
        scoreText.text = score.ToString();
    }

    public void SetTotalScore(int total) {
        scoreText.text = total.ToString();
    }

    public void ShowScoreAddHud(int count) {
        scoreHud.text = $"+ {count.ToString()}";
    
        StartCoroutine(AddScoreCoroutine());
    }

    IEnumerator AddScoreCoroutine() {
        scoreHud.gameObject.SetActive(true);
        CanvasGroup cg = scoreHud.GetComponent<CanvasGroup>();
        cg.alpha = 1f;
        scoreHud.rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.role.scorePoint.position);
        float time = 0;
        while (time <= scoreFadeTime) { 
            scoreHud.rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.role.scorePoint.position) + Vector3.up*300f*time;
            time += Time.deltaTime;
            cg.alpha = 1f - time/scoreFadeTime;
            yield return null;
        }
        cg.alpha = 0;        
        scoreHud.gameObject.SetActive(false);
    }


    public void PauseGame() {
        GameManager.Instance.PauseGame();
        if (Mathf.Approximately(Time.timeScale, 0)) {
            pauseButton.GetComponent<PauseButton>().SetPlay();
        } else {
            pauseButton.GetComponent<PauseButton>().SetPause();
        }
    }

    public void EndGame() {

    }

}
