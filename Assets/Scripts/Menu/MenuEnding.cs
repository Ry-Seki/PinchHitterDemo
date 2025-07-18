using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuEnding : MenuBase {
    [SerializeField]
    private TextMeshProUGUI resultScoreText = null;
    private int resultScore = -1;
    public override async UniTask Initialize() {
        await base.Initialize();
        gameObject.SetActive(false);
    }

    public override async UniTask Open() {
        resultScore = 0;
        await base.Open();
        await FadeManager.instance.FadeIn();
        ShowResultScore(ScoreTextManager.score);
        while (true) {
            if(Input.GetMouseButtonDown(0)) break;

            await UniTask.DelayFrame(1);
        }
        await FadeManager.instance.FadeOut();
        await Close();
    }

    public override async UniTask Close() {
        await base.Close();
    }
    /// <summary>
    /// åãâ ÉXÉRÉAÇê›íË
    /// </summary>
    /// <param name="setScoreValue"></param>
    public void ShowResultScore(int setScoreValue) {
        resultScore = setScoreValue;
        resultScoreText.text = "Score : " + resultScore;
    }
}
