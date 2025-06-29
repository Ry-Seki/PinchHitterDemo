using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;

public class PartMainGame : PartBase {
    [SerializeField]
    private EnemyManager enemyManager = null;
    public static bool isStart { get; private set; } = false;
    private CameraController mainCamera = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        //メインカメラの設定
        mainCamera = Camera.main.GetComponent<CameraController>();
        //メインパートのUIの生成、初期化
        PinchExpansionText pinchText = MenuManager.instance.Get<PinchExpansionText>("Prefabs/Menu/PinchExpansionText");
        await pinchText.Initialize();
        //カメラの拡縮率のテキストにセット
        mainCamera.SetPinchText(pinchText);
        await MenuManager.instance.Get<ScoreText>("Prefabs/Menu/ScoreText").Initialize();
        enemyManager?.Initialize();
    }

    public override async UniTask Setup() {
        await base.Setup();
        await mainCamera.Initialize();
    }
    public override async UniTask Execute() {
        //敵の生成
        enemyManager.SpawnEnemy(INIT_FLOOR_ENEMY);
        //フェードイン
        await FadeManager.instance.FadeIn();
        //カメラの演出
        await mainCamera.Setup(1.0f);
        //スコアテキストの表示
        ScoreText scoreText = MenuManager.instance.Get<ScoreText>();
        UniTask scoreTextTask = scoreText.Open();
        //BGM再生
        AudioManager.instance.PlayBGM(0);
        //スタートフラグの変更
        isStart = true;
    }

    public override async UniTask Teardown() {
        await base.Teardown();
        //BGM停止
        AudioManager.instance.StopBGM();
        //スタートフラグの変更
        isStart = false;
        ScoreText scoreText = MenuManager.instance.Get<ScoreText>();
        UniTask scoreTextTask = scoreText.Close();
        PinchExpansionText pinchText = MenuManager.instance.Get<PinchExpansionText>();
        UniTask pinchTextTask = pinchText.Close();
        mainCamera.Teardown();
    }
}
