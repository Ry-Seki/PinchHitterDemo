using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using static PlayerStatusUtility;
using static SaveDataUtility;

public class MenuStatusUpgrade : MenuBase {
    [SerializeField]
    private TextMeshProUGUI statusPointText = null;
    [SerializeField]
    private TextMeshProUGUI attackLvText = null;
    [SerializeField]
    private TextMeshProUGUI intervalLvText = null;
    [SerializeField]
    private TextMeshProUGUI percentageLvText = null;
    [SerializeField]
    private TextMeshProUGUI limitTimeLvText = null;
    private PlayerStatusData playerData = null;
    private int statusPoint = -1;
    private int attackLv = -1;
    private int intervalLv = -1;
    private int percentageLv = -1;
    private int limitTimeLv = -1;
    private bool isClose = false;
    private const int MAX_POINT = 10000;

    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        playerData = PlayerStatusDataManager.instance.saveData;
        SetStatusPoint(playerData.statusPoint);
        SetAttackLv(playerData.attackLv);
        SetIntervalLv(playerData.atkIntervalLv);
        SetPercentageLv(playerData.atkPercentageLv);
        SetLimitTimeLv(playerData.limitTimeLv);
    }
    public void Setup() {
        playerData = PlayerStatusDataManager.instance.saveData;
        SetStatusPoint(playerData.statusPoint);
    }
    public override async UniTask Open() {
        await base.Open();
        Setup();
        await FadeManager.instance.FadeIn();
        isClose = false;
        while (!isClose) {
            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
        PlayerStatusDataManager.instance.SaveData();
    }
    public void AttackLvUp() {
        if(attackLv >= 50) return;

        if(statusPoint - (attackLv + 1) < 0 ) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        attackLv++;
        ReduceStatusPoint(attackLv);
        SetAttackLv(attackLv);
    }
    public void AttackLvDown() {
        if(attackLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(attackLv);
        attackLv--;
        SetAttackLv(attackLv); 
    }
    public void IntervalLvUp() {
        if(intervalLv >= 50) return;

        if(statusPoint - (intervalLv + 1) < 0 ) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        intervalLv++;
        ReduceStatusPoint(intervalLv);
        SetIntervalLv(intervalLv);
    }
    public void IntervalLvDown() {
        if(intervalLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(intervalLv);
        intervalLv--;
        SetIntervalLv(intervalLv);
    }
    public void PercentageLvUp() {
        if(percentageLv >= 50) return;

        if(statusPoint - (percentageLv + 1) < 0 ) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        percentageLv++;
        ReduceStatusPoint(percentageLv);
        SetPercentageLv(percentageLv);
    }
    public void PercentageLvDown() {
        if(percentageLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(percentageLv);
        percentageLv--;
        SetPercentageLv(percentageLv);
    }
    public void LimitTimeLvUp() {
        if(limitTimeLv >= 10) return;

        if (statusPoint - (limitTimeLv + 1) < 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        limitTimeLv++;
        ReduceStatusPoint(limitTimeLv);
        SetLimitTimeLv(limitTimeLv);
    }
    public void LimitTimeLvDown() {
        if(limitTimeLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(limitTimeLv);
        limitTimeLv--;
        SetLimitTimeLv(limitTimeLv);
    }
    public void ResetAllLevel() {
        SetAttackLv(0);
        SetIntervalLv(0);
        SetPercentageLv(0);
        SetLimitTimeLv(0);
        InitStatus();
    }
    public void CloseScreen() {
        UniTask task = AudioManager.instance.PlaySE(3);
        isClose = true;
    }
    /// <summary>
    /// ステータスポイントの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetStatusPoint(int setValue) {
        statusPoint = Mathf.Clamp(setValue, 0, MAX_POINT);
        SetStatusPointData(statusPoint);
        statusPointText.text = "statusPoint : " + statusPoint;
    }
    /// <summary>
    /// 攻撃レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetAttackLv(int setValue) {
        attackLv = setValue;
        SetAttackStatusLv(attackLv);
        attackLvText.text = "Lv : " + attackLv;
    }
    /// <summary>
    /// 間隔レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetIntervalLv(int setValue) {
        intervalLv = setValue;
        SetIntervalStatusLv(intervalLv);
        intervalLvText.text = "Lv : " + intervalLv;
    }
    /// <summary>
    /// 拡縮率レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetPercentageLv(int setValue) {
        percentageLv = setValue;
        SetPercentageStatusLv(percentageLv);
        percentageLvText.text = "Lv : " + percentageLv;
    }
    /// <summary>
    /// 制限時間レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetLimitTimeLv(int setValue) {
        limitTimeLv = setValue;
        SetLimitTimeStatusLv(limitTimeLv);
        limitTimeLvText.text = "Lv : " + limitTimeLv;
    }
    /// <summary>
    /// ステータスポイント減少
    /// </summary>
    /// <param name="setValue"></param>
    private void ReduceStatusPoint(int setValue) {
        SetStatusPoint(statusPoint - setValue);
    }
    /// <summary>
    /// ステータスポイント増加
    /// </summary>
    /// <param name="setValue"></param>
    private void IncreaseStatusPoint(int setValue) {
        SetStatusPoint(statusPoint + setValue);
    }
}
