using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SystemObject {
    //フェード用画像
    [SerializeField]
    private Image fadeImage = null;

    public static FadeManager instance { get; private set; } = null;

    private const float DEFAULT_FADE_TIME = 1.0f;

    public override async UniTask Initialize() {
        instance = this;
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask FadeOut(float duration = DEFAULT_FADE_TIME) {
        await FadeTargetAlfha(eFadeState.FadeOut, duration);
    }
    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask FadeIn(float duration = DEFAULT_FADE_TIME) {
        await FadeTargetAlfha(eFadeState.FadeIn, duration);
    }
    /// <summary>
    /// フェード画像を指定の不透明度に変化
    /// </summary>
    /// <param name="fadeState"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async UniTask FadeTargetAlfha(eFadeState fadeState, float duration) {
        float elapseTime = 0.0f;
        float startAlpha = fadeImage.color.a;
        float targetAlpha = (float)fadeState;
        Color targetColor = fadeImage.color;
        
        while(elapseTime < duration) {
            elapseTime += Time.deltaTime;
            //補完した府透明度をフェード画像に設定
            float t = elapseTime / duration;
            targetColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = targetColor;
            //1フレーム待つ
            await UniTask.DelayFrame(1);
        }
        targetColor.a = targetAlpha;
        fadeImage.color = targetColor;
    }
}
