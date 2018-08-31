using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音乐盒
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimationBlock : Block {
    
    [Header("AnimationBlockConfig")]

    public float triggerTime;

    public int scoreExtra;

    private Animator animator;



    private float stayTime;


    private void Awake() {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other) {
        stayTime = 0;
    }

    private void OnTriggerStay(Collider other) {
        stayTime += Time.deltaTime;
        if (stayTime > triggerTime) {
            PlayAnimation();
            //GameManager.Instance.
        }
    }


    private void OnTriggerExit(Collider other) {
    }


    void PlayAnimation() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Normal")) {
            animator.Play("AnimationBlock");
            GameManager.Instance.AddScore(scoreExtra);
        }
       
    }

}
