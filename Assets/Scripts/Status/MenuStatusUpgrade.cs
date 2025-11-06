using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static PlayerStatusUtility;
using static SaveDataUtility;

public class MenuStatusUpgrade : MenuBase {
    // ステータスポイントテキスト
    [SerializeField]
    private TextMeshProUGUI _statusPointText = null;
    // 攻撃レベルテキスト
    [SerializeField]
    private TextMeshProUGUI _attackLvText = null;
    // 攻撃間隔レベルテキスト
    [SerializeField]
    private TextMeshProUGUI _intervalLvText = null;
    // 攻撃可能拡縮率レベルテキスト
    [SerializeField]
    private TextMeshProUGUI _percentageLvText = null;
    // 制限時間レベルテキスト
    [SerializeField]
    private TextMeshProUGUI _limitTimeLvText = null;
    // プレイヤーのステータスデータ
    private PlayerStatusData _playerData = null;

    // ステータスポイント
    private int _statusPoint = -1;
    // 攻撃レベル
    private int _attackLv = -1;
    // 攻撃間隔レベル
    private int _intervalLv = -1;
    // 攻撃可能拡縮率レベル
    private int _percentageLv = -1;
    // 制限時間レベル
    private int _limitTimeLv = -1;
    // メニュー開閉フラグ
    private bool _isClose = false;
    // 最大ステータスポイント
    private const int _MAX_POINT = 1000000;

    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        _playerData = PlayerStatusDataManager.instance.saveData;
        SetStatusPoint(_playerData.statusPoint);
        SetAttackLv(_playerData.attackLv);
        SetIntervalLv(_playerData.atkIntervalLv);
        SetPercentageLv(_playerData.atkPercentageLv);
        SetLimitTimeLv(_playerData.limitTimeLv);
    }
    /// <summary>
    /// 準備前処理
    /// </summary>
    public void Setup() {
        _playerData = PlayerStatusDataManager.instance.saveData;
        SetStatusPoint(_playerData.statusPoint);
    }
    public override async UniTask Open() {
        await base.Open();
        Setup();
        await FadeManager.instance.FadeIn();
        _isClose = false;
        while (!_isClose) {
            // 1フレーム待つ
            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
        PlayerStatusDataManager.instance.SaveData();
    }
    /// <summary>
    /// 攻撃レベル上昇
    /// </summary>
    public void AttackLvUp() {
        if(_attackLv >= 50) return;

        if(_statusPoint - (_attackLv + 1) < 0 ) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        _attackLv++;
        ReduceStatusPoint(_attackLv);
        SetAttackLv(_attackLv);
    }
    /// <summary>
    /// 攻撃レベル低下
    /// </summary>
    public void AttackLvDown() {
        if(_attackLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(_attackLv);
        _attackLv--;
        SetAttackLv(_attackLv); 
    }
    /// <summary>
    /// 攻撃間隔レベル上昇
    /// </summary>
    public void IntervalLvUp() {
        if(_intervalLv >= 50) return;

        if(_statusPoint - (_intervalLv + 1) < 0 ) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        _intervalLv++;
        ReduceStatusPoint(_intervalLv);
        SetIntervalLv(_intervalLv);
    }
    /// <summary>
    /// 攻撃間隔レベル低下
    /// </summary>
    public void IntervalLvDown() {
        if(_intervalLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(_intervalLv);
        _intervalLv--;
        SetIntervalLv(_intervalLv);
    }
    /// <summary>
    /// 攻撃拡縮率レベル上昇
    /// </summary>
    public void PercentageLvUp() {
        if(_percentageLv >= 50) return;

        if(_statusPoint - (_percentageLv + 1) < 0 ) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        _percentageLv++;
        ReduceStatusPoint(_percentageLv);
        SetPercentageLv(_percentageLv);
    }
    /// <summary>
    /// 攻撃拡縮率レベル低下
    /// </summary>
    public void PercentageLvDown() {
        if(_percentageLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(_percentageLv);
        _percentageLv--;
        SetPercentageLv(_percentageLv);
    }
    /// <summary>
    /// 制限時間レベル上昇
    /// </summary>
    public void LimitTimeLvUp() {
        if(_limitTimeLv >= 10) return;

        if (_statusPoint - (_limitTimeLv + 1) < 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        _limitTimeLv++;
        ReduceStatusPoint(_limitTimeLv);
        SetLimitTimeLv(_limitTimeLv);
    }
    /// <summary>
    /// 制限時間レベル低下
    /// </summary>
    public void LimitTimeLvDown() {
        if(_limitTimeLv <= 0) return;

        UniTask task = AudioManager.instance.PlaySE(2);
        IncreaseStatusPoint(_limitTimeLv);
        _limitTimeLv--;
        SetLimitTimeLv(_limitTimeLv);
    }
    /// <summary>
    /// 全てのレベルをリセット
    /// </summary>
    public void ResetAllLevel() {
        SetAttackLv(0);
        SetIntervalLv(0);
        SetPercentageLv(0);
        SetLimitTimeLv(0);
        InitStatus();
    }
    /// <summary>
    /// メニューを閉じるフラグの変更
    /// </summary>
    public void CloseScreen() {
        UniTask task = AudioManager.instance.PlaySE(3);
        _isClose = true;
    }
    /// <summary>
    /// ステータスポイントの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetStatusPoint(int setValue) {
        _statusPoint = Mathf.Clamp(setValue, 0, _MAX_POINT);
        SetStatusPointData(_statusPoint);
        _statusPointText.text = "statusPoint : " + _statusPoint;
    }
    /// <summary>
    /// 攻撃レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetAttackLv(int setValue) {
        _attackLv = setValue;
        SetAttackStatusLv(_attackLv);
        _attackLvText.text = "Lv : " + _attackLv;
    }
    /// <summary>
    /// 間隔レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetIntervalLv(int setValue) {
        _intervalLv = setValue;
        SetIntervalStatusLv(_intervalLv);
        _intervalLvText.text = "Lv : " + _intervalLv;
    }
    /// <summary>
    /// 拡縮率レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetPercentageLv(int setValue) {
        _percentageLv = setValue;
        SetPercentageStatusLv(_percentageLv);
        _percentageLvText.text = "Lv : " + _percentageLv;
    }
    /// <summary>
    /// 制限時間レベルの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetLimitTimeLv(int setValue) {
        _limitTimeLv = setValue;
        SetLimitTimeStatusLv(_limitTimeLv);
        _limitTimeLvText.text = "Lv : " + _limitTimeLv;
    }
    /// <summary>
    /// ステータスポイント減少
    /// </summary>
    /// <param name="setValue"></param>
    private void ReduceStatusPoint(int setValue) {
        SetStatusPoint(_statusPoint - setValue);
    }
    /// <summary>
    /// ステータスポイント増加
    /// </summary>
    /// <param name="setValue"></param>
    private void IncreaseStatusPoint(int setValue) {
        SetStatusPoint(_statusPoint + setValue);
    }
}
