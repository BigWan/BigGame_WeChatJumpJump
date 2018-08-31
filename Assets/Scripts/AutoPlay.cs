using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 自动玩的脚本
/// </summary>
public class AutoPlay : MonoBehaviour {

    private Role role;


    private bool isCoroutineRunning = false;

    private void Awake() {
        DontDestroyOnLoad(this);
        role = FindObjectOfType<Role>();
    }

    private void Update() {

        if (GameManager.Instance.isAutoPlay) {
            if (role.IsReadyForJump() && !isCoroutineRunning) {
                StartCoroutine(PressCoroutine());
            }
        }
    }



    IEnumerator PressCoroutine() {
        isCoroutineRunning = true;
        Vector3 distance =role.PerfectVector();
        float time = distance.magnitude / role.jumpSpeed;
        role.StartPress();
        yield return new WaitForSeconds(time);
        role.EndPress();
        yield return new WaitForSeconds(1f);
        isCoroutineRunning = false;
    }


}
