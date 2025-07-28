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
    private int rawLimitTime = -1;
    //‰ŠúƒXƒe[ƒ^ƒX
    private int initAttack = -1;
    private float initInterval = -1;
    private float initPercentage = -1;
    private int initLimitTime = -1;
    //ƒŒƒxƒ‹ƒAƒbƒv‚Ìã¸•
    private int addAttackNum = -1;
    private float shortenInterval = -1;
    private float expansionPercentage = -1;
    private int extentedTime = -1;

    //’è”
    private const int MAX_RAW_ATTACK = 1000000;
    private const float MIN_RAW_INTERVAL = 0.01f;
    private const float MIN_RAW_PERCENTAGE = 10.0f;
    private const int MAX_RAW_LIMIT_TIME = 1000000;

    public override async UniTask Initialize() {
        instance = this;
        PlayerStatusData data = PlayerStatusDataManager.instance.saveData;
        SetupMaster();
        SetupData(data);
        await UniTask.CompletedTask;
    }
    private void SetupMaster() {
        var playerMaster = GetPlayerMaster();
        initAttack = playerMaster.rawAttack;
        initInterval = playerMaster.attackInterval;
        initPercentage = playerMaster.attackPinchPer;
        initLimitTime = playerMaster.rawLimitTime;
        addAttackNum = playerMaster.AddAttackValue;
        shortenInterval = playerMaster.ShortenIntervalValue;
        expansionPercentage = playerMaster.ExpansionAttackArea;
        extentedTime = playerMaster.ExtentedTime;
    }
    /// <summary>
    /// ƒXƒe[ƒ^ƒX‚Ì€”õ
    /// </summary>
    /// <param name="setData"></param>
    public void SetupData(PlayerStatusData setData) {
        InitStatus();
        SetAttackStatusLv(setData.attackLv);
        SetIntervalStatusLv(setData.atkIntervalLv);
        SetPercentageStatusLv(setData.atkPercentageLv);
        SetLimitTimeStatusLv(setData.limitTimeLv);
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
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚—Íİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetAttackStatusLv(int setLevel) {
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
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚ŠÔŠuİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetIntervalStatusLv(int setLevel) {
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
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚‰Â”\Šgk—¦‚Ìİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetPercentageStatusLv(int setLevel) { 
        SetRawPercentage(initPercentage - expansionPercentage  * setLevel);
    }
    /// <summary>
    /// §ŒÀŠÔæ“¾
    /// </summary>
    /// <returns></returns>
    public int GetRawLimitTime() {
        return rawLimitTime;
    }
    /// <summary>
    /// §ŒÀŠÔİ’è
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawLimitTme(int setValue) {
        rawLimitTime = Mathf.Clamp(setValue, 0, MAX_RAW_LIMIT_TIME);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚Ì§ŒÀŠÔ‚Ìİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetLimitTimeStatusLv(int setLevel) {
        SetRawLimitTme(initLimitTime + extentedTime * setLevel);
    }
    /// <summary>
    /// ƒXƒe[ƒ^ƒX‚Ì‰Šú‰»
    /// </summary>
    public void InitStatus() {
        rawAttack = initAttack;
        rawInterval = initInterval;
        rawPercentage = initPercentage;
    }
}
