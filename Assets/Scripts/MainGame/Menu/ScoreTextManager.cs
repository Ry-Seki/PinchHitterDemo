using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreTextManager : MenuBase {
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    public static int score { get; private set; } = -1;
    public static int highScore { get; private set; } = -1;
    public static bool IsHighScore { get { return score > highScore; } }
    private const string SCORE_TEXT = "Score : ";
    private const int MAX_SCORE_NUM = 1000000;

    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        score = 0;
        if (highScore < 0) highScore = 0;

        highScore = SaveDataManager.instance.saveData.highScore;
    }

    public override async UniTask Open() {
        await base.Open();
        score = 0;
        ShowScoreText();
    }

    public override async UniTask Close() {
        await base.Close();
        if(IsHighScore) highScore = score;
    }
    /// <summary>
    /// テキストの表示
    /// </summary>
    private void ShowScoreText() {
        scoreText.text = string.Format("Score : {0}", score);
        Debug.Log(score);
    }
    /// <summary>
    /// スコア値設定
    /// </summary>
    /// <param name="setValue"></param>
    private void SetScore(int setValue) {
        //スコアがマイナスにならないように最小値を固定
        score = Mathf.Clamp(setValue, 0, MAX_SCORE_NUM);
        ShowScoreText();
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
}
