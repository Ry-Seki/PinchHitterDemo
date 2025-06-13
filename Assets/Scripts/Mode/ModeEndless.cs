using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeEndless : ModeBase {

    public override async UniTask Initialize() {
        await base.Initialize();
        await FadeManager.instance.FadeIn();
    }
    public override async UniTask Execute() {
        await UniTask.CompletedTask;
    }
}
