using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static PlayerMasterUtility;

public class PlayerStatusManager : SystemObject{
    public static PlayerStatusManager instance { get; private set; } = null;

    //強化項目
    private int _rawAttack = -1;
    private float _rawInterval = -1;
    private float _rawPercentage = -1;
    private int _rawLimitTime = -1;
    //初期ステータス
    private int _initAttack = -1;
    private float _initInterval = -1;
    private float _initPercentage = -1;
    private int _initLimitTime = -1;
    //レベルアップ時の上昇幅
    private int _addAttackNum = -1;
    private float _shortenInterval = -1;
    private float _expansionPercentage = -1;
    private int _extentedTime = -1;

    //定数
    private const int _MAX_RAW_ATTACK = 1000000;
    private const float _MIN_RAW_INTERVAL = 0.01f;
    private const float _MIN_RAW_PERCENTAGE = 10.0f;
    private const int _MAX_RAW_LIMIT_TIME = 1000000;

    public override async UniTask Initialize() {
        instance = this;
        PlayerStatusData data = PlayerStatusDataManager.instance.saveData;
        SetupMaster();
        SetupData(data);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// マスターデータのセット
    /// </summary>
    private void SetupMaster() {
        var playerMaster = GetPlayerMaster();
        _initAttack = playerMaster.rawAttack;
        _initInterval = playerMaster.attackInterval;
        _initPercentage = playerMaster.attackPinchPer;
        _initLimitTime = playerMaster.rawLimitTime;
        _addAttackNum = playerMaster.AddAttackValue;
        _shortenInterval = playerMaster.ShortenIntervalValue;
        _expansionPercentage = playerMaster.ExpansionAttackArea;
        _extentedTime = playerMaster.ExtentedTime;
    }
    /// <summary>
    /// ステータスの準備
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
    /// 攻撃力取得
    /// </summary>
    /// <returns></returns>
    public int GetRawAttack() {
        return _rawAttack;
    }
    ///
    /// <summary>
    /// 攻撃力の設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawAttack(int setValue) {
        _rawAttack = Mathf.Clamp(setValue, 0, _MAX_RAW_ATTACK);
    }
    /// <summary>
    /// レベル指定の攻撃力設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetAttackStatusLv(int setLevel) {
        SetRawAttack(_initAttack + _addAttackNum * setLevel);
    }
    /// <summary>
    /// 攻撃間隔取得
    /// </summary>
    /// <returns></returns>
    public float GetRawInterval() {
        return _rawInterval;
    }
    /// <summary>
    /// 攻撃間隔設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawInterval(float setValue) {
        _rawInterval = Mathf.Clamp(setValue, _MIN_RAW_INTERVAL, 1.0f);
    }
    /// <summary>
    /// レベル指定の攻撃間隔設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetIntervalStatusLv(int setLevel) {
        SetRawInterval(_initInterval - _shortenInterval * setLevel);
    }
    /// <summary>
    /// 攻撃可能拡縮率取得
    /// </summary>
    /// <returns></returns>
    public float GetRawPercentage() {
        return _rawPercentage;
    }
    /// <summary>
    /// 攻撃可能拡縮率設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawPercentage(float setValue) {
        _rawPercentage = Mathf.Clamp(setValue, _MIN_RAW_PERCENTAGE, MAX_PERCENTAGE);
    }
    /// <summary>
    /// レベル指定の攻撃可能拡縮率の設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetPercentageStatusLv(int setLevel) { 
        SetRawPercentage(_initPercentage - _expansionPercentage  * setLevel);
    }
    /// <summary>
    /// 制限時間取得
    /// </summary>
    /// <returns></returns>
    public int GetRawLimitTime() {
        return _rawLimitTime;
    }
    /// <summary>
    /// 制限時間設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawLimitTme(int setValue) {
        _rawLimitTime = Mathf.Clamp(setValue, 0, _MAX_RAW_LIMIT_TIME);
    }
    /// <summary>
    /// レベル指定の制限時間の設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetLimitTimeStatusLv(int setLevel) {
        SetRawLimitTme(_initLimitTime + _extentedTime * setLevel);
    }
    /// <summary>
    /// ステータスの初期化
    /// </summary>
    public void InitStatus() {
        _rawAttack = _initAttack;
        _rawInterval = _initInterval;
        _rawPercentage = _initPercentage;
    }
}
