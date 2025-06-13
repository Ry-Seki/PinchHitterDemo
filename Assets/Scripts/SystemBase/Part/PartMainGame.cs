using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
    public override async UniTask Setup() {
        await base.Setup();
        await FadeManager.instance.FadeIn();
    }
    public override async UniTask Execute() {
        Camera.main.GetComponent<CameraController>().enabled = true;
        await UniTask.CompletedTask;
    }
}
