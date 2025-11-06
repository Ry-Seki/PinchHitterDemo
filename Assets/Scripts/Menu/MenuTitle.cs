using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuTitle : MenuBase {
    // ボタンオブジェクトの親オブジェクト
    [SerializeField]
    private Transform _buttonRoot = null;
    // タイトルイメージ
    [SerializeField]
    private Image _titleImage = null;
    // ステータスアップグレードクラス
    private MenuStatusUpgrade _statusUpgrade = null;
    // 設定メニュークラス
    private MenuSettings _settings = null;
    // タイトルメニューの列挙体
    private eTitleMenu _titleMenu = eTitleMenu.Invalid;

    // タイトルメニューの基礎大きさ
    private Vector3 _baseScale = Vector3.zero;
    // 移動スピード
    private const float _MOVE_SIZE_SPEED = 0.5f;
    // 最大画像サイズ
    private const float _MAX_IMAGE_SIZE = 1.15f;
    // 最小画像サイズ
    private const float _MIN_IMAGE_SIZE = 0.75f;

    private CancellationToken _token;

    public override async UniTask Initialize() {
        await base.Initialize();
        _statusUpgrade = MenuManager.instance.Get<MenuStatusUpgrade>();
        _settings = MenuManager.instance.Get<MenuSettings>();
        _baseScale = _titleImage.transform.localScale;
    }
    public override async UniTask Open() {
        await base.Open();
        await FadeManager.instance.FadeIn();
        UniTask task = ChangeTitleScale();
        //Spaceキーが押されるまで待つ
        while (true) {
            if(_titleMenu == eTitleMenu.StartGame) break;

            if (_titleMenu == eTitleMenu.Upgrade) await OpenUpgradeScreen();

            if(_titleMenu == eTitleMenu.Setting) await OpenSettingScreen();

            await UniTask.DelayFrame(1);
        }
        _titleMenu = eTitleMenu.Invalid;
        await FadeManager.instance.FadeOut();
        await Close();
    }
    /// <summary>
    /// アップグレードメニュー展開
    /// </summary>
    /// <returns></returns>
    private async UniTask OpenUpgradeScreen() {
        await FadeManager.instance.FadeOut();
        _buttonRoot.gameObject.SetActive(false);
        await _statusUpgrade.Open();
        MenuManager.instance.Get<MenuTitle>().BackScreen();
        await FadeManager.instance.FadeIn();
    }
    /// <summary>
    /// セッティングメニュー展開
    /// </summary>
    /// <returns></returns>
    private async UniTask OpenSettingScreen() {
        await FadeManager.instance.FadeOut();
        _buttonRoot.gameObject.SetActive(false);
        await _settings.Open();
        MenuManager.instance.Get<MenuTitle>().BackScreen();
        await FadeManager.instance.FadeIn();
    }
    /// <summary>
    /// タイトルイメージの拡縮ループ
    /// </summary>
    /// <returns></returns>
    private async UniTask ChangeTitleScale() {
        _token = this.GetCancellationTokenOnDestroy();
        while (_titleMenu == eTitleMenu.Invalid) {
            // 0～1を往復する値を作成（時間に応じて変化）
            float t = Mathf.PingPong(Time.time * _MOVE_SIZE_SPEED, 1f);

            // minScale～maxScale の間を補間
            float scale = Mathf.Lerp(_MIN_IMAGE_SIZE, _MAX_IMAGE_SIZE, t);

            // スケールを適用（UI ImageでもOK）
            _titleImage.transform.localScale = _baseScale * scale;
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        _baseScale = Vector3.one;
        _titleImage.transform.localScale = _baseScale;
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void StartInputGame() {
        UniTask task = AudioManager.instance.PlaySE(5);
        _titleMenu = eTitleMenu.StartGame;
    }
    /// <summary>
    /// 強化画面移動
    /// </summary>
    public void EnhanceScreen() {
        UniTask task = AudioManager.instance.PlaySE(4);
        _titleMenu = eTitleMenu.Upgrade;
    }
    /// <summary>
    /// 設定画面移動
    /// </summary>
    public void SettingScreen() {
        UniTask task = AudioManager.instance.PlaySE(4);
        _titleMenu = eTitleMenu.Setting;
    }
    /// <summary>
    /// 戻る
    /// </summary>
    public void BackScreen() {
        _titleMenu = eTitleMenu.Invalid;
        _buttonRoot.gameObject.SetActive(true);
        UniTask task = ChangeTitleScale();
    }
}
