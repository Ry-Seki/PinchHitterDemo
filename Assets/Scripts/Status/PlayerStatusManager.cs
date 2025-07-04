using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class PlayerStatusManager : SystemObject{
    public static PlayerStatusManager instance { get; private set; } = null;

    //‹­‰»€–Ú
    private int rawAttack = -1;
    private float rawInterval = -1.0f;
    private float rawPercentage = -1;

    //’è”
    private const int MAX_RAW_ATTACK = 1000000;
    private const float MIN_RAW_INTERVAL = 0.01f;
    private const float MIN_RAW_PERCENTAGE = 10.0f;
    private const int ENHANCE_ATTACK_NUM = 5;
    private const float SHOWTEN_INTERVAL = 0.1f;
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
        ExpansionPercentage(setData.rawAtkIntervalLv);
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
    /// UŒ‚—Í‘‰Á
    /// </summary>
    /// <param name="setLevel"></param>
    public void EnhanceAttack(int setLevel) {
        SetRawAttack(rawAttack + ENHANCE_ATTACK_NUM * setLevel);
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
    /// UŒ‚ŠÔŠu’Zk
    /// </summary>
    /// <param name="setLevel"></param>
    public void ShortInterval(int setLevel) {
        SetRawInterval(rawInterval - SHOWTEN_INTERVAL * setLevel);
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
    /// UŒ‚‰Â”\Šgk—¦‚ÌŠg‘å
    /// </summary>
    /// <param name="setLevel"></param>
    public void ExpansionPercentage(int setLevel) { 
        SetRawPercentage(rawPercentage - EXPANSION_ATTACK_AREA  * setLevel);
    }
    public void InitStatus() {
        rawAttack = 10;
        rawInterval = 1.0f;
        rawPercentage = 90.0f;
    }
}
