using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    /*
        MANUALLY CHANGED THE DEFAULT ALPHA TRANSPARENCY TO 0;
    */

    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;
    private IEnumerator fadeRoutine;

    // CALL IN AREAEXIT.CS
    public void FadeToBlack(){
        if (fadeRoutine != null){
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }

    // CALL IN AREAENTRANCE.CS
    public void FadeToClear(){
        if (fadeRoutine != null){
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlphaTrans){
        // WE WANT TO FADE UNTIL ITS TARGET ALPHA TRANSPARENCY
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlphaTrans)){

            //Debug.Log(fadeScreen.color.a); // 0 transparent and 1 is opaque

            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlphaTrans, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);

            yield return null;
        }
    }
}
