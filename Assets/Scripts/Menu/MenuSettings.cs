using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static GameConst;

public class MenuSettings : MenuBase {
    // BGM音量テキスト
    [SerializeField]
    private TextMeshProUGUI _bgmVolumeText = null;
    // SE音量テキスト
    [SerializeField]
    private TextMeshProUGUI _seVolumeText = null;
    // 感度テキスト
    [SerializeField]
    private TextMeshProUGUI _sensitivityText = null;

    // メニュー開閉フラグ
    private bool _isClose = false;
    // BGM音量
    private int _bgmVolume = -1;
    // SE音量
    private int _seVolume = -1;
    // 感度
    private int _moveSensitivity = -1;

    public override async UniTask Initialize() {
        await base.Initialize();
        SetupData();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// セーブデータから取得した値の設定
    /// </summary>
    private void SetupData() {
        SettingStatusData data = SettingStatusDataManager.instance.saveData;
        if(data == null) return;

        //音量の初期化
        SetBGMVolume(data.bgmVolume);
        SetSEVolume(data.seVolume);
        SetSensitivity(data.moveSensitivity);
        SetBGMVolumeData(_bgmVolume);
        SetSEVolumeData(_seVolume);
        SetSensitivityData(_moveSensitivity);
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        while (!_isClose) {
            await UniTask.DelayFrame(1);
        }
        _isClose = false;
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
        SettingStatusDataManager.instance.SaveData();
    }
    /// <summary>
    /// BGM音量設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetBGMVolume(int setValue) {
        _bgmVolume = Mathf.Clamp(setValue, 0, TEN_DEVIDE_VALUE);
        _bgmVolumeText.text = _bgmVolume.ToString();
    }
    /// <summary>
    /// SE音量設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSEVolume(int setValue) {
        _seVolume = Mathf.Clamp(setValue, 0, TEN_DEVIDE_VALUE);
        _seVolumeText.text = _seVolume.ToString();
    }
    /// <summary>
    /// 感度の設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSensitivity(int setValue) {
        _moveSensitivity = Mathf.Clamp(setValue, 1, TEN_DEVIDE_VALUE);
        _sensitivityText.text = _moveSensitivity.ToString();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        UniTask task = AudioManager.instance.PlaySE(3);
        _isClose = true;
    }
    /// <summary>
    /// BGM音量を上げる
    /// </summary>
    public void AddBGMVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        _bgmVolume++;
        SetBGMVolume(_bgmVolume);
        SetBGMVolumeData(_bgmVolume);
    }
    /// <summary>
    /// BGM音量を下げる
    /// </summary>
    public void SubBGMVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        _bgmVolume--;
        SetBGMVolume(_bgmVolume);
        SetBGMVolumeData(_bgmVolume);
    }
    /// <summary>
    /// SE音量を上げる
    /// </summary>
    public void AddSEVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        _seVolume++;
        SetSEVolume(_seVolume);
        SetSEVolumeData(_seVolume);
    }
    /// <summary>
    /// SE音量を下げる
    /// </summary>
    public void SubSEVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        _seVolume--;
        SetSEVolume(_seVolume);
        SetSEVolumeData(_seVolume);
    }
    /// <summary>
    /// 感度を上げる
    /// </summary>
    public void AddMoveSensitivity() {
        UniTask task = AudioManager.instance.PlaySE(2);
        _moveSensitivity++;
        SetSensitivity(_moveSensitivity);
        SetSensitivityData(_moveSensitivity);
    }
    /// <summary>
    /// 感度を下げる
    /// </summary>
    public void SubMoveSensitivity() {
        UniTask task = AudioManager.instance.PlaySE(2);
        _moveSensitivity--;
        SetSensitivity(_moveSensitivity);
        SetSensitivityData(_moveSensitivity);
    }
    /// <summary>
    /// BGM音量データの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetBGMVolumeData(float setValue) {
        AudioManager.instance.SetBGMVolume(setValue);
        SettingStatusDataManager.instance.SetBGMVolume((int)setValue);
    }
    /// <summary>
    /// SE音量データの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSEVolumeData(float setValue) {
        AudioManager.instance.SetSEVolume(setValue);
        SettingStatusDataManager.instance.SetSEVolume((int)setValue);
    }
    /// <summary>
    /// 感度データの設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSensitivityData(float setValue) {
        CameraController.SetMoveSensitivity(setValue);
        SettingStatusDataManager.instance.SetMoveSensitivity((int)setValue);
    }
}
