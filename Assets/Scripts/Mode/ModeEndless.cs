using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeEndless : ModeBase {
    private float limitTimePer = -1;
    public override async UniTask Initialize() {
        await base.Initialize();
        await FadeManager.instance.FadeIn();
    }

    public override async UniTask Execute() {

        await UniTask.CompletedTask;

    }
}
