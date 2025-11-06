using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static GameConst;

public class PinchExpansionText : MenuBase {
    [SerializeField]
    private TextMeshProUGUI _pinchText = null;

    private static float _pinchExpansion = -1;

    private bool _isFadeText = false;

    private const float _DEFAULT_APPEARANCE_TIME = 1.0f;

    public override async UniTask Initialize() {
        await base.Initialize();
        _isFadeText = false;
        _pinchExpansion = MAX_PERCENTAGE;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 文字の可視化
    /// </summary>
    /// <param name="setExpansion"></param>
    public void VisiblePinchExpansion(float setExpansion) {
        _pinchExpansion = setExpansion;
        _pinchText.text = _pinchExpansion.ToString("F1") + "%";
        ResetColorAlpha();
    }
    /// <summary>
    /// 文字のフェード
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask PinchTextFade(float duration = _DEFAULT_APPEARANCE_TIME) {
        if(_pinchText.color.a < 1.0) return;

        _isFadeText = true;
        Color targetColor = _pinchText.color;
        float elapseTime = 0.0f;
        float startAlpha = 1.0f;
        float targetAlpha = 0.0f;

        while (_isFadeText && elapseTime < duration) {
            elapseTime += Time.deltaTime;
            //補完した不透明度を文字に設定
            float t = elapseTime / duration;
            targetColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            _pinchText.color = targetColor;
            // 終了時抜ける
            if(!_isFadeText) break;
            await UniTask.DelayFrame(1);
        }
        if(_isFadeText) {
            targetColor.a = targetAlpha;
            _pinchText.color = targetColor;
        }
        _isFadeText = false;
    }
    /// <summary>
    /// テキストの色のリセット
    /// </summary>
    public void ResetColorAlpha() {
        _isFadeText = false;
        Color textColor = _pinchText.color;
        textColor.a = 1.0f;
        _pinchText.color = textColor;
    }
}
