using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机跟随
/// </summary>
public class CameraFollow : MonoBehaviour {

    private static Vector3 cameraOffset = new Vector3(15f, 15f, -15f);
    private static Vector3 cameraEuler = new Vector3(35, -45, 0);

    public Vector3 target;

    private IEnumerator FollowAt(Vector3 target) {
        Vector3 targetPos = target + cameraOffset;
        while (!Mathf.Approximately(Vector3.Distance(targetPos, transform.localPosition), 0)) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, 0.5f);
            yield return null;
        }

        transform.eulerAngles = cameraEuler;
    }


    public void StartFollow(Vector3 target) {
        this.target = target;
        StartCoroutine(FollowAt(target));
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.25f);
    }

}
