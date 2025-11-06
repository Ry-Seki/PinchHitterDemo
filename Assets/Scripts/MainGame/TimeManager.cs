using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static PlayerStatusUtility;

public class TimeManager : MenuBase{
    // タイムテキスト
    [SerializeField]
    private TextMeshProUGUI _timeText = null;
    // 制限時間
    public static int limitTimerPer { get; private set; } = -1;

    // 総合スコア
    private static int _storeScore = -1;

    // 敵を倒したときにもらえるスコア値
    private const int _BORDER_SCORE = 400;
    // 制限時間手前の警告を流す残り時間
    private const int _ALARMTIME = 3;

    public override async UniTask Initialize() {
        await base.Initialize();
        menuRoot?.SetActive(false);
    }
    public override async UniTask Open() {
        await base.Open();
        _storeScore = 0;
        SetLimitTimePer(GetRawLimitTime());
        float delta = 0.0f;
        while (limitTimerPer > 0) {
            delta += Time.deltaTime;
            if (delta > 1) {
                delta = 0;
                limitTimerPer--;
                _timeText.text = limitTimerPer.ToString();
                LimitTimeCounter();
            }
            await UniTask.DelayFrame(1);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void LimitTimeCounter() {
        if(limitTimerPer > _ALARMTIME) return;

        UniTask task = AudioManager.instance.PlaySE(9);
    }
    /// <summary>
    /// 制限時間の設定
    /// </summary>
    public void SetLimitTimePer(int setTime) {
        limitTimerPer = setTime;
        _timeText.text = limitTimerPer.ToString();
    }
    /// <summary>
    /// 獲得したスコアに応じて制限時間を延長する
    /// </summary>
    /// <param name="setScore"></param>
    public static void AddLimitTime(int setScore) {
        _storeScore += setScore;
        if(_storeScore / _BORDER_SCORE <= 0) return;

        int judgeScore = _storeScore / _BORDER_SCORE;
        limitTimerPer += judgeScore;
        _storeScore = 0;
    }
}
