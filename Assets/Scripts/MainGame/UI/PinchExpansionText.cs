using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PinchExpansionText : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI pinchText;

    private static float pinchExpansion = -1;

    private const float DEFAULT_APPEARANCE_TIME = 1.0f;

    public void VisiblePinchExpansion(float setExpansion, float duration = DEFAULT_APPEARANCE_TIME) {
        pinchExpansion = setExpansion;
        pinchText.text = pinchExpansion.ToString("F1") + "%";
        PinchTextFade(duration);
    }
    private void PinchTextFade(float duration) {
        float elapseTime = 0.0f;
        float startAlpha = pinchText.color.a;
        float targetAlpha = 1.0f;
        Color targetColor = pinchText.color;

        while (elapseTime < duration) {
            elapseTime += Time.deltaTime;
            //補完した府透明度をフェード画像に設定
            float t = elapseTime / duration;
            targetColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            pinchText.color = targetColor;
        }
        targetColor.a = targetAlpha;
        pinchText.color = targetColor;
    }
}
