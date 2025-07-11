using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static PlayerStatusUtility;
using static SaveDataUtility;

public class MenuStatusEnhance : MenuBase {
    private int statusPoint = -1;
    private int attackLv = -1;
    private int intervalLv = -1;
    private int percentageLv = -1;
    private bool isClose = false;

    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        SaveData data = SaveDataManager.instance.saveData;
        Setup(data);
    }
    private void Setup(SaveData setData) {
        statusPoint = setData.statusPoint;
        attackLv = setData.rawAttackLv;
        intervalLv = setData.rawAtkIntervalLv;
        percentageLv = setData.rawAtkPercentageLv;
        Debug.Log(attackLv);
        Debug.Log(intervalLv);
        Debug.Log(percentageLv);
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

        attackLv++;
        EnhanceAttack(attackLv);
        Debug.Log(attackLv);
    }
    public void IntervalLvUp() {
        if(intervalLv >= 50) return;

        intervalLv++;
        ShortInterval(intervalLv);
        Debug.Log(intervalLv);

    }
    public void PercentageLvUp() {
        if(percentageLv >= 50) return;

        percentageLv++;
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
}
