using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSettings : MenuBase {
    [SerializeField]
    private TextMeshProUGUI bgmVolumeText = null;
    [SerializeField]
    private TextMeshProUGUI seVolumeTextt = null;
    public override async UniTask Initialize() {
        await base.Initialize();

    }
    private void SetupData() {
        SettingStatusData data = SettingStatusDataManager.instance.saveData;
        if(data == null) return;

        AudioManager.instance.ChangeVolumeBGM(data.bgmVolume);
        AudioManager.instance.ChangeVolumeSE(data.seVolume);
    }
    public override async UniTask Open() {
        await base.Open();
    }

    public override async UniTask Close() {
        await base.Close();
    }
}
