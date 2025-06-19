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

    public bool isPinchTextFade = false;

    private const float DEFAULT_APPEARANCE_TIME = 1.0f;

    public void VisiblePinchExpansion(float setExpansion, float duration = DEFAULT_APPEARANCE_TIME) {
        pinchExpansion = setExpansion;
        pinchText.text = pinchExpansion.ToString("F1") + "%";
        isPinchTextFade = true;
        ResetColorAlpha();
        UniTask task = PinchTextFade(duration);
    }
    private async UniTask PinchTextFade(float duration) {
        Color targetColor = pinchText.color;
        float elapseTime = 0.0f;
        float startAlpha = 1.0f;
        float targetAlpha = 0.0f;

        while (elapseTime < duration) {
            if(!isPinchTextFade) break;
            elapseTime += Time.deltaTime;
            //補完した府透明度をフェード画像に設定
            float t = elapseTime / duration;
            targetColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            pinchText.color = targetColor;
            await UniTask.DelayFrame(1);
        }
        targetColor.a = targetAlpha;
        pinchText.color = targetColor;
    }

    private void ResetColorAlpha() {
        isPinchTextFade = false;
        Color textColor = pinchText.color;
        textColor.a = 1.0f;
        pinchText.color = textColor;
    }
}
