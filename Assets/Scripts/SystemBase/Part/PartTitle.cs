using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTitle : PartBase {

    public override async UniTask Initialize() {
        await base.Initialize();
        //タイトルメニューの初期化
        await MenuManager.instance.Get<MenuTitle>("Prefabs/Menu/CanvasTitle").Initialize();
    }
    public override async UniTask Execute() {
        Camera.main.GetComponent<CameraController>().Initialize();
        //タイトルメニューの表示
        await MenuManager.instance.Get<MenuTitle>().Open();
        //メインパートへ遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
    }
}
