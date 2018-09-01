using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

    public Sprite pausedImage;
    public Sprite playImage;

    private Image image;
    private void Awake() {
        image = GetComponent<Image>();
        SetPause();
    }

    public void SetPause() {
        image.sprite = pausedImage;
    }
    public void SetPlay() {
        image.sprite = playImage;
    }
}
