using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class UIBase : MonoBehaviour {

    protected CanvasGroup cg;

    /// <summary>
    /// UI渐隐掉
    /// </summary>
    public void FadeOut(int fadeFrame=15) {
        if(fadeFrame > 0)
            StartCoroutine(FadeOutCoroutine(fadeFrame));
        else
            gameObject.SetActive(false);

    }

    IEnumerator FadeOutCoroutine(int fadeFrame) {
        if (fadeFrame <= 0) yield break;
        cg.alpha = 1f;
        cg.blocksRaycasts = false;
        for (int i = 0; i < fadeFrame; i++) {
            cg.alpha = 1 - (float)i / fadeFrame;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void FadeIn(int fadeFrame = 15) {
        gameObject.SetActive(true);
        cg.alpha = 0;
        if (fadeFrame > 0)
            StartCoroutine(FadeInCoroutine(fadeFrame));
        else
            cg.alpha = 1;
    }

    IEnumerator FadeInCoroutine(int fadeFrame) {
        if (fadeFrame <= 0) yield break;
        cg.alpha = 0;
        //cg.blocksRaycasts = true;
        for (int i = 0; i < fadeFrame; i++) {
            cg.alpha = (float)i / (float)fadeFrame;
            yield return null;
        }
        cg.alpha = 1f;
        cg.blocksRaycasts = true;
    }
}
