using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static GameConst;

public class MenuEnding : MenuBase {
    // 結果スコアテキスト
    [SerializeField]
    private TextMeshProUGUI _resultScoreText = null;
    // スコア移動用親オブジェクト
    [SerializeField]
    private Transform _moveScoreRoot = null;
    // ハイスコア用画像
    [SerializeField]
    private Image _highScoreImage = null;

    // 結果スコア
    private int _resultScore = -1;
    // 移動前位置
    private Vector3 _startPos = Vector3.zero;
    // 移動後処理
    private Vector3 _goalPos = Vector3.zero;

    // 待機フレーム数
    private const int _WAIT_FRAME = 60;
    // 開始地点のY座標
    private const int _START_POS_Y = 200;

    private CancellationToken _token;

    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        _startPos = new Vector3(0, _START_POS_Y, 0);
        _goalPos = Vector3.zero;
    }
    public override async UniTask Open() {
        _resultScore = 0;
        _resultScoreText.text = null;
        _highScoreImage.gameObject.SetActive(false);
        _moveScoreRoot.localPosition = _startPos;
        await base.Open();
        await FadeManager.instance.FadeIn();
        await ShowScoreEffect(ScoreTextManager.score);
        while (true) {
            if (Input.GetMouseButtonDown(0))
                break;

            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }
    public override async UniTask Close() {
        await base.Close();
        ScoreTextManager.SetHighScore();
    }
    /// <summary>
    /// 結果スコアを設定
    /// </summary>
    /// <param name="setScoreValue"></param>
    public void ShowResultScore(int setScoreValue) {
        _resultScore = setScoreValue;
        _resultScoreText.text = _resultScore.ToString();
    }
    /// <summary>
    /// 結果スコア演出
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    private async UniTask ShowScoreEffect(int score) {
        _token = this.GetCancellationTokenOnDestroy();
        float elapsedTime = 0.0f;
        // SE再生
        UniTask rouletteInTask = AudioManager.instance.PlaySE(6);
        // ランダム数字を出す時間
        while (elapsedTime < 2.0f) {
            elapsedTime += Time.deltaTime;
            int fakeScore = Random.Range(0, MAX_SCORE_NUM); // 適当に大きめの範囲
            _resultScoreText.text = fakeScore.ToString();
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        UniTask rouletteOutTask = AudioManager.instance.PlaySE(7);
        // 最後に本当のスコアを表示
        ShowResultScore(score);
        //ハイスコア更新時、画像表示
        if (ScoreTextManager.IsHighScore) await HighScoreEffect();
    }
    /// <summary>
    /// 最大スコアだった場合の演出
    /// </summary>
    /// <returns></returns>
    private async UniTask HighScoreEffect() {
        _token = this.GetCancellationTokenOnDestroy();
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        await MoveScoreText();
        _highScoreImage.gameObject.SetActive(true);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float scale = Mathf.Lerp(0, 1.0f, t);
            _highScoreImage.transform.localScale = new Vector3(scale, scale, 1.0f);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        _highScoreImage.transform.localScale = Vector3.one;
        UniTask highScoreTask = AudioManager.instance.PlaySE(8);
    }
    /// <summary>
    /// ハイスコア時のスコアの移動処理
    /// </summary>
    /// <returns></returns>
    private async UniTask MoveScoreText() {
        _token = this.GetCancellationTokenOnDestroy();
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        await UniTask.DelayFrame(_WAIT_FRAME, PlayerLoopTiming.Update, _token);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Vector3 movePos = Vector3.Lerp(_startPos, _goalPos, t);
            _moveScoreRoot.localPosition = movePos;

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
        }
        _moveScoreRoot.localPosition = _goalPos;
    }
}
