using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static EnemyUtility;
using static GameConst;
using static TimeManager;
using static SaveDataUtility;

public class EndlessGame {
    public int phaseNum { get; private set; } = -1;
    public static bool isGameEnd { get; private set; } = false;
    private TimeManager timeManager = null;
    private const int ADD_ENEMY_NUM = 3;
    private const int BORDER_SCORE = 200;
    private static bool isEmptyEnemy = false;

    public async UniTask Initialize(TimeManager setTimeManager) {
        timeManager = setTimeManager;
        await UniTask.CompletedTask;
    }

    public async UniTask<bool> Execute() {
        //ゲームモードの初期化
        isGameEnd = false;
        //フェーズの初期化
        phaseNum = 0;
        //制限時間の管理
        UniTask task = timeManager.Open();
        //フェーズ処理
        while (limitTimerPer > 0) {
            await AddPhase();
            await UniTask.DelayFrame(1);
        }
        await timeManager.Close();
        UnuseAllEnemy();
        //スコアの処理
        ScoreTextManager.SetHighScore();
        EarnStatusPoint();
        isGameEnd = true;
        return isGameEnd;
    }
    /// <summary>
    /// ゲーム内の敵が居なくなるとフェーズを更新させる処理
    /// </summary>
    /// <returns></returns>
    private async UniTask AddPhase() {
        if (!isEmptyEnemy)  return;
        //敵の片付け処理の時間を確保するために遅延を少し入れる
        await UniTask.Delay(1000);
        //フェーズの更新
        phaseNum++;
        SpawnEnemy(SetEnemySpawnCount(phaseNum), phaseNum);
        isEmptyEnemy = false;
    }
    /// <summary>
    /// フェーズごとに生成される敵の数
    /// </summary>
    /// <param name="phaseNum"></param>
    /// <returns></returns>
    private int SetEnemySpawnCount(int phaseNum) {
        int spawnCount = INIT_FLOOR_ENEMY + ADD_ENEMY_NUM * phaseNum;

        return Mathf.Clamp(spawnCount, INIT_FLOOR_ENEMY, MAX_FLOOR_ENEMY);
    }
    /// <summary>
    /// スコア分ステータスポイントを獲得する
    /// </summary>
    private void EarnStatusPoint() {
        int score = ScoreTextManager.score;
        int highScore = ScoreTextManager.highScore;
        if(score < 0) return;

        int point = score / BORDER_SCORE;
        //前回のハイスコアを上回った場合ボーナスポイント獲得
        if(score > highScore) point += (score - highScore) / BORDER_SCORE;
        //セーブデータに設定する
        AddStatusPointData(point);
    }
    /// <summary>
    /// シーン上の敵がいないことを知らせるフラグの変更
    /// </summary>
    public static void EnemyEmpty() {
        isEmptyEnemy = true;
    }
}
