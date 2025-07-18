using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static PlayerMasterUtility;

public class PlayerStatusManager : SystemObject{
    public static PlayerStatusManager instance { get; private set; } = null;

    //‹­‰»€–Ú
    private int rawAttack = -1;
    private float rawInterval = -1;
    private float rawPercentage = -1;
    //‰ŠúƒXƒe[ƒ^ƒX
    private int initAttack = -1;
    private float initInterval = -1;
    private float initPercentage = -1;
    //ƒŒƒxƒ‹ƒAƒbƒv‚Ìã¸•
    private int addAttackNum = 5;
    private float shortenInterval = 0.05f;
    private float expansionPercentage = 1.5f;

    //’è”
    private const int MAX_RAW_ATTACK = 1000000;
    private const float MIN_RAW_INTERVAL = 0.01f;
    private const float MIN_RAW_PERCENTAGE = 10.0f;

    public override async UniTask Initialize() {
        instance = this;
        SaveData data = SaveDataManager.instance.saveData;
        SetupMaster();
        SetupData(data);
        await UniTask.CompletedTask;
    }
    private void SetupMaster() {
        var playerMaster = GetPlayerMaster();
        initAttack = playerMaster.rawAttack;
        initInterval = playerMaster.attackInterval;
        initPercentage = playerMaster.attackPinchPer;
        addAttackNum = playerMaster.AddAttackValue;
        shortenInterval = playerMaster.ShortenIntervalValue;
        expansionPercentage = playerMaster.ExpansionAttackArea;
    }
    /// <summary>
    /// ƒXƒe[ƒ^ƒX‚Ì€”õ
    /// </summary>
    /// <param name="setData"></param>
    public void SetupData(SaveData setData) {
        InitStatus();
        EnhanceAttack(setData.rawAttackLv);
        ShortInterval(setData.rawAtkIntervalLv);
        ExpansionPercentage(setData.rawAtkPercentageLv);
    }
    /// <summary>
    /// UŒ‚—Íæ“¾
    /// </summary>
    /// <returns></returns>
    public int GetRawAttack() {
        return rawAttack;
    }
    ///
    /// <summary>
    /// UŒ‚—Í‚Ìİ’è
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawAttack(int setValue) {
        rawAttack = Mathf.Clamp(setValue, 0, MAX_RAW_ATTACK);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚—Í‘‰Á
    /// </summary>
    /// <param name="setLevel"></param>
    public void EnhanceAttack(int setLevel) {
        SetRawAttack(initAttack + addAttackNum * setLevel);
    }
    /// <summary>
    /// UŒ‚ŠÔŠuæ“¾
    /// </summary>
    /// <returns></returns>
    public float GetRawInterval() {
        return rawInterval;
    }
    /// <summary>
    /// UŒ‚ŠÔŠuİ’è
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawInterval(float setValue) {
        rawInterval = Mathf.Clamp(setValue, MIN_RAW_INTERVAL, 1.0f);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚ŠÔŠu’Zk
    /// </summary>
    /// <param name="setLevel"></param>
    public void ShortInterval(int setLevel) {
        SetRawInterval(initInterval - shortenInterval * setLevel);
    }
    /// <summary>
    /// UŒ‚‰Â”\Šgk—¦æ“¾
    /// </summary>
    /// <returns></returns>
    public float GetRawPercentage() {
        return rawPercentage;
    }
    /// <summary>
    /// UŒ‚‰Â”\Šgk—¦İ’è
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawPercentage(float setValue) {
        rawPercentage = Mathf.Clamp(setValue, MIN_RAW_PERCENTAGE, MAX_PERCENTAGE);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚‰Â”\Šgk—¦‚ÌŠg‘å
    /// </summary>
    /// <param name="setLevel"></param>
    public void ExpansionPercentage(int setLevel) { 
        SetRawPercentage(initPercentage - expansionPercentage  * setLevel);
    }
    public void InitStatus() {
        rawAttack = initAttack;
        rawInterval = initInterval;
        rawPercentage = initPercentage;
    }
}
