using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using static EnemyUtility;

public class NormalEnemy : EnemyBase {
    private const int RAW_ENEMY_HP = 100;
    //画像読み込み用変数
    private static readonly string ENEMY_SPRITE_PATH = "Design/Sprites/";
    private static readonly string[] ANIMATION_SPRITE_NAME =
        new string[] {"NPC"};

    public override void Setup() {
        base.Setup();
        //アニメーションスプライトの読み込み
        int animMax = (int)eEnemyAnimation.Max;
        animSpriteList = new Sprite[animMax][];
        for (int i = 0; i < animMax; i++) {
            spriteNameBuilder.Append(ENEMY_SPRITE_PATH);
            spriteNameBuilder.Append(ANIMATION_SPRITE_NAME[i]);
            animSpriteList[i] = Resources.LoadAll<Sprite>(spriteNameBuilder.ToString());
            spriteNameBuilder.Clear();
        }     
        //HPの設定
        maxHP = RAW_ENEMY_HP;
        HP = maxHP;

        //座標の設定
        transform.position = new Vector2(Random.Range(-50, 51), Random.Range(-50, 51));
        //待機アニメーション設定
        SetAnimation(eEnemyAnimation.Wait);
        //アニメーション再生タスクを実行（すでに実行中ならしない）
        if(animTask.Status.IsCompleted()) animTask = PlayAnimationTask();

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
