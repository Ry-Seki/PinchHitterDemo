using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartStanby : PartBase {

    public override async UniTask Execute() {
        //フェードアウト
        await FadeManager.instance.FadeIn();
        //マスターデータの読み込み
        MasterDataManager.LoadAllData();
        //セーブデータの読み込み
        SaveDataManager.instance.LoadData();
        //タイトルパートへ遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
    }
}
