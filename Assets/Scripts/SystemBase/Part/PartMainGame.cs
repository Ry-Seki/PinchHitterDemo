using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMainGame : PartBase {
    public static bool isStart { get; private set; } = false;
    public override async UniTask Setup() {
        await base.Setup();
        await FadeManager.instance.FadeIn();
    }
    public override async UniTask Execute() {
        Camera.main.GetComponent<CameraController>().enabled = true;
        await Camera.main.GetComponent<CameraController>().Setup(1.0f);
        isStart = true;
        await UniTask.CompletedTask;
    }
}
