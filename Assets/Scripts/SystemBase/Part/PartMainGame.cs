using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

using static GameConst;
using static EnemyUtility;

public class PartMainGame : PartBase {
    // 敵管理クラス
    [SerializeField]
    private EnemyManager _enemyManager = null;
    // 敵死亡エフェクト管理クラス
    [SerializeField]
    private EnemyDeadEffectManager _enemyEffectManager = null;
    // メインゲームスタートフラグ
    public static bool isStart { get; private set; } = false;
    // カメラ管理クラス
    private CameraController _mainCamera = null;
    // メインループクラス
    private EndlessGame _endless = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        // メインカメラの設定
        _mainCamera = Camera.main.GetComponent<CameraController>();
        // メインパートのUIの生成、初期化
        PinchExpansionText pinchText = MenuManager.instance.Get<PinchExpansionText>("Prefabs/Menu/PinchExpansionText");
        await pinchText.Initialize();
        // カメラの拡縮率のテキストにセット
        _mainCamera.SetPinchText(pinchText);
        await MenuManager.instance.Get<ScoreTextManager>("Prefabs/Menu/ScoreText").Initialize();
        // 敵マネージャーの初期化
        _enemyManager?.Initialize();
        // エフェクトマネージャーの初期化
        _enemyEffectManager?.Initialize();
        // タイムテキストの初期化
        TimeManager timer = MenuManager.instance.Get<TimeManager>("Prefabs/Menu/CanvasTimer");
        await timer.Initialize();
        // モードの初期化
        _endless = new EndlessGame();
        await _endless.Initialize(timer);
    }

    public override async UniTask Setup() {
        await base.Setup();
        // カメラの準備前処理
        await _mainCamera.Setup();
    }

    public override async UniTask Execute() {
        // 敵の生成(初回のみ)
        SpawnEnemy(INIT_FLOOR_ENEMY, 0);
        // フェードイン
        await FadeManager.instance.FadeIn();
        // カメラの演出
        await _mainCamera.StartGameCamera(1.0f);
        // スコアテキストの表示
        UniTask scoreTask = MenuManager.instance.Get<ScoreTextManager>().Open();
        // BGM再生
        AudioManager.instance.PlayBGM(1);
        // スタートフラグの変更
        isStart = true;
        // メインループの起動
        bool limitTime = await _endless.Execute();
        // スコアテキストを閉じる
        UniTask scoreEndTask = MenuManager.instance.Get<ScoreTextManager>().Close();
        // BGM停止
        AudioManager.instance.StopBGM();
        if (limitTime) {
            await FadeManager.instance.FadeOut();
            // パートの切り替え
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Ending);
        }
        isStart = false;
    }

    public override async UniTask Teardown() {
        await base.Teardown();
        // ピンチテキストメニューを閉じる
        UniTask pinchTextTask = MenuManager.instance.Get<PinchExpansionText>().Close();
        // カメラの片付け処理
        _mainCamera.Teardown();
    }
}
