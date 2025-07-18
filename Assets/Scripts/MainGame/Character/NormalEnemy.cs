using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class NormalEnemy : EnemyBase {
    //初期HP
    private const int RAW_ENEMY_HP = 100;
    private const int ADD_PHASE_HP = 50;
    //画像読み込み用変数
    private static readonly string ENEMY_SPRITE_PATH = "Design/Sprites/";
    private static readonly string[] ANIMATION_SPRITE_NAME =
        new string[] {"NPC"};
    private const float DEFAULT_MOVE_TIME = 10.0f;
    private bool isMove = false;
    //移動のタスクを中断するためのトークン
    private CancellationToken token;

    public override void Setup(int setPhase) {
        base.Setup(setPhase);
        //アニメーションスプライトの読み込み
        int animMax = (int)eEnemyAnimation.Max;
        animSpriteList = new Sprite[animMax][];
        for (int i = 0; i < animMax; i++) {
            spriteNameBuilder.Append(ENEMY_SPRITE_PATH);
            spriteNameBuilder.Append(ANIMATION_SPRITE_NAME[i]);
            animSpriteList[i] = Resources.LoadAll<Sprite>(spriteNameBuilder.ToString());
            spriteNameBuilder.Clear();
        }
        //TODO:HPの設定
        EnemyPhaseStatusUp(setPhase);
        //座標の設定
        transform.position = new Vector2(Random.Range(-50, 51), Random.Range(-50, 51));
        //待機アニメーション設定
        SetAnimation(eEnemyAnimation.Wait);
        //アニメーション再生タスクを実行（すでに実行中ならしない）
        if(animTask.Status.IsCompleted()) animTask = PlayAnimationTask();
    }
    public override void EnemyPhaseStatusUp(int setPhase) {
        base.EnemyPhaseStatusUp(setPhase);
        //HPゲージの初期化
        enemyHPSlider.value = 1.0f;
        maxHP = RAW_ENEMY_HP + ADD_PHASE_HP * setPhase;
        HP = maxHP;
    }

    public override void Teardown() {
        base.Teardown();
        transform.position = Vector3.zero;
        enemyHPSlider.value = 0.0f;
        maxHP = 0;
        HP = 0;
    }
    protected override void OnWillRenderObject() {
        base.OnWillRenderObject();
        if(!isCameraHit || damageCoolTime) return;

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

        UniTask moveTask = EnemyMoveDirection();
    }
    public override async UniTask EnemyMoveDirection() {
        token = this.GetCancellationTokenOnDestroy();
        isMove = true;
        float elapseTime = 0.0f;
        float duration = Random.Range(5.0f, 10.0f);
        Vector3 startPos = transform.position;
        Vector3 goalPos = new Vector3(Random.Range(-100, 101), Random.Range(-100, 101), 0.0f);
        Vector3 direction = (startPos - goalPos).normalized;

        while (elapseTime < duration) {
            if(isDead) return;
            elapseTime += Time.deltaTime;
            float t = elapseTime / DEFAULT_MOVE_TIME;
            Vector3 setPos = Vector3.Lerp(startPos, goalPos, t);
            transform.position = setPos;
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        isMove = false;
        await UniTask.CompletedTask;
    }
}
