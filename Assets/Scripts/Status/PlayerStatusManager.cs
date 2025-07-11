using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class PlayerStatusManager : SystemObject{
    public static PlayerStatusManager instance { get; private set; } = null;

    //‹­‰»€–Ú
    private int rawAttack = -1;
    private float rawInterval = -1;
    private float rawPercentage = -1;
    //‰ŠúƒXƒe[ƒ^ƒX
    private const int INIT_ATTACK = 10;
    private const float INIT_INTERVAL = 1.0f;
    private const float INIT_PERCENTAGE = 90.0f;

    //’è”
    private const int MAX_RAW_ATTACK = 1000000;
    private const float MIN_RAW_INTERVAL = 0.01f;
    private const float MIN_RAW_PERCENTAGE = 10.0f;
    private const int ENHANCE_ATTACK_NUM = 5;
    private const float SHOWTEN_INTERVAL = 0.05f;
    private const float EXPANSION_ATTACK_AREA = 1.5f;

    public override async UniTask Initialize() {
        instance = this;
        SaveData data = SaveDataManager.instance.saveData;
        SetupData(data);
        await UniTask.CompletedTask;
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
        SetRawAttack(INIT_ATTACK + ENHANCE_ATTACK_NUM * setLevel);
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
        SetRawInterval(INIT_INTERVAL - SHOWTEN_INTERVAL * setLevel);
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
        SetRawPercentage(INIT_PERCENTAGE - EXPANSION_ATTACK_AREA  * setLevel);
    }
    public void InitStatus() {
        rawAttack = INIT_ATTACK;
        rawInterval = INIT_INTERVAL;
        rawPercentage = INIT_PERCENTAGE;
    }
}
