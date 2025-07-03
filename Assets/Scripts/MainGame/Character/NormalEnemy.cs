using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class NormalEnemy : EnemyBase {
    private const int RAW_ENEMY_HP = 100;
    //画像読み込み用変数
    private static readonly string ENEMY_SPRITE_PATH = "Design/Sprites/";
    private static readonly string[] ANIMATION_SPRITE_NAME =
        new string[] {"NPC"};
    private const float DEFAULT_MOVE_TIME = 10.0f;
    private bool isMove = false;

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
        await base.Damage();
        if (isMove) return;

        await EnemyMoveDirection();
    }
    public override async UniTask EnemyMoveDirection() {
        isMove = true;
        float elapseTime = 0.0f;
        float duration = Random.Range(5.0f, 10.0f);
        Vector3 startPos = transform.position;
        Vector3 goalPos = new Vector3(Random.Range(-50, 51), Random.Range(-50, 51), 0.0f);
        Vector3 direction = (startPos - goalPos).normalized;

        while (elapseTime < duration) {
            elapseTime += Time.deltaTime;
            float t = elapseTime / DEFAULT_MOVE_TIME;
            Vector3 setPos = Vector3.Lerp(startPos, goalPos, t);
            transform.position = setPos;
            await UniTask.DelayFrame(1);
        }
        isMove = false;
        await UniTask.CompletedTask;
    }
}
