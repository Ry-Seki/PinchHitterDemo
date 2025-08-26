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

    private bool isClose = false;
    private int bgmVolume = -1;
    private int seVolume = -1;

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
        SetBGMVolumeData(bgmVolume);
        SetSEVolumeData(seVolume);
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
        MenuManager.instance.Get<MenuTitle>().BackScreen();
        await FadeManager.instance.FadeIn();
    }
    /// <summary>
    /// BGM音量設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetBGMVolume(int setValue) {
        bgmVolume = Mathf.Clamp(setValue, 0, TEN_DEVIDE_VOLUME);
        bgmVolumeText.text = bgmVolume.ToString();
    }
    /// <summary>
    /// SE音量設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetSEVolume(int setValue) {
        seVolume = Mathf.Clamp(setValue, 0, TEN_DEVIDE_VOLUME);
        seVolumeText.text = seVolume.ToString();
    }
    /// <summary>
    /// メニュー開閉フラグの変更
    /// </summary>
    public void MenuClose() {
        isClose = true;
    }
    /// <summary>
    /// BGM音量を上げる
    /// </summary>
    public void AddBGMVolume() {
        bgmVolume++;
        SetBGMVolume(bgmVolume);
        SetBGMVolumeData(bgmVolume);
    }
    /// <summary>
    /// BGM音量を下げる
    /// </summary>
    public void SubBGMVolume() {
        bgmVolume--;
        SetBGMVolume(bgmVolume);
        SetBGMVolumeData(bgmVolume);
    }
    /// <summary>
    /// SE音量を上げる
    /// </summary>
    public void AddSEVolume() {
        seVolume++;
        SetSEVolume(seVolume);
        SetSEVolumeData(seVolume);
    }
    /// <summary>
    /// SE音量を下げる
    /// </summary>
    public void SubSEVolume() {
        seVolume--;
        SetSEVolume(seVolume);
        SetSEVolumeData(seVolume);
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

}
