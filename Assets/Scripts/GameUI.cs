using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase {

    public Text scoreText;

    public Text scoreHud;

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
        scoreHud.rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.standBlock.spawnPoint);
        for (int i = 0; i < 30; i++) {
            scoreHud.rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.standBlock.spawnPoint) + Vector3.up*5f*i;
            yield return null;
        }
        scoreHud.gameObject.SetActive(false);
    }

}
