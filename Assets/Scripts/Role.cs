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


    /// <summary>
    /// 跳跃的时间
    /// </summary>
    public float jumpTime = 1f;


    /// <summary>
    /// 跳跃的曲线控制(抛物线)
    /// </summary>
    public AnimationCurve curve;


    public Vector3 currentToward;

    private bool isJumping;



    public void StartJump(float pressTime = 1f) {

        StartCoroutine(JumpCoroutine(pressTime));
    }

    public IEnumerator JumpCoroutine(float pressTime) {

        Vector3 targetPos = transform.localPosition + pressTime * jumpSpeed * currentToward;



        yield return null;
    }

    private void Update() {
        
    }

}
