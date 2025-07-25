using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using static PlayerStatusUtility;
using static SaveDataUtility;

public class MenuStatusEnhance : MenuBase {
    [SerializeField]
    private TextMeshProUGUI statusPointText = null;
    [SerializeField]
    private TextMeshProUGUI attackLvText = null;
    [SerializeField]
    private TextMeshProUGUI intervalLvText = null;
    [SerializeField]
    private TextMeshProUGUI percentageLvText = null;
    private int statusPoint = -1;
    private int attackLv = -1;
    private int intervalLv = -1;
    private int percentageLv = -1;
    private bool isClose = false;
    private const int MAX_POINT = 10000;

    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        SaveData data = SaveDataManager.instance.saveData;
    }
    public void Setup() {
        SaveData data = SaveDataManager.instance.saveData;
        SetStatusPoint(data.statusPoint);
        SetAttackLv(data.rawAttackLv);
        SetIntervalLv(data.rawAtkIntervalLv);
        SetPercentageLv(data.rawAtkPercentageLv);
    }

    public override async UniTask Open() {
        await base.Open();
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
        MenuManager.instance.Get<MenuTitle>().BackScreen();
        await FadeManager.instance.FadeIn();
    }
    public void AttackLvUp() {
        if(attackLv >= 50) return;

        if(statusPoint - (attackLv + 1) < 0 ) return;

        attackLv++;
        ReduceStatusPoint(attackLv);
        EnhanceAttack(attackLv);
        attackLvText.text = "Lv : " + attackLv;
    }
    public void IntervalLvUp() {
        if(intervalLv >= 50) return;

        if(statusPoint - (intervalLv + 1) < 0 ) return;
        intervalLv++;
        ReduceStatusPoint(attackLv);
        ShortInterval(intervalLv);
        intervalLvText.text = "Lv : " + intervalLv;
    }
    public void PercentageLvUp() {
        if(percentageLv >= 50) return;

        if(statusPoint - (percentageLv + 1) < 0 ) return;

        percentageLv++;
        ReduceStatusPoint(percentageLv);
        ExpansionPercentage(percentageLv);
        percentageLvText.text = "Lv : " + percentageLv;
    }
    public void ResetLevel() {
        SetAttackLv(0);
        SetIntervalLv(0);
        SetPercentageLv(0);
        InitStatus();
    }
    public void CloseScreen() {
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
        attackLvText.text = "Lv : " + attackLv;
    }
    /// <summary>
    /// 間隔レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetIntervalLv(int setValue) {
        intervalLv = setValue;
        intervalLvText.text = "Lv : " + intervalLv;
    }
    /// <summary>
    /// 拡縮率レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetPercentageLv(int setValue) {
        percentageLv = setValue;
        percentageLvText.text = "Lv : " + percentageLv;
    }
    private void ReduceStatusPoint(int setValue) {
        SetStatusPoint(statusPoint - setValue);
    }
}
