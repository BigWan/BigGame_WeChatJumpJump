using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳一跳的小人
/// </summary>
public class Role : MonoBehaviour {

    public int totalScore = 0;

    /// <summary>
    /// 跳跃速度(按压一秒跳跃的距离)
    /// </summary>
    public float jumpSpeed = 10f;

    public float jumpHeight = 5f;

    /// <summary>
    /// 按压最大压缩比(仅高度)
    /// </summary>
    public float maxPressRatio = 0.6f;

    private float pressRatio = 1f;

    /// <summary>
    /// 跳跃的曲线控制(抛物线)
    /// </summary>
    public AnimationCurve curve;


    public Vector3 currentToward = Vector3.right;

    
    private bool isJumping;


    private Coroutine jumpCoroutine;

    public void StartJump(Vector3  distance) {
        jumpCoroutine = StartCoroutine(JumpCoroutine(distance));
    }


    public IEnumerator JumpCoroutine(Vector3 distance) {
        isJumping = true;
        Vector3 step = distance / 60f;
        for (int i = 0; i < 60; i++) {
            transform.localPosition += step;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                curve.Evaluate(((float)i + 1f) / 60f)*jumpHeight,
                transform.localPosition.z
                );
            yield return null;
        }
        isJumping = false;

    }

    private void OnGUI() {
        if(GUI.Button(new Rect(30, 30, 100, 30), "jump")) {
            StartJump(new Vector3(0,0,5f));
        }
    }


    private float calcRatio(float pressTime) {
        return Mathf.Exp(-pressTime) * (1- maxPressRatio) + maxPressRatio;
    }

    private bool pressing;
    private float pressTime;
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {

            pressing = true;
            pressTime = 0f;

        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            pressing = false;
            if (!isJumping) {
                Debug.Log(pressTime);
                StartJump(Vector3.right * pressTime * jumpSpeed);
            }
        }

        if (pressing) {
            pressTime += Time.deltaTime;
            pressRatio = calcRatio(pressTime);
        } else {
            pressRatio = 1f;
        }
            transform.localScale = new Vector3(1, pressRatio, 1);

        }


    

}
