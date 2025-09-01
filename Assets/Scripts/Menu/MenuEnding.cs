using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuEnding : MenuBase {
    [SerializeField]
    private TextMeshProUGUI resultScoreText = null;
    [SerializeField]
    private Transform moveScoreRoot = null;
    [SerializeField]
    private Image highScoreImage = null;

    private int resultScore = -1;
    private Vector3 startPos = Vector3.zero;
    private Vector3 goalPos = Vector3.zero;
    private CancellationToken token;

    private const int WAIT_FRAME = 60;
    private const int START_POS_Y = 200;
    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
        startPos = new Vector3(0, START_POS_Y, 0);
        goalPos = Vector3.zero;
    }

    public override async UniTask Open() {
        resultScore = 0;
        resultScoreText.text = null;
        highScoreImage.gameObject.SetActive(false);
        moveScoreRoot.localPosition = startPos;
        await base.Open();
        await FadeManager.instance.FadeIn();
        await ShowScoreEffect(ScoreTextManager.score);
        while (true) {
            if(Input.GetMouseButtonDown(0)) break;

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
        resultScore = setScoreValue;
        resultScoreText.text = resultScore.ToString();
    }
    private async UniTask ShowScoreEffect(int score) {
        token = this.GetCancellationTokenOnDestroy();
        float elapsedTime = 0.0f;

        UniTask rouletteInTask = AudioManager.instance.PlaySE(6);
        // ランダム数字を出す時間
        while (elapsedTime < 2.0f) {
            elapsedTime += Time.deltaTime;
            int fakeScore = Random.Range(0, score * 2); // 適当に大きめの範囲
            resultScoreText.text = fakeScore.ToString();
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        UniTask rouletteOutTask = AudioManager.instance.PlaySE(7);
        // 最後に本当のスコアを表示
        ShowResultScore(score);
        //ハイスコア更新時、画像表示
        if (ScoreTextManager.IsHighScore) await HighScoreEffect();
    }

    private async UniTask HighScoreEffect() {
        token = this.GetCancellationTokenOnDestroy();
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        await MoveScoreText();
        highScoreImage.gameObject.SetActive(true);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float scale = Mathf.Lerp(0, 1.0f, t);
            highScoreImage.transform.localScale = new Vector3(scale, scale, 1.0f);

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        highScoreImage.transform.localScale = Vector3.one;
        UniTask highScoreTask = AudioManager.instance.PlaySE(8);
    }
    private async UniTask MoveScoreText() {
        token = this.GetCancellationTokenOnDestroy();
        float elapsedTime = 0.0f;
        float duration = 1.0f;
        await UniTask.DelayFrame(WAIT_FRAME, PlayerLoopTiming.Update, token);

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            Vector3 movePos = Vector3.Lerp(startPos, goalPos, t);
            moveScoreRoot.localPosition = movePos;

            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        moveScoreRoot.localPosition = goalPos;
    }
}
