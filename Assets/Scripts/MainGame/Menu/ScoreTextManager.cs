using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static GameConst;

public class ScoreTextManager : MenuBase {
    public static ScoreTextManager instance { get; private set; } = null;

    [SerializeField]
    private TextMeshProUGUI _deathValueText = null;

    public static int score { get; private set; } = -1;
    public static int highScore { get; private set; } = -1;
    public static int enemyDeathValue { get; private set; } = -1;
    public static bool IsHighScore { get { return score > highScore; } }

    public override async UniTask Initialize() {
        await base.Initialize();
        instance = this;
        gameObject.SetActive(false);
        highScore = PlayerStatusDataManager.instance.saveData.highScore;
    }
    public override async UniTask Open() {
        await base.Open();
        score = 0;
        enemyDeathValue = 0;
        ShowScoreText();
    }
    public override async UniTask Close() {
        await base.Close();
    }
    /// <summary>
    /// テキストの表示
    /// </summary>
    private void ShowScoreText() {
        _deathValueText.text = enemyDeathValue.ToString();
    }
    /// <summary>
    /// スコア値設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetScore(int setValue) {
        //スコアがマイナスにならないように最小値を固定
        score = Mathf.Clamp(setValue, 0, MAX_SCORE_NUM);
    }
    /// <summary>
    /// スコア値増加
    /// </summary>
    /// <param name="setValue"></param>
    public void AddScore(int setValue) {
        SetScore(score + setValue);
    }
    /// <summary>
    /// スコア値減少
    /// </summary>
    /// <param name="setValue"></param>
    public void RemoveScore(int setValue) {
        SetScore(score - setValue);
    }
    /// <summary>
    /// ハイスコアの設定
    /// </summary>
    public static void SetHighScore() {
        if(!IsHighScore) return;

        highScore = score;
        PlayerStatusDataManager.instance.SetHighScore(highScore);
    }
    /// <summary>
    /// 敵死亡数の増加
    /// </summary>
    public void AddEnemyDeathValue() {
        enemyDeathValue++;
        ShowScoreText();
    }
}
