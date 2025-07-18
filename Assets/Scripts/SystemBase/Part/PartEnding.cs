using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartEnding : PartBase {
    public override async UniTask Initialize() {
        await base.Initialize();
        await MenuManager.instance.Get<MenuEnding>("Prefabs/Menu/CanvasEnding").Initialize();
    }
    public override async UniTask Execute() {
        await MenuManager.instance.Get<MenuEnding>().Open();
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        await UniTask.CompletedTask;
    }
    public override async UniTask Teardown() {
        await base.Teardown();
        SaveDataManager.instance.SaveData();
    }
}
