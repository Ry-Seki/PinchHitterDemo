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
        statusPoint = data.statusPoint;
        attackLv = data.rawAttackLv;
        intervalLv = data.rawAtkIntervalLv;
        percentageLv = data.rawAtkPercentageLv;
        Debug.Log(statusPoint);
        Debug.Log(attackLv);
        Debug.Log(intervalLv);
        Debug.Log(percentageLv);
        SetStatusPoint(statusPoint);
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
        Debug.Log(attackLv);
    }
    public void IntervalLvUp() {
        if(intervalLv >= 50) return;

        if(statusPoint - (intervalLv + 1) < 0 ) return;
        intervalLv++;
        ReduceStatusPoint(attackLv);
        ShortInterval(intervalLv);
        Debug.Log(intervalLv);
    }
    public void PercentageLvUp() {
        if(percentageLv >= 50) return;

        if(statusPoint - (percentageLv + 1) < 0 ) return;

        percentageLv++;
        ReduceStatusPoint(percentageLv);
        ExpansionPercentage(percentageLv);
        Debug.Log(percentageLv);
    }
    public void ResetLevel() {
        attackLv = 0;
        intervalLv = 0;
        percentageLv = 0;
        InitStatus();
    }
    public void CloseScreen() {
        isClose = true;
    }

    private void SetStatusPoint(int setValue) {
        statusPoint = Mathf.Clamp(setValue, 0, MAX_POINT);
        SetStatusPointData(statusPoint);
        statusPointText.text = "statusPoint : " + statusPoint;
    }

    private void ReduceStatusPoint(int setValue) {
        SetStatusPoint(statusPoint - setValue);
    }
}
