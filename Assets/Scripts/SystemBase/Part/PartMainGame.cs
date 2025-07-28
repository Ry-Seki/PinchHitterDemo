using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static EnemyUtility;

public class PartMainGame : PartBase {
    [SerializeField]
    private EnemyManager enemyManager = null;
    public static bool isStart { get; private set; } = false;
    private CameraController mainCamera = null;
    private EndlessGame endless = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        //メインカメラの設定
        mainCamera = Camera.main.GetComponent<CameraController>();
        //メインパートのUIの生成、初期化
        PinchExpansionText pinchText = MenuManager.instance.Get<PinchExpansionText>("Prefabs/Menu/PinchExpansionText");
        await pinchText.Initialize();
        //カメラの拡縮率のテキストにセット
        mainCamera.SetPinchText(pinchText);
        await MenuManager.instance.Get<ScoreTextManager>("Prefabs/Menu/ScoreText").Initialize();
        enemyManager?.Initialize();
        //タイムテキストの初期化
        TimeManager timer = MenuManager.instance.Get<TimeManager>("Prefabs/Menu/CanvasTimer");
        await timer.Initialize();
        //モードの初期化
        endless = new EndlessGame();
        await endless.Initialize(timer);
    }

    public override async UniTask Setup() {
        await base.Setup();
        await mainCamera.Initialize();
    }
    public override async UniTask Execute() {
        //敵の生成(初回のみ)
        SpawnEnemy(INIT_FLOOR_ENEMY, 0);
        //フェードイン
        await FadeManager.instance.FadeIn();
        //カメラの演出
        await mainCamera.Setup(1.0f);
        //スコアテキストの表示
        UniTask scoreTask = MenuManager.instance.Get<ScoreTextManager>().Open();
        //BGM再生
        AudioManager.instance.PlayBGM(1);
        //スタートフラグの変更
        isStart = true;
        //敵の生成
        bool limitTime = await endless.Execute();
        //BGM停止
        AudioManager.instance.StopBGM();
        if (limitTime) {
            await FadeManager.instance.FadeOut();
            UniTask task = PartManager.instance.TransitionPart(eGamePart.Ending);
        }
        isStart = false;
    }

    public override async UniTask Teardown() {
        await base.Teardown();
        //スタートフラグの変更
        ScoreTextManager scoreText = MenuManager.instance.Get<ScoreTextManager>();
        UniTask scoreTextTask = scoreText.Close();
        PinchExpansionText pinchText = MenuManager.instance.Get<PinchExpansionText>();
        UniTask pinchTextTask = pinchText.Close();
        mainCamera.Teardown();
    }
}
