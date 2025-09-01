using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartStanby : PartBase {
    public override async UniTask Initialize() {
        await base.Initialize();
        //セーブデータの読み込み
        //PlayerStatusDataManager.instance.InitSaveStatus();
    }
    public override async UniTask Execute() {
        //フェードアウト
        await FadeManager.instance.FadeIn();
        //タイトルパートへ遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
    }
}
