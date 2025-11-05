using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartStanby : PartBase {

    public override async UniTask Initialize() {
        await base.Initialize();
    }

    public override async UniTask Execute() {
        // フェードアウト
        await FadeManager.instance.FadeIn();
        // タイトルパートへ遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
    }
}
