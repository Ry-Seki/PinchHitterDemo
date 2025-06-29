using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartEnding : PartBase {

    public override async UniTask Execute() {
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
        await UniTask.CompletedTask;
    }

}
