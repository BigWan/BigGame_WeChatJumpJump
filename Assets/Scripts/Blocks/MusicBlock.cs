using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音乐盒
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicBlock : Block {
    
    [Header("MusicBlockConfig")]
    public AudioClip music;

    public float playMusicNeedTime;

    public int scoreExtra;

    private AudioSource audioPlayer;



    private float stayTime;


    private void Awake() {
        audioPlayer = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other) {
        stayTime = 0;
    }

    private void OnTriggerStay(Collider other) {
        stayTime += Time.deltaTime;
        if (stayTime > playMusicNeedTime) {
            PlayMusic();
            //GameManager.Instance.
        }
    }


    private void OnTriggerExit(Collider other) {
        audioPlayer.Stop();
    }


    void PlayMusic() {
        audioPlayer.clip = music;
        if (!audioPlayer.isPlaying) {
            audioPlayer.Play();
            GameManager.Instance.AddScore(scoreExtra);
        }
       
    }

}
