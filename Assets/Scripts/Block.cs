using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 弹跳的方块
/// </summary>
public class Block : MonoBehaviour {

    public float fallTime = 0.5f;

    public Transform perfect;

    [Header("Runtime Info")]
    // 方向
    public MyUtil.Orientation orientation;


    public Vector3 spawnPoint;

    public Block next;// 下一个块
    public Block last;// 上一个块

    //public Ray directionRay {
    //    get {
    //        return new Ray(transform.localPosition + Vector3.up * 3, direction);
    //    }
    //}

    public Vector3 direction {
        get {
            return MyUtil.Util.GetOrientationVector(orientation);
        }
    }

    public float distanceNext() {
        return (next.transform.localPosition - transform.localPosition).magnitude-4f;
    }


    public void StartFall(Vector3 target,Vector3 offset,AnimationCurve fallCurve) {
        StartCoroutine(FallCoroutine(target,offset,fallCurve));
    }

    IEnumerator FallCoroutine(Vector3 target,Vector3 offset, AnimationCurve fallCurve) {

        transform.localPosition = target + offset;
        float time = 0;
        while (time <= fallTime) {
            transform.localPosition = target + fallCurve.Evaluate(time / fallTime) * offset;
            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = target;
    }

}
