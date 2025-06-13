using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : SystemObject {
    public static ModeManager instance { get; private set; } = null;

    public eGameMode gameMode {  get; private set; } = eGameMode.Invalid;

    public override async UniTask Initialize() {
        instance = this;
        await UniTask.CompletedTask;
    }
    public void SetGameMode(eGameMode setMode) {
        gameMode = setMode;
    }
}
