using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

using static PlayerStatusUtility;
using static CommonModule;

public class NormalEnemy : EnemyBase {
    //初期HP
    private const int RAW_ENEMY_HP = 100;
    private const int ADD_PHASE_HP = 100;
    //画像読み込み用変数
    private static readonly string ENEMY_SPRITE_PATH = "Design/Sprites/";
    private static readonly string[] ANIMATION_SPRITE_NAME =
        new string[] { "NPC" };
    private const float DEFAULT_MOVE_TIME = 10.0f;
    private bool isMove = false;
    //移動のタスクを中断するためのトークン
    private CancellationToken token;

    public override void LoadModel() {
        base.LoadModel();
        //アニメーションスプライトの読み込み
        int animMax = (int)eEnemyAnimation.Max;
        animSpriteList = new Sprite[animMax][];
        for (int i = 0; i < animMax; i++) {
            spriteNameBuilder.Append(ENEMY_SPRITE_PATH);
            spriteNameBuilder.Append(ANIMATION_SPRITE_NAME[i]);
            animSpriteList[i] = Resources.LoadAll<Sprite>(spriteNameBuilder.ToString());
            spriteNameBuilder.Clear();
        }
    }

    /// <summary>
    /// 準備前処理
    /// </summary>
    /// <param name="setPhase"></param>
    public override void Setup(int setPhase) {
        base.Setup(setPhase);
        isMove = false;
        // モデルの読み込み (初回のみ)
        if(IsEmpty(animSpriteList)) LoadModel();
        //ステータスの設定
        EnemyPhaseStatusUp(setPhase);
        //座標の設定
        transform.position = new Vector2(Random.Range(-50, 51), Random.Range(-50, 51));
        //待機アニメーション設定
        SetAnimation(eEnemyAnimation.Wait);
        //アニメーション再生タスクを実行（すでに実行中ならしない）
        animTask = PlayAnimationTask();
    }
    /// <summary>
    /// フェーズごとのステータスの設定
    /// </summary>
    /// <param name="setPhase"></param>
    public override void EnemyPhaseStatusUp(int setPhase) {
        base.EnemyPhaseStatusUp(setPhase);
        // HPの設定
        maxHP = RAW_ENEMY_HP + ADD_PHASE_HP * setPhase;
        HP = maxHP;
        //HPゲージの初期化
        enemyHPSlider.value = 1.0f;
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public override void Teardown() {
        base.Teardown();
        maxHP = 0;
        HP = 0;
        enemyHPSlider.value = 0.0f;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
    protected override void OnWillRenderObject() {
        base.OnWillRenderObject();
        if (!isCameraHit || damageCoolTime)
            return;

        //ダメージを与える
        UniTask damageTask = Damage();
    }
    /// <summary>
    /// 死亡判定付きのダメージ付与
    /// </summary>
    protected override async UniTask Damage() {
        if (isDead) return;
        //ダメージ効果
        UniTask damageEffectTask = EnemyDamageEffect();
        //ダメージ処理
        await base.Damage();

        if (isMove) return;

        UniTask moveTask = EnemyMoveDirection();

    }
    /// <summary>
    /// 敵の移動
    /// </summary>
    /// <returns></returns>
    protected override async UniTask EnemyMoveDirection() {
        await base.EnemyMoveDirection();
        token = this.GetCancellationTokenOnDestroy();
        isMove = true;
        float elapseTime = 0.0f;
        float duration = Random.Range(5.0f, 10.0f);
        Vector3 startPos = transform.position;
        Vector3 goalPos = new Vector3(Random.Range(-100, 101), Random.Range(-100, 101), 0.0f);
        Vector3 direction = (startPos - goalPos).normalized;

        while (elapseTime < duration) {
            if (isDead)
                break;
            elapseTime += Time.deltaTime;
            float t = elapseTime / DEFAULT_MOVE_TIME;
            Vector3 setPos = Vector3.Lerp(startPos, goalPos, t);
            transform.position = setPos;
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        isMove = false;
        await UniTask.CompletedTask;
    }
    protected override async UniTask EnemyDamageEffect() {
        await base.EnemyDamageEffect();
        token = this.GetCancellationTokenOnDestroy();
        enemySprite.color = Color.red;
        Color damageColor = enemySprite.color;
        float elapseTime = 0.0f;
        float duration = GetRawInterval();

        while (elapseTime < duration) {
            elapseTime += Time.deltaTime;
            float t = elapseTime / duration;
            Color changeColor = Color.Lerp(Color.red, Color.white, t);
            enemySprite.color = changeColor;
            await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);
        }
        enemySprite.color = Color.white;
        await UniTask.CompletedTask;
    }
}
