using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static EnemyUtility;

public class NormalEnemy : EnemyBase {
    private const int RAW_ENEMY_HP = 100;
    public override void Setup() {
        base.Setup();
        //HPの設定
        maxHP = RAW_ENEMY_HP;
        HP = maxHP;

        transform.position = new Vector2(Random.Range(-100, 101), Random.Range(-100, 101));
    }
    protected override void OnWillRenderObject() {
        base.OnWillRenderObject();
        if(!isCameraHit) return;

        //ダメージを与える
        UniTask task = Damage();
    }
    /// <summary>
    /// 死亡判定付きのダメージ付与
    /// </summary>
    protected override async UniTask Damage() {
        //ヒットSEの再生
        UniTask task = AudioManager.instance.PlaySE(0);
        //ダメージの取得
        float damage = mainCamera.GetRawAttack() * (float)DamageNormScaling(CameraController.pinchPercentage);
        HP -= (int)damage;
        enemyHPSlider.value = (float)HP / (float)maxHP;
        if (enemyHPSlider.value <= 0) {
            enemyHPSlider.value = 0;
            MenuManager.instance.Get<ScoreText>().AddScore(ADD_SCORE);
            //未使用状態にする
            UnuseEnemy(this);
            if (GetEnemyCount() <= 0) {
                await FadeManager.instance.FadeOut();
                UniTask partTask = PartManager.instance.TransitionPart(eGamePart.Ending);
            }
        } else {
            //クールタイム発動
            await DamageCoolTime();
        }
    }
}
