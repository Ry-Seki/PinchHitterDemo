using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static GameConst;

public class MenuSettings : MenuBase {
    [SerializeField]
    private TextMeshProUGUI bgmVolumeText = null;
    [SerializeField]
    private TextMeshProUGUI seVolumeText = null;
    [SerializeField]
    private TextMeshProUGUI sensitivityText = null;

    private bool isClose = false;
    private int bgmVolume = -1;
    private int seVolume = -1;
    private int moveSensitivity = -1;

    public override async UniTask Initialize() {
        await base.Initialize();
        SetupData();
        gameObject.SetActive(false);
    }
    private void SetupData() {
        SettingStatusData data = SettingStatusDataManager.instance.saveData;
        if(data == null) return;

        //音量の初期化
        SetBGMVolume(data.bgmVolume);
        SetSEVolume(data.seVolume);
        SetSensitivity(data.moveSensitivity);
        SetBGMVolumeData(bgmVolume);
        SetSEVolumeData(seVolume);
        SetSensitivityData(moveSensitivity);
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        while (!isClose) {
            await UniTask.DelayFrame(1);
        }
        isClose = false;
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
        bgmVolume = Mathf.Clamp(setValue, 0, TEN_DEVIDE_VALUE);
        bgmVolumeText.text = bgmVolume.ToString();
    }
    /// <summary>
    /// SE音量設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSEVolume(int setValue) {
        seVolume = Mathf.Clamp(setValue, 0, TEN_DEVIDE_VALUE);
        seVolumeText.text = seVolume.ToString();
    }
    private void SetSensitivity(int setValue) {
        moveSensitivity = Mathf.Clamp(setValue, 1, TEN_DEVIDE_VALUE);
        sensitivityText.text = moveSensitivity.ToString();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        UniTask task = AudioManager.instance.PlaySE(3);
        isClose = true;
    }
    /// <summary>
    /// BGM音量を上げる
    /// </summary>
    public void AddBGMVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        bgmVolume++;
        SetBGMVolume(bgmVolume);
        SetBGMVolumeData(bgmVolume);
    }
    /// <summary>
    /// BGM音量を下げる
    /// </summary>
    public void SubBGMVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        bgmVolume--;
        SetBGMVolume(bgmVolume);
        SetBGMVolumeData(bgmVolume);
    }
    /// <summary>
    /// SE音量を上げる
    /// </summary>
    public void AddSEVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        seVolume++;
        SetSEVolume(seVolume);
        SetSEVolumeData(seVolume);
    }
    /// <summary>
    /// SE音量を下げる
    /// </summary>
    public void SubSEVolume() {
        UniTask task = AudioManager.instance.PlaySE(2);
        seVolume--;
        SetSEVolume(seVolume);
        SetSEVolumeData(seVolume);
    }
    /// <summary>
    /// 感度を上げる
    /// </summary>
    public void AddMoveSensitivity() {
        UniTask task = AudioManager.instance.PlaySE(2);
        moveSensitivity++;
        SetSensitivity(moveSensitivity);
        SetSensitivityData(moveSensitivity);
    }
    /// <summary>
    /// 感度を下げる
    /// </summary>
    public void SubMoveSensitivity() {
        UniTask task = AudioManager.instance.PlaySE(2);
        moveSensitivity--;
        SetSensitivity(moveSensitivity);
        SetSensitivityData(moveSensitivity);
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
