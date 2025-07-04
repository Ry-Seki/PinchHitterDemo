using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static PlayerStatusUtility;

public class MenuStatus : MenuBase {
    [SerializeField]
    private TextMeshProUGUI statusPointText = null;
    [SerializeField]
    private TextMeshProUGUI rawAttackText = null;
    [SerializeField]
    private TextMeshProUGUI rawIntervalText = null;
    [SerializeField]
    private TextMeshProUGUI rawPercentageText = null;

    private int statusPoint = -1;
    private int attackLv = -1;
    private int intervalLv = -1;
    private int percentageLv = -1;

    public override async UniTask Initialize() {
        await base.Initialize();
        
    }

    public override UniTask Open() {
        return base.Open();

    }

    public override UniTask Close() {
        return base.Close();

    }
    /// <summary>
    /// ÉfÅ[É^ÇÃèâä˙âª
    /// </summary>
    private void SetupData() {
        SaveData data = SaveDataManager.instance.saveData;

        statusPoint = data.statusPoint;
        attackLv = data.rawAttackLv;
        intervalLv = data.rawAtkIntervalLv;
        percentageLv = data.rawAtkPercentageLv;
    }
}
