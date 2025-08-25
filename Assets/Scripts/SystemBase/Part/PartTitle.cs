using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTitle : PartBase {

    public override async UniTask Initialize() {
        await base.Initialize();
        //タイトルメニューの初期化
        await MenuManager.instance.Get<MenuStatusUpgrade>("Prefabs/Menu/CanvasUpgrade").Initialize();
        await MenuManager.instance.Get<MenuTitle>("Prefabs/Menu/CanvasTitle").Initialize();
    }
    public override async UniTask Execute() {
        //BGMの再生
        AudioManager.instance.PlayBGM(0);
        //タイトルメニューの表示
        await MenuManager.instance.Get<MenuTitle>().Open();
        //メインパートへ遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.MainGame);
        //BGMの停止
        AudioManager.instance.StopBGM();
    }
}
