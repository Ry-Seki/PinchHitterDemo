using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

using static GameConst;

public class PinchExpansionText : MenuBase {
    [SerializeField]
    private TextMeshProUGUI pinchText = null;

    private static float pinchExpansion = -1;

    private bool isFadeText = false;
    private const float DEFAULT_APPEARANCE_TIME = 1.0f;

    public override async UniTask Initialize() {
        await base.Initialize();
        isFadeText = false;
        pinchExpansion = MAX_PERCENTAGE;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 文字の可視化
    /// </summary>
    /// <param name="setExpansion"></param>
    public void VisiblePinchExpansion(float setExpansion) {
        pinchExpansion = setExpansion;
        pinchText.text = pinchExpansion.ToString("F1") + "%";
        ResetColorAlpha();
    }
    /// <summary>
    /// 文字のフェード
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask PinchTextFade(float duration = DEFAULT_APPEARANCE_TIME) {
        if(pinchText.color.a < 1.0) return;

        isFadeText = true;
        Color targetColor = pinchText.color;
        float elapseTime = 0.0f;
        float startAlpha = 1.0f;
        float targetAlpha = 0.0f;

        while (isFadeText && elapseTime < duration) {
            elapseTime += Time.deltaTime;
            //補完した不透明度を文字に設定
            float t = elapseTime / duration;
            targetColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            pinchText.color = targetColor;
            await UniTask.DelayFrame(1);
        }
        if(!isFadeText) return;
        
        targetColor.a = targetAlpha;
        pinchText.color = targetColor;
    }
    /// <summary>
    /// テキストの色のリセット
    /// </summary>
    public void ResetColorAlpha() {
        isFadeText = false;
        Color textColor = pinchText.color;
        textColor.a = 1.0f;
        pinchText.color = textColor;
    }
}
