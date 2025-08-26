using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuTitle : MenuBase {
    public eTitleMenu titleMenu { get; private set; } = eTitleMenu.Invalid;
    [SerializeField]
    private Transform buttonRoot = null;
    [SerializeField]
    private Image titleImage = null;

    private MenuStatusUpgrade statusUpgrade = null;
    private MenuSettings settings = null;

    private Vector3 baseScale = Vector3.zero;
    private const float MOVE_SIZE_SPEED = 0.5f;
    private const float MAX_IMAGE_SIZE = 1.15f;
    private const float MIN_IMAGE_SIZE = 0.75f;
    private CancellationToken token;

    public override async UniTask Initialize() {
        await base.Initialize();
        statusUpgrade = MenuManager.instance.Get<MenuStatusUpgrade>();
        settings = MenuManager.instance.Get<MenuSettings>();
        baseScale = titleImage.transform.localScale;
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        UniTask task = ChangeTitleScale();
        //Spaceキーが押されるまで待つ
        while (true) {
            if(titleMenu == eTitleMenu.StartGame) break;

            if (titleMenu == eTitleMenu.Enhance) await OpenUpgradeScreen();

            if(titleMenu == eTitleMenu.Setting) await OpenSettingScreen();

            await UniTask.DelayFrame(1);
        }
        titleMenu = eTitleMenu.Invalid;
        await FadeManager.instance.FadeOut();
        await Close();
    }
    private async UniTask OpenUpgradeScreen() {
        await FadeManager.instance.FadeOut();
        buttonRoot.gameObject.SetActive(false);
        await statusUpgrade.Open();
        MenuManager.instance.Get<MenuTitle>().BackScreen();
        await FadeManager.instance.FadeIn();
    }
    private async UniTask OpenSettingScreen() {
        await FadeManager.instance.FadeOut();
        buttonRoot.gameObject.SetActive(false);
        await settings.Open();
        MenuManager.instance.Get<MenuTitle>().BackScreen();
        await FadeManager.instance.FadeIn();
    }
    private async UniTask ChangeTitleScale() {
        token = this.GetCancellationTokenOnDestroy();
        while (titleMenu == eTitleMenu.Invalid) {
            // 0～1を往復する値を作成（時間に応じて変化）
            float t = Mathf.PingPong(Time.time * MOVE_SIZE_SPEED, 1f);

            // minScale～maxScale の間を補間
            float scale = Mathf.Lerp(MIN_IMAGE_SIZE, MAX_IMAGE_SIZE, t);

            // スケールを適用（UI ImageでもOK）
            titleImage.transform.localScale = baseScale * scale;
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        baseScale = Vector3.one;
        titleImage.transform.localScale = baseScale;
        await UniTask.CompletedTask;
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
        UniTask task = ChangeTitleScale();
    }
}
