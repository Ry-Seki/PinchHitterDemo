using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;
using static EnemyUtility;
using static PlayerStatusUtility;
using static EnemyDeadEffectUtility;
using System.Threading;

public abstract class EnemyBase : MonoBehaviour {
    protected static StringBuilder spriteNameBuilder = new StringBuilder();
    // カメラ制御クラス
    protected static CameraController mainCamera { get; private set; } = null;
    // 敵のHPゲージ
    [SerializeField]
    protected Slider enemyHPSlider = null;
    // 敵のスプライトコンポーネント
    [SerializeField]
    protected SpriteRenderer enemySprite = null;
    // 敵のアニメーションスプライトリスト
    protected Sprite[][] animSpriteList = null;
    // 敵のアニメーション列挙体
    protected eEnemyAnimation currentAnim = eEnemyAnimation.Invalid;
    // アニメーションタスク
    protected UniTask animTask;
    // アニメーション番号
    protected int animIndex = -1;

    // アニメーション待機時間
    private static readonly int _ANIMATION_DELAY_MILLI_SEC = 150;

    // 最大HP
    public int maxHP { get; protected set; } = -1;
    // HP
    public int HP { get; protected set; } = -1;
    // 死亡フラグ
    protected bool isDead = false;
    // カメラ内か判別フラグ
    protected bool isCameraHit = false;
    // ダメージクールタイムフラグ
    protected bool damageCoolTime = false;
    // ダメージ最小倍率補正
    protected const float MIN_DAMAGE_NORM = 1.0f;
    // ダメージ最大倍率補正
    protected const float MAX_DAMAGE_NORM = 2.0f;
    // 最大ダメージ倍率
    protected const float MAX_DAMAGE_PERCENTAGE = 100.0f;
    // 死亡時に加算されるスコア
    protected readonly int ADD_SCORE = 200;

    private CancellationToken _token;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public static void Initialize() {
        if(mainCamera != null) return;

        //メインカメラの登録
        mainCamera = Camera.main.GetComponent<CameraController>();
    }
    /// <summary>
    /// モデルの読み込み
    /// </summary>
    public virtual void LoadModel() { }
    /// <summary>
    /// 使用前準備
    /// </summary>
    /// <param name="setPhase"></param>
    public virtual void Setup(int setPhase) {
        isDead = false;
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public virtual void Teardown() {
        animIndex = 0;
        isCameraHit = false;
        damageCoolTime = false;
    }
    /// <summary>
    /// カメラ内にいるか計算
    /// </summary>
    protected virtual void OnWillRenderObject() {
#if UNITY_EDITOR

        if (Camera.current.name == "SceneCamera" || Camera.current.name == "Preview Camera") return;

#endif
            if (PartMainGame.isStart && mainCamera.IsHitter() && !damageCoolTime) {
                isCameraHit = true;
            } else {
                isCameraHit = false;
            }
    }
    /// <summary>
    /// 死亡判定付きのダメージ付与
    /// </summary>
    protected virtual async UniTask Damage() {
        //ダメージの取得
        float damage = GetRawAttack() * (float)DamageNormScaling(CameraController.pinchPercentage);
        HP -= (int)damage;
        enemyHPSlider.value = (float)HP / (float)maxHP;
        // HPが0の時
        if (!isDead && enemyHPSlider.value <= 0) {
            isDead = true;
            enemyHPSlider.value = 0;
            enemySprite.sprite = null;
            // スコアに反映
            MenuManager.instance.Get<ScoreTextManager>().AddScore(ADD_SCORE);
            // タイムに反映
            TimeManager.AddLimitTime(ADD_SCORE);
            // 死亡エフェクトの再生
            UseEffect(this);
            // 死亡SEの再生
            UniTask task = AudioManager.instance.PlaySE(1);
            //未使用状態にする
            DeathEnemy(this);
            // 数が0ならシーン上の敵がいないことを知らせる
            if (GetEnemyCount() <= 0) {
                EndlessGame.EnemyEmpty();
            }
        } else {
            UniTask task = AudioManager.instance.PlaySE(0);
            //クールタイム発動
            await DamageCoolTime();
        }
    }
    /// <summary>
    /// ダメージ正規補正
    /// </summary>
    /// <param name="setValue"></param>
    /// <returns></returns>
    protected float DamageNormScaling(float setValue) {
        return MIN_DAMAGE_NORM + 
            ((setValue - GetRawPercentage()) / (MAX_DAMAGE_PERCENTAGE - GetRawPercentage())) * 
            (MAX_DAMAGE_NORM - MIN_DAMAGE_NORM);
    }
    /// <summary>
    /// 攻撃クールタイム
    /// </summary>
    /// <returns></returns>
    public async UniTask DamageCoolTime() {
        damageCoolTime = true;
        float elapsedTime = 0.0f;
        float duration = GetRawInterval();

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        damageCoolTime = false;
    }
    /// <summary>
    /// アニメーション用の画像切り替えタスク
    /// </summary>
    /// <returns></returns>
    protected async UniTask PlayAnimationTask(CancellationToken token) {
        while (!token.IsCancellationRequested && gameObject.activeSelf) {
            //現在のアニメーション取得
            int currentAnimIndex = (int)currentAnim;
            if (!IsEnableIndex(animSpriteList, currentAnimIndex)) {
                //無効なアニメーションなら終了
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _token);
                return;
            }
            Sprite[] currentAnimSpriteList = animSpriteList[currentAnimIndex];
            //ループ判定、処理
            if (!IsEnableIndex(currentAnimSpriteList, animIndex))
                AnimationLoopProcess();
            //画像の設定
            enemySprite.sprite = currentAnimSpriteList[animIndex];
            //規定ミリ秒待ち、インデックス増加
            await UniTask.Delay(_ANIMATION_DELAY_MILLI_SEC, cancellationToken: token);
            animIndex++;
        }
    }
    /// <summary>
    /// アニメーションのループ処理
    /// </summary>
    private void AnimationLoopProcess() {
        //待機と歩行は_animIndexを0にする
        animIndex = 0;
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="setAnim"></param>
    public void SetAnimation(eEnemyAnimation setAnim) {
        //現在と同じアニメーションなら処理しない
        if (currentAnim == setAnim) return;

        animIndex = 0;
        currentAnim = setAnim;
    }
    /// <summary>
    /// 敵の移動処理
    /// </summary>
    /// <returns></returns>

    protected virtual async UniTask EnemyMoveDirection() {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// フェーズが進むごとに敵のステータスが上がる
    /// </summary>
    /// <param name="setPhase"></param>
    public virtual void EnemyPhaseStatusUp(int setPhase) {

    }
    /// <summary>
    /// 敵のダメージ演出
    /// </summary>
    /// <returns></returns>
    protected virtual async UniTask EnemyDamageEffect() {
        await UniTask.CompletedTask;
    }
}
