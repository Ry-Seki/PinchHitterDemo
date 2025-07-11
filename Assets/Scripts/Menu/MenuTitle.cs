using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTitle : MenuBase {
    public eTitleMenu titleMenu { get; private set; } = eTitleMenu.Invalid;
    [SerializeField]
    private Transform buttonRoot = null;
    private MenuStatusEnhance statusEnhance = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        statusEnhance = MenuManager.instance.Get<MenuStatusEnhance>();
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        //SpaceÉLÅ[Ç™âüÇ≥ÇÍÇÈÇ‹Ç≈ë“Ç¬
        while (true) {
            if(titleMenu == eTitleMenu.StartGame) break;

            if (titleMenu == eTitleMenu.Enhance) await OpenEnhanceScreen();

            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
        titleMenu = eTitleMenu.Invalid;
    }

    private async UniTask OpenEnhanceScreen() {
        await FadeManager.instance.FadeOut();
        buttonRoot.gameObject.SetActive(false);
        await statusEnhance.Open();
    }
    public void StartInputGame() {
        titleMenu = eTitleMenu.StartGame;
    }
    public void EnhanceScreen() {
        titleMenu = eTitleMenu.Enhance;
    }
    public void SettingScreen() {
        titleMenu = eTitleMenu.Setting;
    }
    public void BackScreen() {
        titleMenu = eTitleMenu.Invalid;
        buttonRoot.gameObject.SetActive(true);
    }
}
