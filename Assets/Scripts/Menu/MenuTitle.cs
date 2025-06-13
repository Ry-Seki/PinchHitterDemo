using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTitle : MenuBase {
    public eTitleMenu titleMenu { get; private set; } = eTitleMenu.Invalid;

    public override async UniTask Open() {
        await base.Open();
        //SpaceƒL[‚ª‰Ÿ‚³‚ê‚é‚Ü‚Å‘Ò‚Â
        while (true) {
            if(Input.GetKeyDown(KeyCode.Space)) break;

            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }

    public override async UniTask Close() {
        await base.Close();
        titleMenu = eTitleMenu.Invalid;
    }
}
